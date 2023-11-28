/*
Данный аспект предназначен для описание взаимодействия компонентов в документа ГЖИ
в именно Сохранение основных сведений, кнопки Отмена, Удалить, Зарегистрировать
*/

Ext.define('B4.aspects.GjiDocument', {
    extend: 'B4.aspects.GkhEditPanel',

    alias: 'widget.gjidocumentaspect',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function(controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.editPanelSelector + ' #btnCancel'] = { 'click': { fn: this.btnCancelClick, scope: this } };
        actions[this.editPanelSelector + ' #btnDelete'] = { 'click': { fn: this.btnDeleteClick, scope: this } };

        this.on('beforesetdata', this.onBeforeSetData, this);
        this.on('aftersetpaneldata', this.onAfterSetPanelData, this);
        this.on('savesuccess', this.onSaveSuccess, this);

        this.stateButtonSelector = this.editPanelSelector + ' #btnState';
        controller.control(actions);
    },

    reloadTreePanel: function() {
        Ext.ComponentQuery.query(this.controller.params.treeMenuSelector)[0].getStore().load();
    },

    onSaveSuccess: function(asp, rec) {
        if (rec.get('DocumentNumber')) {
            this.getPanel().setTitle(asp.controller.params.title + " " + rec.get('DocumentNumber'));
        } else {
            this.getPanel().setTitle(asp.controller.params.title);
        }
    },

    onBeforeSetData: function() {
        var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');
        var idx = 0;
        //теперь пробегаем по массиву groups и дизаблим все группы кнопок на панели
        while (true) {

            if (!groups[idx])
                break;

            groups[idx].setDisabled(true);
            idx++;
        }

        return true;
    },

    onAfterSetPanelData: function() {
        var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');
        var idx = 0;
        //теперь пробегаем по массиву groups и активируем все группы кнопок на панели
        while (true) {

            if (!groups[idx])
                break;

            groups[idx].setDisabled(false);
            idx++;
        }
    },

    //после нажатия на Удалить идет удаление документа
    btnDeleteClick: function() {

        var panel = this.getPanel();
        var record = panel.getForm().getRecord();

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить документ?', function(result) {
            if (result == 'yes') {
                this.mask('Удаление', B4.getBody());
                record.destroy()
                    .next(function() {

                        //Обновляем дерево меню
                        var tree = Ext.ComponentQuery.query(this.controller.params.treeMenuSelector)[0];
                        tree.getStore().load();

                        Ext.Msg.alert('Удаление!', 'Документ успешно удален');

                        panel.close();
                        this.unmask();
                    }, this)
                    .error(function(result) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        this.unmask();
                    }, this);

            }
        }, this);
    },

    //При нажатии на кнопку Оттмена мы просто загружаем существующие данные документа
    btnCancelClick: function() {
        this.setData(this.controller.params.documentId);
    }
});