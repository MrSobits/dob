using System.Collections.Generic;

namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    using Newtonsoft.Json;

    /// <summary>
    ///     запись для отправки начисления штрафа в гис гмп
    /// </summary>
    public class GisChargeToSend : BaseEntity
    {
        /// <summary>
        ///     Постановление
        /// </summary>
        public virtual Resolution Resolution { get; set; }

        /// <summary>
        ///     Дата и время отправки
        /// </summary>
        public virtual DateTime? DateSend { get; set; }

        /// <summary>
        ///     Флаг, отправлено или нет
        /// </summary>
        public virtual bool IsSent { get; set; }

        /// <summary>
        ///     Json-объект, который нужно отправить
        /// </summary>
        public virtual GisChargeJson JsonObject { get; set; }
    }

    public class GisChargeJson
    {
        [JsonProperty(PropertyName = "pattern_code")]
        public string PatternCode { get; set; }

        [JsonProperty(PropertyName = "bill_date")]
        public string BillDate { get; set; }

        [JsonProperty(PropertyName = "total_amount")]
        public string TotalAmount { get; set; }

        [JsonProperty(PropertyName = "supplier_billd")]
        public string SupplierBillId { get; set; }

        [JsonProperty(PropertyName = "change_status")]
        public string ChargeStatus { get; set; }

        [JsonIgnore]
        public string Kbk { get; set; }

        [JsonIgnore]
        public string Okato { get; set; }

        [JsonProperty(PropertyName = "oktmo")]
        public string Oktmo { get; set; }

        [JsonProperty(PropertyName = "details_payment")]
        public string Details { get; set; }

        [JsonProperty(PropertyName = "payer_info")]
        public GisChargeJsonPayer Payer { get; set; }
        
        public List<GisJsonAdditionalField> addition_fields { get; set; }

        [JsonProperty(PropertyName = "operation_name")]
        public string OperationName { get; set; }

        [JsonIgnore]
        public string SupplierOrgId { get; set; }

        [JsonIgnore]
        public GisChargeJsonSupplier Supplier { get; set; }

        [JsonIgnore]
        public GisChargeJsonAccount Account { get; set; }

        [JsonIgnore]
        public GisChargeJsonBank Bank { get; set; }

        [JsonProperty(PropertyName = "budget_index")]
        public GisChargeBudgetIndex BudgetIndex { get; set; }
    }

    public class GisChargeJsonPayer
    {
        [JsonProperty(PropertyName = "payer_type")]
        public string PayerType { get; set; }

        [JsonProperty(PropertyName = "payer_docnumber")]
        public string PayerDocNumber { get; set; }

        [JsonProperty(PropertyName = "payer_kpp")]
        public string Kpp { get; set; }

        [JsonProperty(PropertyName = "payer_u_code")]
        public string PayerCode { get; set; }

        [JsonProperty(PropertyName = "payer_u_caption")]
        public string PayerCaption { get; set; }
    }

    public class GisChargeJsonSupplier
    {
        [JsonProperty(PropertyName = "INN")]
        public string Inn { get; set; }

        [JsonProperty(PropertyName = "KPP")]
        public string Kpp { get; set; }
    }

    public class GisChargeJsonAccount
    {
        [JsonProperty(PropertyName = "Account")]
        public string Account { get; set; }
    }

    public class GisChargeJsonBank
    {
        [JsonProperty(PropertyName = "BIK")]
        public string Bik { get; set; }
    }

    public class GisChargeBudgetIndex
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "payment_type")]
        public string PaymentType { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "purpose")]
        public string Purpose { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "tax_period")]
        public string TaxPeriod { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "tax_doc_number")]
        public string TaxDocNumber { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "tax_doc_date")]
        public string TaxDocDate { get; set; }
    }

    public class GisJsonAdditionalField
    {
        [JsonProperty(PropertyName = "addition_name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "addition_value")]
        public string Value { get; set; }
    }
}