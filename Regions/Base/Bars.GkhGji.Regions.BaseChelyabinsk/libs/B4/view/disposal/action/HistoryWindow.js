Ext.define('B4.view.disposal.action.HistoryWindow', {
    extend: 'B4.view.disposal.action.BaseDisposalWindow',

    alias: 'widget.historywindow',

    requires: [
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.grid.Panel',
        'B4.view.dict.individualperson.ResolutionGrid'
    ],

    modal: true,
    closable: false,
    maximized: false,
    width: 750,
    minWidth: 750,
    height: 400,
    minHeight: 400,
    title: 'История',
    closeAction: 'destroy',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    bodyPadding: 0,
    border: null,
    accountOperationCode: 'SumAmountOperation',
    resolutionIds: null,

    initComponent: function () {
        var me = this;
        historyStore = Ext.create('B4.base.Store', {
            autoLoad: true,
            proxy: {
                type: 'b4proxy',
                controllerName: 'Resolution',
                listAction: 'ListIndividualPerson',
                timeout: 1000 * 60 * 5
            },
            fields: [
                'DocumentDate',
                'DocumentNum',
                'DocumentNumber',
                'Violation',
                'PenaltyAmount',
                'Paided'
            ],
            //listeners: {
            //    beforeload: function (store, operation) {
            //        operation.params.individualPersonid = me.individualpersonid;
            //        debugger;
            //        Ext.apply(operation.params, me.getParams());
            //    },
               
            //}
        }),
            renderer = function (val) {
            debugger;
                return Ext.util.Format.currency(val);
            };
        Ext.applyIf(me, {
            items: [
                //{
                //    xtype: 'individualpersonresolutiongrid',
                //    flex: 1
                //},
                {
                    xtype: 'b4grid',
                    name: 'IndividualPersonResolutionGrid',
                    margin: '10px 0 0 0',
                    store: historyStore,
                    columnLines: true,
                    flex: 1,
                    selModel: Ext.create('B4.ux.grid.selection.CheckboxModel'),
                    columns: [
                        { header: 'Дата', xtype: 'gridcolumn', dataIndex: 'DocumentDate', width: 100 },
                        { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'DocumentNum', flex: 1 },
                        { header: 'Номер1', xtype: 'gridcolumn', dataIndex: 'DocumentNumber', flex: 1 },
                        { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'Violation', flex: 1 },
                        { header: 'Штраф', xtype: 'gridcolumn', dataIndex: 'PenaltyAmount', flex: 1 },
                        { header: 'Оплата', xtype: 'datecolumn', format: 'd.m.Y', dataIndex: 'Paided', width: 100 },
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: historyStore,
                            dock: 'bottom'
                        }
                    ],
                    listeners: {
                        render: {
                            scope: this,
                            fn: function (grid) {
                                grid.getStore().on('beforeload', this.onBeforeLoad, this);
                            }
                        },
                    }
                }
            ],

            getForm: function () {
                debugger;
                return me.down('form');
            }
        });

        me.callParent(arguments);
    },

    setActive: function (active, newActive) {
        var me = this;
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.individualPersonid = me.individualpersonid;
        debugger;
        Ext.apply(operation.params, me.getParams());
    },

    getParams: function () {
        var me = this,
            params = {
                individualPersonid: me.individualpersonid,
            };
        return params;
    },    
});         
            