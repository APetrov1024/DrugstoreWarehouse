"use strict";

abp.modals.CreateUpdateDrugstoreModal = function () {

    function initModal(modalManager, args) {
        const drugstoresAppService = drugstoreWarehouse.drugstores.drugstores;

        $('.modal-dialog button:submit').on('click', function submitClicked(e) {
            e.preventDefault();
            submitData(modalManager, args.drugstoreId);
        });

        function submitData(modalManager, drugstoreId) {
            let dto = {
                name: document.getElementById('VM_Name').value,
                address: document.getElementById('VM_Address').value,
                telNumber: document.getElementById('VM_TelNumber').value,
            };

            if (validateDto(dto)) {
                if (drugstoreId) {
                    drugstoresAppService.update(drugstoreId, dto)
                        .done(response => onDataSubmited(response, modalManager, 'update'))
                        .catch(err => handleError(err));
                }
                else {
                    drugstoresAppService.create(dto)
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