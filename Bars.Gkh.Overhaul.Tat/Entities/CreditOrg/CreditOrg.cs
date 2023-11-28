namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;

    public class CreditOrg : BaseEntity
    {
        /// <summary>
        /// Родительская кредитная организация
        /// </summary>
        public virtual CreditOrg Parent { get; set; }

        /// <summary>
        /// Фактический адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasAddress { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Является филиалом
        /// </summary>
        public virtual bool IsFilial { get; set; }

        /// <summary>
        /// Строковый адрес фиас
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Адрес за пределами субъекта
        /// </summary>
        public virtual string AddressOutSubject { get; set; }

        /// <summary>
        /// Адрес за пределами субъекта
        /// </summary>
        public virtual bool IsAddressOut { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public virtual string Kpp { get; set; }

        /// <summary>
        /// БИК
        /// </summary>
        public virtual string Bik { get; set; }

        /// <summary>
        /// ОКПО
        /// </summary>
        public virtual string Okpo { get; set; }

        /// <summary>
        /// Корреспондентский счет
        /// </summary>
        public virtual string CorrAccount { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual string Ogrn { get; set; }

        /// <summary>
        /// Почтовый адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasMailingAddress { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public virtual string MailingAddress { get; set; }
    }
}
