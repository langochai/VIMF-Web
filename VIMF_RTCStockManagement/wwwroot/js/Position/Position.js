$(() => {
    loadData()
    $('#btn_add').on('click', add)
    $('#btn_save').on('click', modalSave)
});
async function loadData() {
    let datas = await getAll();
    const areas = await getAreas()
    datas = datas.map(data => ({
        ...data,
        AreaName: areas.find(a => a.Id == data.AreaId)?.AreaName
    }))
    const grouped = Object.groupBy(datas, ({ AreaName }) => AreaName)
    $('#tbody_data').empty()
    for (let areaName in grouped) {
        console.log(areaName);
        const table = $(`
        <tr>
            <td colspan='3'>
                <table class="table mb-0">
                <thead>
                <th>
                <td colspan='3'>${areaName}</td>
                <th>
                </thead>
                    <tbody>
                    </tbody>
                </table>
            </td>
        </tr>
        `)
        grouped[areaName].forEach(postion => {
            const row = $(`
                <tr>
                    <td>
                        <a class="btn btn-info btn-icon btn-sm btn-edit">
                            <i class="bi bi-pencil"></i>
                        </a>
                        <a class="btn btn-danger btn-icon btn-sm btn-delete">
                            <i class="bi bi-trash"></i>
                        </a>
                    </td>
                    <td>${postion.PositionCode ?? ''}</td>
                    <td>${postion.PositionName ?? ''}</td>
                </tr>
            `)
            row.find('.btn-edit').on('click', edit)
            row.find('.btn-delete').on('click', deleteData)
            row.data('data', postion)
            table.find('tbody').append(row)
        })
        $('#tbody_data').append(table)
    }
}
async function add() {
    $('#position_id').val('')
    $('#position_code').val('')
    $('#position_name').val('')
    await loadAreas()
    $('#modal_position').modal('show')
}
async function edit() {
    await loadAreas()
    const data = $(this).closest('tr').data('data')
    $('#position_id').val(data.Id ?? 0)
    $('#position_code').val(data.PositionCode ?? '')
    $('#position_name').val(data.PositionName ?? '')
    $('#area_id').val(data.AreaId ?? '')
    $('#modal_position').modal('show')
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
async function deleteData() {
    if (!confirm('Are you sure to delete this area?')) return;
    const data = $(this).closest('tr').data('data')
    await deletePosition(data.Id)
    $(this).closest('tr').remove()
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