"use strict";

var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7123/WeatherHub")
        .withAutomaticReconnect()
        .build()

connection.start()
    .then(async () => {
        console.log("Connected!");

        await connection.send("JoinGroup");


    })
    .catch(e => {
        console.log(e);
    });

connection.on("Send", message => {

    const obj = JSON.parse(message);
    console.log(obj.Temp);
});

connection.on("Log", (message) => {
    console.log(message);
});