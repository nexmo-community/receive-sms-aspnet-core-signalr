"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/smsHub").build();

connection.on("InboundSms", function (fromNumber, text) {
    var rowHtml =
        '<tr><td>' +
        fromNumber +
        '</td><td>' +
        text +
        '</td></tr>';
    $('#messageList tbody').append(rowHtml);
});

connection.start()
    .then(function () {
        console.log("connection started");
    })
    .catch(function (err) {
        console.log("Error encountered: " + err);
    })
