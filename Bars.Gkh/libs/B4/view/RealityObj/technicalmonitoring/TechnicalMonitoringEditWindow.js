Ext.define('B4.view.realityobj.technicalmonitoring.TechnicalMonitoringEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.technicalmonitoringeditwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 500,
    minWidth: 400,
    minHeight: 250,
    maxHeight: 300,
    bodyPadding: 10,
    title: 'Технический мониторинг',

    requires: [
        'B4.form.ComboBox',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNo'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата документа',
                    allowBlank: false,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.MonitoringTypeDict',
                    name: 'MonitoringTypeDict',
                    fieldLabel: 'Тип мониторинга',
                    allowBlank: false,
                    editable: false,
                    itemId: 'sfMonitoringTypeDict'
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    allowBlank: false,
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'b4combobox',
                    name: 'UsedInExport',
                    fieldLabel: 'Выводить на портал',
                    displayField: 'Display',
                    items: B4.enums.YesNo.getItems(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});