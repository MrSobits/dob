/*
Данный аспект предназначен для смены статусов из карточки редатирования объекта по кнопке с выпадающим списком
{
    xtype:'statebuttonaspect',
    name:'statebutton',
    stateButtonSelector: '#stateButton123',
}
*/

Ext.define('B4.aspects.HistoryButton', {
    extend: 'B4.base.Aspect',

    alias: 'widget.gkhbuttonhistoryaspect',

    requires: [
        'B4.view.dict.violationgji.ViolationGjiMunicipalityGrid',
        'B4.QuickMsg'
    ],

    gridSelector: null,
    emptyDescription: 'Перевод статуса из карточки',
    //entityId и currentState проставляются после вызова метода setStateData
    entityId: 0,
    currentState: null,
    //Форма показа истории 
    windowHistorySelector: '#violationGjiMunicipalityGrid',
    windowHistoryView: 'B4.view.dict.violationgji.ViolationGjiMunicipalityGrid',

    stateStore: null,

    stateButtonSelector: null,

    constructor: function(config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents('transfersuccess');
    },

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions['#disposalEditPanel #btnHistory'] = { 'click': { fn: this.onStateMenuItemClick, scope: this } };

        //this.stateStore.on('beforeload', this.onStateStoreBeforeLoad, this);
      //  this.stateStore.on('load', this.onStateStoreLoad, this);

        controller.control(actions);
    },

    getWindowHistory: function() {
        var window = Ext.ComponentQuery.query(this.windowHistorySelector)[0];
        debugger;
        if (!window) {
            window = this.controller.getView(this.windowHistoryView).create();
        }

        return window;
    },

    onStateMenuItemClick: function (itemMenu) {
        debugger;
            var historyWin = this.getWindowHistory();
        debugger;
            if (this.entityId > 0 && this.currentState)
                historyWin.updateGrid(this.entityId, this.currentState.TypeId);
            historyWin.show();

        debugger;
    },


});