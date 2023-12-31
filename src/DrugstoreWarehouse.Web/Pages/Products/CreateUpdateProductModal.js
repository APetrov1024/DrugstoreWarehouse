"use strict";

abp.modals.CreateUpdateProductModal = function () {

   // const L = abp.localization.localize;
    function initModal(modalManager, args) {
        const productsAppService = drugstoreWarehouse.products.products;

        $('.modal-dialog button:submit').on('click', function submitClicked(e) {
            e.preventDefault();
            submitData(modalManager, args.productId);
        });

        function submitData(modalManager, productId) {
            let dto = {
                name: document.getElementById('VM_Name').value,
            };

            if (validateDto(dto)) {
                if (productId) {
                    productsAppService.update(productId, dto)
                        .done(response => onDataSubmited(response, modalManager))
                        .catch(err => handleError(err));
                }
                else {
                    productsAppService.create(dto)
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