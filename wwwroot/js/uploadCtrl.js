//inject angular file upload directives and services.
var app = angular.module('fileUpload', ['ngFileUpload']);
// Main controller
app.controller('MyCtrl', ['$scope', 'Upload', '$timeout', function ($scope, Upload, $timeout) {
  // Record object 
  var record = {
    master: '',
    name: '',
    offset: '',
    length: ''
  };
  $scope.items = [];
  $scope.records = [];
  $scope.progressStyle = {'width': '0%'}; 
  $scope.downloadReady = false ; 

  var inProgress = false ; 
    
  // Upload file
  $scope.uploadPic = function (file) {
    var temp = [
          {
              master : "XCX-14.mp3",
              name:"Machines.mp3",
              offset:"00:17:58",
              length:"00:03:16"
          },
          {
              master : "XCX-14.mp3",
              name:"Alcoholic.mp3",
              offset:"00:41:50",
              length:"00:02:42"
          }
        ];
    file.upload = Upload.upload({
      url: '/Home/UploadTrack',
      data: { items: temp, file: file },
    });
    // Upload respsonse 
    file.upload.then(function (response) {
      $timeout(function () {
        if(response.data){
          if(file.progress == 100)
            $scope.progressStyle.width = '100%'; 
          $scope.downloadReady = true ;
        }else{
          alert("Error converting files");
        }
      });
    }, function (response) {
      if (response.status > 0)
        $scope.errorMsg = response.status + ': ' + response.data;
    }, function (evt) {
      // Math.min is to fix IE which reports 200% sometimes
      file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
      if(file.progress == 100){
        $scope.progressStyle.width = '10%'; 
      }
    });
  };
  // Add valid track
  $scope.add = function (record) {
    try {
      record.master = $scope.picFile.name;
      var index = $scope.records.indexOf(record);
      $scope.records.splice(index, 1);
      $scope.items.push(angular.copy(record));
    } catch (ex) {
      console.log(ex);
    }
  };
  // Add new track 
  $scope.addNew = function () {
    $scope.records.push(this.record);
  };
  // Remove Track
  $scope.remove = function(){
    $scope.items.pop(); 
  };
}]);

// Check file extenstion
app.directive('fileExtension', function () {
  return {
    require: 'ngModel',
    link: function (scope, element, attr, mCtrl) {
      function myValidation(value) {
        if (value.includes(".")) {
          mCtrl.$setValidity('extenstion', true);
        } else {
          mCtrl.$setValidity('extenstion', false);
        }
        return value;
      }
      mCtrl.$parsers.push(myValidation);
    }
  };
});