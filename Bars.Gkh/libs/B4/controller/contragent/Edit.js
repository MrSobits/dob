Ext.define('B4.controller.contragent.Edit', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.FieldRequirementAspect',
        'B4.view.contragent.ActivityStageEditWindow',
        'B4.view.contragent.ActivityStageGrid',
        'B4.aspects.GridEditWindow',
        'B4.enums.ActivityStageOwner',
        'B4.enums.ActivityStageType',
        'B4.aspects.EntityChangeLog'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'Contragent',
        'contragent.ActivityStage'
    ],

    views: [
        'contragent.GeneralInfoPanel',
        'SelectWindow.MultiSelectWindow',
        'B4.view.contragent.ActivityStageEditWindow',
        'contragent.ActivityStageGrid'
    ],

    stores: [
        'contragent.ActivityStage'
    ],

    mainView: 'contragent.GeneralInfoPanel',
    mainViewSelector: 'contragentgeneralinfopanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'contragentgeneralinfopanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.Contragent.Edit', applyTo: 'b4savebutton', selector: 'contragentEditPanel' },
                {
                    name: 'Gkh.Orgs.Contragent.ChangeLog_View',
                    applyTo: 'entitychangeloggrid',
                    selector: 'contragentgeneralinfopanel',
                    applyBy: function (component, allowed) {
                        var tabPanel = component.ownerCt;
                        if (allowed) {
                            tabPanel.showTab(component);
                        } else {
                            tabPanel.hideTab(component);
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Register.Provider.ProviderCode_Edit',
                    applyTo: 'textfield[name=ProviderCode]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        component.setReadOnly(allowed);
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Register.Provider.ProviderCode_View',
                    applyTo: 'textfield[name=ProviderCode]',
                    selector: 'contragentEditPanel',
                    applyBy: function(component, allowed) {
                        var fieldSet = component.up('fieldset'),
                            button = fieldSet.down('button[action=GenerateProviderCode]');

                        component.setVisible(allowed);

                        if (button.isVisible() || allowed) {
                            fieldSet.show();
                        } else {
                            fieldSet.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Register.Provider.ProviderCode_Generate',
                    applyTo: 'button[action=GenerateProviderCode]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        var fieldSet = component.up('fieldset'),
                            textField = fieldSet.down('textfield[name=ProviderCode]');

                        component.setVisible(allowed);

                        if (textField.isVisible() || allowed) {
                            fieldSet.show();
                        } else {
                            fieldSet.hide();
                        }
                    }
                },

                {
                    name: 'Gkh.Orgs.Contragent.Field.FrguRegNumber_View',
                    applyTo: 'textfield[name=FrguRegNumber]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.FrguRegNumber_Edit',
                    applyTo: 'textfield[name=FrguRegNumber]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.FrguOrgNumber_View',
                    applyTo: 'textfield[name=FrguOrgNumber]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.FrguOrgNumber_Edit',
                    applyTo: 'textfield[name=FrguOrgNumber]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.FrguServiceNumber_View',
                    applyTo: 'textfield[name=FrguServiceNumber]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.FrguServiceNumber_Edit',
                    applyTo: 'textfield[name=FrguServiceNumber]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.AddressCoords_View',
                    applyTo: 'textfield[name=Coords]',
                    selector: '#fiasSelectAddressWindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.AddressCoords_Edit',
                    applyTo: 'textfield[name=Coords]',
                    selector: '#fiasSelectAddressWindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.ActivityStage_View',
                    applyTo: 'activitystagegrid',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.ActivityStage_Edit',
                    applyTo: 'b4addbutton',
                    selector: 'contragentEditPanel activitystagegrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.ActivityStage_Edit',
                    applyTo: 'b4savebutton',
                    selector: 'activitystageeditwincontragent',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },

                {
                    name: 'Gkh.Orgs.Contragent.Field.TimeZoneType_View',
                    applyTo: '[name=TimeZoneType]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.TimeZoneType_Edit',
                    applyTo: '[name=TimeZoneType]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },

                {
                    name: 'Gkh.Orgs.Contragent.Field.Okogu_View',
                    applyTo: '[name=Okogu]',
                    selector: 'contragentEditPanel',
                    applyBy: function(component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.Okogu_Edit',
                    applyTo: '[name=Okogu]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.Okfs_View',
                    applyTo: '[name=Okfs]',
                    selector: 'contragentEditPanel',
                    applyBy: function(component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Field.Okfs_Edit',
                    applyTo: '[name=Okfs]',
                    selector: 'contragentEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'Gkh.Orgs.Contragent.Field.Inn_Rqrd', applyTo: '[name=Inn]', selector: 'contragentEditPanel' },
                { name: 'Gkh.Orgs.Contragent.Field.Kpp_Rqrd', applyTo: '[name=Kpp]', selector: 'contragentEditPanel' },
                { name: 'Gkh.Orgs.Contragent.Field.FiasJuridicalAddress_Rqrd', applyTo: '[name=FiasJuridicalAddress]', selector: 'contragentEditPanel' },
                { name: 'Gkh.Orgs.Contragent.Field.FiasFactAddress_Rqrd', applyTo: '[name=FiasFactAddress]', selector: 'contragentEditPanel' },
                { name: 'Gkh.Orgs.Contragent.Field.Ogrn_Rqrd', applyTo: '[name=Ogrn]', selector: 'contragentEditPanel' },
                { name: 'Gkh.Orgs.Contragent.Field.Oktmo_Rqrd', applyTo: '[name=Oktmo]', selector: 'contragentEditPanel' },
                { name: 'Gkh.Orgs.Contragent.Field.FrguRegNumber_Rqrd', applyTo: '[name=FrguRegNumber]', selector: 'contragentEditPanel' },
                { name: 'Gkh.Orgs.Contragent.Field.FrguOrgNumber_Rqrd', applyTo: '[name=FrguOrgNumber]', selector: 'contragentEditPanel' },
                { name: 'Gkh.Orgs.Contragent.Field.FrguServiceNumber_Rqrd', applyTo: '[name=FrguServiceNumber]', selector: 'contragentEditPanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'contragentEditPanelAspect',
            editPanelSelector: 'contragentEditPanel',
            modelName: 'Contragent',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' button'] = { 'click': { fn: this.btnClick, scope: this } };

                //actions[this.editPanelSelector + ' #tfcontragentOutsideAddress'] = { 'change': { fn: this.onChangeAddressOutside, scope: this} };
                //actions[this.editPanelSelector + ' #sfCtrgOrgForm'] = { 'change': { fn: this.onChangeOrgForm, scope: this } };
                actions[this.editPanelSelector + ' #sfParent'] = { 'beforeload': { fn: this.onBeforeLoadParent, scope: this } };

                actions[this.editPanelSelector + ' button[action=GenerateProviderCode]'] = { 'click': { fn: this.onProviderCodeButtonClick, scope: this } }
            },
            disableField: function (fieldSelector, disabled) {
                var field = this.getPanel().down(fieldSelector);
                field.allowBlank = disabled;
                field.setDisabled(disabled);
            },
            onBeforeLoadParent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.contragentId = this.controller.getContextValue(this.controller.getMainComponent(), 'contragentId');
            },
            onChangeOrgForm: function (field, newValue) {
                var form = this.getPanel();
                if (newValue) {
                    if (newValue.Code == '98') {
                        this.disableField('#tfCtrgInn', true);
                        this.disableField('#tfCtrgKpp', true);
                        this.disableField('#tfCtrgOgrn', true);
                    } else {
                        this.disableField('#tfCtrgInn', false);
                        newValue.Code = newValue.Code.replace(new RegExp(" ", 'g'), " "); // замена пустого символа на пробел
                        newValue.Code == '91' || newValue.Code == '5 01 02' ? form.down('#tfCtrgKpp').allowBlank = true : this.disableField('#tfCtrgKpp', false);
                        this.disableField('#tfCtrgOgrn', false);
                    }
                }
            },
            onChangeAddressOutside: function (field, newValue) {
                var panel = this.getPanel(),
                    factAddressField = panel.down('b4fiasselectaddress[name=FiasFactAddress]'),
                    jurAddressField = panel.down('b4fiasselectaddress[name=FiasJuridicalAddress]');

                if (newValue) {
                    factAddressField.allowBlank = true;
                    jurAddressField.allowBlank = true;
                } else {
                    factAddressField.allowBlank = false;
                    jurAddressField.allowBlank = false;
                }
                jurAddressField.isValid();
                factAddressField.isValid();

            },

            btnClick: function (btn) {
                var pasteField = null;

                if (btn.itemId == 'btnCopyButtonFactAddress') {
                    pasteField = Ext.ComponentQuery.query('#contragentFiasFactAddressField')[0];
                }
                else if (btn.itemId == 'btnCopyButtonAddressOutsideSubject') {
                    pasteField = Ext.ComponentQuery.query('#contragentFiasAddressOutsideSubjectField')[0];
                }
                else if (btn.itemId == 'btnCopyButtonMailingAddress') {
                    pasteField = Ext.ComponentQuery.query('#contragentFiasMailingAddressField')[0];
                }

                if (pasteField)
                    this.copyPaste(pasteField);
            },

            copyPaste: function (pasteField) {
                var jurAddressField = Ext.ComponentQuery.query('#contragentFiasJuridicalAddressField')[0];
                var copy = jurAddressField.getValue();

                if (!copy) {
                    Ext.Msg.alert('Внимание', 'Необходимо заполнить юридический адрес!');
                    return;
                }

                var currAdtr = pasteField.getValue();
                var newadrr = {
                    Id: currAdtr ? currAdtr.Id : 0,
                    AddressName: copy.AddressName,
                    PlaceCode: copy.PlaceCode,
                    PlaceGuidId: copy.PlaceGuidId,
                    PlaceName: copy.PlaceName,
                    PlaceAddressName: copy.PlaceAddressName,
                    StreetCode: copy.StreetCode,
                    StreetGuidId: copy.StreetGuidId,
                    StreetName: copy.StreetName,
                    House: copy.House,
                    Housing: copy.Housing,
                    Building: copy.Building,
                    Flat: copy.Flat,
                    Coordinate: copy.Coordinate,
                    PostCode: copy.PostCode
                };

                pasteField.setValue(newadrr);
            },
            
            onProviderCodeButtonClick: function(button) {
                var me = this,
                    panel = me.getPanel(),
                    tfProviderName = panel.down('textfield[name=ShortName]'),
                    tfProviderCode = panel.down('textfield[name=ProviderCode]'),
                    params = {
                        providerName: tfProviderName ? tfProviderName.getValue() : null
                    };

                me.controller.mask('Генерация кода', panel);

                B4.Ajax.request({
                    url: B4.Url.action(button.action, 'Contragent'),
                    method: 'POST',
                    params: params
                }).next(function (res) {
                    var data = Ext.decode(res.responseText);

                    if (tfProviderCode) {
                        tfProviderCode.setValue(data.ProviderCode);
                    }

                    me.controller.unmask();
                }).error(function (e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка получения кода поставщика!', e.message || e);
                });
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'contragentActivityStageGridEditWindowAspect',
            gridSelector: 'contragentEditPanel activitystagegrid',
            editFormSelector: 'activitystageeditwincontragent',
            modelName: 'contragent.ActivityStage',
            editWindowView: 'contragent.ActivityStageEditWindow',
            getRecordBeforeSave: function (record) {
                if (record && record.get('Id') > 0) {
                    return record;
                }

                var me = this,
                    mainView = me.controller.getMainView(),
                    contragentId = me.controller.getContextValue(mainView, 'contragentId');
                record.set('EntityId', contragentId);
                record.set('EntityType', B4.enums.ActivityStageOwner.Contragent);
                return record;
            },
            otherActions: function (actions) {
                actions['activitystageeditwincontragent [name=ActivityStageType]'] = {
                     'change': {
                         fn: this.disableFields, scope: this
                     }
                };
            },
            disableFields: function (field, value) {
                var grid = field.up(),
                    dateField = grid.down('[name=DateEnd]'),
                    disabled = !(value === B4.enums.ActivityStageType.Liquidated || value === B4.enums.ActivityStageType.Reorganized);

                dateField.setDisabled(disabled);
            }
        },
        {
            xtype: 'entitychangelogaspect',
            gridSelector: 'contragentgeneralinfopanel entitychangeloggrid',
            entityType: 'Bars.Gkh.Entities.Contragent',
            getEntityId: function() {
                var asp = this,
                    me = asp.controller;
                return me.getContextValue(me.getMainView(), 'contragentId');
            }
        }
    ],

    init: function() {
        var me = this,
            actions = {};

        actions['contragentEditPanel activitystagegrid'] = { 'store.beforeload': { fn: me.onBeforeLoadActivityStage, scope: me } };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentgeneralinfopanel'),
            stageGrid = view.down('activitystagegrid');

        me.bindContext(view);
        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view, 'contragent_info');

        me.getAspect('contragentEditPanelAspect').setData(id);
        stageGrid.getStore().load();
    },

    onBeforeLoadActivityStage: function (store, operation) {
        var me = this;
        operation.params.entityId = me.getContextValue(me.getMainView(), 'contragentId');
        operation.params.entityType = B4.enums.ActivityStageOwner.Contragent;
    }
});