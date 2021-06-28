const signalR = require('@microsoft/signalr');
var connection = new signalR.HubConnectionBuilder()
 .withUrl('http://localhost:5000/hubs/testHub')
 .withAutomaticReconnect()
 .build();

 connection.on('Broadcast', function (data) {
  console.log(data);
  console.log(arguments);
 })
 
 connection.on('Send', function (data) {
  console.log(data);
})
 
 connection.on('doit', function (data) {
  console.log(data);
})

 async function start() {
  console.log('start');
  try {
      await connection.start();
      console.log("SignalR Connected.");
  } catch (err) {
      console.log("SignalR failed..");
      console.log(err);
      setTimeout(start, 5000);
  }
};

connection.onclose(start);
start();

// Info: https://docs.microsoft.com/en-us/aspnet/core/signalr/javascript-client?view=aspnetcore-5.0