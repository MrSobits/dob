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
    /// Заседания комиссии
    /// </summary>
    public partial class ComissionMeeting : BaseEntity, IStatefulEntity
    {

        /// <summary>
        /// Статус заседания
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Наименование заседания
        /// </summary>
        public virtual string ComissionName { get; set; }

        /// <summary>
        /// Заседающая комиссия
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Дата заседания
        /// </summary>
        public virtual DateTime CommissionDate { get; set; }

        /// <summary>
        /// Номер заседания
        /// </summary>
        public virtual string CommissionNumber { get; set; }

        /// <summary>
        /// Время начала
        /// </summary>
        public virtual string TimeStart { get; set; }

        /// <summary>
        /// Время окончания 
        /// </summary>
        public virtual string TimeEnd { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

    }
}