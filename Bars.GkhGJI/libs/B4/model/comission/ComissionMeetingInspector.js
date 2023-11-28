Ext.define('B4.model.comission.ComissionMeetingInspector', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ComissionMeetingInspector'
    },
    fields: [
        { name: 'Id' },
        { name: 'ComissionMeeting'},
        { name: 'Inspector'},
        { name: 'Description'},
        { name: 'YesNoNotSet', defaultValue: 30 },
        { name: 'Position' },
        { name: 'NotMemberPosition' },
        { name: 'TypeCommissionMember'}
    ]
});