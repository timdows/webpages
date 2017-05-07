var express = require('express')
var app = express()
var dir = require('node-dir');

app.use(express.static('dist'));
app.use('/images', express.static(__dirname + '/images'));

app.get('/images.json', function (req, res) {
	dir.files('images', function(err, files) {
		result = [];
		files.forEach(file => {
			result.push(file.replace(/\\/g, "/"));
		});
		res.send(result);
	});
});

app.get('/', function (req, res) {
	res.render("index.html");
});

app.listen(3000, function () {
	console.log('Node js server running on port 3000!')
});