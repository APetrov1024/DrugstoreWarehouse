"use strict";

let batchesTable = null;
let selectedWhId = null;

const batchesAppService = drugstoreWarehouse.batches.batches;
const warehousesAppService = drugstoreWarehouse.warehouses.warehouses;

const masterDetailsManager = new MasterDetailsManager({
    masterAppService: warehousesAppService,
    onClick: onSelectWh,
    onDelete: onDeleteWh,
    onEditBtnClick: id => createUpdateWarehouseModal.open({ warehouseId: id }),
    deleteConfirmationMessage: {
        header: L('Message:Warehouse:DeleteConfirmHeader'),
        text: L('Message:Warehouse:DeleteConfirmMessage')
    },
});

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
        masterDetailsManager.add(result.id, result.name);
    } else {
        masterDetailsManager.update(result.id, result.name);
    }
});

createUpdateBatchModal.onResult(function (result) {
    batchesTable.updateOrAddData([result]);
});

function onDeleteWh(id, isActive) {
    if (isActive) {
        selectedWhId = null;
         batchesTable.destroy();
         batchesTable = null;
         document.querySelector('.select-wh-alert').classList.remove('hidden');
         document.getElementById('addBatchBtn').setAttribute('disabled', true);
    }
}

function onSelectWh(id) {
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
                    'btn-edit': editBatchClicked,
                    'btn-delete': deleteBatchClicked
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

function editBatchClicked(cell) {
    let rowData = cell.getRow().getData();
    createUpdateBatchModal.open({ warehouseId: selectedWhId, batchId: rowData.id })
}

function deleteBatchClicked(cell) {
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