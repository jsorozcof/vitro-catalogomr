var origin = $("input[name='UrlSite']").val();

var app = angular.module('app', []);
app.controller('searchController', ['$scope','retrieveService', function ($scope, retrieveService) {
    $scope.modelos = new Array();
    $scope.notfound = false;

    $scope.marcaChange = function () {
        var selected = $scope.marcaSelected;

        retrieveService.retrieve(origin + 'api/Container/Modelos/' + selected).then(function (data) {
            $scope.modelos = data;
        });
    };

    $scope.modeForm = function () {
        console.log("MODE FORM ACTIVATE");
        $scope.notfound = true;
    };

    this.$onInit = function () {
    };
}]);
app.service('retrieveService', ['$http', '$q', function ($http, $q) {
    this.retrieve = function (path) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: path
        }).then(function success(response) {
            deferred.resolve(response.data);
        }, function error(response) {
            deferred.reject(response);
            alert('OCURRIO UN PROBLEMA AL CONSULTAR AL SERVIDOR ' + response.message);
        });
        return deferred.promise;
    };
}]);