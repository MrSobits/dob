Ext.define('B4.ux.form.field.GkhTimeField', {
    extend: 'Ext.form.field.Time',

    alias: 'widget.gkhtimefield',

    editable: false,
    minValue: '00:00',
    maxValue: '23:00',
    increment: 60,
    format: 'H:i',
    //altFormats: 'Y-m-d\\TH:i:s',
    lastValue: new Date(),

    setValue: function(value) {
        if (typeof value == "string") {
            value = Ext.Date.parse(value, 'H:i:s');
        }

        this.callParent([value]);
    },

    getValue: function () {
        if (this.value) {
            return Ext.Date.format(this.value, this.format);
        }

        return null;
    },

    isEqual: function (date1, date2) {
        if (date1 && date1.getTime && date2 && date2.getTime) {
            return (date1.getTime() === date2.getTime());
        }

        return date1 == date2;
    }
});