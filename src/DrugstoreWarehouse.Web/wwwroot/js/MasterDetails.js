class MasterDetailsManager {
    #selectedId = null;
    #masterListElement = null;
    #detailsContainer = null;
    #deleteConfirmationMessage = { header: "Are you sure?", text: '', };
    #masterAppService = null;
    #renderItem = this.#renderItem_default;
    #onDelete = (id, isActive) => { };
    #onClick = (id) => { };
    #onEditBtnClick = (id) => { };
    #readOnly = false;


    constructor(options) {
        this.#masterListElement = document.querySelector('.master-details__master-list-container > .list-group');
        if (!this.#masterListElement) throw new Error('Master list not found');
        this.#detailsContainer = document.querySelector('.master-details__details-container')
        if (!this.#detailsContainer) throw new Error('Details container not found');

        const contentHeight = calculateContentHeight();
        document.querySelector('.master-details__master-list-container').setAttribute('style', `height:${contentHeight}px;  max-height:${contentHeight}`);
        document.querySelector('.master-details__details-container').setAttribute('style', `height:${contentHeight}px; max-height:${contentHeight}`);

        if (options.deleteConfirmationMessage.header) this.#deleteConfirmationMessage.header = options.deleteConfirmationMessage.header;
        if (options.deleteConfirmationMessage.text) this.#deleteConfirmationMessage.header = options.deleteConfirmationMessage.text;

        this.#masterAppService = options.masterAppService;
        if (!this.#masterAppService) throw new Error("Master application service must be set");

        if (typeof (options.renderItem) === 'function') this.#renderItem = options.renderItem;
        if (typeof (options.onClick) === 'function') this.#onClick = options.onClick;
        if (typeof (options.onDelete) === 'function') this.#onDelete = options.onDelete;
        if (typeof (options.onEditBtnClick) === 'function') this.#onEditBtnClick = options.onEditBtnClick;
        this.#readOnly = !!(options.readOnly);

        this.#initExistingListItems();
    }

    #initExistingListItems() {
        this.#masterListElement.querySelectorAll('.master-details__master-list-item').forEach(x => this.#addItemEventHandlers(x));
    }

    get selectedId() {
        return this.#selectedId;
    }

    #renderItem_default(id, text) {
        const item = document.createElement('div');
        item.classList.add('list-group-item', 'list-group-item-action', 'master-details__master-list-item')
        item.setAttribute('data-id', id);
        item.innerHTML = `<span class="master-details__master-list-item__text">${text}</span>
                          <span class="master-details__master-list-item__toolbar">`
        if (!this.#readOnly) {
            item.innerHTML += `<span class="master-details__master-list-item__tool-btn master-details__master-list-item__edit-btn"><i class="fas fa-edit"></i></span>
                              <span class="master-details__master-list-item__tool-btn master-details__master-list-item__delete-btn"><i class="fas fa-trash"></i></span>`;
        }
        item.innerHTML += `</span>`;
        this.#masterListElement.appendChild(item);
        return item;
    }

    add(id, text) {
        if (this.#readOnly) {
            console.warn('You are not allowed to perform this operation!')
            return;
        }
        const item = this.#renderItem(id, text);
        this.#masterListElement.appendChild(item);
        this.#addItemEventHandlers(item);
        return item;
    }

    #addItemEventHandlers(item) {
        const id = item.getAttribute('data-id');

        item.addEventListener('click', e => {
            e.preventDefault();
            e.stopPropagation();
            const oldSelected = this.#masterListElement.querySelector('.master-details__master-list-item.active')
            if (oldSelected) oldSelected.classList.remove('active');
            const newSelected = e.target.closest('.master-details__master-list-item');
            newSelected.classList.add('active');
            this.#selectedId = id;
            this.#onClick(id);
        });
        item.querySelector('.master-details__master-list-item__edit-btn')?.addEventListener('click', e => {
            e.preventDefault();
            e.stopPropagation();
            this.#onEditBtnClick(id)
        });
        item.querySelector('.master-details__master-list-item__delete-btn')?.addEventListener('click', e => {
            e.preventDefault();
            e.stopPropagation();
            this.delete(id);
        });
    }

    update(id, text) {
        if (this.#readOnly) {
            console.warn('You are not allowed to perform this operation!')
            return;
        }
        const item = this.#masterListElement.querySelector(`.master-details__master-list-item[data-id="${id}"]`);
        item.querySelector('.master-details__master-list-item__text').innerHTML = text;
    }

    delete(id) {
        if (this.#readOnly) {
            console.warn('You are not allowed to perform this operation!')
            return;
        }
        abp.message.confirm(this.#deleteConfirmationMessage.text, this.#deleteConfirmationMessage.header)
            .then(confirmed => {
                if (confirmed) {
                    abp.ui.setBusy();
                    this.#masterAppService.delete(id)
                        .done(() => {
                            const item = this.#masterListElement.querySelector(`.master-details__master-list-item[data-id="${id}"]`);
                            this.#onDelete(item.getAttribute('data-id'), item.classList.contains('active'));
                            item.remove();
                            abp.ui.clearBusy();
                        })
                        .catch(err => handleError(err));
                }
            });
    }


}