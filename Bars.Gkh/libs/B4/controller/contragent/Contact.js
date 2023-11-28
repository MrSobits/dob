Ext.define('B4.controller.contragent.Contact', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.EntityChangeLog'
    ],

    models: [
        'contragent.Contact'
    ],

    stores: [
        'contragent.Contact'
    ],

    views: [
        'contragent.ContactPanel',
        'contragent.ContactEditWindow',
        'contragent.ContactCasesPanel'
    ],

    mainView: 'contragent.ContactPanel',
    mainViewSelector: 'contragentcontactpanel',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'contragentcontactpanel'
        }
    ],

    parentCtrlCls: 'B4.controller.contragent.Navi',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.Contragent.Register.Contact.Create', applyTo: 'b4addbutton', selector: 'contragentContactGrid' },
                { name: 'Gkh.Orgs.Contragent.Register.Contact.Edit', applyTo: 'b4savebutton', selector: '#contragentContactEditWindow' },
                { name: 'Gkh.Orgs.Contragent.Register.Contact.Delete', applyTo: 'b4deletecolumn', selector: 'contragentContactGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Register.Contact.ChangeLog_View',
                    applyTo: 'entitychangeloggrid',
                    selector: 'contragentcontactpanel',
                    applyBy: function (component, allowed) {
                        var tabPanel = component.ownerCt;
                        if (allowed) {
                            tabPanel.showTab(component);
                        } else {
                            tabPanel.hideTab(component);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'contragentContactGridWindowAspect',
            gridSelector: 'contragentContactGrid',
            editFormSelector: '#contragentContactEditWindow',
            storeName: 'contragent.Contact',
            modelName: 'contragent.Contact',
            editWindowView: 'contragent.ContactEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.Contragent = asp.controller.getContextValue(asp.controller.getMainComponent(), 'contragentId');
                    }
                }
            }
        },
        {
            xtype: 'entitychangelogaspect',
            gridSelector: 'contragentcontactpanel entitychangeloggrid',
            entityType: 'Bars.Gkh.Entities.ContragentContact',
            inheritEntityChangeLogCode: 'ContragentContact',
            getEntityId: function() {
                var asp = this,
                    me = asp.controller;
                return me.getContextValue(me.getMainView(), 'contragentId');
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('contragent.Contact').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentcontactpanel');

        me.bindContext(view);
        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view, 'contragent_info');

        this.getStore('contragent.Contact').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.contragentId = me.getContextValue(me.getMainComponent(), 'contragentId');
    }
});