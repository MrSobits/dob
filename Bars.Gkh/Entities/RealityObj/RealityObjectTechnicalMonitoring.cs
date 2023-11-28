namespace Bars.Gkh.Entities.RealityObj
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    public class RealityObjectTechnicalMonitoring : BaseEntity
    {
        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual MonitoringTypeDict MonitoringTypeDict { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime DocumentDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Выводить документ на портал
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}