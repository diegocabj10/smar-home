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

        vm.Guardar = function () {

            List < String > lista = new List<string>();
            DataRow fila;
            Datatable tabla = new Datatable();

            String Linea = vm.Dto.V_TextoCompleto;

            while ((linea != null))
            {
                lista.add(linea);
                linea = fic.ReadLine();
            }


            $http.post('/Inquilinos/Guardar', { DtoInquilino: vm.Dto })
                .then(function (response) { alert("Se inserto con exito"); }
                ); //Esto apunta directamente al controller con el mismo nombre de metodo, fijate que recibe un parametro DtoConf, es el que estas pasando y solo resuelve el bindeo
        };
    }
]);