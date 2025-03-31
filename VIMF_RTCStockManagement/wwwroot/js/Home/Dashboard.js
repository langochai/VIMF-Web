$(async () => {
    try {
        await InitializeSignal({ RefreshData })
    } catch (e) {
        console.log(e)
    }
    var positionGa = await LoadPosition(1);
    // Đảo ngược thứ tự của danh sách position
    positionGa.reverse();

    let html = `<div class="row">`;

    positionGa.forEach(position => {
        html += `
       <div class="col-6 p-3">
           <div class="frame-cell border h-100">
               <div class="frame-cell-inner row text-center" id="position_${position.ID}">
                   <div></div>
                   <div></div>
                   <div>${position.PositionCode}</div>
               </div>
           </div>
       </div>
    `;
    });

    html += `</div>`;
    $('#warehouse_ga').html(html);


    var positionModula = await LoadPosition(2);

    // Nhóm các position theo hàng (giả sử position.Row là số thứ tự hàng)
    const rows = {};
    positionModula.forEach(position => {
        const rowKey = position.Row;
        if (!rows[rowKey]) {
            rows[rowKey] = [];
        }
        rows[rowKey].push(position);
    });

    html = '';

    // Lấy các key (row) và sắp xếp theo thứ tự giảm dần
    const sortedRows = Object.keys(rows).sort((a, b) => b - a);

    sortedRows.forEach(rowKey => {
        const positionsInRow = rows[rowKey];
        const count = positionsInRow.length;
        // Tính số cột: nếu 12 chia hết cho count thì dùng col-{12/count} 
        // ngược lại dùng col để tự động phân chia đều
        let colClass = (12 % count === 0) ? `col-${12 / count}` : 'col';

        html += `<div class="row">`;
        positionsInRow.forEach(position => {
            html += `
            <div class="${colClass} p-3">
                <div class="frame-cell border h-100">
                    <div class="frame-cell-inner row text-center" id="position_${position.ID}">
                        <div></div>
                        <div></div>
                        <div>${position.PositionCode}</div>
                    </div>
                </div>
            </div>
        `;
        });
        html += `</div>`;
    });

    $('#warehouse_modula').html(html);

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
function RefreshData(type, inserted, deleted) {

}

async function LoadPosition(warehouseId) {
    try {
        const response = await fetch(`/Home/GetPositionWarehouse?warehouseID=${warehouseId}`)
        return await response.json();
    }
    catch (e) {
        console.error(e)
        return []
    }
}