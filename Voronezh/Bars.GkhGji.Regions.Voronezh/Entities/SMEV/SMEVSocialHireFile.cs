﻿namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    public class SMEVSocialHireFile : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС
        /// </summary>
        public virtual SMEVSocialHire SMEVSocialHire { get; set; }

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
