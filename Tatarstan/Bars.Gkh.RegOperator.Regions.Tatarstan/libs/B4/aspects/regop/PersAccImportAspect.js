// Этот "псевдо"-аспект сделан для того чтобы не переопределять в регионе контроллер чисто из за этого аспекта


Ext.define('B4.aspects.regop.PersAccImportAspect', {
    extend: 'B4.aspects.GkhButtonImportAspect',

    alias: 'widget.persaccimportaspect',

    requires: ['B4.view.regop.personal_account.PersonalAccountGrid',
        'B4.view.import.PersAccImportWindow'
    ],

    name: 'personalAccImportAspect',
    buttonSelector: 'paccountgrid #btnImport',
    codeImport: 'PersonalAccountImport',
    windowImportView: 'import.PersAccImportWindow',
    windowImportSelector: 'persaccimportwin',
    listeners: {
        aftercreatewindow: function (window, importId) {
            /*var chkBox = window.down('[name=replaceData]');
              if (importId == 'ThirdPartyPersonalAccountImport' || importId == 'BenefitsCategoryImport') {
                  chkBox.setVisible(true);
              } else {
                  chkBox.setVisible(false);
            }*/
        }
    }
});