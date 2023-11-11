/* busquedas por modales y jquery */
/* Abrir Modal de buscar pagos por su codigo de contrato */
function abrirModalBuscarPago() {
    $('#modalBuscarPago').modal('show');
    $('#tblPagos').empty();
}

function buscarPagos() {
    var codigo = $('#txtBuscar').val();
    $.getJSON('/Pago/BuscarPagos', { codigo: codigo }, function (resultados) {
        $('#tblPagos').empty();
        $.each(resultados, function (index, resultado) {
            var fila = '<tr><td>' + resultado.idPago +
                '</td><td>' + resultado.fecha +
                '</td><td>' + resultado.importe +
                '</td><td>' + resultado.idContrato +
                '</td><td>' + resultado.numeroPago +
                '</td><td><a href="Pago/CreateModal/' + resultado.idPago + '"> <i class="bi bi-pencil-square" style="font-size: 2em; color: blue;"></i> </a>' +
                '</td></tr>';
            $('#tblPagos').append(fila);
        });
    });
}

// Función para cerrar el modal al hacer clic en el botón
function cerrarModalPago() {
    $('#modalBuscarPago').modal('hide');
}


/* Abrir modal buscar contratos por fecha y pagos */
function abrirModalBuscarContratos() {
    $('#modalBuscarContrato').modal('show');
    $('#tblContrato').empty();
}

function buscarContratosPor() {
    //var opcion = $('#buscarContratoPor').val();
    if ($('#Codigo').val()) {
        var codigo = $('#Codigo').val();
        $.getJSON('/Contrato/BuscarContratosPorCodigo', { codigo: codigo }, function (resultados) {
            $('#tblContrato').empty();
            $.each(resultados, function (index, resultado) {
                var fila = '<tr><td>' + resultado.idContrato +
                    '</td><td>' + resultado.idInmueble +
                    '</td><td>' + resultado.nombre + " " + resultado.apellido +
                    '</td><td>' + resultado.dni +
                    '</td><td>' + resultado.fechaInicio +
                    '</td><td>' + resultado.fechaFin +
                    '</td><td><button type="button" id="boton-general" class="btn btn-primary" onclick="seleccionarContrato(' + resultado.idContrato + ')">Seleccionar</button></td></tr>';
                $('#tblContrato').append(fila);
            });
        });
    }
    if ($('#fechaDesde').val() && $('#fechaHasta').val()) {
        var fechaDesde = $.datepicker.formatDate("yy-mm-dd", new Date($('#fechaDesde').val())) + " 00:00:00";
        var fechaHasta = $.datepicker.formatDate("yy-mm-dd", new Date($('#fechaHasta').val())) + " 00:00:00";
        $.getJSON('/Contrato/BuscarContratosPorFecha', { fechaDesde: fechaDesde, fechaHasta: fechaHasta }, function (resultados) {
            $('#tblContrato').empty();
            $.each(resultados, function (index, resultado) {
                var fila = '<tr><td>' + resultado.idContrato +
                    '</td><td>' + resultado.idInmueble +
                    '</td><td>' + resultado.nombre + " " + resultado.apellido +
                    '</td><td>' + resultado.dni +
                    '</td><td>' + resultado.fechaInicio +
                    '</td><td>' + resultado.fechaFin +
                    '</td><td><button type="button" id="boton-general" class="btn btn-primary" onclick="seleccionarContrato(' + resultado.idContrato + ')">Seleccionar</button></td></tr>';
                $('#tblContrato').append(fila);
            });
        });
    }
}

/* opciones del select de buscar contratos */
$(document).ready(function () {
    $(".optionContrato").change(function () {
        var selectedOption = $(this).children("option:selected").val();
        $(".optionInputContrato").empty();
        if (selectedOption == "Fecha") {
            $(".optionInputContrato").append('<label for="fechaDesde" class="input-group-text">Desde:</label>');
            $(".optionInputContrato").append('<input type="date" class="form-control" id="fechaDesde">');
            $(".optionInputContrato").append('<label for="fechaHasta" class="input-group-text">Hasta:</label>');
            $(".optionInputContrato").append('<input type="date" class="form-control" id="fechaHasta">');
        } else {
            $(".optionInputContrato").append('<label for="Codigo" class="input-group-text">Codigo: </label>');
            $(".optionInputContrato").append('<input type="number" class="form-control" id="Codigo">');
        }
    });
    // por defecto, agregar los inputs de fecha
    $(".optionInputContrato").append('<label for="fechaDesde" class="input-group-text">Desde:</label>');
    $(".optionInputContrato").append('<input type="date" class="form-control" id="fechaDesde">');
    $(".optionInputContrato").append('<label for="fechaHasta" class="input-group-text">Hasta:</label>');
    $(".optionInputContrato").append('<input type="date" class="form-control" id="fechaHasta">');
});


/* Abrir Modal de buscar inmuebles por disponibilidad, propietario y sin contrato */
function abrirModalBuscarInmueble() {
    $('#modalBuscarInmuebles').modal('show');
    $('#tblInmuebles').empty();
}

function buscarInmuebles() {
    var busqueda = $('#txtBuscar').val();
    var opcion = $('#buscarInmueblePor').val();
    if (opcion == "Disponibles" || opcion == "Propietario") {
        $.getJSON('/Inmueble/BuscarInmuebles', { busqueda: busqueda, opcion: opcion }, function (resultados) {
            $('#tblInmuebles').empty();
            $.each(resultados, function (index, resultado) {
                var fila = '<tr><td>' + resultado.idInmueble +
                    '</td><td>' + resultado.nombre + " " + resultado.apellido +
                    '</td><td>' + resultado.tipo +
                    '</td><td>' + resultado.direccion +
                    '</td><td>' + resultado.coordenadas +
                    '</td><td>' + resultado.precio +
                    '</td><td>' + resultado.ambientes +
                    '</td><td>' + resultado.uso +
                    '</td></tr>';
                $('#tblInmuebles').append(fila);
            });
        });
    }
    if(opcion == "InmuebleNotIn"){
        var fechaInicio = $.datepicker.formatDate("yy-mm-dd", new Date($('#fechaInicio').val())) + " 00:00:00";
        var fechaFin = $.datepicker.formatDate("yy-mm-dd", new Date($('#fechaFin').val())) + " 00:00:00";

        $.getJSON('/Inmueble/BuscarInmueblesSinContrato', { fechaInicio: fechaInicio, fechaFin: fechaFin }, function (resultados) {
            $('#tblInmuebles').empty();
            $.each(resultados, function (index, resultado) {
                var fila = '<tr><td>' + resultado.idInmueble +
                    '</td><td>' + resultado.nombre + " " + resultado.apellido +
                    '</td><td>' + resultado.tipo +
                    '</td><td>' + resultado.direccion +
                    '</td><td>' + resultado.coordenadas +
                    '</td><td>' + resultado.precio +
                    '</td><td>' + resultado.ambientes +
                    '</td><td>' + resultado.uso +
                    '</td></tr>';
                $('#tblInmuebles').append(fila);
            });
        });
    }

}

/* cargar input segun select de buscar inmueble */
$(document).ready(function () {
    $(".optionInmueble").change(function () {
        var selectedOption = $(this).children("option:selected").val();
        $(".optionInputInmueble").empty();
        if (selectedOption == "InmuebleNotIn") {
            $(".optionInputInmueble").append('<label for="fechaInicio" class="input-group-text">Inicio:</label>');
            $(".optionInputInmueble").append('<input type="date" class="form-control" id="fechaInicio">');
            $(".optionInputInmueble").append('<label for="fechaFin" class="input-group-text">Fin:</label>');
            $(".optionInputInmueble").append('<input type="date" class="form-control" id="fechaFin">');
        } else if (selectedOption == "Propietario") {
            $(".optionInputInmueble").append('<input type="text" id="txtBuscar" class="form-control" placeholder="Nombre" />');
        }
    });
});

/* Ocultar input text */
$(document).ready(function () {
    $(".optionInmueble").change(function () {
        var selectedOption = $(this).children("optionInmueble:selected").val();
        if (selectedOption == "Disponibles") {
            $("#txtBuscar").hide();
        }
        else {
            $("#txtBuscar").show();
        }
    });
    $(".optionInput").hide();
});

// Función para cerrar el modal al hacer clic en el botón
function cerrarModalBuscarInmueble() {
    $('#modalBuscarInmuebles').modal('hide');
}


/* Abrir Modal de buscar contrato */
function abrirModalContrato() {
    $('#modalBuscarContrato').modal('show');
    $('#tblContrato').empty();
}

function buscarContratos() {
    var busqueda = $('#txtBuscar').val();
    var opcion = $('#buscarContratoPor').val();
    $.getJSON('/Pago/BuscarContratos', { busqueda: busqueda, opcion: opcion }, function (resultados) {
        $('#tblContrato').empty();
        $.each(resultados, function (index, resultado) {
            var fila = '<tr><td>' + resultado.idContrato +
                '</td><td>' + resultado.idInmueble +
                '</td><td>' + resultado.nombre + " " + resultado.apellido +
                '</td><td>' + resultado.dni +
                '</td><td>' + resultado.fechaInicio +
                '</td><td>' + resultado.fechaFin +
                '</td><td><button type="button" id="boton-general" class="btn btn-primary" onclick="seleccionarContrato(' + resultado.idContrato + ')">Seleccionar</button></td></tr>';
            $('#tblContrato').append(fila);
        });
    });
}

function seleccionarContrato(idContrato) {
    $('#IdContrato').val(idContrato);
    $('#modalBuscarContrato').modal('hide');
}

// Función para cerrar el modal al hacer clic en el botón
function cerrarModalContrato() {
    $('#modalBuscarContrato').modal('hide');
}

/* Abrir Modal de buscar propietario */
function abrirModal() {
    $('#modalBuscarPropietarios').modal('show');
    $('#tblPropietarios').empty();
}

function buscarPropietarios() {
    var nombre = $('#txtBuscar').val();
    $.getJSON('/Inmueble/BuscarPropietarios', { busqueda: nombre }, function (resultados) {
        $('#tblPropietarios').empty();
        $.each(resultados, function (index, resultado) {
            var fila = '<tr><td>' + resultado.idPropietario +
                '</td><td>' + resultado.nombre +
                '</td><td>' + resultado.apellido +
                '</td><td>' + resultado.telefono +
                '</td><td>' + resultado.email +
                '</td><td><button type="button" id="boton-general" class="btn btn-primary" onclick="seleccionarPropietario(' + resultado.idPropietario + ')">Seleccionar</button></td></tr>';
            $('#tblPropietarios').append(fila);
        });
    });
}

function seleccionarPropietario(idPropietario) {
    $('#IdPropietario').val(idPropietario);
    $('#modalBuscarPropietarios').modal('hide');
}

// Función para cerrar el modal al hacer clic en el botón
function cerrarModal() {
    $('#modalBuscarPropietarios').modal('hide');
}

/* Abrir Modal de buscar inquilino */
function abrirModalInquilino() {
    $('#modalBuscarInquilinos').modal('show');
    $('#tblInquilinos').empty();
}

function buscarInquilino() {
    var nombre = $('#txtBuscar').val();
    $.getJSON('/Contrato/BuscarInquilinos', { busqueda: nombre }, function (resultados) {
        $('#tblInquilinos').empty();
        $.each(resultados, function (index, resultado) {
            var fila = '<tr><td>' + resultado.idInquilino +
                '</td><td>' + resultado.nombre +
                '</td><td>' + resultado.apellido +
                '</td><td>' + resultado.telefono +
                '</td><td>' + resultado.email +
                '</td><td><button type="button" id="boton-general" class="btn btn-primary" onclick="seleccionarInquilino(' + resultado.idInquilino + ')">Seleccionar</button></td></tr>';
            $('#tblInquilinos').append(fila);
        });
    });
}

function seleccionarInquilino(idInquilino) {
    $('#IdInquilino').val(idInquilino);
    $('#modalBuscarInquilinos').modal('hide');
}

// Función para cerrar el modal al hacer clic en el botón
function cerrarModalInquilino() {
    $('#modalBuscarInquilinos').modal('hide');
}

/* Abrir Modal de buscar inmuebles */
function abrirModalInmueble() {
    $('#modalBuscarInmuebles').modal('show');
    $('#tblInmuebles').empty();
}

function buscarInmueble() {
    var busqueda = $('#txtBuscar').val();
    var opcion = $('#buscarInmueblePor').val();
    $.getJSON('/Contrato/BuscarInmuebles', { busqueda: busqueda, opcion: opcion }, function (resultados) {
        $('#tblInmuebles').empty();
        $.each(resultados, function (index, resultado) {
            var fila = '<tr><td>' + resultado.idInmueble +
                '</td><td>' + resultado.tipo +
                '</td><td>' + resultado.coordenadas +
                '</td><td>' + resultado.precio +
                '</td><td>' + resultado.ambientes +
                '</td><td>' + resultado.uso +
                '</td><td><button type="button" id="boton-general" class="btn btn-primary" onclick="seleccionarInmueble(' + resultado.idInmueble + ')">Seleccionar</button></td></tr>';
            $('#tblInmuebles').append(fila);
        });
    });
}

function seleccionarInmueble(idInmueble) {
    $('#IdInmueble').val(idInmueble);
    $('#modalBuscarInmuebles').modal('hide');
}

// Función para cerrar el modal al hacer clic en el botón
function cerrarModalInmueble() {
    $('#modalBuscarInmuebles').modal('hide');
}


/* Mostrar modales de mensajes */
$(document).ready(function () {
    $('#modalOK').modal('show');
    setTimeout(function () {
        $('#modalOK').modal('hide');
    }, 6000);
});

$(document).ready(function () {
    $('#modalOK').on('hidden.bs.modal', function () {
        $(this).remove();
    });

    $('.close').click(function () {
        $('#modalOK').modal('hide');
    });
});

$(document).ready(function () {
    $('#modalMessage').modal('show');
    setTimeout(function () {
        $('#modalMessage').modal('hide');
    }, 6000);
});

$(document).ready(function () {
    $('#modalMessage').on('hidden.bs.modal', function () {
        $(this).remove();
    });

    $('.close').click(function () {
        $('#modalMessage').modal('hide');
    });
});

$(document).ready(function () {
    $('#modalError').modal('show');
    setTimeout(function () {
        $('#modalError').modal('hide');
    }, 6000);
});

$(document).ready(function () {
    $('#modalError').on('hidden.bs.modal', function () {
        $(this).remove();
    });

    $('.close').click(function () {
        $('#modalError').modal('hide');
    });
});

//vista previa de imagen usuario
function mostrarImagenSeleccionada(event) {
    var imagen = event.target.files[0];
    var reader = new FileReader();
    reader.onload = function (e) {
        var vistaPrevia = document.getElementById("vista-previa");
        var imagenPrevia = vistaPrevia.querySelector("#imagen-previa");
        imagenPrevia.src = e.target.result;
    }
    if (imagen) {
        reader.readAsDataURL(imagen);
    } else {
        var vistaPrevia = document.getElementById("vista-previa");
        var imagenPrevia = vistaPrevia.querySelector("#imagen-previa");
        imagenPrevia.src = "/img/default.webp";
    }
}

//menu de navegacion dropdown
$(document).ready(function () {
    $(".dropdown-hover").hover(
        function () {
            $('.dropdown-menu', this).stop(true, true).slideDown("fast");
            $(this).toggleClass('open');
        },
        function () {
            $('.dropdown-menu', this).stop(true, true).slideUp("fast");
            $(this).toggleClass('open');
        }
    );
});