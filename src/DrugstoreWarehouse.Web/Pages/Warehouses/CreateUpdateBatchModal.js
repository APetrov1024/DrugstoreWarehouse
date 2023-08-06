"use strict";

abp.modals.CreateUpdateBatchModal = function () {

    function initModal(modalManager, args) {
        const batchesAppService = drugstoreWarehouse.batches.batches;

        $('.modal-dialog button:submit').on('click', function submitClicked(e) {
            e.preventDefault();
            submitData(modalManager, args.warehouseId, args.batchId);
        });

        function submitData(modalManager, warehouseId, batchId) {
            let dto = {
                quantity: document.getElementById('VM_Quantity').value,
                productId: document.getElementById('VM_ProductId').value,
                warehouseId: warehouseId,
            };

            if (validateDto(dto)) {
                if (batchId) {
                    batchesAppService.update(batchId, dto)
                        .done(response => onDataSubmited(response, modalManager))
                        .catch(err => handleError(err));
                }
                else {
                    batchesAppService.create(dto)
                        .done(response => onDataSubmited(response, modalManager))
                        .catch(err => handleError(err));
                }
            }
        }

        function validateDto(dto) {
            if (!dto.quantity) {
                abp.message.error(L('Errors:FieldEmpty:Quantity_Int_Positive'));
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