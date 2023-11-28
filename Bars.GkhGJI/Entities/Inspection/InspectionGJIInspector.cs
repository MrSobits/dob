﻿namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Инспектора в проверке
    /// </summary>
    public class InspectionGjiInspector : BaseGkhEntity
    {
        /// <summary>
        /// Мероприятие комиссии
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }
    }
}