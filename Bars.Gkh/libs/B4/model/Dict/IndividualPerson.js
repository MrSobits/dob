Ext.define('B4.model.dict.IndividualPerson', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'IndividualPerson'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Fio' },
        { name: 'INN' },
        { name: 'Job' },
        { name: 'PassportIssued' },
        { name: 'PassportNumber' },
        { name: 'PassportSeries' },
        { name: 'PlaceResidence' },
        { name: 'BirthPlace' },
        { name: 'DateBirth' },
        { name: 'DateBirthTxt' },
        { name: 'DateIssue' },
        { name: 'FiasRegistrationAddress' },
        { name: 'FiasFactAddress' },
        { name: 'ActuallyResidence' },
        { name: 'DepartmentCode' },
        { name: 'SocialStatus' },
        { name: 'DependentsNumber' },

        { name: 'PlaceResidenceOutState' },
        { name: 'ActuallyResidenceOutState' },
        { name: 'IsPlaceResidenceOutState', defaultValue: false },
        { name: 'IsActuallyResidenceOutState', defaultValue: false },

        { name: 'FamilyStatus' },

        { name: 'PhoneNumber' }
    ]
});