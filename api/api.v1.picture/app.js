process.on('uncaughtException', function (error) {
    console.log("Uncaught Exception Encountered!");
    console.log("Error: ", error);
    console.log("Stack trace: ", error.stack);
    setInterval(function () { }, 1000);
});



const http = require("http");
const WebSocket = require("ws");
const fs = require('node:fs')
const express = require("express");


const app = express();
app.use(express.static("public"));

const server = http.createServer(app);



const webSocketServer = new WebSocket.Server({ server });

webSocketServer.on('connection', ws => {
    fs.readFile('60percent.glb', (e, data) => {
        ws.send(data)
    })
});



server.listen(3000, () => console.log("Server started"))