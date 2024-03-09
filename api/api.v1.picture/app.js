process.on('uncaughtException', function (error) {
    console.log("Uncaught Exception Encountered!");
    console.log("Error: ", error);
    console.log("Stack trace: ", error.stack);
    setInterval(function () { }, 1000);
});



const http = require("http");
const express = require("express");
const WebSocket = require("ws");


const app = express();
app.use(express.static("public"));

const server = http.createServer(app);



const webSocketServer = new WebSocket.Server({ server });

const dispatchEvent = (message, ws) => {
    const json = JSON.parse(message);
    console.log(json)

    switch (json.event) {
        case "chat-message":
            webSocketServer.clients.forEach(client =>
                client.send(json)
            );
        default:
            ws.send((new Error("Wrong query")).message);
    }
}

webSocketServer.on('connection', ws => {
    ws.on('message', m => dispatchEvent(m, ws));
    ws.on("error", e => ws.send(e));

    ws.send('Hi there, I am a WebSocket server');
});



server.listen(3000, () => console.log("Server started"))