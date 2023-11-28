Ext.define('B4.controller.report.FillGenInformationHousesRankingYK', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.FillGenInformationHousesRankingYKPanel',
    mainViewSelector: '#fillGenInformationHousesRankingYKPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#fillGenInformationHousesRankingYKPanel #tfMunicipality'
        },
        {
            ref: 'PeriodDiSelectField',
            selector: '#fillGenInformationHousesRankingYKPanel #sfPeriodDi'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'fillGenInformationHousesRankingYKMultiselectwindowaspect',
            fieldSelector: '#fillGenInformationHousesRankingYKPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#fillGenInformationHousesRankingYKMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    validateParams: function () {
        return true;
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var periodDiSelectField = this.getPeriodDiSelectField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            periodDi: (periodDiSelectField ? periodDiSelectField.getValue() : null)
        };
    }
});