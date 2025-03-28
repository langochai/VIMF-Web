$(async () => {
    try {
        await InitializeSignal({ RefreshData })
    } catch (e) {
        console.log(e)
    }
})
/**
 * Initialize signal to backend
 * @param {object} callbacks List of callbacks. Save each of them to a property
 * @returns {Promise<void>}
 */
function InitializeSignal(callbacks) {
    return new Promise((resolve, reject) => {
        let signalConn = new signalR.HubConnectionBuilder()
            .withUrl(`/notificationHub`)
            .withAutomaticReconnect()
            .build();

        signalConn.on("Refresh", (data) => {
            data = JSON.parse(data)
            const parser = new DOMParser();
            const insertXMLDoc = parser.parseFromString(data.Inserted, "text/xml");
            const deleteXMLDoc = parser.parseFromString(data.Deleted, "text/xml");
            const inserted = data.Inserted === null ? '' : xmlToObject(insertXMLDoc.documentElement);
            const deleted = data.Deleted === null ? '' : xmlToObject(deleteXMLDoc.documentElement);
            callbacks.RefreshData?.(data.NotificationType, inserted, deleted) // null properties will be omitted
        });
        signalConn.on("Error", (error) => {
            console.log(error)
        });
        signalConn.start().then(() => resolve()).catch(err => reject(err));
    })
}
function xmlToObject(xml) {
    const obj = {};
    if (xml.nodeType === Node.TEXT_NODE) {
        return xml.nodeValue.trim();
    }
    if (xml.attributes && xml.attributes.length > 0) {
        obj["@attributes"] = {};
        for (let attr of xml.attributes) {
            obj["@attributes"][attr.nodeName] = attr.nodeValue;
        }
    }
    for (let child of xml.childNodes) {
        if (child.nodeType === Node.TEXT_NODE) {
            const trimmedText = child.nodeValue.trim();
            if (trimmedText) return trimmedText;
        } else {
            const nodeName = child.nodeName;
            const nodeValue = xmlToObject(child);

            if (obj[nodeName] === undefined) {
                obj[nodeName] = nodeValue;
            } else {
                // If the node already exists, turn it into an array
                if (!Array.isArray(obj[nodeName])) {
                    obj[nodeName] = [obj[nodeName]];
                }
                obj[nodeName].push(nodeValue);
            }
        }
    }
    return (obj);
}
function RefreshData() {

}