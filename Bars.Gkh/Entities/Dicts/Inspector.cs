namespace Bars.Gkh.Entities

{

    using Bars.Gkh.Enums;

    /// <summary>
    /// Члены комиссии
    /// </summary>
    public class Inspector : BaseGkhEntity
    {
        /// <summary>
        /// Тип участника комиссии
        /// </summary>
        public virtual TypeCommissionMember TypeCommissionMember { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string Position { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual Position NotMemberPosition { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string Fio { get; set; }

        /// <summary>
        /// Фамилия И.О.
        /// </summary>
        public virtual string ShortFio { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// Электронный адрес
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Начальник
        /// </summary>
        public virtual bool IsHead { get; set; }

        /// <summary>
        /// Комиссия
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// ФИО Родительный падеж
        /// </summary>
        public virtual string FioGenitive { get; set; }

        /// <summary>
        /// ФИО Дательный падеж
        /// </summary>
        public virtual string FioDative { get; set; }

        /// <summary>
        /// ФИО Винительный падеж
        /// </summary>
        public virtual string FioAccusative { get; set; }

        /// <summary>
        /// ФИО Творительный падеж
        /// </summary>
        public virtual string FioAblative { get; set; }

        /// <summary>
        /// ФИО Предложный падеж
        /// </summary>
        public virtual string FioPrepositional { get; set; }

        /// <summary>
        /// Должность Родительный падеж
        /// </summary>
        public virtual string PositionGenitive { get; set; }

        /// <summary>
        /// Должность Дательный падеж
        /// </summary>
        public virtual string PositionDative { get; set; }

        /// <summary>
        /// Должность Винительный падеж
        /// </summary>
        public virtual string PositionAccusative { get; set; }

        /// <summary>
        /// Должность Творительный падеж
        /// </summary>
        public virtual string PositionAblative { get; set; }

        /// <summary>
        /// Должность Предложный падеж
        /// </summary>
        public virtual string PositionPrepositional { get; set; }

        /// <summary>
        /// Работает
        /// </summary>
        public virtual bool Active { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        public virtual Subdivision Subdivision { get; set; }
    }
}
