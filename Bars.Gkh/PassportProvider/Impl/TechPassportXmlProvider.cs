namespace Bars.Gkh.PassportProvider
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4.IoC;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.BasePassport;
    using Bars.Gkh.Serialization;

    using Castle.Windsor;

    /// <summary>
    /// Провайдер для работы с Техпаспортом жилого дома
    /// </summary>
    public class TechPassportXmlProvider : IPassportProvider
    {
        private readonly SerializeTechPassport structure;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TechPassportXmlProvider()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.TechPassportDecs));
            this.structure = SerializeTechPassport.Serializer.Deserialize(stream).To<SerializeTechPassport>();
            stream.Close();
            this.Validate();
        }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<TehPassportValue> TehPassportValueDomain { get; set; }

        public IDomainService<TehPassport> TehPassportDomain { get; set; }

        /// <summary>
        /// Наименование техпаспорта
        /// </summary>
        public string Name
        {
            get { return "Техпаспорт"; }
        }

        /// <summary>
        /// Тип источника данных
        /// </summary>
        public string TypeDataSource
        {
            get { return "xml"; }
        }

        /// <summary>
        /// Вернуть меню
        /// </summary>
        /// <returns></returns>
        public IList<MenuItem> GetMenu()
        {
            return this.structure.GetMenu();
        }

        /// <summary>
        /// Вернуть форму с данными
        /// </summary>
        /// <param name="formId">Идентификатор формы</param>
        /// <param name="data">Данные</param>
        /// <returns>Форма паспорта</returns>
        public object GetFormWithData(string formId, IDictionary<string, Dictionary<string, string>> data)
        {
            var formStruct = this.structure.Forms.FirstOrDefault(x => x.Id == formId);
            if (formStruct == null)
            {
                return new FormTechPassport();
            }

            var form = (FormTechPassport)formStruct.Clone();
            foreach (var comp in form.Components)
            {
                switch (comp.Type)
                {
                    case "Panel":
                    case "PropertyGrid":
                        TechPassportXmlProvider.FillPanelValues(data, comp);

                        break;
                    case "Grid":
                    case "InlineGrid":
                        TechPassportXmlProvider.FillGridValues(data, comp);
                        break;
                }
            }

            return form;
        }

        /// <summary>
        /// Обновить форму
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        /// <param name="formId">Идентификатор формы</param>
        /// <param name="values">Значения паспорта</param>
        /// <param name="fromSync">Маркер источника вызова</param>
        /// <returns>Результат работы</returns>
        public bool UpdateForm(long realityObjectId, string formId, List<SerializePassportValue> values, bool fromSync = false)
        {
            if (realityObjectId == 0)
            {
                throw new ArgumentNullException("realityObjectId");
            }

            if (string.IsNullOrEmpty(formId))
            {
                throw new ArgumentNullException("formId");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            // получаем техпаспорт по объекту недвижимости (!!! в дальнейшем надо будет расширить фильтрацию версией)
            var tehPassport = this.TehPassportDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == realityObjectId);
            if (tehPassport == null)
            {
                tehPassport = new TehPassport { RealityObject = this.RealityObjectDomain.Get(realityObjectId) };
                this.TehPassportDomain.Save(tehPassport);
            }

            // удаляем из данных не редактируемые ячейки
            values.RemoveAll(x => !this.IsEditableCell(x.ComponentCode, x.CellCode));

            foreach (var componentCode in this.GetComponentCodes(formId).Where(this.IsInlineGrid))
            {
                // получим все сохраненые идентификаторы значений техпаспорта с типом InlineGrid с возможность добавлять строки
                var inlineGridIds = this.TehPassportValueDomain.GetAll()
                    .Where(x => x.FormCode == componentCode && x.TehPassport.Id == tehPassport.Id)
                    .Select(x => (object)x.Id)
                    .ToList();

                //когда мы попадаем сюда из метода синхронизации "Общие сведение ЖД <==> Технический паспорт",
                //то получается мы затираем гриды, а новые значения не заполняем (потому что их нет), в результате ошибка.
                //но когда мы попадаем сюда непосредственно с формы редактирования паспорта, то информация по гридам у нас есть,
                //скорее всего поэтому ее сначала затирали, затем сохраняли заново, так как инфа по гридам летит каждый раз с формы паспорта
                if (!fromSync)
                {
                    foreach (var inlineGridId in inlineGridIds)
                    {
                        this.TehPassportValueDomain.Delete(inlineGridId);
                    }
                }
            }

            foreach (var value in values)
            {
                var tehPassportValue = this.TehPassportValueDomain.GetAll()
                    .FirstOrDefault(x => x.TehPassport == tehPassport && x.FormCode == value.ComponentCode && x.CellCode == value.CellCode);

                if (tehPassportValue == null)
                {
                    if (string.IsNullOrEmpty(value.Value))
                    {
                        continue;
                    }

                    tehPassportValue = new TehPassportValue
                    {
                        TehPassport = tehPassport,
                        FormCode = value.ComponentCode,
                        CellCode = value.CellCode,
                        Value = this.CorrectValue(value)
                    };
                }
                else
                {
                    var correctValue = this.CorrectValue(value);
                    if (string.IsNullOrEmpty(value.Value))
                    {

                        tehPassportValue.Value = correctValue;

                        // пришла ячейка с пустым значением, удаляем значение из базы
                        this.TehPassportValueDomain.Delete(tehPassportValue.Id);
                        continue;
                    }

                    if (correctValue == tehPassportValue.Value)
                    {
                        // значение не изменилось, пропускаем, чтобы каждый раз не менять ObjectEditDate
                        continue;
                    }
                    tehPassportValue.Value = correctValue;
                }

                this.TehPassportValueDomain.Save(tehPassportValue);
            }

            return true;
        }

        /// <summary>
        /// Вернуть редактор
        /// </summary>
        /// <returns></returns>
        public object GetEditors()
        {
            return this.structure.Editors;
        }

        /// <summary>
        /// Получить редакторы
        /// </summary>
        /// <param name="formId">Идентификатор формы</param>
        /// <returns></returns>
        public object GetEditors(string formId)
        {
            var editors = new List<EditorTechPassport>();
            var form = this.structure.Forms.FirstOrDefault(x => x.Id == formId);
            if (form == null)
            {
                return null;
            }

            foreach (var comp in form.Components)
            {
                switch (comp.Type)
                {
                    case "Panel":
                        editors.AddRange(comp.Elements.Select(el => (int)el.Editor).Select(x => this.structure.Editors.FirstOrDefault(y => y.Code == x)).Where(x => x != null && !editors.Contains(x)));
                        break;
                    case "Grid":
                        editors.AddRange(comp.Columns.Select(col => this.structure.Editors.FirstOrDefault(x => x.Code == (int)col.Editor)).Where(x => x != null && !editors.Contains(x)));
                        break;
                    case "PropertyGrid":
                        editors.AddRange(comp.Elements.Select(el => (int)el.Editor).Select(x => this.structure.Editors.FirstOrDefault(y => y.Code == x)).Where(x => x != null && !editors.Contains(x)));
                        break;
                    case "InlineGrid":
                        editors.AddRange(comp.Columns.Select(col => this.structure.Editors.FirstOrDefault(x => x.Code == (int)col.Editor)).Where(x => x != null && !editors.Contains(x)));
                        break;

                    default:
                        return this.structure.Editors;
                }
            }

            return editors;
        }
        
        /// <summary>
        /// Вернуть редактор по форме и компоненту
        /// </summary>
        /// <param name="formId">Идентификатор формы</param>
        /// <param name="componentId">Идентификатор компонента</param>
        /// <param name="code">Код столбца</param>
        /// <returns>Редактор</returns>
        public TypeEditor GetEditorByFormAndComponentAndCode(string formId, string componentId, string code)
        {
            //TODO: safe
            return this.structure.Forms.FirstOrDefault(x => x.Id == formId)
                .Components.FirstOrDefault(x => x.Id == componentId)
                .Columns.FirstOrDefault(x => x.Code == code)
                .Editor;
        }

        /// <summary>
        /// Получить компонент по ИД формы и ИД компонента
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="componentId"></param>
        /// <returns>ComponentTechPassport либо NULL</returns>
        public object GetComponentBy(string formId, string componentId)
        {
            var form = this.structure.Forms.FirstOrDefault(x => x.Id == formId);
            return form == null ? null : form.Components.FirstOrDefault(x => x.Id == componentId);
        }

        /// <summary>
        /// Проверка ячейки на редактируемость
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <param name="cellCode">Код ячейки</param>
        /// <returns>Маркер редактируемости</returns>
        public bool IsEditableCell(string componentCode, string cellCode)
        {
            ComponentTechPassport component = null;
            foreach (var form in this.structure.Forms)
            {
                component = form.Components.FirstOrDefault(x => x.Id == componentCode);
                if (component != null)
                {
                    break;
                }
            }

            if (component == null)
            {
                return false;
            }

            switch (component.Type)
            {
                case "Grid":
                    var codeColumn = cellCode.Split(':')[1];
                    var col = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    return col != null && col.Editable;
                case "Panel":
                    return true;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Вернуть наименование элемента
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <param name="cellCode">Код ячейки</param>
        /// <returns>Наименование</returns>
        public string GetLabelForFormElement(string componentCode, string cellCode)
        {
            ComponentTechPassport component = null;
            FormTechPassport frm;
            foreach (var form in this.structure.Forms)
            {
                component = form.Components.FirstOrDefault(x => x.Id == componentCode);
                if (component != null)
                {
                    frm = form;
                    break;
                }
            }

            if (component == null)
            {
                return string.Empty;
            }

            switch (component.Type)
            {
                case "Grid":
                case "InlineGrid":
                    var codeColumn = cellCode.Split(':')[0];
                    var col =
                        component.Cells.Where(x => x.Code.Contains(codeColumn + ":"))
                            .OrderBy(x => x.Code)
                            .Select(x => x.Value)
                            .ToList();
                    if (col.Any())
                    {
                        var val = String.Join(",", col.ToArray());

                        return String.Format("{0} : {1}", component.Title, val);
                    }

                    break;

                case "Panel":
                case "PropertyGrid":
                    var elem = component.Elements.FirstOrDefault(x => x.Code == cellCode);

                    if (elem != null)
                    {
                        return String.Format("{0} : {1}", component.Title, elem.Label);
                    }

                    break;
            }

            return string.Empty;
        }

        /// <summary>
        /// Вернуть значения паспорта по дому
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        /// <returns></returns>
        public IList<SerializePassportValue> GetPassportValuesByRo(long realityObjectId)
        {
            if (realityObjectId == 0)
            {
                throw new ArgumentNullException("realityObjectId");
            }

            var values = new List<SerializePassportValue>();

            var serviceTehPassportDomain = this.Container.Resolve<IDomainService<TehPassport>>();
            var serviceTehPassportValueDomain = this.Container.ResolveDomain<TehPassportValue>();
            using (this.Container.Using(serviceTehPassportDomain, serviceTehPassportValueDomain))
            {
                // получаем техпаспорт по объекту недвижимости (!!! в дальнейшем надо будет расширить фильтрацию версией)
                var tehPassport = serviceTehPassportDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == realityObjectId);
                if (tehPassport == null)
                {
                    return values;
                }

                values = serviceTehPassportValueDomain.GetAll()
                    .Where(x => x.TehPassport.Id == tehPassport.Id)
                    .Select(x => new SerializePassportValue
                    {
                        CellCode = x.CellCode,
                        ComponentCode = x.FormCode,
                        Value = x.Value
                    }).ToList();

                var editorsDict = values
                    .Select(x => x.ComponentCode)
                    .Distinct()
                    .ToDictionary(x => x, y => ((List<EditorTechPassport>) this.GetEditors(y)));
                values.ForEach(x =>
                {
                    var editor = editorsDict[x.ComponentCode];
                    if (editor != null && editor.Any())
                    {
                        x.EditorValue =
                            editor
                                .SelectMany(y => y.Values)
                                .FirstOrDefault(y => y.Code == x.Value)
                                .Return(y => y.Name);
                    }
                });
            }

            return values;
        }

        /// <summary>
        /// Вернуть текст для ячейки
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <param name="cellCode">Код ячейки</param>
        /// <param name="value">Значение</param>
        /// <returns>Конвертированное поле</returns>
        public string GetTextForCellValue(string componentCode, string cellCode, string value)
        {
            ComponentTechPassport component = null;
            foreach (var form in this.structure.Forms)
            {
                component = form.Components.FirstOrDefault(x => x.Id == componentCode);
                if (component != null)
                {
                    break;
                }
            }

            if (component == null)
            {
                return string.Empty;
            }

            TypeEditor editor = 0;

            switch (component.Type)
            {
                case "Grid":
                case "InlineGrid":
                    var codeColumn = cellCode.Split(':')[1];
                    var col = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    if (col != null)
                    {
                        editor = col.Editor;
                    }

                    break;

                case "Panel":
                case "PropertyGrid":
                    var elements = component.Elements;
                    if (elements != null)
                    {
                        var element = elements.FirstOrDefault(x => x.Code == cellCode);
                        if (element != null)
                        {
                            editor = element.Editor;
                        }
                    }

                    break;
            }

            return this.ValueAsText(editor, value);
        }

        private string ValueAsText(TypeEditor typeEditor, string value)
        {
            var result = string.Empty;

            switch (typeEditor)
            {
                case 0:
                case TypeEditor.Text:
                case TypeEditor.Int:
                case TypeEditor.Decimal:
                case TypeEditor.Double:
                    result = value;
                    break;

                case TypeEditor.Bool:
                    if (value == "0")
                    {
                        result = "Нет";
                    }
                    else if (value == "1")
                    {
                        result = "Да";
                    }
                    break;

                case TypeEditor.Date:
                    {
                        DateTime date;
                        if (DateTime.TryParse(value, out date))
                        {
                            result = date.ToShortDateString();
                        }

                        break;
                    }

                case TypeEditor.TypeProject:
                    {
                        int intVal;
                        if (int.TryParse(value, out intVal))
                        {
                            result = this.Container.Resolve<IRepository<TypeProject>>().GetAll()
                                         .Where(x => x.Id == intVal)
                                         .Select(x => x.Name)
                                         .FirstOrDefault();
                        }

                        break;
                    }
                case TypeEditor.RoofingMaterial:
                    {
                        int intVal;
                        if (int.TryParse(value, out intVal))
                        {
                            result = this.Container.Resolve<IRepository<RoofingMaterial>>().GetAll()
                                         .Where(x => x.Id == intVal)
                                         .Select(x => x.Name)
                                         .FirstOrDefault();
                        }

                        break;
                    }
                case TypeEditor.TypeWalls:
                    {
                        int intVal;
                        if (int.TryParse(value, out intVal))
                        {
                            result = this.Container.Resolve<IRepository<WallMaterial>>().GetAll()
                                         .Where(x => x.Id == intVal)
                                         .Select(x => x.Name)
                                         .FirstOrDefault();
                        }

                        break;
                    }


                default:
                    {
                        var editor = this.structure.Editors.FirstOrDefault(x => x.Type == typeEditor.ToString());

                        if (editor != null)
                        {
                            var text = editor.Values.Where(x => x.Code == value).Select(x => x.Name).FirstOrDefault();

                            if (!string.IsNullOrEmpty(text))
                            {
                                result = text;
                            }
                        }

                        break;
                    }
            }

            return result;
        }

        /// <summary>
        /// Проверяем, является ли ячейка типа <see cref="bool"/>
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <param name="cellCode">Код ячейки</param>
        /// <returns>Маркер принадлежности</returns>
        public bool IsBoolean(string componentCode, string cellCode)
        {
            ComponentTechPassport component = null;
            foreach (var form in this.structure.Forms)
            {
                component = form.Components.FirstOrDefault(x => x.Id == componentCode);
                if (component != null)
                {
                    break;
                }
            }

            if (component == null)
            {
                return false;
            }

            var codeColumn = cellCode.Split(':')[1];
            switch (component.Type)
            {
                case "Grid":
                    var gridcol = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    return gridcol != null && gridcol.Editor == TypeEditor.Bool;
                case "InlineGrid":
                    var inlineGridcol = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    return inlineGridcol != null && inlineGridcol.Editor == TypeEditor.Bool;
                case "Panel":
                    var cell = component.Elements.FirstOrDefault(x => x.Code == cellCode);
                    return cell != null && cell.Editor == TypeEditor.Bool;
                case "PropertyGrid":
                    var cellPg = component.Elements.FirstOrDefault(x => x.Code == cellCode);
                    return cellPg != null && cellPg.Editor == TypeEditor.Bool;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Проверяем, является ли ячейка типа <see cref="DateTime"/>
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <param name="cellCode">Код ячейки</param>
        /// <returns>Маркер принадлежности</returns>
        public bool IsDate(string componentCode, string cellCode)
        {
            ComponentTechPassport component = null;
            foreach (var form in this.structure.Forms)
            {
                component = form.Components.FirstOrDefault(x => x.Id == componentCode);
                if (component != null)
                {
                    break;
                }
            }

            if (component == null)
            {
                return false;
            }

            var codeColumn = cellCode.Split(':')[1];
            switch (component.Type)
            {
                case "Grid":
                    var gridcol = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    return gridcol != null && gridcol.Editor == TypeEditor.Date;
                case "InlineGrid":
                    var inlineGridcol = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    return inlineGridcol != null && inlineGridcol.Editor == TypeEditor.Date;
                case "Panel":
                    var cell = component.Elements.FirstOrDefault(x => x.Code == cellCode);
                    return cell != null && cell.Editor == TypeEditor.Date;
                case "PropertyGrid":
                    var cellPg = component.Elements.FirstOrDefault(x => x.Code == cellCode);
                    return cellPg != null && cellPg.Editor == TypeEditor.Date;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Проверяем, является ли компонент InlineGrid'ом
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <returns>Маркер принадлежности</returns>
        public bool IsInlineGrid(string componentCode)
        {
            return this.structure.Forms.Select(x => x.Components.FirstOrDefault(y => y.Id == componentCode))
                        .Where(x => x != null)
                        .Select(x => x.Type == "InlineGrid")
                        .FirstOrDefault();
        }

        /// <summary>
        /// Вернуть ячейки компонента
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <returns>Словарь ячеек</returns>
        public Dictionary<string, string> GetCells(string componentCode)
        {
            var cells = new Dictionary<string, string>();

            var comp = this.structure.Forms.Select(form => form.Components.FirstOrDefault(x => x.Id == componentCode)).FirstOrDefault();

            if (comp == null)
            {
                return cells;
            }

            foreach (var cell in comp.Cells)
            {
                cells.Add(cell.Code, cell.Value);
            }

            return cells;
        }

        /// <summary>
        /// Вернуть коды компонентов формы
        /// </summary>
        /// <param name="formId">Идентификатор формы</param>
        /// <returns>Список компонентов</returns>
        public List<string> GetComponentCodes(string formId)
        {
            var form = this.structure.Forms.FirstOrDefault(x => x.Id == formId);
            return form == null ? new List<string>() : form.Components.Select(x => x.Id).ToList();
        }

        private static void FillGridValues(IDictionary<string, Dictionary<string, string>> data, ComponentTechPassport comp)
        {
            if (!data.ContainsKey(comp.Id)) return;

            foreach (var cell in data[comp.Id].Select(x => new CellTechPassport { Code = x.Key, Value = x.Value }))
            {
                if (comp.Cells.Any(x => x.Equals(cell)))
                {
                    // throw new Exception("Данная ячейка уже добавлена");
                    continue;
                }

                comp.Cells.Add(cell);
            }
        }

        private static void FillPanelValues(IDictionary<string, Dictionary<string, string>> data, ComponentTechPassport comp)
        {
            if (comp.Cells == null)
            {
                comp.Cells = new List<CellTechPassport>();
            }

            foreach (var code in comp.Elements
                .Select(x => x.Code)
                .Where(x => data.ContainsKey(comp.Id))
                .Where(code => data[comp.Id].ContainsKey(code)))
            {
                comp.Cells.Add(new CellTechPassport { Code = code, Value = data[comp.Id][code] });
            }
        }

        /// <summary>
        /// Валидация xml
        /// </summary>
        private void Validate()
        {
            // Проверка на уникальность идентификаторов форм
            var forms = new Dictionary<string, object>();
            foreach (var form in this.structure.Forms)
            {
                if (forms.ContainsKey(form.Id))
                {
                    var msg = string.Format("Не корректный формат описания техпаспорта. Дублирование идентификатора {0}", form.Id);
                    throw new Exception(msg);
                }

                forms.Add(form.Id, form);
            }

            var components = new Dictionary<string, object>();
            foreach (var form in this.structure.Forms)
            {
                foreach (var comp in form.Components)
                {
                    if (components.ContainsKey(comp.Id))
                    {
                        var msg = string.Format("Не корректный формат описания техпаспорта. Дублирование идентификатора {0}", comp.Id);
                        throw new Exception(msg);
                    }

                    components.Add(comp.Id, comp);
                }
            }
        }

        /// <summary>
        /// Корректирование приходящих данных
        /// </summary>
        /// <param name="pswValue"></param>
        /// <returns>Скорректированное значение</returns>
        private string CorrectValue(SerializePassportValue pswValue)
        {
            var value = pswValue.Value;

            if (this.IsBoolean(pswValue.ComponentCode, pswValue.CellCode))
            {
                value = value.Trim().ToLower();
                switch (value)
                {
                    case "true":
                        value = "1";
                        break;
                    case "false":
                        value = "0";
                        break;
                    default:
                        value = "0";
                        break;
                }

                return value;
            }

            if (this.IsDate(pswValue.ComponentCode, pswValue.CellCode))
            {
                value = value.Trim();
                var date = value.ToDateTime();
                return date != DateTime.MinValue ? date.ToString("s", DateTimeFormatInfo.InvariantInfo) : value;
            }

            return value;
        }
    }
}
