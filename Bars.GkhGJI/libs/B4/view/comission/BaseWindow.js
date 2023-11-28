Ext.define('B4.view.comission.BaseWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.basecomissionwin',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    modal: true,
    closeAction: 'destroy',
    saveBtnClickListeners: null,
    bodyPadding: 10,

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4savebutton'
                        }, '->', {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function (btn) {
                                    win = btn.up('comissiondecisionwindow');
                                    debugger;
                                    if (win)
                                    {
                                        Ext.Msg.confirm('Внимание',
                                            'Закрыть форму без сохранения изменений?',
                                            function (result) {
                                                if (result === 'yes') {
                                                    win.clearListeners(); // чистим все слушателей, чтобы опять сюда не попасть
                                                    win.close();
                                                }
                                            });
                                    }
                                }
                            }
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});