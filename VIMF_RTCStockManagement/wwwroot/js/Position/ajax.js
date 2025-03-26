async function getAll() {
    try {
        const response = await fetch('/Position/GetAll')
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}
async function save(data) {
    try {
        const response = await fetch('/Position/Save', {
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
        console.log(e)
        //iziToast.error({
        //    title: e.Message,
        //    position: 'topRight'
        //})
        return {}
    }
}
async function deletePosition(id) {
    try {
        const response = await fetch(`/Position/Delete?id=${id}`, {
            method: 'DELETE',
        })
        return true
    }
    catch (e) {
        console.error(e)
        return false
    }
}
async function getAreas() {
    try {
        const response = await fetch('/Areas/GetAll')
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}