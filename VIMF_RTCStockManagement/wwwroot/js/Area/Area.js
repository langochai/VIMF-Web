$(() => {
    loadData()
    $('#btn_add').on('click', add)
    $('#btn_save').on('click', modalSave)
});
var table
async function loadData() {
    let data = await getAll();
    const warehouses = await getWarehouse()
    data = data.map(d => ({
        ...d,
        WarehouseName: warehouses.find(a => a.Id == d.WarehouseId)?.WarehouseName,
        ActionHTML: `<a class="btn btn-info btn-icon btn-sm btn-edit"><i class="bi bi-pencil"></i></a>
                     <a class="btn btn-danger btn-icon btn-sm btn-delete"><i class="bi bi-trash"></i></a>`
    }))
    console.log(data);
    if (!table) {
        table = new Tabulator("#area_table", {
            data: data,
            layout: "fitColumns",
            movableRows: true,
            groupBy: "WarehouseName",
            columns: [
                { title: "Actions", field: "ActionHTML", formatter: "html", cellClick: cellClicked },
                { title: "Area Code", field: "AreaCode" },
                { title: "Area Name", field: "AreaName" },
                { title: "Row", field: "Row" },
            ],
        });
    } else {
        table.replaceData(data)
    }
}
async function add() {
    $('#area_Id').val('')
    $('#area_code').val('')
    $('#area_name').val('')
    $('#row').val('')
    await loadWarehouse()
    $('#modal_area').modal('show')
}
async function edit(data) {
    await loadWarehouse()
    $('#area_Id').val(data.Id ?? 0)
    $('#area_code').val(data.AreaCode ?? '')
    $('#area_name').val(data.AreaName ?? '')
    $('#row').val(data.Row ?? '')
    $('#warehouse_id').val(data.WarehouseId ?? '')
    $('#modal_area').modal('show')
}
async function modalSave() {
    if (!Validate()) return alert('Please enter valid information');
    const area = {
        Id: +$('#area_Id').val() ?? 0,
        AreaCode: $('#area_code').val(),
        AreaName: $('#area_name').val(),
        Row: +$('#row').val(),
        WarehouseId: +$('#warehouse_id').val(),
    }
    await save(area)
    $('#tbody_data').empty();
    await loadData()
    $('#modal_area').modal('hide')
}
async function deleteData(data) {
    if (!confirm('Are you sure to delete this area?')) return;
    await deleteArea(data.Id)
    await loadData()
}
function Validate() {
    return ['#area_code', '#area_name', '#row', '#warehouse_id'].every(selector => $(selector).val());
}
async function loadWarehouse() {
    const warehouse = await getWarehouse()
    $('#warehouse_id').empty()
    $('#warehouse_id').append(`<option value="" hidden selected disabled></option>`)
    warehouse.forEach(wh => $('#warehouse_id').append(`<option value="${wh.Id}">${wh.WarehouseName}</option>`))
}
function cellClicked(e, cell) {
    const index = cell.getRow().getPosition();
    const data = cell.getRow().getData();
    if (e.target == document.querySelectorAll(`.btn-edit`)[index - 1]) { edit(data) };
    if (e.target == document.querySelectorAll(`.btn-delete`)[index - 1]) { deleteData(data) };
}