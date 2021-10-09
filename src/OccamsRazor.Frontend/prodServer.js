var path = require('path')
var express = require('express')
var app = express()

app.use(express.static(path.join(__dirname, '/build')))

app.get('*', function (req, res) {
  res.sendFile(path.join(__dirname, 'build/index.html'))
})
const port = process.env.PORT || 3000;

app.listen(port, function (err) {
  if (err) {
    console.log(err)
  }
  console.info('pointing to %s', process.env.REACT_APP_API_URL)
  console.info('==> 🌎 Listening on port %s.', port)
})
