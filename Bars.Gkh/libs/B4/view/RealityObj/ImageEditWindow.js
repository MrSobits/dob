﻿Ext.define('B4.view.realityobj.ImageEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realityobjimageeditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 500,
    minWidth: 400,
    minHeight: 250,
    bodyPadding: 5,
    title: 'Фото-архив',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.store.dict.Period',
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.enums.ImagesGroup'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 120,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'DateImage',
                    fieldLabel: 'Дата изображения',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    maxLength: 100
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Группа',
                    store: B4.enums.ImagesGroup.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'ImagesGroup',
                    itemId: 'imagesGroupComboBox'
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Period',
                    fieldLabel: 'Период',
                    anchor: '100%',
                   

                    store: 'B4.store.dict.Period',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 },
                        { xtype: 'datecolumn', format: 'd.m.Y', text: 'Дата начала', dataIndex: 'DateStart', flex: 1 },
                        { xtype: 'datecolumn', format: 'd.m.Y', text: 'Дата окончания', dataIndex: 'DateEnd', flex: 1 }
                    ],
                    itemId: 'periodSelectfield'
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'WorkCr',
                    fieldLabel: 'Вид работы',
                   

                    store: 'B4.store.realityobj.Work',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                    ],
                    itemId: 'workSelectField'
                },
                {
                    xtype: 'checkbox',
                    name: 'ToPrint',
                    fieldLabel: 'Выводить на печать'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    possibleFileExtensions: 'img,png,jpg,jpeg,gif',
                    maxFileSize: (Gkh.config.General.MaxUploadPhotoSize || Gkh.config.General.MaxUploadFileSize) * 1048576 // Размер в байтах
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    anchor: '100% -125',
                    maxLength: 300
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