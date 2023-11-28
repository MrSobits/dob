﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    public class SMEVFNSLicRequestFile : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС ФНС
        /// </summary>
        public virtual SMEVFNSLicRequest SMEVFNSLicRequest { get; set; }

        /// <summary>
        /// Тип запроса/ответа
        /// </summary>
        public virtual SMEVFileType SMEVFileType { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual  FileInfo FileInfo { get; set; }
    }
}
