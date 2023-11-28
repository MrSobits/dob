Ext.define('B4.aspects.GkhPassport', {
    extend: 'B4.base.Aspect',

    requires: [
        'B4.Url',
        'B4.Ajax',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.base.Model'
    ],

    alias: 'widget.gkhpassportaspect',

    init: function () {

        this.callParent(arguments);
        //иницилизируем коллекцию экшенов, что бы не дублировать подписки в дальнейшем проверяем есть ли
        //подписка в коллекции или нет
        this.actionsCollection = new Ext.util.MixedCollection();
    },

    //метод создающий метаструктуру. Точка входа. data - сериализованый json
    createMetastruct: function (data) {

        //инициализируем массив селекторы компонентов. При создании добавляем в массив. 
        //При сохранение сохраняем все формы которые в массиве селекторов
        this.arraySelectors = [];

        this.editors = data.editors;

        var mainComponent = this.controller.getMainComponent();
        var components = data.form.Components;

        ////подписываемся на сохранение
        var actions = {};
        var mainPanelSaveSelector = '#' + this.controller.mainPanelSelector + ' b4savebutton';
        if (!this.actionsCollection.containsKey(mainPanelSaveSelector)) {
            actions[mainPanelSaveSelector] = { 'click': { fn: this.save, scope: this } };
            this.actionsCollection.add(mainPanelSaveSelector, mainPanelSaveSelector);
        }
        this.controller.control(actions);

        var asp = this;

        //Смотрим какие компоненты пришли. По типу строим их и наполняем
        //Panel - form, Grid - grid(inline), InlineGrid - grid(inline с кнопкой добавить), PropertyGrid - propertygrid
        mainComponent.removeAll();

        Ext.Array.each(components, function (value) {
            var component;
            switch (value.Type) {
                case 'Panel':
                    component = asp.createFormComponent(value);
                    break;
                case 'Grid':
                    component = asp.createGridComponent(value, false);
                    break;
                case 'InlineGrid':
                    component = asp.createGridComponent(value, true);
                    break;
                case 'PropertyGrid':
                    component = asp.createPropertyGridComponent(value);
                    break;
            }


            if (!Ext.isEmpty(component)) {
                mainComponent.add(component);
                mainComponent.doLayout();
            }
        });

    },

    //метод создания грида. withAddButton  - с кнопкой добавить
    createGridComponent: function (component, withAddButton) {
        //парсим json
        component = this.convertGridComponent(component);

        var gridSelector = 'grid' + component.Id;


        this.arraySelectors.push({ type: 'grid', selector: '#' + gridSelector });

        //формируем конфиг
        var config = {
            title: component.Title,
            scroll: 'horizontal',
            withAddButton: withAddButton,
            itemId: gridSelector,
            store: component.store,
            columns: component.gridColumns,
            sortableColumns: false,
            padding: 5,
            flex: component.Flex,
            height: component.Height,
            anchor: component.Anchor,
            width: component.Width ? component.Width : '100%',
            plugins: [Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 1,
                pluginId: 'cellEditing'
            })],
            listeners: {
                render: function (grid) {
                    if (!component.NoSort) {
                        grid.getStore().sort(grid.columns[0].dataIndex, 'ASC');                        
                    }
                }
            },
            viewConfig: {
                markDirty: false
            }
        };

        //добавляем кнопки добавить и удалить. Подписываемся на событие добавления
        if (withAddButton) {
            if (!component.Height)
                config.height = 500;
            
            config.columns.push(
                {
                    xtype: 'b4deletecolumn',
                    icon: B4.Url.content('content/img/icons/delete.png'),
                    width: 20,
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var grid = Ext.ComponentQuery.query('#' + gridSelector)[0];
                        var store = grid.getStore();
                        store.remove(rec);
                    }
                });

            config.dockedItems = [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                }
                            ]
                        }
                    ]
                }
            ];

            var actions = {};
            var addSelector = '#' + gridSelector + ' b4addbutton';
            if (!this.actionsCollection.containsKey(addSelector)) {
                actions[addSelector] = { 'click': { fn: this.addRow, scope: this, selector: '#' + gridSelector } };
                this.actionsCollection.add(addSelector, addSelector);
            }
            this.controller.control(actions);
        }

        var componentInst = Ext.create(component.type, config);
        return componentInst;
    },

    //парсим json для grid
    convertGridComponent: function (component) {

        component.type = 'Ext.grid.Panel';
        var asp = this;

        //формируем метаструктуру полей
        var storeFields = [];
        var recFields = [];
        component.gridColumns = [];
        Ext.Array.each(component.Columns, function (value) {
            var field = {};
            if (value.IsId) {
                field.name = 'Id';
                recFields.push({ name: 'Id' });
            }
            else {
                field.name = value.Code;
                recFields.push({ name: value.Code });

                var renderer = asp.createRenderer(value.Editor);
                
                //накапливаем колонки грида
                var col = {
                    xtype: 'gridcolumn',
                    dataIndex: field.name,
                    tooltip: value.Title,
                    text: value.Title,
                    editor: value.Editable ? (value.MaxValue != 0 ? asp.createField(value.Editor, value.MaxValue, value.MinValue) : asp.createField(value.Editor)) : null,
                    typeEditor: value.Editor,
                    hidden: value.Hidden,
                    renderer: function (val, meta, record) {
                        var res = val;
                        if (renderer)
                            res = renderer(val);
                        if (res)
                            meta.tdAttr = 'data-qtip="' + res + '"';
                        return res;
                    }
                };

                var widthCol = +value.Width;
                if (widthCol > 0) {
                    col.width = widthCol;

                    col.header = Ext.apply(col.header, {
                        style: {
                            whiteSpace: 'normal !important',
                            lineHeight: '15px'
                        }
                    });

                } else {
                    col.flex = 1;
                }



                component.gridColumns.push(col);
            }

            //накапливаем поля стора
            storeFields.push(field);
        });

        //формируем строки и заполняем их.
        var storeData = [];
        var idCollection = new Ext.util.MixedCollection();
        var functionGetBool = this.getBool;

        var modelName = 'B4.model.' + component.Id;

        Ext.define(modelName, {
            extend: 'B4.base.Model',
            idProperty: 'Id',
            fields: recFields
        });

        Ext.Array.each(component.Cells, function (cell) {
            var rec,
                celRowCollumnPair = cell.Code.split(':'), // 0 - row , 1 - code
                row = celRowCollumnPair[0],
                code = celRowCollumnPair[1],
                columns = this.Columns.filter(function (el) { return el.Code == code; }) || [],
                columnMeta = columns[0],
                specModel;

            if (columnMeta) {
                var editor = columnMeta.Editor;
                //найти editor по коду
                Ext.Array.each(this.Columns, function (column) {
                    if (column.Code == code) {
                        editor = column.Editor;
                        return false;
                    }
                });
                var value = cell.Value;
                if (editor == 70) {
                    // меняем приходящий с сервера текст на булево значение
                    value = functionGetBool(cell.Value);
                }

                if (Ext.isEmpty(idCollection.getByKey(row))) {
                    rec = Ext.create(modelName);
                    rec.setId(row);
                    rec.set(code, value);
                    idCollection.add(row, rec);
                }
                else {
                    rec = idCollection.getByKey(row);
                    rec.set(code, value);
                }
                
                if (editor === 320 && value) {
                    specModel = asp.controller.getModel('B4.model.realityobj.StructuralElement');
                    if (specModel) {
                        specModel.load(parseInt(value), {
                            callback: function(record, op) {
                                if (record) {
                                    rec.set(code, record.data);
                                }
                            }
                        });
                    }                    
                }
            }
        }, component);
        idCollection.each(function (rec) { storeData.push(rec); });

        //создаем компоненту стор
        component.store = Ext.create('Ext.data.ArrayStore', {
            autoDestroy: true,
            storeId: 'store' + component.Id,
            fields: storeFields,
            data: storeData
        });

        return component;
    },
    // меняет текст на булево значение
    getBool: function (val) {
        if (Ext.isEmpty(val)) {
            return null;
        }

        if (Ext.isBoolean(val)) {
            return val;
        }

        if (Ext.isNumber(+val)) {
            //возможно имеет смысл проверить на val<2
            return Boolean(+val);
        }

        if (val === '1' || val.toLowerCase() === 'true') {
            return true;
        }
        else if (val === '0' || val.toLowerCase() === 'false') {
            return false;
        }

        return null;
    },
    //метод создания propertygrid
    createPropertyGridComponent: function (component) {
        //парсим json
        component = this.convertPropertyGridComponent(component);

        var propertyGridSelector = 'propertyGrid' + component.Id;
        this.arraySelectors.push({ type: 'propertyGrid', selector: '#' + propertyGridSelector });

        var config =
        {
            title: component.Title,
            itemId: propertyGridSelector,
            source: component.source,
            customEditors: component.customEditors,
            customRenderers: component.customRenderers,
            propertyNames: component.propertyNames,
            nameColumnWidth: '45%',
            padding: 5,
            width: component.Width?component.Width:'100%',
            height: component.Height,
            flex: component.Flex,
            sortableColumns: false,
            viewConfig: {
                markDirty: false
            }
        };

        //Пробуем получить через selector(если повторный клик), иначе создаем
        var componentInst = Ext.create(component.Type, config);
        
        //подгрузка полноценных значений
        for (property in component.source) {
            if ((component.typeEditor[property] === 80 || component.typeEditor[property] === 220 || component.typeEditor[property] === 330) && component.source[property]) {
                var editor = component.customEditors[property];
                editor.getStore().model.load(parseInt(component.source[property]), {
                    callback: function(rec, op) {
                        editor.setValue(rec.data);
                    }
                });
            }
        }

        return componentInst;

    },

    convertPropertyGridComponent: function (component) {

        var asp = this;
        var functionGetBool = this.getBool;

        //создание конфига
        component.Type = 'Ext.grid.property.Grid';
        component.source = {};
        component.customEditors = {};
        component.customRenderers = {};
        component.propertyNames = {};
        component.typeEditor = {};

        //для каждого элемента создаем render, editor, имя
        Ext.Array.each(component.Elements, function (value) {
            var editor = value.MaxValue != 0 ? asp.getEditorConfig(value.Editor, value.MaxValue, value.MinValue) : asp.getEditorConfig(value.Editor);
            if (value.Editor == '80' || value.Editor == '220' || value.Editor == '330') {
                editor.on('change', function (sfl) {
                    var grid = Ext.ComponentQuery.query('#propertyGrid' + component.Id)[0];
                    grid.setProperty(value.Code, sfl.value);
                });
            }

            component.customEditors[value.Code] = editor;
            component.customRenderers[value.Code] = asp.createRenderer(value.Editor, editor);
            component.propertyNames[value.Code] = value.Label;
            component.source[value.Code] = '';
            component.typeEditor[value.Code] = value.Editor;
        });
        Ext.Array.each(component.Cells, function (cell) {
            if (!Ext.isEmpty(cell.Value)) {
                var cellValue = cell.Value;

                if (component.typeEditor[cell.Code] == 70) {
                    // меняем приходящий с сервера текс на булево значение
                    cellValue = functionGetBool(cellValue);
                }
                
                component.source[cell.Code] = cellValue;

            }
        });

        return component;
    },

    //метод создания form
    createFormComponent: function (component) {
        component = this.convertFormComponent(component);

        var formSelector = 'form' + component.Id;
        this.arraySelectors.push({ type: 'form', selector: '#' + formSelector });

        var config =
        {
            xtype: component.Type,
            itemId: formSelector,
            items: component.items,
            bodyPadding: 5,
            closeAction: 'hide',
            trackResetOnLoad: true,
            title: component.Title,
            padding: 5,
            width: component.Width ? component.Width : '100%',
            height: component.Height,
            flex: component.Flex
        };

        if (!Ext.isEmpty(component.Layout)) {
            config.layout = component.Layout;
        }
        if (!Ext.isEmpty(component.Defaults)) {
            this.defaults = config.defaults = component.Defaults;
        }

        //Пробуем получить через selector(если повторный клик), иначе создаем
        var componentInst = Ext.create(component.Type, config);
        return componentInst;
    },

    convertFormComponent: function (component) {

        var type;
        var asp = this;
        var functionGetBool = this.getBool;

        //создание контейнеров для hbox
        var containers = [];
        Ext.Array.each(component.Elements, function (value) {
            if (value.ColumnIndex in containers) {

            }
            else {
                containers[value.ColumnIndex] = {
                    xtype: 'container',
                    layout: 'anchor',
                    flex: 1,
                    defaults: asp.defaults,
                    items: []
                };
            }
        });

        //создание конфига
        type = 'Ext.form.Panel';
        component.Layout = {
            type: 'hbox',
            pack: 'start'
        };
        component.Defaults = {
            anchor: '100%',
            labelWidth: component.LabelWidth || 200,
            labelAlign: 'right'
        };
        //создание items
        component.items = [];
        Ext.Array.each(component.Elements, function (value) {
            var item = asp.createField(value.Editor);
            var editor = value.Editor;

            item.name = value.Code;
            item.anchor = '100%',
            item.fieldLabel = value.Label;

            Ext.Array.each(component.Cells, function (cell) {
                if (cell.Code == value.Code) {
                    var cellValue = cell.Value;
                    if (editor == 70) {
                        // меняем приходящий с сервера текст на булевское значение
                        cellValue = functionGetBool(cellValue);
                    }
                    else if (editor === 80 || editor === 220 || editor === 330) {
                        var editorField = item;
                        editorField.getStore().model.load(cellValue, {
                                callback: function (rec) {
                                    editorField.setValue(rec.data);
                                }
                            });
                    }

                    item.value = cellValue;
                }
            });
            containers[value.ColumnIndex].items.push(item);
        });

        //кладем в панель контейнеры
        Ext.Array.each(containers, function (value) {
            component.items.push(value);
        });

        component.Type = type;

        return component;
    },

    //создание поля по типу. Тип приходит в json-e
    createField: function (code, maxValue, minValue) {
        var me = this,
            field;

        switch (code) {
            case 10:
                field = {
                    xtype: 'textfield',
                    maxLength: 250
                };
                break;
            case 20:
                field = {
                    xtype: 'datefield',
                    format: 'd.m.Y'
                };
                break;
            case 30:
                field = {
                    xtype: 'gkhintfield'
                };
                
                if (!Ext.isEmpty(maxValue)) {
                    field.maxValue = maxValue;
                }

                if (!Ext.isEmpty(minValue)) {
                    field.minValue = minValue;
                }

                break;
            case 40:
            case 50:
            case 60:
                {
                    field = {
                        xtype: 'gkhdecimalfield'
                    };
                    
                    if (!Ext.isEmpty(maxValue)) {
                        field.maxValue = maxValue;
                    }
                    
                    if (!Ext.isEmpty(minValue)) {
                        field.minValue = minValue;
                    }
                    break;
                }
            case 70:
                field = {
                    xtype: 'checkboxfield'
                };
                break;

            case 80:
            case 220:
            case 330:
                {
                    var config = {};
                    Ext.Array.each(this.editors, function (editor) {
                        if (editor.Code == code) {
                            config.store = Ext.getStore(editor.Store);
                            config.listView = editor.View;
                            config.textProperty = editor.TextProperty;
                            config.columns = [];
                            config.isGetOnlyIdProperty = false;
                            config.editable = false;

                            Ext.Array.each(editor.Columns, function (column) {
                                config.columns.push({ text: column.Text, dataIndex: column.DataIndex, flex: 1 });
                            });
                        }
                    });

                    field = Ext.create('B4.form.SelectField', config);
                }
                break;
            case 90:
            case 100:
            case 110:
            case 120:
            case 130:
            case 140:
            case 150:
            case 160:
            case 170:
            case 180:
            case 190:
            case 200:
            case 210:
            case 230:
            case 240:
            case 250:
            case 260:
            case 270:
            case 280:
            case 290:
            case 300:
                {
                    var items = [];
                    Ext.Array.each(this.editors, function (editor) {
                        if (editor.Code == code) {
                            Ext.Array.each(editor.Values, function (value) {
                                items.push([value.Code, value.Name]);
                            });
                            if (!items.some(function (v) { return v[0] == '0'; }))
                                items.unshift(['0', 'Не задано']);
                        }
                    });

                    field = {
                        xtype: 'b4combobox',
                        editable: false,
                        items: items
                    };
                    break;
                }
            case 320:
                {
                    try {
                        field = Ext.create('B4.grid.RealObjStructuralElementSelectiFieldEditor', {
                            editable: false,
                            isGetOnlyIdProperty: false,
                            windowCfg: { width: 900 },
                            listeners: {
                                beforeload: function(sf, operation, store) {
                                    operation.params.objectId = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                                    operation.params.showLiftTp = true;
                                }
                            }
                        });
                    } catch (exception) {
                        field = {
                            xtype: 'textfield'
                        };
                    }
                }
                break;
            default:
                field = {
                    xtype: 'textfield'
                };
        }

        return field;
    },

    //создание editor-ов по типу. Тип приходит в json-e
    getEditorConfig: function (code, maxValue, minValue) {
        var customEditors;
        var config = {};
        
        switch (code) {
            case 0:
                customEditors = Ext.create('Ext.form.field.Text', { readOnly: true });
                break;
            case 10:
                customEditors = Ext.create('Ext.form.field.Text');
                break;
            case 20:
                customEditors = Ext.create('Ext.form.field.Date', { format: 'd.m.Y' });
                break;
            case 30:
                config = {};
                if (!Ext.isEmpty(maxValue)) {
                    config.maxValue = maxValue;
                }
                
                if (!Ext.isEmpty(minValue)) {
                    config.minValue = minValue;
                }
                //config = Ext.isEmpty(maxValue) ? { minValue: 0, maxValue: 100000000000 } : { minValue: 0, maxValue: maxValue };
                customEditors = Ext.create('B4.view.Control.GkhIntField', config);
                break;
            case 40:
            case 50:
            case 60:
                config = {};
                if (!Ext.isEmpty(maxValue)) {
                    config.maxValue = maxValue;
                }

                if (!Ext.isEmpty(minValue)) {
                    config.minValue = minValue;
                }
                customEditors = Ext.create('B4.view.Control.GkhDecimalField', config);
                break;
            case 70:
                customEditors = Ext.create('Ext.form.field.Checkbox');
                break;
            case 80:
            case 220:
            case 330:
                {
                    Ext.Array.each(this.editors, function (editor) {
                        if (editor.Code == code) {
                            config.store = Ext.getStore(editor.Store);
                            config.listView = editor.View;
                            config.textProperty = editor.TextProperty;
                            config.columns = [];
                            config.isGetOnlyIdProperty = false;
                            config.editable = false;
                            
                            Ext.Array.each(editor.Columns, function (column) {
                                config.columns.push({ text: column.Text, dataIndex: column.DataIndex, flex: 1 });
                            });
                        }
                    });
                    customEditors = Ext.create('B4.form.SelectField', config);
                }
                break;
            case 90:
            case 100:
            case 110:
            case 120:
            case 130:
            case 140:
            case 150:
            case 160:
            case 170:
            case 180:
            case 190:
            case 200:
            case 210:
            case 230:
            case 240:
            case 250:
            case 260:
            case 270:
            case 280:
            case 290:
            case 300:
            case 310:
                {
                    var items = [];
                    Ext.Array.each(this.editors, function (editor) {
                        if (editor.Code == code) {
                            Ext.Array.each(editor.Values, function (value) {
                                items.push([value.Code, value.Name]);
                            });
                            if (!items.some(function(v) { return v[0] == '0'; }))
                                items.unshift(['0', 'Не задано']);
                        }
                    });

                    customEditors = Ext.create('B4.form.ComboBox', { items: items, editable: false });
                    break;
                }
        case 320:
                {
                    Ext.Array.each(this.editors, function (editor) {
                        if (editor.Code == code) {
                            config.isGetOnlyIdProperty = false;
                            config.editable = false;
                            config.windowCfg = {
                                width: 900
                            }
                        }
                    });
                    customEditors = Ext.create('B4.form.RealObjStructuralElementSelectiField', config);
                }
                break;
            default:
                customEditors = Ext.create('Ext.form.field.Text');
        }

        return customEditors;
    },

    //создание render-ов по типу. Тип приходит в json-e
    createRenderer: function (code, editor) {
        var render;
        switch (code) {
            case 20:
                render = function (val) {
                    var result = '',
                        day, month, year;
                    if (!Ext.isEmpty(val)) {

                        //*facepalm*
                        if (Ext.isString(val) && val.length == 10) {
                            val = new Date(
                                parseInt(val.substring(6, 10)),
                                parseInt(val.substring(3, 5)) - 1,
                                parseInt(val.substring(0, 2)));
                        }

                        val = Ext.isDate(val) ? val : new Date(val);
                        day = ('0' + val.getDate()).slice(-2);
                        month = ('0' + (1 + val.getMonth())).slice(-2);
                        year = val.getFullYear();
                        result = day + '.' + month + '.' + year;
                    }
                    return result;
                };
                break;
            case 40:
            case 60:
                render = function (val) {
                    if (!Ext.isEmpty(val)) {
                        val = '' + val;
                        if (val.indexOf('.') != -1) {
                            val = val.replace('.', ',');
                        }
                        return val;
                    }
                    return '';
                };
                break;
            case 70:
                render = function (val) {
                    if (Ext.isEmpty(val)) {
                        return 'Не задано';
                    }

                    if (Ext.isBoolean(val)) {
                        if (val == true) {
                            return 'Да';
                        } else {
                            return 'Нет';
                        }
                    }

                    if (Ext.isNumber(val)) {
                        var bval = Boolean(+val);

                        switch (bval) {
                            case true:
                                return 'Да';
                            case false:
                                return 'Нет';
                        }
                    }

                    if (val === '1' || val === 'True' || val === 'true') {
                        return 'Да';
                    }

                    if (val === '0' || val === 'False' || val === 'false') {
                        return 'Нет';
                    }


                    return 'Не задано';
                };
                break;
            case 80:
            case 220:
            case 330:
                render = function (val) {
                    if (!val) return;
                    return val[editor.textProperty];
                };
                break;
            case 90:
            case 100:
            case 110:
            case 120:
            case 130:
            case 140:
            case 150:
            case 160:
            case 170:
            case 180:
            case 190:
            case 200:
            case 210:
            case 230:
            case 240:
            case 250:
            case 260:
            case 270:
            case 280:
            case 290:
            case 300:
            case 310:
                {
                    var values = [];
                    Ext.Array.each(this.editors, function (editor) {
                        if (editor.Code === code) {
                            values = editor.Values;
                        }
                    });

                    render = function (val) {
                        var result = val;
                        Ext.Array.each(values, function (editorVal) {
                            if (editorVal.Code === val) {
                                result = editorVal.Name;
                            }
                        });
                        return result;
                    };
                }
                break;
            case 320:
                {
                    render = function(val) {
                        if (val && Ext.isObject(val)) {
                            val = val.Name || val.ElementName;
                        }

                        return val;
                    }
                }
                break;
        }

        return render;
    },

    //метод добавления record-a в грид с кнопкой
    addRow: function (btn, event, eOpts) {
        var grid = Ext.ComponentQuery.query(eOpts.selector)[0],
            store, record;
        if (!Ext.isEmpty(grid)) {
            store = grid.getStore();
            record = Ext.create('B4.base.Model', { fields: store.fields });
            record.setId(this.getNumberRow(store));
            Ext.Array.each(grid.columns, function (column) {
                record.set(column.dataIndex, null);
            });
            store.insert(0, record);
        }
    },

    //метод вычисления id записи
    getNumberRow: function (store) {
        var num = 0;
        store.each(function (item) {
            if (item.getId() > num) {
                num = item.getId();
            }
        });
        return Number(num) + 1;
    },

    //Сохранение данных
    save: function () {
        var records = [],
            asp = this,
            controller = asp.controller,
            view = controller.getMainComponent();

        Ext.Array.each(this.arraySelectors, function (val) {
            var component = asp.controller.getMainComponent().down(val.selector);
            if (val.type === 'grid') {
                asp.gridSave(records, component);
            }
            if (val.type === 'form') {
                asp.formSave(records, component);
            }
            if (val.type === 'propertyGrid') {
                asp.propertyGridSave(records, component);
            }
        });
        //аякс строк на сервер
        asp.controller.mask('Сохранение', asp.controller.getMainComponent());

        //аякс строк на сервер
        B4.Ajax.request({
            timeout: 999999,
            url: B4.Url.action('UpdateForm', 'TechPassport'),
            params: {
                values: Ext.encode(records),
                sectionId: controller.getContextValue(view, 'sectionId'),
                realityObjectId: controller.getContextValue(view, 'realityObjectId')
            }
        }).next(function () {
            B4.QuickMsg.msg('Сохранение записи!', 'Успешно сохранено', 'success');
            asp.controller.unmask();
            return true;
        }).error(function (result) {
            asp.controller.unmask();
            Ext.Msg.alert('Ошибка', result.message);
        });
        
    },

    //сохранение грида
    gridSave: function (records, grid) {
        var formCode = grid.itemId.replace('grid', ''),
            recId = 0;

        Ext.Array.each(grid.getStore().getModifiedRecords(), function(rec) {
            Ext.Array.each(grid.columns, function(column) {
                if (column.dataIndex != 'Id') {
                    var row = grid.withAddButton ? recId : rec.getId(),
                        value = rec.get(column.dataIndex);
                    
                    if (value && Ext.isObject(value)) {
                        value = value.Id;
                    }

                    records.push({ ComponentCode: formCode, CellCode: row + ':' + column.dataIndex, Value: value });
                }
            });
            recId++;
        });
    },

    //сохранение формы
    formSave: function (records, form) {
        var formCode = form.itemId.replace('form', '');

        var fieldCollection = form.getForm().getFields().items;
        Ext.Array.each(fieldCollection, function(field) {
            if (field.name != 'Id') {
                var value = field.getValue();
                if (Ext.isObject(value)) {
                    value = field.getSubmitValue();
                }

                records.push({ ComponentCode: formCode, CellCode: field.name, Value: value });
            }
        });
    },

    //сохранение проперти грида
    propertyGridSave: function (records, propertyGrid) {
        var propertyGridCode = propertyGrid.itemId.replace('propertyGrid', '');
        var source = propertyGrid.getSource();
        var editors = propertyGrid.customEditors;
        
        Ext.iterate(source, function (key, value, myself) {
            //дергаем обратно сохраняемые значения вместо объектов
            if (Ext.isObject(value)) {
                if (editors[key].field) {
                    source[key] = editors[key].field.getSubmitValue();
                }
                else {
                    source[key] = editors[key].getSubmitValue();
                }
            }
            records.push({ ComponentCode: propertyGridCode, CellCode: key, Value: Ext.isEmpty(source[key]) ? null : source[key] });
        });
        
    },

    convertToBool: function () {

    }
});