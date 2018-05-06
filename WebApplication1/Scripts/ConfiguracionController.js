//para acceder al scope -> angular.element('#DivPrincipal').scope()
// angular.element($0).scope()
// inspeccionar elemento y angular.element($0).scope().vm.DtoSel.

var myApp = angular.module('myApp', ['ui.bootstrap']);

myApp.controller('myCtrl', ['$scope', '$http', '$interval',
    function EventosController($scope, $http, $interval) {
        var vm = this;
        vm.Controller = window.location.pathname + "/";
        ///**FUNCIONES**///

        //vm.arduinos = [{ id:1, valor: "arduino1" }, { id: 2, valor: "arduino2" }];
        vm.arduinos = function () {
                $interval(function () {
                    $http.post('/Configuracion/arduinos')
                        .then(
                        function (successResponse) {
                            vm.arduinos = successResponse.data.Lista;
                        }
                        );
                }, 1000, 100
                );
        };

        vm.Buscar = function () {
            $interval(function () {
                $http.post('/Configuracion/Buscar')
                    .then(
                    function (successResponse) {
                        vm.Lista = successResponse.data.Lista;
                    }
                    );
            }, 1000, 100
            );
        };
    }
]);