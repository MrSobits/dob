﻿namespace Bars.GkhGji.Regions.Khakasia.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities;

    public class KhakasiaProtocolDefinition : ProtocolDefinition
    {
        /// <summary>
        /// Время начала
        /// </summary>
        public virtual DateTime? TimeStart { get; set; }

        /// <summary>
        /// Время окончания
        /// </summary>
        public virtual DateTime? TimeEnd { get; set; }

        /// <summary>
        /// Файл описание
        /// </summary>
        public virtual FileInfo FileDescription { get; set; }
    }
}
