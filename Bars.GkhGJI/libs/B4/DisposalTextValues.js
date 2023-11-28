Ext.define('B4.DisposalTextValues', {

    singleton: true,
    loadedCss: {},

    //Именительный падеж 
    getSubjectiveCase: function () {
        return 'Дело об административных правонарушениях';
    },

    //Именительный падеж Приказ напроверку предписания 
    getSubjectiveForPrescriptionCase: function () {
        return 'Распоряжение на проверку предписания';
    },

    //Именительный множественный падеж 
    getSubjectiveManyCase: function () {
        return 'Дела об административных правонарушениях';
    },

    //Родительный множественный падеж 
    getGenetiveManyCase: function () {
        return 'Дела об административных правонарушениях';
    },
    
    //Дательный падеж 
    getDativeCase: function () {
        return 'Делу об административных правонарушениях';
    }
});