/**
 * formatterParams: {
 *  buttons:[
 *      {
 *          iconClass: класс иконки из fontawesome
 *          btnClass: класс кнопки для ее идентификации
 *      },
 *      ...
 *  ]
 * }
 * @param {any} cell
 * @param {any} formatterParams
 * @param {any} onRendered
 */
function toolsFormatter(cell, formatterParams, onRendered) {
    if (formatterParams && formatterParams.buttons) {
        let html = '<div class="tabulator-tools-cell">';
        formatterParams.buttons.forEach(function (btn) {
            html += '<button type="button" class="tabulator-tools-btn ' + btn.btnClass + '">';
            html += '<i class="' + btn.iconClass + '" ></i>';
            html += '</button > ';
        });
        html += '</div>';
        return html;
    } else {
        throw 'Buttons definition is missed in toolsFormatter params';
    };
}

/**
 *  buttons - описание кнопок (см. toolsFormatter)
 *  handlers - объект-словарь обработчиков клика по кнопкам. ключ - класс кнопки из buttons, значение - function(cell)
 * @param {any} buttons
 * @param {any} handlers
 */
function ToolsColumn(buttons, handlers, params) {
    this.title = '';
    this.field = '';
    this.width = params?.width ?? '' + buttons.length * 2 + 'em';
    this.resizable = false;
    this.frozen = true;
    this.headerSort = false;
    this.formatter = toolsFormatter;
    this.formatterParams = { buttons: buttons };
    this.cellClick = function (e, cell) {
        let classes = e.target.closest('button').classList;
        classes.forEach(function (cl) {
            if (handlers[cl]) {
                handlers[cl](cell);
            };
        });
    };
}