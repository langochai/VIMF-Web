$(() => {
    loadData()
    $('#btn_add').on('click', add)
    $('#btn_save').on('click', modalSave)
});
var table
async function loadData() {
    let data = await getAll();
    const areas = await getAreas()
    data = data.map(d => ({
        ...d,
        AreaName: areas.find(a => a.Id == d.AreaId)?.AreaName,
        ActionHTML: `<a class="btn btn-info btn-icon btn-sm btn-edit"><i class="bi bi-pencil"></i></a>
                     <a class="btn btn-danger btn-icon btn-sm btn-delete"><i class="bi bi-trash"></i></a>`
    }))
    if (!table) {
        table = new Tabulator("#position_table", {
            data: data,
            layout: "fitColumns",
            movableRows: true,
            groupBy: "AreaName",
            columns: [
                { title: "Actions", field: "ActionHTML", formatter: "html", cellClick: cellClicked },
                { title: "Position Code", field: "PositionCode" },
                { title: "Position Name", field: "PositionName" },
            ],
        });
    } else {
        table.replaceData(data)
    }
}
async function add() {
    $('#position_id').val('')
    $('#position_code').val('')
    $('#position_name').val('')
    await loadAreas()
    $('#modal_position').modal('show')
}
async function edit(data) {
    await loadAreas()
    $('#position_id').val(data.Id ?? 0)
    $('#position_code').val(data.PositionCode ?? '')
    $('#position_name').val(data.PositionName ?? '')
    $('#area_id').val(data.AreaId ?? '')
    $('#modal_position').modal('show')
}
async function deleteData(data) {
    if (!confirm('Are you sure to delete this area?')) return;
    await deletePosition(data.Id)
    await loadData()
}
async function modalSave() {
    if (!Validate()) return alert('Please enter valid information');
    const area = {
        Id: +$('#position_id').val() ?? 0,
        PositionCode: $('#position_code').val(),
        PositionName: $('#position_name').val(),
        AreaId: +$('#area_id').val(),
    }
    await save(area)
    await loadData()
    $('#modal_position').modal('hide')
}
function Validate() {
    return ['#position_code', '#position_name', '#area_id'].every(selector => $(selector).val());
}
async function loadAreas() {
    const areas = await getAreas()
    $('#area_id').empty()
    $('#area_id').append(`<option value="" hidden selected disabled></option>`)
    areas.forEach(a => $('#area_id').append(`<option value="${a.Id}">${a.AreaName}</option>`))
}
function cellClicked(e, cell) {
    const index = cell.getRow().getPosition();
    const data = cell.getRow().getData();
    if (e.target == document.querySelectorAll(`.btn-edit`)[index - 1]) { edit(data) };
    if (e.target == document.querySelectorAll(`.btn-delete`)[index - 1]) { deleteData(data) };
}