﻿namespace Bars.GkhGji.Regions.Voronezh.Entities.SMEVOwnershipProperty
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    public class SMEVOwnershipPropertyFile : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС ЕГРЮЛ
        /// </summary>
        public virtual SMEVOwnershipProperty SMEVOwnershipProperty { get; set; }

        /// <summary>
        /// Тип запроса/ответа
        /// </summary>
        public virtual SMEVFileType SMEVFileType { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}
