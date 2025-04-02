async function getWarehouses() {
    try {
        const response = await fetch('/Warehouse/GetAll')
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}
async function getExportWarehouses() {
    try {
        const response = await fetch('/ExportWarehouse/GetAll')
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}
async function postTicket(exportId, actionType, positionId) {
    try {
        const response = await fetch('/ExportWarehouse/Create', {
            method: 'POST',
            headers: {
                //'Accept': 'application/json',
                'Content-Type': 'application/x-www-form-urlencoded;'
            },
            body: new URLSearchParams({ exportId, actionType, positionId })
        })
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}
async function getMaterials() {
    try {
        const response = await fetch('/Material/GetAll')
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}
async function getPositions() {
    try {
        const response = await fetch('/Position/GetAll')
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}
async function getDetails(id) {
    try {
        const response = await fetch(`/ExportWarehouse/GetTicket?id=${id}`)
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}
async function postDetails(data) {
    try {
        const response = await fetch('/ExportWarehouse/SaveDetails', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return {}
    }
}
async function deleteDetails(id) {
    try {
        const response = await fetch(`/ExportWarehouse/DeleteDetails?id=${id}`, {
            method: 'DELETE',
        })
        return true
    }
    catch (e) {
        console.error(e)
        return false
    }
}