process.on('uncaughtException', function (error) {
    console.log("Uncaught Exception Encountered!");
    console.log("Error: ", error);
    console.log("Stack trace: ", error.stack);
    setInterval(function () { }, 1000);
});



const http = require("http");
const WebSocket = require("ws");
const fs = require('node:fs');
const amqp = require('amqplib/callback_api');
const express = require("express");



const app = express();
app.use(express.static("public"));
const server = http.createServer(app);



const socket = new WebSocket.Server({ server });

let channel1;

amqp.connect("amqp://rabbitmq:5672", (e0, rConn) => {
    rConn.createChannel((e1, channel) => {
        channel1 = channel
        const queue = "preview"
        
        channel.assertQueue(queue, {
            durable: false
        });
        
        console.log(" [*] Waiting for messages in %s. To exit press CTRL+C", queue);
        
        channel.consume(queue, (msg) => {
            fs.readFile(msg, (e, data) => {
                socket.send(data)
            })
        }, { noAck: true });
    });
    
    rConn.createChannel((e1, channel) => {
        const queue = "preview_22"
        channel.assertQueue(queue, {
            durable: false
        });
    })
});

const getModelFile = (file) => {
    channel1.sendToQueue("preview_22", file)
}

socket.on('connection', ws => { 
    ws.on('message', file => {
        getModelFile(file)
    })
});
    


server.listen(3000, () => console.log("Server started"));