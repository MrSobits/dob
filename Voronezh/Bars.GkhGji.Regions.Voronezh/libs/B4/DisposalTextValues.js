Ext.define('B4.DisposalTextValues', {

    singleton: true,
    loadedCss: {},

    //Именительный падеж 
    getSubjectiveCase: function () {
        return 'Материалы правонарушения';
    },

    //Именительный падеж Приказ напроверку предписания 
    getSubjectiveForPrescriptionCase: function () {
        return 'Материалы правонарушения';
    },

    //Именительный множественный падеж 
    getSubjectiveManyCase: function () {
        return 'Материалы правонарушения';
    },

    //Родительный множественный падеж 
    getGenetiveManyCase: function () {
        return 'Материалы правонарушения';
    },
    
    //Дательный падеж 
    getDativeCase: function () {
        return 'Материалы правонарушения';
    }
});