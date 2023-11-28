Ext.define('B4.controller.CostLimit', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
    ],
    stores: [
        'CostLimit',
    ],
    models: [
        'CostLimit',
    ],
    views: [
        'costlimit.Grid',
        'costlimit.Panel',
        'costlimit.EditWindow',
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    refs: [
        {
            ref: 'mainView',
            selector: 'costlimitPanel'
        }
    ],
    mainView: 'costlimit.Panel',
    mainViewSelector: 'costlimitPanel',
    //codeParam: null,
    init: function () {
        var me = this,
            actions = {
            };
        me.control(actions);
        me.callParent(arguments);
    },
    index: function () {
        var view = this.getMainView() || Ext.widget('costlimitPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('CostLimit').load();
    },
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'costlimitGridAspect',
            gridSelector: 'costlimitgrid',
            editFormSelector: '#costlimitEditWindow',
            storeName: 'CostLimit',
            modelName: 'CostLimit',
            editWindowView: 'costlimit.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            //otherActions: function (actions) {
                    //actions['#actualisedpkrEditWindow #cbNumberApartments'] = { 'change': { fn: this.onChangeNumberApartments, scope: this } },
            //},
            //onChangeNumberApartments: function (field, newValue) {
            //    var form = this.getForm(),
            //        cbNumberApartmentsCondition = form.down('#cbNumberApartmentsCondition'),
            //        nfNumberApartments = form.down('#nfNumberApartments');
            //    if (newValue == true) {
            //        cbNumberApartmentsCondition.setDisabled(false);
            //        nfNumberApartments.setDisabled(false);
            //    }
            //    else {
            //        cbNumberApartmentsCondition.setDisabled(true);
            //        nfNumberApartments.setDisabled(true);
            //    }
            //},
        }]
});