"use strict";

const productsAppService = drugstoreWarehouse.products.products;

const readOnly = document.getElementById('ReadOnly')?.value === 'True';

const createUpdateProductModal = new abp.ModalManager({
    viewUrl: abp.appPath + 'Products/CreateUpdateProductModal',
    modalClass: 'CreateUpdateProductModal'
});

createUpdateProductModal.onResult(function (result) {
    productsTable.updateOrAddData([result]);
});

document.getElementById('addBtn')?.addEventListener('click', function () {
    createUpdateProductModal.open();
});

let productsTable;
(function createProductsTable() {
    const columns = [];
    console.log(!readOnly);
    if (!readOnly) {
        columns.push(
            new ToolsColumn(
                [
                    { btnClass: 'btn-edit', iconClass: 'fas fa-edit' },
                    { btnClass: 'btn-delete', iconClass: 'fas fa-trash' },
                ],
                {
                    'btn-edit': editClicked,
                    'btn-delete': deleteClicked,
                }
            ),
        );
    }
    columns.push({ title: L('FieldName:Product:Name'), field: 'name', headerFilter: 'input' });

    productsTable = new Tabulator('#productsTable', {
        height: calculateContentHeight(),
        layout: 'fitColumns',
        ajaxURL: true,
        ajaxRequestFunc: productsQuery,
        pagination: true,
        paginationSize: 25,
        paginationSizeSelector: [25, 50, 100, 250, 500, 999],
        columns: columns,
    });

    if (!readOnly) {
        productsTable.on('rowDblClick', function (e, row) {
            let rowData = row.getData();
            createUpdateProductModal.open({ productId: rowData.id });
        });
    }
})()


function productsQuery(url, config, params) {
    return new Promise(function (resolve, reject) {
        productsAppService.getList()
            .done(function (result) {
                resolve(result);
            })
            .catch(err => handleError(err));
    });
}

function editClicked(cell) {
    let rowData = cell.getRow().getData();
    createUpdateProductModal.open({ productId: rowData.id });
}

function deleteClicked(cell) {
    abp.message.confirm(L('Message:Product:DeleteConfirmMessage'), L('Message:Product:DeleteConfirmHeader') )
        .then(function (confirmed) {
            if (confirmed) {
                let row = cell.getRow();
                let id = row.getData().id;
                abp.ui.setBusy();
                productsAppService.delete(id)
                    .done(function () {
                        abp.ui.clearBusy();
                        abp.notify.success(L('Message:Common:SuccessfullyDone'));
                        row.delete();
                    })
                    .catch(err => handleError(err));
            }
        });

}