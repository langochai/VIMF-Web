async function getAll() {
    try {
        const response = await fetch('/Areas/GetAll')
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}
async function save(data) {
    try {
        const response = await fetch('/Areas/Save', {
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
async function deleteArea(id) {
    try {
        const response = await fetch(`/Areas/Delete?id=${id}`, {
            method: 'DELETE',
        })
        return true
    }
    catch (e) {
        console.error(e)
        return false
    }
}
async function getWarehouse() {
    try {
        const response = await fetch('/Warehouse/GetAll')
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}