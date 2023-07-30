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
                drugstoreId
            };

            if (validateDto(dto)) {
                if (productId) {
                    warehousesAppService.update(warehouseId, dto)
                        .done(response => onDataSubmited(response, modalManager))
                        .catch(err => handleError(err));
                }
                else {
                    warehousesAppService.create(dto)
                        .done(response => onDataSubmited(response, modalManager))
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

        function onDataSubmited(result, modalManager) {
            abp.ui.clearBusy();
            abp.notify.success(L('Message:Common:SuccessfullyDone'));
            modalManager.setResult(result);
            modalManager.close();
        }
    }

    return {
        initModal: initModal
    };


};