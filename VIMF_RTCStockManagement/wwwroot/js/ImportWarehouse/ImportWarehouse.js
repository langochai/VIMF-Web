$(async () => {
    await showWarehouseInputs()
    await showImportList()
    $('#btn_add').on('click', add)
    $('#btn_add_details').on('click', addDetails)
    $('.tickets').on('click', 'tr.table-primary', openModal)
    $('.details').on('click', '.btn-edit-detail,.btn-delete-detail', detailActions)
})
async function showWarehouseInputs() {
    const warehouses = await getWarehouses()
    $('#import_warehouse, #export_warehouse').empty()
    warehouses.forEach(wh => {
        $('#import_warehouse, #export_warehouse').append($(`<option value="${wh.Id}">${wh.WarehouseCode} - ${wh.WarehouseName}</option>`))
    })
}
async function showMaterials() {
    const materials = await getMaterials()
    $('#material').empty()
    materials.forEach(m => {
        $('#material').append($(`<option value="${m.Id}">${m.Id} : ${m.MaterialCode} - ${m.MaterialName}</option>`))
    })
}
async function showImportList() {
    const data = await getImportWarehouses()
    $('.tickets').empty()
    data.forEach(d => {
        $('.tickets').append(`
            <tr class="table-primary" style="cursor:pointer" data-id="${d.Id}">
                <td>${d.ImportCode}</td> 
                <td>${d.WarehouseId}</td> 
                <td>${d.ImportType == 1 ? '1 - Manual' : '2 - Automatic'}</td> 
            </tr>
        `)
    })
}
async function add() {
    const importWarehouseID = +$('#import_warehouse').val()
    const actionType = +$('#action_type').val()
    await postTicket(importWarehouseID, actionType)
    await showImportList()
}
async function loadDetails(id) {
    $('.details').empty()
    const details = await getDetails(id)
    details.forEach(d => {
        $('.details').append(`
            <tr class="table-primary" style="cursor:pointer" data-id="${d.Id}">
                <td>
                    <button class="btn btn-success btn-edit-detail"><i class="bi bi-pencil-fill"></i></button>
                    <button class="btn btn-danger btn-delete-detail"><i class="bi bi-trash-fill"></i></button>
                </td>
                <td class="material-id" contenteditable>${d.MaterialId}</td> 
                <td class="material-quantity" contenteditable>${d.Quantity}</td> 
            </tr>
        `)
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
    const result = await postDetails({ MaterialId, Quantity, ImportWarehouseId: id })
    $('.details').append(`
            <tr class="table-primary" style="cursor:pointer" data-id="${result.Id}">
                <td>
                    <button class="btn btn-success btn-edit-detail"><i class="bi bi-pencil-fill"></i></button>
                    <button class="btn btn-danger btn-delete-detail"><i class="bi bi-trash-fill"></i></button>
                </td>
                <td class="material-id" contenteditable>${result.MaterialId}</td> 
                <td class="material-quantity" contenteditable>${result.Quantity}</td> 
            </tr>
        `)
}
async function detailActions() {
    if ($(this).hasClass('btn-edit-detail')) editDetail($(this).closest('tr'))
    else deleteDetail($(this).closest('tr'))
}
async function editDetail(row) {
    const Id = row.attr('data-id')
    const warehouseId = $('#modal_details').attr('data-id')
    const MaterialId = row.find('.material-id').text()
    const Quantity = row.find('.material-quantity').text()
    const result = await postDetails({ Id, MaterialId, Quantity, ImportWarehouseId: warehouseId })
    row.find('.material-id').text(result.MaterialId)
    row.find('.material-quantity').text(result.Quantity)
}
async function deleteDetail(row) {
    const goAhead = confirm("Are you sure to delete this shiet?")
    if (!goAhead) return;
    const Id = row.attr('data-id')
    await deleteDetails(Id)
    row.remove()
}