Ext.define('B4.aspects.GridEditWindow', {
    extend: 'B4.aspects.GridEditForm',

    alias: 'widget.grideditwindowaspect',

    editWindowView: null,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.on('aftersetformdata', this.onAfterSetFormData, this);

        this.on('savesuccess', this.onSaveSuccess, this);
    },

    init: function (controller) {
        var me = this,
            actions = {};

        me.test = [];

        me.callParent(arguments);

        if (me.editFormSelector) {
            actions[me.editFormSelector + ' b4closebutton'] = {
                'click': {
                    fn: me.closeWindowHandler,
                    scope: me
                }
            };
        }

        controller.control(actions);
    },

    getForm: function () {
        var me = this,
            editWindow;

        if (me.editFormSelector) {
            editWindow = me.componentQuery(me.editFormSelector);

            if (editWindow && !editWindow.getBox().width) {
                editWindow = editWindow.destroy();
            }

            if (!editWindow) {

                editWindow = me.controller.getView(me.editWindowView).create(
                    {
                        constrain: true,
                        renderTo: B4.getBody().getActiveTab().getEl(),
                        closeAction: 'destroy',
                        ctxKey: me.controller.getCurrentContextKey ? me.controller.getCurrentContextKey() : ''
                    });

                editWindow.show();
            }
            
            return editWindow;
        }
    },

    closeWindowHandler: function () {
        var form = this.getForm();
        if (form) {
            form.close();
        }
    },

    onAfterSetFormData: function (aspect, rec, form) {
        if (form) {
            form.show();
        }
    },

    onSaveSuccess: function (aspect) {
        var form = aspect.getForm();
        if (form) {
            form.close();
        }
    },

    editRecord: function (record) {
        var me = this,
            id = record ? record.getId() : null,
            model = me.getModel(record);
        if (id) {
            model.load(id, {
                success: function (rec) {
                    me.setFormData(rec);
                },
                scope: me
            });
        } else {
            me.setFormData(new model({ Id: 0 }));
        }
    }
});