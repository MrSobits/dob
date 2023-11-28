Ext.define('B4.model.version.VersionRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VersionRecord',
        timeout: 2 * 60 * 1000 // 2 минуты
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality'},
        { name: 'RealityObject' },
        { name: 'CommonEstateObjects' },
        { name: 'Year' },
        { name: 'IndexNumber' },
        { name: 'IsChangedYear' },
        { name: 'HouseNumber' },
        { name: 'Point' },
        { name: 'Sum' },
        { name: 'Changes' },
        { name: 'Remark' },
        { name: 'StructuralElements' },
        { name: 'EntranceNum' },
        { name: 'KPKR' },
        { name: 'Hidden' },
        { name: 'IsSubProgram' }
    ]
});