Ext.define('B4.controller.OtherService', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.InlineGrid'],

    models: ['OtherService'],
    stores: ['OtherService'],
    views: ['otherservice.Grid'],

    mainView: 'otherservice.Grid',
    mainViewSelector: '#otherServiceGrid',

    aspects: [{
        xtype: 'inlinegridaspect',
        name: 'otherServiceAspect',
        storeName: 'OtherService',
        modelName: 'OtherService',
        gridSelector: '#otherServiceGrid',
        listeners: {
            beforesave: function (asp, store) {
                store.each(function (rec) {
                    if (!rec.get('Id')) {
                        rec.set('DisclosureInfoRealityObj', asp.controller.params.disclosureInfoRealityObjId);
                    }
                });
                return true;
            }
        }
    }],

    init: function () {
        this.getStore('OtherService').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('OtherService').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
        }
    }
});