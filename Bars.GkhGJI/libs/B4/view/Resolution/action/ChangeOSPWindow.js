Ext.define('B4.view.resolution.action.ChangeOSPWindow', {
    extend: 'B4.view.resolution.action.BaseResolution4Window',

    alias: 'widget.changeospwindow',

    requires: [
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.grid.Panel',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.dict.Inspector',
        'B4.store.dict.SanctionGji',
        'B4.store.Contragent',
        'B4.store.dict.MunicipalitySelectTree',
        'B4.store.dict.MunicipalityTree',
        'B4.store.dict.municipality.ListAllWithParent',
        'B4.enums.TypeInitiativeOrgGji',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeTerminationBasement',
        'B4.form.TreeSelectField',
        'B4.store.dict.JurInstitution',
        'B4.store.dict.ConcederationResult',
        'B4.enums.OSPDecisionType'
    ],

    modal: true,
    closable: false,
    maximized: false,
    width: 500,
    minWidth: 300,
    height: 150,
    minHeight: 150,
    title: 'Измение отдела судебных приставов',
    closeAction: 'destroy',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    bodyPadding: 0,
    border: null,
    accountOperationCode: 'ChangeOSPOperation',
    resolutionIds: null,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    flex: 0.7,
                    title: 'Отдел судебных приставов',
                    bodyStyle: Gkh.bodyStyle,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items:[
                        {
                            xtype: 'form',
                            border: null,
                            bodyPadding: '10px 10px 0 10px',
                            bodyStyle: Gkh.bodyStyle,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                flex: 1,
                                labelWidth: 80,
                                labelAlign: 'right',
                                readOnly: false
                            },
                            items:[
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    store: 'B4.store.dict.JurInstitution',
                                    textProperty: 'ShortName',
                                    name: 'OSP',
                                    fieldLabel: 'Отдел ССП',
                                    itemId: 'sfSSP',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальное образование',
                                            dataIndex: 'Municipality',
                                            flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListWithoutPaging'
                                            }
                                        }
                                    ],
                                    listeners: {
                                        'beforeload': function (store, operation) {
                                            operation.params['type'] = 20;
                                        }
                                    }
                                } 
                            ]
                        },
                    ]
                },
            ],

            getForm: function() {
                return me.down('form');
            }
        });

        me.callParent(arguments);
    },

    getParams: function() {
        var me = this,
            params = {
                operationCode: me.accountOperationCode,
                resolutionIds: Ext.JSON.encode(me.resolutionIds),
                ospId: me.down('[name=OSP]').getValue(),
            };

        return params;
    }    
});         
            