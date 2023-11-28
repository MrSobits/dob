Ext.define('B4.view.import.PersAccImportWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.persaccimportwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 400,
    bodyPadding: 5,
    title: 'Импорт',
    resizable: false,

    requires: [
        'B4.form.FileField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4filefield',
                    name: 'FileImport',
                    labelWidth: 100,
                    labelAlign: 'right',
                    fieldLabel: 'Файл',
                    allowBlank: false,
                    itemId: 'fileImport'
                },
                {
                    xtype: 'checkbox',
                    name: 'replaceData',
                    hidden: true,
                    checked: true,
                    style: 'margin-left: 10px; margin-top: 20px; ',
                    boxLabel: 'Заменить данные по лс',
                    margin: '0 0 0 100'
                },
                {
                    xtype: 'displayfield',
                    itemId: 'log'
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
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });



        me.callParent(arguments);
    }
});
