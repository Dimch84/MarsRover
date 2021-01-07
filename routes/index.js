var express = require('express');
var router = express.Router();

/* GET home page. */
//router.get('/', function(req, res, next) {
//  res.render('index', { title: 'Express' });
//});

//console.log(__dirname);

router.get('/', function(req, res) {
  res.sendFile(__dirname + '\\main.html');
});

module.exports = router;
