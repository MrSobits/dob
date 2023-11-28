﻿

using System.Xml.Serialization;
namespace Bars.GkhGji.Regions.Voronezh.SGIO.SMEVRedevelopment
{
    //------------------------------------------------------------------------------
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://ru.sgio.residentreconstructinfo/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute("request", Namespace = "urn://ru.sgio.residentreconstructinfo/1.0.0", IsNullable = false)]
    public partial class RequestType
    {

        private string governmentnameField;

        private string actdateField;

        private string actnumField;

        private string objectnameField;

        private string addressField;

        private string cadastralnumberField;

        private string receiverOKTMOField;

        /// <remarks/>
        public string governmentname
        {
            get
            {
                return this.governmentnameField;
            }
            set
            {
                this.governmentnameField = value;
            }
        }

        /// <remarks/>
        public string actdate
        {
            get
            {
                return this.actdateField;
            }
            set
            {
                this.actdateField = value;
            }
        }

        /// <remarks/>
        public string actnum
        {
            get
            {
                return this.actnumField;
            }
            set
            {
                this.actnumField = value;
            }
        }

        /// <remarks/>
        public string objectname
        {
            get
            {
                return this.objectnameField;
            }
            set
            {
                this.objectnameField = value;
            }
        }

        /// <remarks/>
        public string address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        public string cadastralnumber
        {
            get
            {
                return this.cadastralnumberField;
            }
            set
            {
                this.cadastralnumberField = value;
            }
        }

        /// <remarks/>
        public string ReceiverOKTMO
        {
            get
            {
                return this.receiverOKTMOField;
            }
            set
            {
                this.receiverOKTMOField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://ru.sgio.residentreconstructinfo/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute("response", Namespace = "urn://ru.sgio.residentreconstructinfo/1.0.0", IsNullable = false)]
    public partial class ResponseType
    {

        private string governmentnameField;

        private string actdateField;

        private string actnumField;

        private string objectnameField;

        private string addressField;

        private string cadastralnumberField;

        private ResponseTypeErrorcode errorcodeField;

        private bool errorcodeFieldSpecified;

        private string fileDocField;

        /// <remarks/>
        public string governmentname
        {
            get
            {
                return this.governmentnameField;
            }
            set
            {
                this.governmentnameField = value;
            }
        }

        /// <remarks/>
        public string actdate
        {
            get
            {
                return this.actdateField;
            }
            set
            {
                this.actdateField = value;
            }
        }

        /// <remarks/>
        public string actnum
        {
            get
            {
                return this.actnumField;
            }
            set
            {
                this.actnumField = value;
            }
        }

        /// <remarks/>
        public string objectname
        {
            get
            {
                return this.objectnameField;
            }
            set
            {
                this.objectnameField = value;
            }
        }

        /// <remarks/>
        public string address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        public string cadastralnumber
        {
            get
            {
                return this.cadastralnumberField;
            }
            set
            {
                this.cadastralnumberField = value;
            }
        }

        /// <remarks/>
        public ResponseTypeErrorcode errorcode
        {
            get
            {
                return this.errorcodeField;
            }
            set
            {
                this.errorcodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool errorcodeSpecified
        {
            get
            {
                return this.errorcodeFieldSpecified;
            }
            set
            {
                this.errorcodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string FileDoc
        {
            get
            {
                return this.fileDocField;
            }
            set
            {
                this.fileDocField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.residentreconstructinfo/1.0.0")]
    public enum ResponseTypeErrorcode
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Item0,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,
    }



}
