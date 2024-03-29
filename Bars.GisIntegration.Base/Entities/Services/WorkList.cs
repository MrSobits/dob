﻿namespace Bars.GisIntegration.Base.Entities.Services
{
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.HouseManagement;

    /// <summary>
    /// Перечень работ/услуг
    /// </summary>
    public class WorkList : BaseRisEntity
    {
        /// <summary>
        /// Договор управления
        /// </summary>
        public virtual RisContract Contract { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual RisHouse House { get; set; }

        /// <summary>
        /// Период "с"(месяц)
        /// </summary>
        public virtual int MonthFrom { get; set; }

        /// <summary>
        /// Период "с"(год)
        /// </summary>
        public virtual short YearFrom { get; set; }

        /// <summary>
        /// Период "по"(месяц)
        /// </summary>
        public virtual int MonthTo { get; set; }

        /// <summary>
        /// Период "по"(год)
        /// </summary>
        public virtual short YearTo { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}