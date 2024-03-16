process.on('uncaughtException', function (error) {
    console.log("Uncaught Exception Encountered!")
    console.log("Error: ", error)
    console.log("Stack trace: ", error.stack)
    setInterval(function () { }, 1000)
})



const http = require("http")
const express = require("express")
const app = express()
app.use(express.static("public"))
const server = http.createServer(app)

const WebSocket = require("ws")
const wsServer = new WebSocket.Server({ server })
let wsClient = null
wsServer.on('connection', (ws) => {
    wsClient = ws

    const amqp = require('amqplib/callback_api')
    amqp.connect('amqp://rabbitmq:6031', (error0, connection) => {
        connection.createChannel((error1, channel) => {
            const queue = 'preview_raw'
            const msgExchange = 'component.v1.preview:PreviewDTO'
            const queueExchange = 'preview_raw'

            channel.assertQueue(queue, durable = true)
            channel.assertExchange(msgExchange, 'fanout', durable = true)
            channel.assertExchange(queueExchange, 'fanout', durable = true)
            channel.bindExchange(queueExchange, msgExchange)
            channel.bindQueue(queue, queueExchange, '')

            console.log("Waiting for messages in %s", queue)
            channel.consume(queue, (msg) => {
                const json = JSON.parse(msg.content.toString())
                json.File = Buffer.from(json.File, 'base64')
                wsClient.send(json)
            }, noAck = false)
        })

        connection.createChannel((error1, channel) => {
            const queue = 'preview_complete'
            const msgExchange = 'component.v1.preview:PreviewDTO'
            const queueExchange = 'preview_complete'

            channel.assertQueue(queue, durable = true)
            channel.assertExchange(msgExchange, 'fanout', durable = true)
            channel.assertExchange(queueExchange, 'fanout', durable = true)
            channel.bindExchange(queueExchange, msgExchange)
            channel.bindQueue(queue, queueExchange, '')

            console.log("Waiting for messages to %s", queue)

            wsClient.on('message', (m) => {
                const json = JSON.parse(m)
                json.File = json.File.toString('base64')
                channel.sendToQueue('preview_complete', json)
            })
        })
    })
})

const host = '0.0.0.0'
const port = 6004
const url = `http://${host}:${port}`

server.listen(port, host, () => console.log(`Server started at ${url}`))

let start = (process.platform == 'darwin'? 'open': process.platform == 'win32'? 'start': 'xdg-open')
let cmd = `${start} ${url}`
require('child_process').exec(cmd, (err) => {
    console.log(err)
})