$(async () => {
    await showExportList()
    await showWarehouseInputs()
    await showPositions()
    $('#btn_add').on('click', add)
    $('#btn_add_details').on('click', addDetails)
    $('.tickets').on('click', 'tr.table-primary', openModal)
    $('.details').on('click', '.btn-edit-detail,.btn-delete-detail', detailActions)
})
async function showWarehouseInputs() {
    const warehouses = await getWarehouses()
    $('#export_warehouse').empty()
    warehouses.forEach(wh => {
        $('#export_warehouse').append($(`<option value="${wh.Id}">${wh.WarehouseCode} - ${wh.WarehouseName}</option>`))
    })
}
async function showMaterials() {
    const materials = await getMaterials()
    $('#material').empty()
    materials.forEach(m => {
        $('#material').append($(`<option value="${m.Id}">${m.MaterialCode} - ${m.MaterialName}</option>`))
    })
}
async function showPositions() {
    const positions = await getPositions()
    $('#position').empty()
    positions.forEach(m => {
        $('#position').append($(`<option value="${m.Id}">${m.PositionCode} - ${m.PositionName}</option>`))
    })
}
async function showExportList() {
    let data = await getExportWarehouses()
    const warehouses = await getWarehouses()
    data = data.map(d => ({
        ...d,
        WarehouseName: warehouses.find(wh => wh.Id == d.WarehouseId)?.WarehouseName
    }))
    $('.tickets').empty()
    data.forEach(d => {
        $('.tickets').append(`
            <tr class="table-primary" style="cursor:pointer" data-id="${d.Id}">
                <td>${d.ExportCode}</td> 
                <td>${d.WarehouseName}</td> 
                <td>${d.ExportType == 1 ? '1 - Manual' : '2 - Automatic'}</td> 
            </tr>
        `)
    })
}
async function add() {
    const exportWarehouseId = +$('#export_warehouse').val()
    const actionType = +$('#action_type').val()
    const positionId = +$('#action_type').val()
    await postTicket(exportWarehouseId, actionType, positionId)
    await showExportList()
}
async function loadDetails(id) {
    $('.details').empty()
    let details = await getDetails(id)
    const materials = await getMaterials()
    const positions = await getPositions()
    details = details.map(d => ({
        ...d,
        MaterialName: materials.find(m => m.Id == d.MaterialId)?.MaterialName,
        PositionName: positions.find(p => p.Id == d.PositionId)?.PositionName,
    }))
    details.forEach(d => {
        const $row = $(`
            <tr class="table-primary" style="cursor:pointer" data-id="${d.Id}">
                <td>
                    <button class="btn btn-success btn-edit-detail" title="Save"><i class="bi bi-save-fill"></i></button>
                    <button class="btn btn-danger btn-delete-detail" title="Delete"><i class="bi bi-trash-fill"></i></button>
                </td>
                <td class="material-id">
                    <select class="form-control">
                        <option value="0">nờ ull null</option>
                    </select>
                </td> 
                <td class="material-quantity"><input type="number" class="form-control"></td> 
                <td class="position-id">
                    <select class="form-control">
                        <option value="0">nờ ull null</option>
                    </select>
                </td> 
            </tr>
        `)
        materials.forEach(m => {
            $row.find('.material-id select')
                .append(`<option value="${m.Id}" ${m.Id == d.MaterialId ? 'selected' : ''}>${m.MaterialName}</option>`)
        })
        $row.find('.material-quantity input').val(d.Quantity)
        positions.forEach(p => {
            $row.find('.position-id select')
                .append(`<option value="${p.Id}" ${p.Id == d.PositionId ? 'selected' : ''}>${p.PositionName}</option>`)
        })
        $('.details').append($row)
    })
}
async function openModal() {
    const id = $(this).attr('data-id')
    await showMaterials()
    await loadDetails(id)
    $('#modal_details').attr('data-id', id).modal('show')
}
async function addDetails() {
    const id = $('#modal_details').attr('data-id')
    const MaterialId = +$('#material').val()
    const Quantity = +$('#quantity').val()
    const PositionId = +$('#position').val()
    const result = await postDetails({ MaterialId, Quantity, PositionId, ExportWarehouseId: id })
    loadDetails(id)
}
async function detailActions() {
    if ($(this).hasClass('btn-edit-detail')) editDetail($(this).closest('tr'))
    else deleteDetail($(this).closest('tr'))
}
async function editDetail(row) {
    const Id = row.attr('data-id')
    const warehouseId = $('#modal_details').attr('data-id')
    const MaterialId = +row.find('.material-id select').val()
    const Quantity = +row.find('.material-quantity input').val()
    const PositionId = +row.find('.position-id select').val()
    const result = await postDetails({ Id, MaterialId, Quantity, PositionId, ExportWarehouseId: warehouseId })
    const ticketId = $('#modal_details').attr('data-id')
    loadDetails(ticketId)
}
async function deleteDetail(row) {
    const goAhead = confirm("Are you sure to delete this shiet?")
    if (!goAhead) return;
    const Id = row.attr('data-id')
    await deleteDetails(Id)
    row.remove()
}