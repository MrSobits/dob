Ext.define('B4.aspects.GkhInlineGrid', {
    extend: 'B4.aspects.InlineGrid',

    alias: 'widget.gkhinlinegridaspect',

    saveButtonSelector: null,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents(
            'beforeaddrecord'
        );
    },

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.saveButtonSelector] = { click: { fn: this.save, scope: this} };

        this.otherActions(actions);

        controller.control(actions);
    },
    
    otherActions: function () {
        //Данный метод служит для перекрытия в контроллерах где используется данный аспект
        //наслучай если потребуется к данному аспекту добавить дополнительные обработчики
    },

    addRecord: function () {
        var plugin,
            store = this.getStore(),
            rec = this.controller.getModel(this.modelName).create(),
            grid = this.getGrid();

        this.fireEvent('beforeaddrecord', this, rec);

        store.insert(0, rec);

        if (this.cellEditPluginId && grid) {
            plugin = grid.getPlugin(this.cellEditPluginId);
            plugin.startEditByPosition({ row: 0, column: this.firstEditColumnIndex });
        }
    },
    
    save: function () {
        var me = this,
            store = me.getStore(),
            grid;

        var modifiedRecs = store.getModifiedRecords();
        var removedRecs = store.getRemovedRecords();
        if (modifiedRecs.length > 0 || removedRecs.length > 0) {
            if (me.fireEvent('beforesave', me, store) !== false) {
                grid = me.getGrid();
                if (grid && grid.container) {
                    me.mask('Сохранение', grid);
                } else {
                    me.mask('Сохранение');
                }

                store.sync({
                    callback: function () {
                        me.unmask();
                        store.load();
                    },
                    // выводим сообщение при ошибке сохранения
                    failure: function (result) {
                        me.unmask();
                        if (result && result.exceptions[0] && result.exceptions[0].response) {
                            Ext.Msg.alert('Ошибка!', Ext.JSON.decode(result.exceptions[0].response.responseText).message);
                        }
                    }
                });
            }
        }
    }
});