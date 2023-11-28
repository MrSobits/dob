Ext.define('B4.GjiTextValuesOverride',
    {
        extend: 'B4.TextValues',
        singleton: true,
        overrideItems: {
            'комисиия, рассмотревшая обращение': 'Отдел',
            'Административная комиссия': 'Отдел',
            'зональные жилищные инспекции': 'Отделы'
        }
    });