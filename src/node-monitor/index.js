const signalR = require('@microsoft/signalr');

// To set the API_URL in dev mode use:
// > $env:BUYSCOUT__API_URL = 'http://localhost:5000' ; node .\index.js

const API_URL = process.env['BUYSCOUT__API_URL'] || "https://prod-api-url.com";

console.log('BUYSCOUT__API_URL', API_URL);

var connection = new signalR.HubConnectionBuilder()
 .withUrl(`${API_URL}/hubs/testHub`) // 'http://localhost:5000'
 .withAutomaticReconnect()
 .build();

connection.on('Broadcast', function (data) {
  console.log('Broadcast', data);
  console.log(arguments);
 })
 
connection.on('Send', function (data) {
  console.log('Send', data);
})
 
connection.on('doit', function (data) {
  console.log('doit', data);
})

async function start() {
  console.log('start');
  try {
      await connection.start();
      console.log("SignalR Connected.");
  } catch (err) {
      console.log("SignalR failed. Will retry in 5 secs...");
      console.log(err);
      setTimeout(start, 5000);
  }
};

connection.onclose(start);
start();

// Info: https://docs.microsoft.com/en-us/aspnet/core/signalr/javascript-client?view=aspnetcore-5.0