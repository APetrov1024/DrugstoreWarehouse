"use strict";

let productsTable = null;
let selectedDrugstoreId = null;

const readOnly = document.getElementById('ReadOnly')?.value === 'True';

const drugstoresAppService = drugstoreWarehouse.drugstores.drugstores;

const masterDetailsManager = new MasterDetailsManager({
    masterAppService: drugstoresAppService,
    onClick: onSelectDrugstore,
    onDelete: onDeleteDrugstore,
    onEditBtnClick: id => createUpdateDrugstoreModal.open({ drugstoreId: id }),
    deleteConfirmationMessage: {
        header: L('Message:Drugstore:DeleteConfirmHeader'),
        text: L('Message:Drugstore:DeleteConfirmMessage')
    },
    readOnly: readOnly,
});

const createUpdateDrugstoreModal = new abp.ModalManager({
    viewUrl: abp.appPath + 'Drugstores/CreateUpdateDrugstoreModal',
    modalClass: 'CreateUpdateDrugstoreModal'
});

document.getElementById('addBtn')?.addEventListener('click', function () {
    createUpdateDrugstoreModal.open();
});

createUpdateDrugstoreModal.onResult(function (result) {
    if (result.actionType === 'create') {
        masterDetailsManager.add(result.id, stringifyDrugstore(result));
    } else {
        masterDetailsManager.update(result.id, stringifyDrugstore(result));
    }
});

function stringifyDrugstore(drugstore) {
    var result = `${drugstore.name}`;
    if (drugstore.telNumber || drugstore.address) result += ' ('
    if (drugstore.telNumber) result += `${drugstore.telNumber}; `;
    if (drugstore.address) result += `${drugstore.address}`;
    if (drugstore.telNumber || drugstore.address) result += ')'
    return result
}

function onDeleteDrugstore(id, isActive) {
    if (isActive) {
        selectedDrugstoreId = null;
        productsTable.destroy();
        productsTable = null;
        document.querySelector('.select-drugstore-alert').classList.remove('hidden');
    }
}


function onSelectDrugstore(id) {
    selectedDrugstoreId = id;
    if (productsTable === null) {
        // первоначальный выбор аптеки, инициализируем интерфейс для товаров
        document.querySelector('.select-drugstore-alert').classList.add('hidden');
        createProductsTable();
    }
    productsTable.setData();
}

function createProductsTable() {
    productsTable = new Tabulator('#productsTable', {
        height: calculateContentHeight(),
        layout: 'fitColumns',
        ajaxURL: true,
        ajaxRequestFunc: productsQuery,
        pagination: true,
        paginationSize: 25,
        paginationSizeSelector: [25, 50, 100, 250, 500, 999],
        columns: [
            { title: L('FieldName:Drugstore:ProductName'), field: 'name', headerFilter: 'input' },
            { title: L('FieldName:Drugstore:Quantity'), field: 'quantity', headerFilter: 'number', width: '15%' },
        ],
    });
}

function productsQuery(url, config, params) {
    return new Promise(function (resolve, reject) {
        drugstoresAppService.getProducts(selectedDrugstoreId)
            .done(function (result) {
                resolve(result);
            })
            .catch(err => handleError(err));
    });
}