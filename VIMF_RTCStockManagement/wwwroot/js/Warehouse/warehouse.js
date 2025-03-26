$(() => {
    GetAll();
});
var lstWarehouse;
function GetAll() {
    $.ajax({
        url: '/Warehouse/GetAll',
        type: 'GET',
        success: function (response) {
            lstWarehouse = response;
            var html = ``;
            $.each(lstWarehouse, (index, item) => {
                html +=`<tr>
                            <td>
                                <a class="btn btn-info btn-icon btn-sm" onclick="Edit(${item.Id})">
                                    <i class="bi bi-pencil"></i>
                                </a>
                                <a class="btn btn-danger btn-icon btn-sm" onclick="Delete(${item.Id})">
                                    <i class="bi bi-trash"></i>
                                </a>
                            </td>
                            <td>${item.WarehouseCode}</td>
                            <td>${item.WarehouseName}</td>
                        </tr>`
            });
            $('#tbodyWarehouse').html(html);
        }
    });
}

function Add() {
    $('#warehouseID').val(0);
    $('#warehouseCode').val('');
    $('#warehouseName').val('');
    $('#modal_warehouse').modal('show');
}

function Edit(id) {
    var warehouse = lstWarehouse.find(x => x.Id == id);
    $('#warehouseID').val(id);
    $('#warehouseCode').val(warehouse.WarehouseCode);
    $('#warehouseName').val(warehouse.WarehouseName);
    $('#modal_warehouse').modal('show');
}

function validate() {

    if ($('#warehouseCode').val() == '') {
        alert('Please enter warehouse code');
        return false;
    }
    if ($('#warehouseName').val() == '') {
        alert('Please enter warehouse name');
        return false;
    }

    return true;
}

function Save() {
    if (!validate()) return;

    var data = {
        WarehouseCode: $('#warehouseCode').val(),
        WarehouseName: $('#warehouseName').val(),
        FactoryId: 0,
        Id: parseInt($('#warehouseID').val())
    };

    $.ajax({
        url: '/Warehouse/Save',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json',
        success: function (response)
        {
            GetAll();
            $('#modal_warehouse').modal('hide');
        }
    });
}

function Delete(id) {
    $.ajax({
        url: `/Warehouse/Delete?id=${id}`,
        type: 'Delete',
        success: function (response) {
            GetAll();
        }
    });
}