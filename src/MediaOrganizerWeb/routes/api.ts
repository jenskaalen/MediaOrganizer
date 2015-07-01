var express = require('express');
var router = express.Router();

var appDirectory = "\\\\devstation\\i$\\App\\MediaOrganizer";
var storageDirectory = "Storage";
var showsFile = "Shows_media.xml";
var moviesFile = "movies_media.xml";
var configFile = "MediaDirectories.xml";
var modulesFile = "Modules.xml";

var xml2js = require('xml2js');
var fs = require('fs');

router.get('/mediaDirectories', function(req, res){
  var xmlText = fs.readFileSync(appDirectory + '\\' + configFile, 'utf-8');

  xml2js.parseString(xmlText, function(err, xmlObject) {
      res.send(xmlObject);
  });
});

router.get('/getMedia', function(req, res){
  var file = appDirectory + '\\' + storageDirectory + '\\' + req.query.file;
  console.log(file);
  var xmlText = fs.readFileSync(file, 'utf-8');

  xml2js.parseString(xmlText, function(err, xmlObject) {
      res.send(xmlObject.ArrayOfMediaFile);
  });
});

router.get('/mediaFiles', function(req, res){
  var xmlText = fs.readFileSync(appDirectory + '\\' + configFile, 'utf-8');
  var mediaHandlers = new Array();

  xml2js.parseString(xmlText, function(err, xmlObject){
    // res.send(xmlObject.ArrayOfMedia
      for (var i = xmlObject.Handlers.Handler.length - 1; i >= 0; i--) {
      var name = xmlObject.Handlers.Handler[i].Name[0];
      mediaHandlers.push(name);
    };

    res.send(mediaHandlers);
  });
});

router.post('/saveHandlers', function(req, res){
  var mediaDirectoriesFile = appDirectory + '\\' + configFile;
  var builder = new xml2js.Builder();
  var xml = builder.buildObject(req.body);

  fs.writeFile(mediaDirectoriesFile, xml, function(){
    console.log('saved ' + mediaDirectoriesFile);
  });

});

router.get('/modules', function(req, res){
  var xmlText = fs.readFileSync(appDirectory + '\\' + modulesFile, 'utf-8');

  xml2js.parseString(xmlText, function(err, xmlObject) {
      res.send(xmlObject);
  });
});

// router.get('/getAllMediaFiles', function(req, res){

// }

// router.get('/getMedia', function(req, res){

//   var file = appDirectory + "\\" + showsFile;
//   var xmlText = fs.readFileSync(file, 'utf-8');

//   xml2js.parseString(xmlText, function(err, xmlObject){
//     res.send(xmlObject.ArrayOfMediaFile.MediaFile)
//   });
// });


// var someAsyncThing = function() {
//   return new Promise(function(resolve, reject) {
//     // this will throw, x does not exist
//     resolve(x + 2);
//   });
// };

