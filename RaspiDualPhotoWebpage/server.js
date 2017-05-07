var express = require('express')
var app = express()
var dir = require('node-dir');

app.use(express.static('dist'));
app.use(express.static('images'));

app.get('/images.json', function (req, res) {
	dir.files('images', function(err, files) { 
		res.send(files);
	});
});

app.get('/', function (req, res) {
	res.render("index.html");
});

app.listen(3000, function () {
	console.log('Node js server running on port 3000!')
});