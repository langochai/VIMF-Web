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
async function getImportWarehouses() {
    try {
        const response = await fetch('/ImportWarehouse/GetAll')
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}
async function postTicket(importId, actionType) {
    try {
        const response = await fetch('/ImportWarehouse/Create', {
            method: 'POST',
            headers: {
                //'Accept': 'application/json',
                'Content-Type': 'application/x-www-form-urlencoded;'
            },
            body: new URLSearchParams({ importId, actionType })
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
async function getDetails(id) {
    try {
        const response = await fetch(`/ImportWarehouse/GetTicket?id=${id}`)
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}
async function postDetails(data) {
    try {
        const response = await fetch('/ImportWarehouse/SaveDetails', {
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
        const response = await fetch(`/ImportWarehouse/DeleteDetails?id=${id}`, {
            method: 'DELETE',
        })
        return true
    }
    catch (e) {
        console.error(e)
        return false
    }
}