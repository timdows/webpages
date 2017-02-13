// This file is required by the index.html file and will
// be executed in the renderer process for that window.
// All of the Node.js APIs are available in this process.

const SerialPort = require('serialport')

var port = new SerialPort('/dev/ttyAMA0', {
  baudRate: 9600,
  parser: SerialPort.parsers.readline('\n')
});

port.on('open', function() {
});

// open errors will be emitted as an error event
port.on('error', function(err) {
  console.log('Error: ', err.message);
});

var table = document.getElementById("data");
port.on('data', function (data) {
//  console.log('Data: ' + data);
  var row = table.insertRow(0);
  var cell = row.insertCell(0);
  cell.innerHTML = data;
});

// serialport.list((err, ports) => {
//   console.log('ports', ports);
//   if (err) {
//     document.getElementById('error').textContent = err.message
//     return
//   } else {
//     document.getElementById('error').textContent = ''
//   }

//   if (ports.length === 0) {
//     document.getElementById('error').textContent = 'No ports discovered'
//   }

//   const headers = Object.keys(ports[0])
//   const table = createTable(headers)
//   tableHTML = ''
//   table.on('data', data => tableHTML += data)
//   table.on('end', () => document.getElementById('ports').innerHTML = tableHTML)
//   ports.forEach(port => table.write(port))
//   table.end();
// })
