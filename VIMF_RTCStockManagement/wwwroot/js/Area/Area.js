$(() => {
    loadData()
    $('#btn_add').on('click', add)
    $('#btn_save').on('click', modalSave)
});
async function loadData() {
    const datas = await getAll();
    datas.forEach(data => {
        const row = $(`<tr>
                            <td>
                                <a class="btn btn-info btn-icon btn-sm btn-edit">
                                    <i class="bi bi-pencil"></i>
                                </a>
                                <a class="btn btn-danger btn-icon btn-sm btn-delete">
                                    <i class="bi bi-trash"></i>
                                </a>
                            </td>
                            <td>${data.AreaCode ?? ''}</td>
                            <td>${data.AreaName ?? ''}</td>
                            <td>${data.Row ?? ''}</td>
                        </tr>`)
        row.find('.btn-edit').on('click', edit)
        row.find('.btn-delete').on('click', deleteData)
        row.data('data', data)
        $('#tbody_data').append(row)
    })
}
async function add() {
    $('#area_Id').val('')
    $('#area_code').val('')
    $('#area_name').val('')
    $('#row').val('')
    await loadWarehouse()
    $('#modal_area').modal('show')
}
async function edit() {
    await loadWarehouse()
    const data = $(this).closest('tr').data('data')
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
async function deleteData() {
    if (!confirm('Are you sure to delete this area?')) return;
    const data = $(this).closest('tr').data('data')
    await deleteArea(data.Id)
    $(this).closest('tr').remove()
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