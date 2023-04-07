var dataTable, listaEmpleados = [], listaRoles = [];
$(document).ready(function () {
    LLenarTabla();

    ObtenerRoles();


    $("#btnNuevo").on("click", function () {
        LevantaModal(1, 0);
    });

    $("#btnAgregar").on("click", function () {
        if ( $("#ddlRol").val() == '0')
        {
            alert( "Debe elegir un puesto para el empleado" );
            return false;
        }
        AgregarEmpleado();
    });

    $("#btnAgregarMovimiento").on("click", function () {
        if ($("#ddlMes_mov").val() == '0') {
            alert("Debe elegir un mes del movimiento");
            return false;
        } 
        if ($("#txtEntregas_mov").val() == '') {
            alert("Debe ingresar una cantidad de entregas");
            return false;
        }
        AgregarMovimiento()
    });

});

function LLenarTabla() {
    
    $.ajax({
        type: "GET",
        url: "/Empleados/GetEmpleados",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (!data.Error) {

                $("#dtEmpleados").dataTable().fnClearTable();
                $("#dtEmpleados").dataTable().fnDestroy();

                listaEmpleados = JSON.parse(data.Data);

                dataTable = $("#dtEmpleados").DataTable({
                    data: listaEmpleados,
                    columns: [
                        { "data": "Id" },
                        { "data": "Nombre" }, 
                        { "data": "Rol" }, 
                        {
                            "data": "Estatus", "render": function (data) {
                                return data  == true ? " Activo " : "Inactivo ";
                            },
                            "orderable": false,
                            "searchable": false,
                            "width": "120px"
                        },
                        {
                            "data": "Id", "render": function (data) {
                                return "<a class='btn btn-primary btn-sm' style='margin-left:5px'onclick=LevantaModalmoviento(" + data + ")><i class='fa fa-trash'></i> Capturar Movimiento</a> <a class='btn btn-default btn-sm' onclick='LevantaModal(2," + data + ")' ><i class='fa fa-pencil'></i> Editar</a>";
                            },
                            "orderable": false,
                            "searchable": false,
                            "width": "250px"
                        }
                    ],
                    columnDefss: [{
                        "targets": 0,
                        "visible": true
                    }],
                    "language": {

                        "emptyTable": "tabla vacia, por favor has click en boton <b>Nuevo</b> "
                    }

                });
            } else {
                alert("Error al consultar datos.");
                console.log(data.Data);
            }

        }

    });
}

function ObtenerRoles() {
    $.ajax({
        type: "GET",
        url: "/Empleados/GetRoles",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (!data.Error) {
                if (data.Data.length > 0) {

                    listaRoles = JSON.parse(data.Data);

                    $('#ddlRol').append('<option value="0">Seleccione</option>');
                    $.each(listaRoles, function (element, obj) {
                        $('#ddlRol').append('<option value="' + obj.Id + '">' + obj.Descripcion + '</option>');
                        $('#ddlRol_mov').append('<option value="' + obj.Id + '">' + obj.Descripcion + '</option>');
                    });
                    $('#ddlRol').val(0);
                } else {
                    $('#ddlRol').val(0);
                    swal("Información", "No se encontraron roles.", "info");
                }

            }
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            swal("Error", "Ocurrió un error al cargar la lista de Colaboradores.", "error");
        }

    });
}

function LevantaModal(opc, data) {

    LimpiarModal();

    if (opc == 2) {

        var objArray = listaEmpleados.filter(element => {
            return element["Id"] === data
        });
        var obj = objArray[0];

        $("#txtId").val(obj.Id);
        $("#txtNombre").val(obj.Nombre);
        $("#ddlRol").val(obj.IdRol);
        $("#chkActivo").prop('checked', obj.Estatus == true ? true : false);
        $("#btnAgregar").text("Actualizar");
        $(".modal-title").text("ACTUALIZAR EMPLEADO");
        $("#divId").show();
        $("#divEstatus").show();
    }

    $("#dvAgregar").modal("show");

}

function LevantaModalmoviento( data) {

    LimpiarModalMovimiento();

    var objArray = listaEmpleados.filter(element => {
        return element["Id"] === data
    });
    var obj = objArray[0];

    if (!obj.Estatus) {
        alert("No se puede capturar un movimiento a un empleado inactivo");
        return;
    }

    $("#txtId_mov").val(obj.Id);
    $("#txtNombre_mov").val(obj.Nombre);
    $("#ddlRol_mov").val(obj.IdRol); 
 

    $("#divCaptura").modal("show");

}

function LimpiarModalMovimiento() {
    $("#txtId_mov").val("0");
    $("#txtNombre_mov").val("");
    $("#ddlRol_mov").val("0");
    $("#ddlMes_mov").val("0");
    $("#txtEntregas_mov").val("");
}

function LimpiarModal() {
    $("#txtId").val("0");
    $("#txtNombre").val("");
    $("#ddlRol").val("0");
    $("#chkEstatus").prop('checked', false);
    $("#btnAgregar").text("Agregar");
    $(".modal-title").text("AGREGAR EMPLEADO");
    $("#divId").hide();
    $("#divEstatus").hide();
}

function AgregarEmpleado() {

    var datos = {
        Id: $("#txtId").val(),
        Nombre: $("#txtNombre").val(),
        Rol: $("#ddlRol").val(),
        Estatus: $("#chkActivo").is(':checked') ? true : false 
    };

    $.ajax({
        type: "POST",
        url: "/Empleados/PostEmpleado",
        data: JSON.stringify(datos),
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#dvAgregar").modal("hide");
        },
        success: function (data) {

            if (!data.Error) {

                var res = JSON.parse(data.Data);

                if (res.Id > 0) {
                    alert( datos.Nombre + " ha sido actualizado con éxito." );

                } else {
                    alert( datos.Nombre + " ha sido guardado con éxito." );
                }
                LLenarTabla();
            }
            else {
                alert("Error al realizar la accion.");
                console.log(data.Data);
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert( 'A ocurrido un error' + textStatus );
        }
    })
}

function AgregarMovimiento() {

    var datos = {
        Id: $("#txtId_mov").val(),
        Mes: $("#ddlMes_mov").val(),
        Entregas: $("#txtEntregas_mov").val()
    };

    $.ajax({
        type: "POST",
        url: "/Empleados/PostMovimiento",
        data: JSON.stringify(datos),
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#divCaptura").modal("hide");
        },
        success: function (data) {

            if (!data.Error) {

                var res = JSON.parse(data.Data);
                alert(res.Data);
                LimpiarModalMovimiento();

            }
            else {

                alert("Error al realizar la accion.");
                console.log(data.Data);
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert('A ocurrido un error' + textStatus);
        }
    })
}

function Delete(id) {

    if (confirm('¿Desea realmente eliminar este dato?')) {
        $.ajax({
            type: "POST",
            url: "/Empleados/Delete",
            async: true,
            data: JSON.stringify({ Id: id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (!data.Error) {

                    Swal.fire("Eliminado", "El registro ha sido eliminado con éxito.", "success");
                    LLenarTabla();
                } else {
                    Swal.fire("Error", "Ocurrio un error al eliminar el registro.", "error");
                    console.log(data.Data);
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                Swal.fire('Error', 'A ocurrido un error' + textStatus, 'error');
            }
        });
    }
}

function filterFloat(evt, input) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;
    if (key >= 48 && key <= 57) {
        if (filter(tempValue) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }
}

function filter(__val__) {
    var preg = /^([0-9]+\.?[0-9]{0,2})$/;
    if (preg.test(__val__) === true) {
        return true;
    } else {
        return false;
    }

}