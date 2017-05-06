var express = require('express')
var app = express()

app.use(express.static('dist'))

app.get('/', function (req, res) {
  res.render("index.html");
})

app.listen(3000, function () {
  console.log('Node js server running on port 3000!')
})