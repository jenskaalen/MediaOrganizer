

var app = angular.module('app', ['ui.bootstrap']);
console.log('scripto');

app.controller('handlerController', function($scope, $http, $modal){
	$scope.mediaInfos = new Array();
	$scope.visible = new Array();

    $scope.klekk = function() {
        alert('aaa');
    };

	$http.get('/api/mediaDirectories').success(function(mediaDirectories){
		console.log('set scope');
		$scope.mediaDirectories = mediaDirectories;
	});

    $http.get('/api/mediaFiles').success(function(mediaFiles) {
        console.log(mediaFiles);

        for (var i = mediaFiles.length - 1; i >= 0; i--) {
            var handlerName = mediaFiles[i];

            loadMediaHandler(handlerName);
        };
    });

	function loadMediaHandler(handlerName){
		var mediaFile = handlerName + "_media.xml";

		$http.get('/api/getMedia?file=' + mediaFile).success(function(media){
			// $scope.mediaFiles = shows;
			media.$.name = handlerName;
			$scope.mediaInfos.push(media);
		});		
	}

	$scope.openDemo = function(){
		console.log("attempt to open demo view");
	    $modal.open({ template: 'demoTemplate' });
	};

    $scope.saveHandlers = function() {
        $http.post('/api/saveHandlers', $scope.mediaDirectories);
    };
});



app.controller('modulesController', function($scope, $http){

	$http.get('/api/modules').success(function(modules){
		$scope.modules = modules;
	});

    $scope.saveModules = function() {
        //$http.post('/api/saveModules', $scope.modules);
        var builder = new xml2js.Builder();
        var xml = builder.buildObject(obj);
    };
});



// fs.readFile('e:\\nodejs\\read\\MediaDirectories.xml', 'utf-8', function(err, data){
// 	parseString = require('xml2js').parseString;

// 	parseString(data, function(err, xmlData){
// 		console.log(xmlData.Handlers.Handler[0].$.type);

// 		xmlData.Handlers.Handler[0].$.type = "peeen";


// 		var builder = new xml2js.Builder();
// 		var xml = builder.buildObject(xmlData);

// 		fs.writeFile('e:\\temp\\testen.xml', xml, function(err){ });
// 	});