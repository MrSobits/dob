Ext.define('B4.model.dict.ViolationGjiMunicipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ViolationGjiMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'MunicipalityRegion' }
    ]
});