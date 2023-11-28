Ext.define('B4.catalogs.ProgramCrSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор программы кап. ремонта',
    store: 'B4.store.dict.ProgramCr',
    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
    editable: false
});