Ext.define('B4.controller.realityobj.TechnicalMonitoring',
    {
        extend: 'B4.controller.MenuItemController',

        requires: [
            'B4.aspects.GridEditWindow',
            'B4.enums.YesNo.Yes',
            'B4.aspects.permission.GkhStatePermissionAspect'
        ],

        models: [
            'realityobj.TechnicalMonitoring'
        ],

        stores: [
            'realityobj.TechnicalMonitoring'
        ],

        views: [
            'realityobj.technicalmonitoring.TechnicalMonitoringGrid',
            'realityobj.technicalmonitoring.TechnicalMonitoringEditWindow'
        ],

        mixins: {
            context: 'B4.mixins.Context'
        },

        refs: [
            {
                ref: 'mainView',
                selector: 'technicalmonitoringgrid'
            }
        ],

        parentCtrlCls: 'B4.controller.realityobj.Navi',

        aspects: [
            {
                xtype: 'gkhstatepermissionaspect',
                name: 'TechnicalMonitoringPermission',
                permissions: [
                    { name: 'Gkh.RealityObject.Register.TechnicalMonitoring.Create', applyTo: 'b4addbutton', selector: 'technicalmonitoringgrid' },
                    { name: 'Gkh.RealityObject.Register.TechnicalMonitoring.Edit', applyTo: 'b4savebutton', selector: 'technicalmonitoringeditwindow' },
                    {
                        name: 'Gkh.RealityObject.Register.TechnicalMonitoring.Delete', applyTo: 'b4deletecolumn', selector: 'technicalmonitoringgrid',
                        applyBy: function (component, allowed) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                ]
            },
            {
                xtype: 'grideditwindowaspect',
                name: 'TechnicalMonitoringGridWindowAspect',
                gridSelector: 'technicalmonitoringgrid',
                editFormSelector: 'technicalmonitoringeditwindow',
                modelName: 'realityobj.TechnicalMonitoring',
                editWindowView: 'realityobj.technicalmonitoring.TechnicalMonitoringEditWindow',
                listeners: {
                    getdata: function (me, record) {
                        if (!record.data.Id) {
                            record.set('RealityObject',
                                me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                        }
                    },
                    aftersetformdata: function (me) {
                        me.controller.getAspect('TechnicalMonitoringPermission').setPermissionsByRecord({ getId: function() {
                             return me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                        } });
                    }
                }
            }
        ],

        init: function() {
            var me = this;

            me.callParent(arguments);
        },

        index: function(id) {
            var me = this,
                view = me.getMainView() || Ext.widget('technicalmonitoringgrid'),
                store = view.getStore();

            me.bindContext(view);
            me.setContextValue(view, 'realityObjectId', id);
            me.application.deployView(view, 'reality_object_info');

            store.clearFilter(true);
            store.filter('realityObjectId', id);

            this.getAspect('TechnicalMonitoringPermission').setPermissionsByRecord({ getId: function () { return id; } });
        }
    });