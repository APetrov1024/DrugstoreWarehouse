"use strict";

abp.modals.CreateUpdateWarehouseModal = function () {

    function initModal(modalManager, args) {
        const warehousesAppService = drugstoreWarehouse.warehouses.warehouses;

        $('.modal-dialog button:submit').on('click', function submitClicked(e) {
            e.preventDefault();
            submitData(modalManager, args.warehouseId);
        });

        function submitData(modalManager, warehouseId) {
            let dto = {
                name: document.getElementById('VM_Name').value,
                drugstoreId: document.getElementById('VM_DrugstoreId').value,
            };

            if (validateDto(dto)) {
                if (warehouseId) {
                    warehousesAppService.update(warehouseId, dto)
                        .done(response => onDataSubmited(response, modalManager, 'update'))
                        .catch(err => handleError(err));
                }
                else {
                    warehousesAppService.create(dto)
                        .done(response => onDataSubmited(response, modalManager, 'create'))
                        .catch(err => handleError(err));
                }
            }
        }

        function validateDto(dto) {
            if (!dto.name || dto.name === '') {
                abp.message.error(L('Errors:FieldEmpty:Name'));
                return false;
            }
            return true;
        }

        function onDataSubmited(response, modalManager, actionType) {
            abp.ui.clearBusy();
            abp.notify.success(L('Message:Common:SuccessfullyDone'));
            const result = response;
            result.actionType = actionType;
            modalManager.setResult(result);
            modalManager.close();
        }
    }

    return {
        initModal: initModal
    };


};