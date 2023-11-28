Ext.define('B4.model.BaseJurPerson', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseJurPerson'
    },
    fields: [
        { name: 'Municipality', defaultValue: null },
        { name: 'Plan', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'InspectionNumber' },
        { name: 'UriRegistrationNumber' },
        { name: 'UriRegistrationDate' },
        { name: 'DateStart' },
        { name: 'CountDays' },
        { name: 'CountHours' },
        { name: 'Reason' },
        { name: 'JurPersonInspectors' },
        { name: 'JurPersonZonalInspections' },
        { name: 'TypeBase', defaultValue: 30 },
        { name: 'TypeBaseJuralPerson', defaultValue: 10 },
        { name: 'TypeFact', defaultValue: 10 },
        { name: 'TypeForm', defaultValue: 10 },
        { name: 'DisposalNumber' },
        { name: 'InspectorNames' },
        { name: 'ZonalInspectionNames' },
        { name: 'DateStart' },
        { name: 'RealityObjectCount' },
        { name: 'State', defaultValue: null },
        { name: 'AnotherReasons' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'ControlType', defaultValue: 10 }
    ]
});