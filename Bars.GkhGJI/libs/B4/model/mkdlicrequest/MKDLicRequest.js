Ext.define('B4.model.mkdlicrequest.MKDLicRequest', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicRequest'
    },
    fields: [
        { name: 'Id' },
        { name: 'State'},
        { name: 'ExecutantDocGji'},
        { name: 'Contragent'},
        { name: 'StatmentContragent' },
        { name: 'PhysicalPerson' },
        { name: 'StatementDate' }, 
        { name: 'StatementNumber' }, 
        { name: 'MKDLicTypeRequest' }, 
        { name: 'Inspector' }, 
        { name: 'RealityObject' },
        { name: 'RealityObjects' },
        { name: 'LicStatementResult', defaultValue: 0 },
        { name: 'Description' },
        { name: 'LicStatementResultComment' },
        { name: 'ConclusionNumber' },
        { name: 'Objection', defaultValue: false },
        { name: 'ConclusionDate' },
        { name: 'ObjectionResult', defaultValue: 0 }      
    ]
});