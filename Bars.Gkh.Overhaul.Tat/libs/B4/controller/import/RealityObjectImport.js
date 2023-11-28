Ext.define('B4.controller.import.RealityObjectImport', {
    extend: 'B4.base.Controller',
    views: ['import.realityobj.Panel'],
    
    mainView: 'import.realityobj.Panel',
    mainViewSelector: 'nsoRealityObjectImportPanel',
    
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [{
        ref: 'mainView',
        selector: 'nsoRealityObjectImportPanel'
    }],

    init: function () {
        var me = this;
        
        this.control({
            'nsoRealityObjectImportPanel button': {
                click: me.loadButtonClick
            }
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('nsoRealityObjectImportPanel');
        this.bindContext(view);
        this.application.deployView(view);
    },
    
    loadButtonClick: function () {
        var me = this;
        
        me.importPanel = me.getMainComponent();
            
        var fileImport = me.importPanel.down('#fileImport');
      
        if (!fileImport.isValid()) {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл.', 'warning');
            return;
        }

        if (!fileImport.isFileExtensionOK()) {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл с допустимым расширением: ' + fileImport.possibleFileExtensions, 'warning');
            return;
        }

        if (!Ext.isEmpty(me.maxFileSize) && fileImport.isFileLoad() && fileImport.getSize() > me.maxFileSize) {
            B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл допустимого размера', 'warning');
            return;
        }

        me.mask('Загрузка данных', me.getMainComponent());

        var formImport = me.importPanel.down('#importForm');

        me.params = {};
        me.params.importId = 'NsoRealtyObjectImport';

        formImport.submit({
            url: B4.Url.action('/GkhImport/Import'),
            params: me.params,
            success: function (form, action) {
                me.unmask();
                var message;
                if (!Ext.isEmpty(action.result.message)) {
                    message = action.result.title + ' <br/>' + action.result.message;
                } else {
                    message = action.result.title;
                }
                
                Ext.Msg.show({
                    title: 'Успешная загрузка',
                    msg: message,
                    width: 300,
                    buttons: Ext.Msg.OK,
                    icon: Ext.window.MessageBox.INFO
                });
                
                var log = me.importPanel.down('#log');
                if (log) {
                    log.setValue(me.createLink(action.result.logFileId));
                }

            },
            failure: function (form, action) {
                me.unmask();
                var message;
                if (!Ext.isEmpty(action.result.message)) {
                    message = action.result.title + ' <br/>' + action.result.message;
                } else {
                    message = action.result.title;
                }

                Ext.Msg.alert('Ошибка загрузки', message, function () {
                    if (action.result.logFileId > 0) {
                        var log = me.importPanel.down('#log');
                        if (log) {
                            log.setValue(me.createLink(action.result.logFileId));
                        }
                    }
                });
            }
        }, this);

    },
    createLink: function (id) {
        if (!id) return '';
        var url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', id));
        return '<a href="' + url + '" target="_blank" style="color: #04468C !important; float: right;">Скачать лог</a>';
    }
});