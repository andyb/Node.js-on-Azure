var http = require('http');
var port = process.env.AZURE_PORT;
var ip = process.env.AZURE_IP;

http.createServer(function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end('This is node.js v0.5.1 running natively on Windows Azure. Cool eh!\n');
}).listen(port, ip);

console.log('Server running at ' + ip + ':' + port);