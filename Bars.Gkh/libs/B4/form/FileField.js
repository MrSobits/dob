Ext.define('B4.form.FileField', {
    extend: 'Ext.form.field.Trigger',
    alias: ['widget.b4filefield'],

    childEls: ['fileInputEl', 'fileInputWrap', 'downloadFrame', 'downloadForm'],
    requires: [
        'B4.view.PreviewFileWindow'
    ],
    iconClsSelectFile: 'x-form-search-trigger',
    iconClsDownloadFile: 'x-form-trigger',
    iconClsClearFile: 'x-form-clear-trigger',
    iconClsPreviewFile: 'x-form-picture-trigger',

    triggerTooltips: ['Загрузить', 'Просмотр файла', 'Скачать', 'Очистить'],

    fileController: 'FileUpload',
    downloadAction: 'Download',
    fileExtension: '',

    // Список расширений имен файлов, которые пользователь может выбрать для загрузки.
    // Представлен в виде строки разделенной запятыми, например: 'zip,rar'
    possibleFileExtensions: null,

    constructor: function (config) {
        var me = this,
            globalMaxFileSize,
            fileSize;

        Ext.apply(this, config || {});

        if (Gkh.config.General.MaxUploadFileSize) {
            globalMaxFileSize = Gkh.config.General.MaxUploadFileSize * 1048576; // Размер в байтах
            fileSize = me.maxFileSize || globalMaxFileSize;
            if (fileSize > globalMaxFileSize)
                fileSize = globalMaxFileSize;
        } else {
            fileSize = -1; //infinity
        }

        Ext.apply(me, {
            maxFileSize: fileSize
        });

        me.callParent(arguments);

        me.addEvents('beforeSelectClick', 'fileselected', 'filechange', 'fileclear');
    },

    isFileUpload: function () {
        return true;
    },

    /**
     * Возвращает размер файла. 
     */
    getSize: function () {
        var me = this;

        return me.fileInputEl.dom.files[0] ? me.fileInputEl.dom.files[0].size : 0;
    },

    extractFileInput: function () {
        var me = this,
            fileInput = me.fileInputEl.dom;
        me.reset();
        return fileInput;
    },

    createFileInput: function () {
        var me = this;
        me.fileInputEl = me.fileInputWrap.createChild({
            id: me.id + '-fileInputEl',
            name: me.getName(),
            cls: Ext.baseCSSPrefix + 'form-file-input',
            tag: 'input',
            type: 'file',
            size: 1
        });
        me.fileInputEl.on({
            scope: me,
            change: me.onInputFileChange
        });
    },

    initTriggers: function () {
        var me = this;

        if (!me.trigger1Cls) {
            me.trigger1Cls = me.iconClsSelectFile;
        }
        if (!me.trigger2Cls) {
            me.trigger2Cls = me.iconClsPreviewFile;
            me.trigger2tooltip = 'Предварительный просмотр';
        }
        if (!me.trigger3Cls) {
            me.trigger3Cls = me.iconClsDownloadFile;

        }
        if (!me.trigger4Cls) {
            me.trigger4Cls = me.iconClsClearFile;
        }
    },

    onApplyBy: function (allowed, triggerCls) {
        var field = this,
            hideField = true;
        var el = this.triggerCell.elements.filter(function (e) {
            return e.dom.innerHTML.indexOf(triggerCls) >= 0;
        })[0];

        if (el) {
            el.setStyle('display', allowed ? 'table-cell' : 'none');
        }

        // если все элементы управления скрыты, то скроем всё поле
        Ext.Array.forEach(this.triggerCell.elements,
            function (e) {
                hideField = hideField && !e.isDisplayed();
            });

        if (hideField) {
            field.hide();
        }
    },

    getTriggerMarkup: function () {
        var me = this,
            i,
            hideTrigger = (me.readOnly || me.hideTrigger),
            triggerCls,
            triggerBaseCls = me.triggerBaseCls,
            triggerConfigs = [],
            inputElCfg = {
                id: me.id + '-fileInputEl',
                name: me.getName(),
                cls: Ext.baseCSSPrefix + 'form-file-input',
                tag: 'input',
                type: 'file',
                size: 1
            };

        me.initTriggers();

        for (i = 0; (triggerCls = me['trigger' + (i + 1) + 'Cls']) || i < 1; i++) {
            triggerConfigs.push({
                tag: 'td',
                valign: 'top',
                cls: Ext.baseCSSPrefix + 'trigger-cell',
                style: 'width:' + me.triggerWidth + (hideTrigger ? 'px;display:none' : 'px'),
                cn: {
                    cls: [Ext.baseCSSPrefix + 'trigger-index-' + i, triggerBaseCls, triggerCls].join(' '),
                    role: 'button'
                }
            });
        }
        triggerConfigs[i - 1].cn.cls += ' ' + triggerBaseCls + '-last';

        triggerConfigs.push({
            id: me.id + '-fileInputWrap',
            tag: 'td',
            valign: 'top',
            cls: me.trigger1Cls,
            style: 'width:0px; display:none;',
            cn: inputElCfg
        },
            {
                id: me.id + '-downloadForm',
                tag: 'form',
                style: 'display: none'
            });

        return Ext.DomHelper.markup(triggerConfigs);
    },

    reset: function () {
        var me = this;
        if (me.rendered) {
            me.fileInputEl.remove();
            me.createFileInput();
        }
        me.callParent();
    },

    setValue: function (file) {
        var me = this;
        debugger;
        me.fileIsDelete = false;
        me.fileIsLoad = false;

        if (file) {
            me.fileId = file.id || file.Id;
            me.callParent([file.name || file.Name]);
        }
        else {
            me.fileId = null;
            me.callParent([]);
        }
    },

    getValue: function () {
        var me = this;
        return Ext.isEmpty(me.fileId) ? null : { Id: me.fileId };
    },

    /* Warning
    * В связи с тем, что пользователь может нажать кнопку удаления файла, потом прикрепить файл, потом ещё раз удалить, и еще раз прикрепить
    * не всегда получиться получить текущее значение, по этому отбработка удаления файла перенесена на сервер
    */
    getModelData: function () {
        var me = this,
            data = null;
        if (!me.disabled) {
            data = {};
            if (me.fileIsDelete) {
                data[me.getName()] = null;
            }
            else {
                data[me.getName()] = me.getValue();
            }
        }
        return data;
    },

    onRender: function () {
        var me = this,
            inputEl;
        me.callParent(arguments);

        inputEl = me.inputEl;
        inputEl.dom.name = ''; //name goes on the fileInput, not the text input

        me.fileInputEl.dom.name = me.getName();
        me.fileInputEl.on({
            scope: me,
            change: me.onInputFileChange
        });
    },

    onTrigger1Click: function () {
        var me = this;

        if (me.fireEvent('beforeSelectClick', me)) {
            me.onSelectFile();
        }
    },


    onTrigger2Click: function () {
        this.onPreviewFile();
    },

    onTrigger3Click: function () {
        this.onFileDownLoad();
    },

    onTrigger4Click: function () {
        this.onClearFile();
    },

    onSelectFile: function () {
        this.fileInputEl.dom.click();
    },

    onPreviewFile: function () {
        if (!this.fileId) {
            B4.QuickMsg.msg('Внимание', 'Файл еще не загружен', 'warning');
            return;
        }

        var me = this,
            win = Ext.widget('previewFileWindow', {
                renderTo: B4.getBody().getActiveTab().getEl(),
                fileId: me.fileId
            });

        win.down('button[name=Save]').on({
            click: function () {
                me.onFileDownLoad();
            }
        });

        win.show();
    },

    onFileDownLoad: function () {
        var me = this;

        if (me.fileId) {
            if (me.downloadFrame) {
                Ext.destroy(me.downloadFrame);
            }

            me.downloadFrame = me.downloadForm.createChild({
                id: me.id + '-downloadFrame',
                tag: 'iframe',
                src: B4.Url.content(Ext.String.format('{0}/{1}?id={2}', me.fileController, me.downloadAction, me.fileId)),
                style: 'display: none'
            });
        }
    },

    onClearFile: function () {
        var me = this,
            currentValue = me.getValue();

        me.setValue(null);

        me.fileIsDelete = true;
        me.fileIsLoad = false;

        me.fireEvent('fileclear', me, me.getName(), currentValue);
        me.reset();
    },

    onInputFileChange: function () {
        var me = this,
            v = me.fileInputEl.dom.value;
        /* Во всех браузерах кроме FireFox после выбора файла в текстовом поле отображается
        * примерно следующее знчение "C:\fakepath\[наименование файла]". Данная проблема 
        * существует в примерах на сайте производителя. Данная ошибка исключительно визуальная,
        * на работоспасобность не влияет. Поэтому выделяем наименование файла 
        * из не корректной записи.
        */
        if (v.indexOf('\\') >= 0) {
            var vArr = v.split('\\');
            var vArrLength = vArr.length;
            v = vArr[vArrLength - 1];
        }

        me.fireEvent('fileselected', me, v);

        if (!me.isFileExtensionOK()) {
            Ext.Msg.show({
                title: 'Поле выбора файла',
                msg: me.getMessage(v, me.fileExtension, me.possibleFileExtensions),
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.WARNING
            });

            me.reset();

            return;
        }

        if (!me.isFileSizeOK()) {
            Ext.Msg.show({
                title: 'Поле выбора файла',
                msg: me.getInvalidFileSizeMessage(v, me.maxFileSize),
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.WARNING
            });

            me.reset();

            return;
        }

        if (v) {
            me.fileId = null;
            me.fileIsDelete = false;
            me.fileIsLoad = true;
        }

        if (me.lastValue !== v) {
            me.fireEvent('filechange', me);
        }

        me.lastValue = v;
        B4.form.FileField.superclass.setValue.call(me, v);
    },

    isFileLoad: function () {
        return this.fileIsLoad;
    },

    isFileDelete: function () {
        return this.fileIsDelete;
    },

    getFileUrl: function (id) {
        var me = this;
        return B4.Url.content(Ext.String.format('{0}/{1}?id={2}', me.fileController, me.downloadAction, id));
    },

    isFileExtensionOK: function () {
        var me = this;

        if (!me.possibleFileExtensions) {
            return true;
        }
        var fileExtension = me.fileInputEl.dom.value.split('.');

        this.fileExtension = fileExtension[fileExtension.length - 1];
        if (fileExtension.length > 0) {
            return me.possibleFileExtensions.split(',')
                .indexOf(fileExtension[fileExtension.length - 1].toLowerCase()) != -1;
        }
        return false;
    },

    isFileSizeOK: function () {
        var me = this;
        if (me.maxFileSize === -1) {
            return true;
        } else {
            return me.getSize() <= me.maxFileSize;
        }
    },

    onDestroy: function () {
        var me = this;

        Ext.destroyMembers(me, 'fileInputEl', 'fileInputWrap', 'downloadFrame', 'downloadForm');
        me.callParent();
    },

    getInvalidExtensionMessage: function (fileName, errorExtension, needExtensions) {
        var msg = Ext.String.format('Выбранный файл {0} имеет недопустимое расширение.<br/>Допустимы следующие расширения: {1}', fileName, needExtensions);
        return msg;
    },

    getInvalidFileSizeMessage: function (fileName, fileSize) {
        var msg = Ext.String.format('Выбранный файл {0} имеет недопустимый размер.<br/>Размер файла не должен превышать: {1}', fileName, this.formatSize(fileSize));

        return msg;
    },

    getMessage: function (fileName, errorExtension, needExtensions) {
        var msg = Ext.String.format('Выбранный файл {0} имеет недопустимое расширение.<br/>Допустимы следующие расширения: {1}', fileName, needExtensions);
        return msg;
    },

    formatSize: function (size) {
        var kbConst = 1024,
            mbConst = kbConst * 1024,
            gbConst = mbConst * 1024,
            gb, mb, kb, result = '';

        if (size / gbConst > 1) {
            gb = Math.round(size / gbConst);
            size %= gbConst;

            result += gb + 'Гб ';
        }

        if (size / mbConst > 1) {
            mb = Math.round(size / mbConst);
            size %= mbConst;
            result += mb + ' Мб ';
        }

        if (size / kbConst > 1) {
            kb = Math.round(size / kbConst);
            result += kb + ' Кб ';
        }

        if (size > 0) {
            result += size + ' б';
        }

        return result.trim();
    }
});