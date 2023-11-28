namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    public class SectionImport19 : ISectionImport
    {
        public IDomainService<InfoAboutPaymentCommunal> InfoAboutPaymentCommunalService { get; set; }

        public IDomainService<DisclosureInfoRealityObj> DisclosureInfoRealityObjService { get; set; }

        public IDomainService<BaseService> BaseService { get; set; }
        
        public IDomainService<DisclosureInfoRelation> DisclosureInfoRelationService { get; set; }

        public IDomainService<DisclosureInfo> DisclosureInfoService { get; set; }

        public IDomainService<PeriodDi> PeriodDiService { get; set; }

        public string Name
        {
            get { return "Импорт из комплат секция #19"; }
        }

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var infoPaymentComm = new List<InfoAboutPaymentCommunal>();

            var sectionsData = importParams.SectionData;

            if (sectionsData.Section19.Count == 0)
            {
                return;
            }

            var inn = importParams.Inn;
            var logImport = importParams.LogImport;
            var realityObjects = importParams.RealityObjectIds;
            var realityObjectDict = importParams.RealObjsImportInfo;

            if (realityObjects.Count == 0)
            {
                logImport.Warn(this.Name, string.Format("Нет домов под управлением УК с ИНН {0}", inn));
                return;
            }

            var disclosureInfo = this.DisclosureInfoService
                .GetAll()
                .FirstOrDefault(
                    x => x.ManagingOrganization.Contragent.Inn == inn
                        && x.PeriodDi.Id == importParams.PeriodDiId);

            if (disclosureInfo == null)
            {
                logImport.Warn(this.Name, string.Format("Для УК с ИНН {0} не начато раскрытие делятельности в периоде {1}",
                    inn,
                    PeriodDiService.GetAll().Where(x => x.Id == importParams.PeriodDiId).Select(x => x.Name)));
                return;
            }

            var disclosureInfoRelations =
                this.DisclosureInfoRelationService.GetAll().Where(
                    x => realityObjects.Contains(x.DisclosureInfoRealityObj.RealityObject.Id) && x.DisclosureInfo.Id == disclosureInfo.Id).ToList();

            foreach (var section19Record in sectionsData.Section19)
            {
                var realityObject = realityObjectDict.ContainsKey(section19Record.CodeErc) ? realityObjectDict[section19Record.CodeErc] : null;
                if (realityObject == null)
                {
                    logImport.Warn(this.Name, string.Format("Не удалось получить дом с кодом ЕРЦ {0}", section19Record.CodeErc));
                    continue;
                }

                if (section19Record.Code.IsEmpty())
                {
                    logImport.Warn(this.Name, string.Format("Не найдена услуга с кодом  {0}", section19Record.CodeCommunalPay));
                    continue;
                }

                var disclosureInfoRelation = disclosureInfoRelations.FirstOrDefault(x => x.DisclosureInfoRealityObj.RealityObject.Id == realityObject.Id);

                if (disclosureInfoRelation == null)
                {
                    logImport.Warn(this.Name, string.Format("Не добавлена услуга с кодом  {0}", section19Record.Code));
                    continue;
                }

                var baseService = this.BaseService
                    .GetAll()
                    .Where(
                        x => x.DisclosureInfoRealityObj.Id == disclosureInfoRelation.DisclosureInfoRealityObj.Id
                            && x.TemplateService.TypeGroupServiceDi == TypeGroupServiceDi.Communal)
                    .FirstOrDefault(x => x.TemplateService.Code == section19Record.Code);

                if (baseService == null)
                {
                    logImport.Warn(this.Name, string.Format("Не добавлена услуга с кодом  {0}", section19Record.Code));
                    continue;
                }

                var infoAboutPaymentCommunal = this.InfoAboutPaymentCommunalService
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRelation.DisclosureInfoRealityObj.Id)
                    .FirstOrDefault(x => x.BaseService.TemplateService.Code == section19Record.Code);

                if (infoAboutPaymentCommunal == null)
                {
                    infoAboutPaymentCommunal = new InfoAboutPaymentCommunal
                    {
                        DisclosureInfoRealityObj = disclosureInfoRelation.DisclosureInfoRealityObj,
                        BaseService = baseService
                    };
                }

                infoAboutPaymentCommunal.CounterValuePeriodStart = section19Record.MeterBegin;
                infoAboutPaymentCommunal.CounterValuePeriodEnd = section19Record.MeterEnd;
                infoAboutPaymentCommunal.TotalConsumption = section19Record.ConsVol;
                infoAboutPaymentCommunal.Accrual = section19Record.AssessedCons;
                infoAboutPaymentCommunal.Payed = section19Record.PaidCons;
                infoAboutPaymentCommunal.Debt = section19Record.DebtCons;
                infoAboutPaymentCommunal.AccrualByProvider = section19Record.AssessedSupp;
                infoAboutPaymentCommunal.PayedToProvider = section19Record.PaidSupp;
                infoAboutPaymentCommunal.DebtToProvider = section19Record.DebtSupp;
                infoAboutPaymentCommunal.ReceivedPenaltySum = section19Record.ConsPenaltySum;

                if (infoAboutPaymentCommunal.Id > 0)
                {
                    logImport.CountChangedRows++;
                }
                else
                {
                    logImport.CountAddedRows++;
                }

                infoPaymentComm.Add(infoAboutPaymentCommunal);
            }

            this.InTransaction(infoPaymentComm, this.InfoAboutPaymentCommunalService);
        }

        /// <summary>
        /// Транзакция
        /// </summary>
        /// <param name="list"></param>
        /// <param name="repos"></param>
        private void InTransaction(IEnumerable<PersistentObject> list, IDomainService repos)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        if (entity.Id > 0)
                        {
                            repos.Update(entity);
                        }
                        else
                        {
                            repos.Save(entity);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }
}