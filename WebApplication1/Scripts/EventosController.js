var myApp = angular.module('myApp', ['ui.bootstrap']);


myApp.controller('myCtrl', ['$scope', '$http', '$interval',
    function EventosController($scope, $http, $interval) {

        var vm = this;

        ///**VARIABLES**///
        vm.AccionABMC = 'L';  // inicia mostrando el Listado
        //vm.DtoFiltro = {};    // dto con las opciones para buscar en grilla
        //vm.DtoFiltro.NumeroPaginaListado = 1;
        vm.Controller = window.location.pathname + "/";
        ///**FUNCIONES**///
        vm.Agregar = function () {
            vm.submitted = false;
            $scope.FormReg.$setUntouched()
            vm.AccionABMC = 'A';
            vm.DtoSel = {};
            vm.DtoSel.Domicilio = {};
            vm.DtoSel.Activo = 'SI';
        };

        //Buscar segun los filtros, establecidos en DtoFiltro
        vm.Buscar = function () {
            $interval(function () {
                $http.post('/Eventos/Buscar') //DtoFiltro: vm.DtoFiltro
                    .then(
                    function (successResponse) {
                        vm.Lista = successResponse.data.Lista;
                    }, 1000);

                //  console.log(successResponse.data.Salida);
            }
            );
        };

        //Obtengo un registro especifico según el Id
        vm.BuscarPorId = function (Dto, AccionABMC) {
            $http.post(vm.Controller + 'BuscarPorId', { sId: Dto.Id })
                .then(function (response) {
                    vm.DtoSel = response.data.Dto;
                    vm.AccionABMC = AccionABMC;
                });
        };

        vm.Consultar = function (Dto) {
            vm.BuscarPorId(Dto, 'C');
        };

        vm.Modificar = function (Dto) {
            if (Dto.Activo == 'NO') {
                py.Alert("No puede modificarse un registro Inactivo.");
                return;
            }
            vm.submitted = false;
            vm.BuscarPorId(Dto, 'M');
        };

        vm.Grabar = function () {
            //evento.Id_Arduino = 2;
            //evento.Id_Senal = 2;
            //evento.N_Valor = 2; 
            var a = "{Id_Arduino: 3, Id_Senal:3, N_Valor: 3}";
            $http.post('/Eventos/GuardarEvento', { cadenaArduino: JSON.stringify(a) })
                .then(function (successResponse) {
                    console.log(successResponse.data.Salida);
                });

        };
        vm.ActivarDesactivar = function (Dto) {
            py.ConfirmGeneral("Esta seguro de " + (Dto.Activo == 'SI' ? "desactivar" : "activar") + " este registro?", fun, null, "Confirmación", "Aceptar", "Cancelar")
            function fun() {
                $http.post(vm.Controller + 'ActivarDesactivar', { Dto: Dto }).then(function () {
                    vm.Cancelar();
                });
            }
        };

        vm.Cancelar = function () {
            vm.DtoSel = null;
            vm.submitted = false;  //usado en FormFiltro y FormReg
            if (vm.AccionABMC != 'C')
                vm.Buscar();  //despues de grabar o de activar/desactivar recuperamos nuevamente los datos del servidor
            vm.AccionABMC = 'L';
        };


        vm.VerHistorial = function (Titulo, IdTipoEntidad, Dto) {

            py.Grilla(Titulo, vm.Controller + 'VerHistorial', { IdTipoEntidad: IdTipoEntidad, sId: Dto.Id },
                function (rowData) { py.Grilla('Detalle', '@Url.Action("GetDetalle", "Permisos")', rowData, null); });

        };

        vm.VerHistorialDomicilio = function (Dto) {

            py.Grilla('Historial Domicilio', vm.Controller + 'VerHistorialDomicilio', { sId: Dto.Id },
                function (rowData) { py.Grilla('Detalle', '@Url.Action("GetDetalle", "Permisos")', rowData, null); });

        };



        vm.FormatearNombre = function (nombre) {
            var nombres = nombre.split(/(?=[A-Z])/); //separar PascalCasing o camelCasing 
            for (var i = 0, len = nombres.length; i < len; i++) {
                if (nombres.length > 1 && nombres[0] == 'Id' && nombres[i] == 'Id') nombres[i] = ''; //IdGrupoInfracion (eliminar palabra Id)
                else if (nombres[i] == "Codigo") nombres[i] = "Código";
                else if (nombres[i].slice(-4) == "cion") nombres[i] = nombres[i].substring(0, nombres[i].length - 4) + "ción";  //acentuar cion
            }
            return nombres.join(' ');
        };


        // cuando mostrar mostra error invalid
        vm.CampoError = function (campo) {
            if (!campo) return '';
            return ((vm.submitted || campo.$touched) && campo.$invalid);
        }
        //devuelve clase de error si el campo tiene error invalid
        vm.CampoErrorCss = function (campo) {
            return vm.CampoError(campo) ? 'has-error' : ''
        }

        // Cuando mostrar msj error: requerido
        vm.CampoReq = function (campo) {
            if (!campo) return false;
            return ((vm.submitted || campo.$touched) && campo.$error.required);
        }
        // Cuando mostrar msj error expresion regular
        vm.CampoReg = function (campo) {
            if (!campo) return false;
            return ((vm.submitted || campo.$touched) && campo.$error.pattern);
        }

        vm.ImprimirListado = function () {
            py.OpenWindowWithPost(vm.Controller + 'ImprimirListado', { titulo: $scope.MaestroNombre, DtoFiltro: vm.DtoFiltro }, 'Impresion', '_blank');
        }
        //vm.ImprimirListado = function () {
        //    py.OpenWindowWithPost(vm.Controller + 'ImprimirListado', { Lista: vm.Lista }, 'Impresion', '_blank');
        //};
        vm.ImprimirExcel = function () {
            if (vm.DtoFiltro == undefined || vm.DtoFiltro.Periodo == undefined || vm.DtoFiltro.Periodo == null) {
                py.Alert("Debe ingresar un Periodo");
            }
            else {
                py.OpenDownload(vm.Controller + 'ImprimirExcel', { DtoFiltro: vm.DtoFiltro, periodoExcel: vm.DtoFiltro.Periodo }, 'Impresion', '_blank');
            }
        }


    }
]);