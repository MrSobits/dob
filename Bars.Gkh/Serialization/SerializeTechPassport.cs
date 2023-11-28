namespace Bars.Gkh.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;
    using B4;
    using Gkh.Enums.BasePassport;

    [XmlRoot(ElementName = "techPassport")]
    public class SerializeTechPassport
    {
        [XmlArrayAttribute(ElementName = "partitions")]
        [XmlArrayItemAttribute(ElementName = "partition", IsNullable = false)]
        public List<Partition> Partitions { get; set; }

        [XmlArrayAttribute(ElementName = "forms")]
        [XmlArrayItemAttribute(ElementName = "form", IsNullable = false)]
        public List<FormTechPassport> Forms { get; set; }

        [XmlArrayAttribute(ElementName = "editors")]
        [XmlArrayItemAttribute(ElementName = "editor", IsNullable = false)]
        public List<EditorTechPassport> Editors { get; set; }

        public IList<MenuItem> GetMenu()
        {
            return this.Partitions.Select(x => x.GetMenu()).ToList();
        }

        private static XmlSerializer serializer;

        public static XmlSerializer Serializer
        {
            get
            {
                return SerializeTechPassport.serializer ?? (SerializeTechPassport.serializer = new XmlSerializer(typeof(SerializeTechPassport)));
            }
        }
    }

    [XmlType(TypeName = "partition")]
    public class Partition
    {
        [XmlIgnore]
        private const string DefaulControllerName = "realityobjectedit/{0}/techpassport/{1}";

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "order")]
        public int Order { get; set; }

        [XmlAttribute(AttributeName = "formId")]
        public string FormId { get; set; }

        [XmlAttribute(AttributeName = "controllerName")]
        public string ControllerName { get; set; }

        [XmlIgnore]
        public bool IsForm
        {
            get
            {
                return !string.IsNullOrEmpty(this.FormId);
            }
        }

        [XmlIgnore]
        public string Controller
        {
            get { return string.IsNullOrEmpty(this.ControllerName) ? Partition.DefaulControllerName : this.ControllerName; }
        }

        [XmlArrayAttribute(ElementName = "items")]
        [XmlArrayItemAttribute(ElementName = "partition")]
        public List<Partition> Items { get; set; }

        public MenuItem GetMenu()
        {
            MenuItem item;
            if (string.IsNullOrEmpty(this.FormId) && string.IsNullOrEmpty(this.ControllerName))
            {
                item = new MenuItem { Caption = this.Name, RequiredPermissions = new List<string> { "Gkh.RealityObject.Register.TechPassport.View" } };
            }
            else
            {
                item = new MenuItem { Caption = this.Name, Href = this.Controller, RequiredPermissions = new List<string> { "Gkh.RealityObject.Register.TechPassport.View" } }
                    .AddOption("SectionId", this.FormId)
                    .AddOption("title", this.Name);
            }

            foreach (var partition in this.Items)
            {
                partition.GetMenu(item);
            }

            return item;
        }

        public void GetMenu(MenuItem parentItem)
        {
            MenuItem item;
            if (string.IsNullOrEmpty(this.FormId) && string.IsNullOrEmpty(this.ControllerName))
            {
                item = new MenuItem { Caption = this.Name };
            }
            else
            {
                item = new MenuItem { Caption = this.Name, Href = this.Controller }
                        .AddOption("SectionId", this.FormId)
                        .AddOption("title", this.Name);
            }

            foreach (var partition in this.Items)
            {
                partition.GetMenu(item);
            }

            parentItem.Items.Add(item);
        }
    }

    [XmlType(TypeName = "form")]
    public class FormTechPassport : ICloneable
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlArrayAttribute(ElementName = "components")]
        [XmlArrayItemAttribute(ElementName = "component")]
        public List<ComponentTechPassport> Components { get; set; }

        public object Clone()
        {
            var form = new FormTechPassport { Id = this.Id, Components = new List<ComponentTechPassport>() };
            foreach (var comp in this.Components)
            {
                form.Components.Add((ComponentTechPassport)comp.Clone());
            }

            return form;
        }
    }

    [XmlType(TypeName = "editor")]
    public class EditorTechPassport
    {
        [XmlAttribute(AttributeName = "code")]
        public int Code { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "view")]
        public string View { get; set; }

        [XmlAttribute(AttributeName = "store")]
        public string Store { get; set; }

        [XmlAttribute(AttributeName = "textProperty")]
        public string TextProperty { get; set; }

        [XmlArrayAttribute(ElementName = "values")]
        [XmlArrayItemAttribute(ElementName = "value")]
        public List<EditorValueTechPassport> Values { get; set; }

        [XmlArrayAttribute(ElementName = "columnEditors")]
        [XmlArrayItemAttribute(ElementName = "columnEditor")]
        public List<ColumnEditorTechPassport> Columns { get; set; }
    }

    [XmlType(TypeName = "value")]
    public class EditorValueTechPassport
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "view")]
        public string View { get; set; }

        [XmlAttribute(AttributeName = "store")]
        public string Store { get; set; }

        [XmlAttribute(AttributeName = "textProperty")]
        public string TextProperty { get; set; }
    }

    [XmlType(TypeName = "columnEditor")]
    public class ColumnEditorTechPassport
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }

        [XmlAttribute(AttributeName = "dataIndex")]
        public string DataIndex { get; set; }
    }

    [XmlType(TypeName = "component")]
    public class ComponentTechPassport : ICloneable
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }

        [XmlAttribute(AttributeName = "labelWidth")]
        public int LabelWidth { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "anchor")]
        public string Anchor { get; set; }

        [XmlAttribute(AttributeName = "order")]
        public int Order { get; set; }

        [XmlAttribute(AttributeName = "noSort")]
        public bool NoSort { get; set; }

        [XmlAttribute(AttributeName = "columns")]
        public int CountColumns { get; set; }

        [XmlAttribute(AttributeName = "flex")]
        public int Flex { get; set; }

        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }

        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }

        [XmlArrayAttribute(ElementName = "elements")]
        [XmlArrayItemAttribute(ElementName = "element")]
        public List<ElementTechPassport> Elements { get; set; }

        [XmlArrayAttribute(ElementName = "columns")]
        [XmlArrayItemAttribute(ElementName = "column")]
        public List<ColumnTechPassport> Columns { get; set; }

        [XmlArrayAttribute(ElementName = "rows")]
        [XmlArrayItemAttribute(ElementName = "row")]
        public List<RowTechPassport> Rows { get; set; }

        [XmlArrayAttribute(ElementName = "cells")]
        [XmlArrayItemAttribute(ElementName = "cell")]
        public List<CellTechPassport> Cells { get; set; }

        public object Clone()
        {
            var comp = new ComponentTechPassport
                {
                    Id = this.Id,
                    Title = this.Title,
                    LabelWidth = this.LabelWidth,
                    Type = this.Type,
                    Anchor = this.Anchor,
                    Order = this.Order,
                    NoSort = this.NoSort,
                    Flex = this.Flex,
                    Height = this.Height,
                    Width = this.Width,
                    CountColumns = this.CountColumns,
                    Elements = new List<ElementTechPassport>(),
                    Columns = new List<ColumnTechPassport>(),
                    Rows = new List<RowTechPassport>(),
                    Cells = new List<CellTechPassport>()
                };

            foreach (var element in this.Elements)
            {
                comp.Elements.Add((ElementTechPassport)element.Clone());
            }

            foreach (var col in this.Columns)
            {
                comp.Columns.Add((ColumnTechPassport)col.Clone());
            }

            foreach (var row in this.Rows)
            {
                comp.Rows.Add((RowTechPassport)row.Clone());
            }

            foreach (var cell in this.Cells)
            {
                comp.Cells.Add((CellTechPassport)cell.Clone());
            }

            return comp;
        }
    }

    [XmlType(TypeName = "row")]
    public class RowTechPassport : ICloneable
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        public object Clone()
        {
            return new RowTechPassport { Code = this.Code };
        }
    }

    [XmlType(TypeName = "element")]
    public class ElementTechPassport : ICloneable
    {
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "columnIndex")]
        public string ColumnIndex { get; set; }

        [XmlAttribute(AttributeName = "editor")]
        public TypeEditor Editor { get; set; }

        [XmlAttribute(AttributeName = "maxValue")]
        public string MaxValue { get; set; }

        [XmlAttribute(AttributeName = "minValue")]
        public string MinValue { get; set; }

        public object Clone()
        {
            return new ElementTechPassport
                {
                    Label = this.Label,
                    Code = this.Code,
                    ColumnIndex = this.ColumnIndex,
                    Editor = this.Editor,
                    MaxValue = this.MaxValue,
                    MinValue = this.MinValue
                };
        }
    }

    [XmlType(TypeName = "column")]
    public class ColumnTechPassport : ICloneable
    {
        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }

        [XmlAttribute(AttributeName = "editable")]
        public bool Editable { get; set; }

        [XmlAttribute(AttributeName = "hidden")]
        public bool Hidden { get; set; }

        [XmlAttribute(AttributeName = "isId")]
        public bool IsId { get; set; }

        [XmlAttribute(AttributeName = "editor")]
        public TypeEditor Editor { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "order")]
        public int Order { get; set; }

#warning хак, исправить
        [XmlAttribute(AttributeName = "maxValue")]
        public string MaxValue { get; set; }

        [XmlAttribute(AttributeName = "minValue")]
        public string MinValue { get; set; }

        [XmlAttribute(AttributeName = "width")]
        public int Width { get; set; }

        public object Clone()
        {
            return new ColumnTechPassport
                {
                    Title = this.Title,
                    Editable = this.Editable,
                    Hidden = this.Hidden,
                    IsId = this.IsId,
                    Editor = this.Editor,
                    Code = this.Code,
                    MaxValue = this.MaxValue,
                    MinValue = this.MinValue,
                    Width = this.Width,
                    Order = this.Order,
                };
        }
    }

    [XmlType(TypeName = "cell")]
    public class CellTechPassport : ICloneable
    {
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// проверка строк без сравнения componentId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj)) return false;
            if (object.ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && this.Equals((CellTechPassport)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Value != null ? this.Value.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.Code != null ? this.Code.GetHashCode() : 0);
                return hashCode;
            }
        }

        public object Clone()
        {
            return new CellTechPassport
                {
                    Value = this.Value,
                    Code = this.Code
                };
        }

        protected bool Equals(CellTechPassport other)
        {
            return string.Equals(this.Value, other.Value) && string.Equals(this.Code, other.Code);
        }
    }
}
