Ext.define('B4.view.dict.violationgji.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    height: 650,
    maxHeight: 650,
    width: 900,
    itemId: 'violationGjiEditWindow',
    title: 'Нарушение',
    closeAction: 'hide',

    border: false,
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.form.TreeSelectField',
        'B4.store.dict.NormativeDoc',
        'B4.store.dict.NormativeDocItem',
        'B4.store.dict.NormativeDocItemGrouping',
        'B4.store.dict.ViolationFeatureGroupsGji',
        'B4.store.dict.ViolationGjiMunicipality',
        'B4.store.dict.ViolationGji',
        'B4.view.dict.violationgji.ViolationGroupsGjiGrid',
        'B4.view.dict.violationgji.ViolationNormativeDocItemGrid',
        'B4.view.dict.violationgji.ViolationGjiMunicipalityGrid',
        'B4.store.dict.NormativeDocItemTreeStore',,
        'B4.enums.TypeMunicipality',
        'B4.form.EnumCombo'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 300
            },
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'panel',
                            layout: { type: 'vbox', align: 'stretch' },
                            border: false,
                            frame: true,
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 180
                            },
                            title: 'Нарушение',
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.ViolationGji',
                                    textProperty: 'Name',
                                    name: 'ParentViolationGji',
                                    allowblank:true,
                                    fieldLabel: 'Базовое нарушение',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'НпД', xtype: 'gridcolumn', dataIndex: 'NormDocNum', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'Name',
                                    fieldLabel: 'Текст нарушения',
                                    allowBlank: false,
                                    maxLength: 2000
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'NormativeDocNames',
                                    fieldLabel: 'НПД',
                                    maxLength: 2000,
                                    hidden: false
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'TypeMunicipality',
                                    itemId: 'cbTypeMunicipality',
                                    fieldLabel: 'Тип муниципального образования',
                                    enumName: B4.enums.TypeMunicipality
                                },
                                //{
                                //    xtype: 'treeselectfield',
                                //    name: 'Municipality',
                                //    itemId: 'fiasMunicipalitiesTrigerField',
                                //    fieldLabel: 'Территориальный признак',
                                //    titleWindow: 'Выбор муниципального образования',
                                //    store: 'B4.store.dict.MunicipalitySelectTree',
                                //    allowBlank: false,
                                //    editable: false
                                //},
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'PpRf170',
                                    fieldLabel: 'ПП РФ №170',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'PpRf25',
                                    fieldLabel: 'ПП РФ №25',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'PpRf307',
                                    fieldLabel: 'ПП РФ №307',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'PpRf491',
                                    fieldLabel: 'ПП РФ №491',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'OtherNormativeDocs',
                                    fieldLabel: 'Прочие норм. док.',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'textarea',
                                    height: 40,
                                    name: 'GkRf',
                                    fieldLabel: 'ЖК РФ',
                                    maxLength: 2000,
                                    hidden: true
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.ArticleLawGji',
                                    textProperty: 'Name',
                                    name: 'ArticleLaw',
                                    allowblank: true,
                                    fieldLabel: 'Статья закона',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'Статья', xtype: 'gridcolumn', dataIndex: 'Article', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'Пункт', xtype: 'gridcolumn', dataIndex: 'Part', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.ArticleLawGji',
                                    textProperty: 'Name',
                                    name: 'ArticleLawRepeatative',
                                    allowblank: true,
                                    fieldLabel: 'Статья закона при повторном нарушении',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'Статья', xtype: 'gridcolumn', dataIndex: 'Article', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'Пункт', xtype: 'gridcolumn', dataIndex: 'Part', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'violationNormativeDocItemGrid',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'violationGroupsGjiGrid',
                            flex: 1
                        },
                        {
                            xtype: 'violationGjiMunicipalityGrid',
                            flex: 1
                        },
                        {
                            xtype: 'violationActionsRemovGrid',
                            flex: 1
                        }
                    ]
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