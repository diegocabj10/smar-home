//para acceder al scope -> angular.element('#DivPrincipal').scope()
// angular.element($0).scope()
// inspeccionar elemento y angular.element($0).scope().vm.DtoSel.

var myApp = angular.module('myApp', ['ui.bootstrap']);

myApp.controller('myCtrl', ['$scope', '$http', '$interval',
    function EventosController($scope, $http, $interval) {
        var vm = this;
        vm.Controller = window.location.pathname + "/";
        ///**FUNCIONES**///

        vm.arduinos = [{ id: 1, valor: "1" }, { id: 2, valor: "2" }];

        vm.senal = [{ id: 1, valor: "LUZ" }, { id: 2, valor: "GAS" }];

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


        vm.Agregar_Delay = vm.arduinos.id(); vm, senal.id();;
    }
]);