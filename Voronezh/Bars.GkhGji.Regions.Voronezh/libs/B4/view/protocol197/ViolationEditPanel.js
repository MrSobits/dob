Ext.define('B4.view.protocol197.ViolationEditPanel', {
    extend: 'Ext.panel.Panel',
    storeName: null,
    title: 'Нарушения',
    itemId: 'protocol197ViolationEditPanel',
    layout: {
        type: 'border'
    },
    minHeight: 300,
    border: false,
    alias: 'widget.protocol197ViolationEditPanel',

    requires: [
        'B4.view.protocol197.ViolationGrid',
        'B4.form.SelectField',
        'B4.store.dict.ResolveViolationClaim'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    region: 'north',
                    itemId: 'protocol197NorthPanel',
                    height: 100,
                    border: false,
                    unstyled: true,
                    layout: 'hbox',
                    padding: 5,
                    defaults: {
                        labelAlign: 'right'
                    },
                    shrinkWrap: true,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'anchor',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 160
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата правонарушения',
                                    name: 'DateOfViolation'
                                },
                                {
                                    xtype: 'container',
                                    margin: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 160
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'HourOfViolation',
                                            fieldLabel: 'Время правонарушения',
                                            width: 210,
                                            maxValue: 23,
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'label',
                                            text: ':',
                                            margin: '5'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'MinuteOfViolation',
                                            width: 45,
                                            maxValue: 59,
                                            minValue: 0
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'tabtextarea',
                            fieldLabel: 'Свидетели',
                            flex: 1,
                            width: 350,
                            name: 'Witnesses'
                        },
                        {
                            xtype: 'tabtextarea',
                            fieldLabel: 'Потерпевшие',
                            flex: 1,
                            width: 350,
                            name: 'Victims'
                        }
                        //{
                        //    xtype: 'b4selectfield',
                        //    store: 'B4.store.dict.ResolveViolationClaim',
                        //    fieldLabel: 'Наименование требования',
                        //    labelWidth: 160,
                        //    name: 'ResolveViolationClaim',
                        //    itemId: 'sfResolveViolationClaim',
                        //    hidden: true
                        //}
                    ]
                },
                {
                    xtype: 'panel',
                    region: 'north',
                    itemId: 'protocol197ViolPanel',
                    height: 100,
                    border: false,
                    unstyled: true,
                    layout: 'hbox',
                    padding: 5,
                    defaults: {
                        labelAlign: 'right'
                    },
                    shrinkWrap: true,
                    items: [

                        {
                            xtype: 'tabtextarea',
                            fieldLabel: 'Нарушение',
                            flex: 1,
                            height:80,
                            width: 350,
                            name: 'Violations'
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    region: 'north',
                    itemId: 'protocol197MidPanel',
                    height: 50,
                    border: false,
                    unstyled: true,
                    layout: 'hbox',
                    padding: 5,
                    defaults: {
                        labelAlign: 'right'
                    },
                    shrinkWrap: true,
                    items: [
                        
                        {
                            //Адрес места правонаружения
                            xtype: 'b4fiasselectaddress',
                            flex: 1.5,
                            labelAlign: 'right',
                            name: 'FiasPlaceAddress',
                            itemId: 'protocolFiasPlaceAddressField',
                            fieldLabel: 'Адрес места правонарушения',
                            fieldsRegex: {
                                tfHousing: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                },
                                tfBuilding: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                }
                            }
                        },
                        {
                            xtype: 'textfield',
                            name: 'AddressPlace',
                            fieldLabel: 'Дополнительная информация',
                            itemId: 'tfAddressPlace',
                            width: 450,
                            minWidth: 450,
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    region: 'center',
                    layout: 'fit',
                    border: false,
                    items: [
                        {
                            xtype: 'protocol197ViolationGrid',
                            border: false
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});