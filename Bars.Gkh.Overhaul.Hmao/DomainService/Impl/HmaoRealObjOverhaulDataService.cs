﻿namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System;
    using Bars.B4;
    using System.Linq;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Сервис для работы с домами в ДПКР
    /// </summary>
    public class HmaoRealObjOverhaulDataService : IRealObjOverhaulDataService
    {
        /// <summary>
        /// Домен-сервис для сущности Запись Опубликованной программы
        /// </summary>
        public IDomainService<PublishedProgramRecord> PublishProgramRecordDomain { get; set; }

        /// <summary>
        /// Получить Дату публикации для дома
        /// </summary>
        /// <param name="ro">Жилой дом</param>
        /// <returns></returns>
        public DateTime? GetPublishDateByRo(RealityObject ro)
        {
            return this.PublishProgramRecordDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .Where(x => x.RealityObject.Id == ro.Id)
                    .OrderByDescending(x => x.PublishedProgram.ObjectEditDate)
                    .Select(x => x.PublishedProgram.PublishDate)
                    .FirstOrDefault();
        }
    }
}