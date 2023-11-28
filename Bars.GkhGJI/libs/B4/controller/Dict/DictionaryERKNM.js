Ext.define('B4.controller.dict.DictionaryERKNM', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: ['dict.DictionaryERKNM'],
    stores: ['dict.DictionaryERKNM'],
    views: [
        'dict.erknm.EditWindow',
        'dict.erknm.Grid',
    ],

    //aspects: [

    //    {
    //        xtype: 'grideditwindowaspect',
    //        name: 'oSPGridAspect',
    //        gridSelector: 'oSPGrid',
    //        editFormSelector: '#oSPEditWindow',
    //        storeName: 'dict.OSP',
    //        modelName: 'dict.OSP',
    //        editWindowView: 'dict.OSP.EditWindow'
    //    }
    //],

    mainView: 'dict.erknm.Grid',
    mainViewSelector: 'erknmGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'erknmGrid'
        }
    ],

    //mixins: {
    //    context: 'B4.mixins.Context'
    //},

    index: function () {
        var view = this.getMainView() || Ext.widget('erknmGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.erknm').load();
    }
});