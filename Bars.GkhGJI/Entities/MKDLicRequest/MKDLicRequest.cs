using Bars.Gkh.Enums;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Voronezh.Enums;

    /// <summary>
    /// Обращение граждан
    /// </summary>
    public partial class MKDLicRequest : BaseEntity, IStatefulEntity
    {

        /// <summary>
        /// Статус заявления
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Тип заявителя
        /// </summary>
        public virtual ExecutantDocGji ExecutantDocGji { get; set; }

        /// <summary>
        /// Заявитель-контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Контрагент в заявлении
        /// </summary>
        public virtual Contragent StatmentContragent { get; set; }

        /// <summary>
        /// Заявитель - физлицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Дата заявления
        /// </summary>
        public virtual DateTime StatementDate { get; set; }

        /// <summary>
        /// Номер заявления
        /// </summary>
        public virtual string StatementNumber { get; set; }

        /// <summary>
        /// Тип запроса
        /// </summary>
        public virtual MKDLicTypeRequest MKDLicTypeRequest { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Inspector { get; set; }


        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Результат рассмотрения
        /// </summary>
        public virtual LicStatementResult LicStatementResult { get; set; }

        /// <summary>
        /// Комментарий к результату рассмотрения
        /// </summary>
        public virtual string LicStatementResultComment { get; set; }

        /// <summary>
        /// Номер заключения
        /// </summary>
        public virtual string ConclusionNumber { get; set; }

        /// <summary>
        /// Дата заключения
        /// </summary>
        public virtual DateTime? ConclusionDate { get; set; }

        /// <summary>
        /// Обжалование
        /// </summary>
        public virtual bool Objection { get; set; }

        /// <summary>
        /// Обжаловано
        /// </summary>
        public virtual DisputeResult ObjectionResult { get; set; }

    }
}