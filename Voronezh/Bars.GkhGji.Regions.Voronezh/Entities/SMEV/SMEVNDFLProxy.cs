﻿

using System.Xml.Serialization;
namespace Bars.GkhGji.Regions.Voronezh.SMEVNDFLProxy
{//------------------------------------------------------------------------------
 // <auto-generated>
 //     Этот код создан программой.
 //     Исполняемая версия:4.0.30319.42000
 //
 //     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
 //     повторной генерации кода.
 // </auto-generated>
 //------------------------------------------------------------------------------

    using System.Xml.Serialization;

    // 
    // Этот исходный код был создан с помощью xsd, версия=4.8.3928.0.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1", IsNullable = false)]
    public partial class NDFL2Response
    {

        private NDFL2ResponseДохФЛ дохФЛField;

        private string идЗапросField;

        /// <remarks/>
        public NDFL2ResponseДохФЛ ДохФЛ
        {
            get
            {
                return this.дохФЛField;
            }
            set
            {
                this.дохФЛField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ИдЗапрос
        {
            get
            {
                return this.идЗапросField;
            }
            set
            {
                this.идЗапросField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    public partial class NDFL2ResponseДохФЛ
    {

        private NDFL2ResponseДохФЛПолучДох получДохField;

        private NDFL2ResponseДохФЛДохФЛ_НА[] дохФЛ_НАField;

        private ushort отчетГодField;

        /// <remarks/>
        public NDFL2ResponseДохФЛПолучДох ПолучДох
        {
            get
            {
                return this.получДохField;
            }
            set
            {
                this.получДохField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ДохФЛ_НА")]
        public NDFL2ResponseДохФЛДохФЛ_НА[] ДохФЛ_НА
        {
            get
            {
                return this.дохФЛ_НАField;
            }
            set
            {
                this.дохФЛ_НАField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort ОтчетГод
        {
            get
            {
                return this.отчетГодField;
            }
            set
            {
                this.отчетГодField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    public partial class NDFL2ResponseДохФЛПолучДох
    {

        private NDFL2ResponseДохФЛПолучДохФИО фИОField;

        private NDFL2ResponseДохФЛПолучДохУдЛичнФЛ удЛичнФЛField;

        private System.DateTime датаРождField;

        /// <remarks/>
        public NDFL2ResponseДохФЛПолучДохФИО ФИО
        {
            get
            {
                return this.фИОField;
            }
            set
            {
                this.фИОField = value;
            }
        }

        /// <remarks/>
        public NDFL2ResponseДохФЛПолучДохУдЛичнФЛ УдЛичнФЛ
        {
            get
            {
                return this.удЛичнФЛField;
            }
            set
            {
                this.удЛичнФЛField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime ДатаРожд
        {
            get
            {
                return this.датаРождField;
            }
            set
            {
                this.датаРождField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    public partial class NDFL2ResponseДохФЛПолучДохФИО
    {

        private string familyNameField;

        private string firstNameField;

        private string patronymicField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FamilyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Patronymic
        {
            get
            {
                return this.patronymicField;
            }
            set
            {
                this.patronymicField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    public partial class NDFL2ResponseДохФЛПолучДохУдЛичнФЛ
    {

        private byte documentCodeField;

        private uint seriesNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte DocumentCode
        {
            get
            {
                return this.documentCodeField;
            }
            set
            {
                this.documentCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint SeriesNumber
        {
            get
            {
                return this.seriesNumberField;
            }
            set
            {
                this.seriesNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    public partial class NDFL2ResponseДохФЛДохФЛ_НА
    {

        private NDFL2ResponseДохФЛДохФЛ_НАСвНА свНАField;

        private NDFL2ResponseДохФЛДохФЛ_НАСведДох сведДохField;

        /// <remarks/>
        public NDFL2ResponseДохФЛДохФЛ_НАСвНА СвНА
        {
            get
            {
                return this.свНАField;
            }
            set
            {
                this.свНАField = value;
            }
        }

        /// <remarks/>
        public NDFL2ResponseДохФЛДохФЛ_НАСведДох СведДох
        {
            get
            {
                return this.сведДохField;
            }
            set
            {
                this.сведДохField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    public partial class NDFL2ResponseДохФЛДохФЛ_НАСвНА
    {

        private NDFL2ResponseДохФЛДохФЛ_НАСвНАСвНАЮЛ свНАЮЛField;

        /// <remarks/>
        public NDFL2ResponseДохФЛДохФЛ_НАСвНАСвНАЮЛ СвНАЮЛ
        {
            get
            {
                return this.свНАЮЛField;
            }
            set
            {
                this.свНАЮЛField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    public partial class NDFL2ResponseДохФЛДохФЛ_НАСвНАСвНАЮЛ
    {

        private uint иННЮЛField;

        private uint кППField;

        private string наимОргField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint ИННЮЛ
        {
            get
            {
                return this.иННЮЛField;
            }
            set
            {
                this.иННЮЛField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint КПП
        {
            get
            {
                return this.кППField;
            }
            set
            {
                this.кППField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string НаимОрг
        {
            get
            {
                return this.наимОргField;
            }
            set
            {
                this.наимОргField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    public partial class NDFL2ResponseДохФЛДохФЛ_НАСведДох
    {

        private NDFL2ResponseДохФЛДохФЛ_НАСведДохСвСумДох[] дохВычField;

        private NDFL2ResponseДохФЛДохФЛ_НАСведДохСГДНалПер сГДНалПерField;

        private byte ставкаField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("СвСумДох", IsNullable = false)]
        public NDFL2ResponseДохФЛДохФЛ_НАСведДохСвСумДох[] ДохВыч
        {
            get
            {
                return this.дохВычField;
            }
            set
            {
                this.дохВычField = value;
            }
        }

        /// <remarks/>
        public NDFL2ResponseДохФЛДохФЛ_НАСведДохСГДНалПер СГДНалПер
        {
            get
            {
                return this.сГДНалПерField;
            }
            set
            {
                this.сГДНалПерField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Ставка
        {
            get
            {
                return this.ставкаField;
            }
            set
            {
                this.ставкаField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    public partial class NDFL2ResponseДохФЛДохФЛ_НАСведДохСвСумДох
    {

        private ushort кодДоходField;

        private byte месяцField;

        private decimal сумДоходField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort КодДоход
        {
            get
            {
                return this.кодДоходField;
            }
            set
            {
                this.кодДоходField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Месяц
        {
            get
            {
                return this.месяцField;
            }
            set
            {
                this.месяцField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal СумДоход
        {
            get
            {
                return this.сумДоходField;
            }
            set
            {
                this.сумДоходField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://x-artefacts-fns-ndfl2/root/260-10/4.1.1")]
    public partial class NDFL2ResponseДохФЛДохФЛ_НАСведДохСГДНалПер
    {

        private decimal налБазаField;

        private uint налИсчислField;

        private byte налНеУдержField;

        private uint налПеречислField;

        private uint налУдержField;

        private byte налУдержЛишField;

        private decimal сумДохОбщField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal НалБаза
        {
            get
            {
                return this.налБазаField;
            }
            set
            {
                this.налБазаField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint НалИсчисл
        {
            get
            {
                return this.налИсчислField;
            }
            set
            {
                this.налИсчислField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte НалНеУдерж
        {
            get
            {
                return this.налНеУдержField;
            }
            set
            {
                this.налНеУдержField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint НалПеречисл
        {
            get
            {
                return this.налПеречислField;
            }
            set
            {
                this.налПеречислField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint НалУдерж
        {
            get
            {
                return this.налУдержField;
            }
            set
            {
                this.налУдержField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte НалУдержЛиш
        {
            get
            {
                return this.налУдержЛишField;
            }
            set
            {
                this.налУдержЛишField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal СумДохОбщ
        {
            get
            {
                return this.сумДохОбщField;
            }
            set
            {
                this.сумДохОбщField = value;
            }
        }
    }


}
