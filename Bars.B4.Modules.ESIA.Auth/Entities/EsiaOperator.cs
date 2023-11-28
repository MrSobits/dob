namespace Bars.Gkh.Entities.Administration.Operator
{
    using B4.DataAccess;
    using Operator = Entities.Operator;

    /// <summary>
    /// Оператор с привязанной учеткой ЕСИА
    /// </summary>
    public class EsiaOperator : BaseEntity
    {
        /// <summary>
        /// Оператор
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// esia attribute: userId
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// esia attribute: userName
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// esia attribute: gender
        /// </summary>
        public virtual string Gender { get; set; }

        /// <summary>
        /// esia attribute: lastName
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// esia attribute: firstName
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// esia attribute: middleName
        /// </summary>
        public virtual string MiddleName { get; set; }

        /// <summary>
        /// esia attribute: personSNILS
        /// </summary>
        public virtual string PersonSnils { get; set; }

        /// <summary>
        /// esia attribute: personEMail
        /// </summary>
        public virtual string PersonEmail { get; set; }

        /// <summary>
        /// esia attribute: birthDate
        /// </summary>
        public virtual string BirthDate { get; set; }

        /// <summary>
        /// esia attribute: orgPosition
        /// </summary>
        public virtual string OrgPosition { get; set; }

        /// <summary>
        /// esia attribute: orgName
        /// </summary>
        public virtual string OrgName { get; set; }

        /// <summary>
        /// esia attribute: orgShortName
        /// </summary>
        public virtual string OrgShortName { get; set; }

        /// <summary>
        /// esia attribute: orgType
        /// </summary>
        public virtual string OrgType { get; set; }

        /// <summary>
        /// esia attribute: orgOGRN
        /// </summary>
        public virtual string OrgOgrn { get; set; }

        /// <summary>
        /// esia attribute: orgINN
        /// </summary>
        public virtual string OrgInn { get; set; }

        /// <summary>
        /// esia attribute: orgKPP
        /// </summary>
        public virtual string OrgKpp { get; set; }

        /// <summary>
        /// esia attribute: orgAddresses
        /// </summary>
        public virtual string OrgAddresses { get; set; }

        /// <summary>
        /// esia attribute: orgLegalForm
        /// </summary>
        public virtual string OrgLegalForm { get; set; }

        /// <summary>
        /// Наименование организации
        /// </summary>
        public virtual string FullName { get; set; }
    }
}