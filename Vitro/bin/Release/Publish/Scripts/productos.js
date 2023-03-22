var origin = document.location.origin;

var app = angular.module('app', []);
app.controller('prodController', ['$scope', 'retrieveService', function ($scope, retrieveService) {
    $scope.marcas = new Array();
    $scope.modelos = new Array();
    $scope.tpartes = new Array();

    $scope.changePais = function () {
        var selected = $scope.paisSelected;
        if (selected) {
            retrieveService.retrieve(origin + '/api/Container/Marcas/' + selected).then(function (data) {
                $scope.marcas = data;
            });
        }
    };

    $scope.changeMarcas = function () {
        var selected = $scope.marcaSelected;
        if (selected) {
            retrieveService.retrieve(origin + '/api/Container/Modelos/' + selected).then(function (data) {
                $scope.modelos = data;
            });
        }
    };

    $scope.changeClasificacion = function () {
        var selected = $scope.clasificacionSelected
        if (selected) {
            retrieveService.retrieve(origin + '/api/Container/TipoPartes/' + selected).then(function (data) {
                $scope.tpartes = data;
            });
        }
    };

    this.$onInit = function () {
        var ps = angular.element("#_PS");
        var md = angular.element("#_MD");
        var mc = angular.element("#_MC");
        var tp = angular.element("#_TP");
        var cl = angular.element("#_CL");

        if (ps.length > 0) {
            $scope.paisSelected = ps.val();
        }
        if (mc.length > 0) {
            retrieveService.retrieve(origin + '/api/Container/Marcas/' + ps.val()).then(function (data) {
                $scope.marcas = data;
            });
            $scope.marcaSelected = mc.val();
        }
        if (md.length > 0) {
            retrieveService.retrieve(origin + '/api/Container/Modelos/' + mc.val()).then(function (data) {
                $scope.modelos = data;
            });
            $scope.modeloSelected = md.val();
        }
        if (cl.length > 0) {
            $scope.clasificacionSelected = cl.val();
        }
        if (tp.length > 0) {
            retrieveService.retrieve(origin + '/api/Container/TipoPartes/' + cl.val()).then(function (data) {
                $scope.tpartes = data;
            });
            $scope.tparteSelected = tp.val();
        }
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

$(document).ready(function () {
    $("input[name=Files]").change(function () {
        var container = $("#tblimg");
        container.empty();
        if (this.files.length) {
            var imgsizes = this.files.length;
            var header = $("<tr/>", {});
            var bodytbl = $("<tr/>", {});

            for (i = 0; i < imgsizes; i++) {
                var reader = new FileReader();
                header.append($("<th/>", { html: this.files[i].name, class: 'bg-teal fg-white text-upper' }));
                reader.onload = function (e) {
                    var imagen = $("<img/>", { src: e.target.result, class: 'img-fluid thumbnail' });
                    var tdtbl = $("<td/>").append(imagen);
                    bodytbl.append(tdtbl);
                };
                reader.readAsDataURL(this.files[i]);
            }

            container.append(header);
            container.append(bodytbl);
        }
    });
});