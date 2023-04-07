var dataTable, listaMovimentos = [] 
$(document).ready(function () {

    LLenarTabla();

});

function LLenarTabla() {

    $.ajax({
        type: "GET",
        url: "/Movimientos/GetMovimientos",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (!data.Error) {

                $("#dtEmpleados").dataTable().fnClearTable();
                $("#dtEmpleados").dataTable().fnDestroy();

                listaMovimentos = JSON.parse(data.Data);

                dataTable = $("#dtMovimientos").DataTable({
                    data: listaMovimentos,
                    //ordering: true,
                    columns: [ 
                        { "data": "Id" },
                        { "data": "IdEmpleado" },
                        { "data": "Nombre" },
                        { "data": "Rol" },
                        { "data": "Mes" },
                        { "data": "Entregas" },
                        { "data": "SueldoBruto" },
                        { "data": "Retenciones" },
                        { "data": "BonoDespensa" },
                        { "data": "SueldoNeto" },
                    ],
                    columnDefs: [{
                        "targets": 1,
                        "visible": false
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