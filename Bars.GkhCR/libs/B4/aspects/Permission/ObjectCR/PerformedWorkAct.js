Ext.define('B4.aspects.permission.objectcr.PerformedWorkAct', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.performedworkactobjectcrstateperm',

    permissions: [
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Edit', applyTo: 'b4savebutton', selector: 'perfactwin' },
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Work', applyTo: '#sfTypeWorkCr', selector: 'perfactwin' },
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.DocumentNum', applyTo: '#tfDocumentNum', selector: 'perfactwin' },
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Sum', applyTo: '#nfSum', selector: 'perfactwin' },
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Volume', applyTo: '#nfVolume', selector: 'perfactwin' },
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.DateFrom', applyTo: '#dfDateFrom', selector: 'perfactwin' },
        {
            name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.CostFile', applyTo: 'b4filefield[name="CostFile"]', selector: 'perfactwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.DocumentFile', applyTo: 'b4filefield[name="DocumentFile"]', selector: 'perfactwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.AdditionFile', applyTo: 'b4filefield[name="AdditionFile"]', selector: 'perfactwin',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Rec.Edit', applyTo: '#btnSaveRecs', selector: 'perfworkactrecgrid' },
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Rec.Delete', applyTo: 'b4deletecolumn', selector: 'perfworkactrecgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment.View', applyTo: '#tabPayment', selector: 'perfactwin',
            applyBy: function (component, allowed) {
                if (allowed) component.tab.show();
                else component.tab.hide();
            }
        },
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment.Create', applyTo: 'b4addbutton', selector: 'perfworkactpaymentgrid' },
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment.Edit', applyTo: '[action=savePayments]', selector: 'perfworkactpaymentgrid' },
        { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment.Delete', applyTo: 'b4deletecolumn', selector: 'perfworkactpaymentgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});