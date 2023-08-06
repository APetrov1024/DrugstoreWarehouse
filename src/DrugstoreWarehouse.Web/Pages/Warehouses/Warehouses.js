"use strict";

let batchesTable = null;
let selectedWhId = null;

const batchesAppService = drugstoreWarehouse.batches.batches;
const warehousesAppService = drugstoreWarehouse.warehouses.warehouses;

const createUpdateWarehouseModal = new abp.ModalManager({
    viewUrl: abp.appPath + 'Warehouses/CreateUpdateWarehouseModal',
    modalClass: 'CreateUpdateWarehouseModal'
});

const createUpdateBatchModal = new abp.ModalManager({
    viewUrl: abp.appPath + 'Warehouses/CreateUpdateBatchModal',
    modalClass: 'CreateUpdateBatchModal'
});

document.getElementById('addWarehouseBtn').addEventListener('click', function () {
    createUpdateWarehouseModal.open();
});

document.getElementById('addBatchBtn').addEventListener('click', function () {
    createUpdateBatchModal.open({ warehouseId: selectedWhId })
});

createUpdateWarehouseModal.onResult(function (result) {
    if (result.actionType === 'create') {
        addWarehouseListItem(result);
    } else {
        var item = document.querySelector(`.master-details__master-list-item[data-id="${result.id}"]`);
        item.querySelector('.master-details__master-list-item__text').innerHTML = result.name;
    }
});

function addWarehouseListItem(data) {
    const whItem = document.createElement('div');
    whItem.classList.add('list-group-item', 'list-group-item-action', 'master-details__master-list-item')
    whItem.setAttribute('data-id', data.id);
    whItem.innerHTML = `<span class="master-details__master-list-item__text">${data.name}</span>
                        <span class="master-details__master-list-item__toolbar">
                            <span class="master-details__master-list-item__tool-btn master-details__master-list-item__edit-btn"><i class="fas fa-edit"></i></span>
                            <span class="master-details__master-list-item__tool-btn master-details__master-list-item__delete-btn"><i class="fas fa-trash"></i></span>
                        </span>`;
    document.querySelector('.master-details__master-list-container > .list-group').appendChild(whItem);
    addWarehouseListItemEventListeners(whItem);
}

function addWarehouseListItemEventListeners(item) {
    const id = item.getAttribute('data-id');

    item.addEventListener('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        const oldSelected = document.querySelector('.master-details__master-list-item.active')
        if (oldSelected) oldSelected.classList.remove('active');
        const newSelected = e.target.closest('.master-details__master-list-item');
        newSelected.classList.add('active');
        loadDetails(newSelected.getAttribute('data-id'))
    });
    item.querySelector('.master-details__master-list-item__edit-btn').addEventListener('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        createUpdateWarehouseModal.open({ warehouseId: id });
    });
    item.querySelector('.master-details__master-list-item__delete-btn').addEventListener('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        deleteWhItem(id)
    });
}

function deleteWhItem(id) {
    abp.message.confirm(L('Message:Warehouse:DeleteConfirmMessage'), L('Message:Warehouse:DeleteConfirmHeader'))
        .then(function (confirmed) {
            if (confirmed) {
                abp.ui.setBusy();
                warehousesAppService.delete(id)
                    .done(function () {
                        var item = document.querySelector(`.master-details__master-list-item[data-id="${id}"]`);
                        if (item.classList.contains('active')) {
                            selectedWhId = null;
                            batchesTable.destroy();
                            batchesTable = null;
                            document.querySelector('.select-wh-alert').classList.remove('hidden');
                            document.getElementById('addBatchBtn').setAttribute('disabled', true);

                        }
                        item.remove();
                        abp.ui.clearBusy();
                    })
                    .catch(err => handleError(err));
            }
        });
}

createUpdateBatchModal.onResult(function (result) {
    batchesTable.updateOrAddData([result]);
});

document.querySelectorAll('.master-details__master-list-item').forEach(x => addWarehouseListItemEventListeners(x));

function loadDetails(id) {
    selectedWhId = id;
    if (batchesTable === null) {
        // первоначальный выбор склада, инициализируем интерфейс для партий
        document.getElementById('addBatchBtn').removeAttribute('disabled');
        document.querySelector('.select-wh-alert').classList.add('hidden');
        createBatchesTable();
    }
    batchesTable.setData();
}

function createBatchesTable() {
    batchesTable = new Tabulator('#batchesTable', {
        height: calculateContentHeight(),
        layout: 'fitColumns',
        ajaxURL: true,
        ajaxRequestFunc: batchesQuery,
        pagination: true,
        paginationSize: 25,
        paginationSizeSelector: [25, 50, 100, 250, 500, 999],
        columns: [
            new ToolsColumn(
                [
                    { btnClass: 'btn-edit', iconClass: 'fas fa-edit' },
                    { btnClass: 'btn-delete', iconClass: 'fas fa-trash' },
                ],
                {
                    'btn-edit': editClicked,
                    'btn-delete': deleteClicked
                }
            ),
            { title: L('FieldName:Batch:ProductName'), field: 'productName', headerFilter: 'input' },
            { title: L('FieldName:Batch:Quantity'), field: 'quantity', headerFilter: 'number', width: '15%' },
        ],
    });

    batchesTable.on('rowDblClick', function (e, row) {
        let rowData = row.getData();
        createUpdateBatchModal.open({ warehouseId: selectedWhId, batchId: rowData.id })
    });
}

function batchesQuery(url, config, params) {
    return new Promise(function (resolve, reject) {
        batchesAppService.getList(selectedWhId)
            .done(function (result) {
                resolve(result);
            })
            .catch(err => handleError(err));
    });
}

function editClicked(cell) {
    let rowData = cell.getRow().getData();
    createUpdateBatchModal.open({ warehouseId: selectedWhId, batchId: rowData.id })
}

function deleteClicked(cell) {
    abp.message.confirm(L('Message:Batch:DeleteConfirmMessage'), L('Message:Batch:DeleteConfirmHeader'))
        .then(function (confirmed) {
            if (confirmed) {
                let row = cell.getRow();
                let id = row.getData().id;
                abp.ui.setBusy();
                batchesAppService.delete(id)
                    .done(function () {
                        abp.ui.clearBusy();
                        abp.notify.success(L('Message:Common:SuccessfullyDone'));
                        row.delete();
                    })
                    .catch(err => handleError(err));
            }
        });
}