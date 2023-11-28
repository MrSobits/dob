Ext.define('B4.model.protocol197.Petition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol197Petition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Protocol197', defaultValue: null },
        { name: 'PetitionAuthorFIO' },
        { name: 'PetitionAuthorDuty' },
        { name: 'Workplace' },
        { name: 'Inspector' },
        { name: 'PetitionDate' },
        { name: 'PetitionText' },
        { name: 'Aprooved', defaultValue: 0 },
        { name: 'PetitionDecisionText' }
    ]
});