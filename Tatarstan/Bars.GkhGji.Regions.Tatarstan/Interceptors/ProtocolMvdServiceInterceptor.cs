namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Interceptors;

    public class ProtocolMvdServiceInterceptor : ProtocolMvdServiceInterceptor<ProtocolMvd>
    {
        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ProtocolMvd> service, ProtocolMvd entity)
        {
            var protocolMvdRealityObjectService = this.Container.ResolveDomain<ProtocolMvdRealityObject>();
            try
            {
                if (!protocolMvdRealityObjectService.GetAll().Any(x => x.ProtocolMvd.Id == entity.Id))
                {
                    return this.Failure("Следующий раздел \"Адрес правонарушения\" обязателен для заполнения");
                }

                if (entity.DateOffense.HasValue)
                {
                    var date = entity.DateOffense.ToDateTime();
                    var dateTime = DateTime.Now;

                    DateTime.TryParse(entity.TimeOffense, out dateTime);

                    entity.DateOffense = new DateTime(date.Year, date.Month, date.Day, dateTime.Hour, dateTime.Minute, 0);
                }
            }
            finally
            {
                this.Container.Release(protocolMvdRealityObjectService);
            }

            return base.BeforeUpdateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<ProtocolMvd> service, ProtocolMvd entity)
        {
            var protocolMvdArticleLawDomain = this.Container.Resolve<IDomainService<ProtocolMvdArticleLaw>>();
            var articleLawGjiDomain = this.Container.Resolve<IDomainService<ArticleLawGji>>();

            try
            {
                var articleLawGj = articleLawGjiDomain.GetAll().FirstOrDefault(x => x.Code == "15");

                var protocolMvdArticleLaw = new ProtocolMvdArticleLaw
                {
                    ProtocolMvd = entity,
                    ArticleLaw = articleLawGj
                };

                protocolMvdArticleLawDomain.Save(protocolMvdArticleLaw);
            }
            finally
            {
                this.Container.Release(protocolMvdArticleLawDomain);
                this.Container.Release(articleLawGjiDomain);
            }

            return base.AfterCreateAction(service, entity);
        }
    }
}