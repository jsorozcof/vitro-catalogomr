//var domain = document.location.origin;

//var app = angular.module("app", []);
//app.controller('atributoController', ['$scope', 'requestService', function ($scope, requestService) {
//    $scope.definiciones = [];
//    $scope.definicion = '';
//    $scope.atributo = '';

//    this.$onInit = function () {
//    }
//}]);
//app.service('requestService', ['$http', '$q', function ($http, $q) {
//    this.retrieve = function (url) {
//        var deferrer = $q.defer();
//        $http({
//            method: 'GET',
//            url: url
//        }).then(function success(response) {
//            deferrer.resolve(response.data);
//        }, function error() {
//            deferrer.reject(response);
//            alert('Ha ocurrido un problema al consultar al servidor');
//        });
//        return deferrer.promise;
//    };
//}]);
//app.directive('keyPressEnter', function () {
//    return {
//        link: function link(scope, el, attr) {
//            el.bind('keyup', function (event) {
//                if (event.key === 'Enter') {
//                    if (scope.atributo.length > 0 && el.val().length > 0) {
//                        scope.$apply(function () {
//                            var definiciones = Array.from(scope.definiciones);
//                            var index = definiciones.indexOf(el.val());
//                            console.log(index);
//                            if (index === -1) {
//                                definiciones.push(scope.definicion);
//                                scope.definiciones = definiciones;
//                            }
//                            el.val('');
//                        });
//                    } else {
//                        alert('No se seleccionado ningún atributo');
//                    }
//                }
//                event.preventDefault();
//            });
//        }
//    };
//});
//app.directive('atributoFinder', ['requestService', function (requestService) {
//    return {
//        link: function link(scope, el, attr) {
//            el.bind('click', function (event) {
//                scope.$apply(function () {
//                    scope.atributo = el.attr('data-path');
//                    var url = '';
//                    switch (scope.atributo) {
//                        case 'pais':
//                            url = domain + '/api/Container/Paises';
//                            break;
//                        case 'marca':
//                            url = domain + '/api/Container/Marcas';
//                            break;
//                        case 'modelo':
//                            url = domain + '/api/Container/Modelos';
//                            break;
//                        case 'tipo parte':
//                            url = domain + '/api/Container/TipoPartes';
//                            break;
//                        case 'tipo vidrio':
//                            url = domain + '/api/Container/TipoVidrios';
//                            break;
//                        case 'color':
//                            url = domain + '/api/Container/Colores';
//                            break;
//                        case 'mercado':
//                            url = domain + '/api/Container/Mercados';
//                            break;
//                    }
//                    console.log(url);
//                    requestService.retrieve(url).then(function (data) {
//                        scope.definiciones = data;
//                    });
//                });
//                event.preventDefault();
//            });
//        }
//    };
//}]);
var origin = document.location.origin;
$(document).ready(function () {
    var marcasControl = $("select[name='Referencia']");

    $("select[name='PaisSelect']").change(function () {
        var selected = this.value;
        marcasControl.find('option').remove();
        $.get(origin + '/api/Container/Marcas/' + selected).done(function (data) {
            for (i = 0; i < data.length; i++) {
                marcasControl.append($("<option/>", { value: data[i].MarcaId, text: data[i].Nombre }));
            }
        });
    });
});