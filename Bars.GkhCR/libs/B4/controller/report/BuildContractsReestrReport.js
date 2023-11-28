Ext.define('B4.controller.report.BuildContractsReestrReport', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.BuildContractsReestrReportPanel',
    mainViewSelector: '#buildContractsReestrReportPanel',

    requires: [
        'B4.form.ComboBox'
    ],

    views: [
        'report.BuildContractsReestrReportPanel'
    ],

    refs: [
        {
            ref: 'ProgramCrSelectField',
            selector: '#buildContractsReestrReportPanel #sfProgramCr'
        }
    ],
   

    validateParams: function () {
        var prCrId = this.getProgramCrSelectField();
        return (prCrId && prCrId.isValid());
    },

    init: function () {
        this.control({
            '#buildContractsReestrReportPanel #sfProgramCr': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            }
        });
        this.callParent(arguments);
    },

    getParams: function () {
        var programmField = this.getProgramCrSelectField();

        //получаем компонент
        return {
            programCrId: (programmField ? programmField.getValue() : null)
        };
    }
});