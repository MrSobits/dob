Ext.define('B4.model.dict.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'municipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Group' },
        { name: 'Name' },
        { name: 'Okato' },
        { name: 'Description' },
        { name: 'FederalNumber' },
        { name: 'Cut' },
        { name: 'FiasId' },
        { name: 'Level', defaultValue: 30 },
        { name: 'DinamicFias', defaultValue: null },
        { name: 'Code' },
        { name: 'RegionGuid' },
        { name: 'RegionName' },
        { name: 'CheckCertificateValidity' },
        { name: 'Oktmo' },
        { name: 'Bik' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'Uin' },
        { name: 'UFC' },
        { name: 'PersonalAcc' },
        { name: 'BankName' },
        { name: 'CorrespAcc' },
        { name: 'Index' },
        { name: 'ParentMo' },
        { name: 'HasChildren', defaultValue: false}
    ]
});