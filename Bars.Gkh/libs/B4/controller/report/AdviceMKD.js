﻿Ext.define('B4.controller.report.AdviceMKD', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.AdviceMKDPanel',
    mainViewSelector: '#reportAdviceMKDPanel',

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

    municipalityTriggerFieldSelector: '#reportAdviceMKDPanel #tfMunicipality',
    controlTypeSelector: '#reportAdviceMKDPanel #cbControlType',
        
    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportAdviceMKDPanelMultiselectwindowaspect',
            fieldSelector: '#reportAdviceMKDPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportAdviceMKDPanelMunicipalitySelectWindow',
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
        var coType = Ext.ComponentQuery.query(this.controlTypeSelector)[0];
        return (coType && coType.isValid());
    },

    getParams: function () {
        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var cType = Ext.ComponentQuery.query(this.controlTypeSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            controlType: (cType ? cType.getValue() : null)
        };
    }
});