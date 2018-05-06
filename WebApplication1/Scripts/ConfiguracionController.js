//para acceder al scope -> angular.element('#DivPrincipal').scope()
// angular.element($0).scope()
// inspeccionar elemento y angular.element($0).scope().vm.DtoSel.

var myApp = angular.module('myApp', ['ui.bootstrap']);

myApp.controller('myCtrl', ['$scope', '$http', '$interval',
    function EventosController($scope, $http, $interval) {
        var vm = this;
        vm.Controller = window.location.pathname + "/";
        vm.guardado = false;
        ///**FUNCIONES**///
        vm.Dto = {};
        vm.arduinos = [{ id: 1, valor: "1" }, { id: 2, valor: "2" }];

        vm.senales = [{ id: 1, valor: "LUZ" }, { id: 2, valor: "GAS" }];


        vm.Guardar = function () {
        
            $http.post('/Configuracion/Guardar', { DtoConfig: vm.Dto }); //Esto apunta directamente al controller con el mismo nombre de metodo, fijate que recibe un parametro DtoConf, es el que estas pasando y solo resuelve el bindeo
        };
    }
]);