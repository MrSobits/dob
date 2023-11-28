namespace Bars.GkhDi.Controllers
{
    using System.Web.Mvc;
    using B4;

    using Bars.B4.Modules.FileStorage;
    using Bars.GkhDi.DomainService;

    using Entities;

    public class ServiceController : FileStorageDataController<BaseService>
    {
        public ActionResult GetCountMandatory(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IServService>().GetCountMandatory(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CopyService(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IServService>().CopyService(baseParams);
            return result.Success ? new JsonNetResult(new { success = true, message = result.Message, data = result.Data }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetInfoServPeriod(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IServService>().GetInfoServPeriod(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CopyServPeriod(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IServService>().CopyServPeriod(baseParams);
            return result.Success ? new JsonNetResult(new { success = true, message = result.Message, data = result.Data }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetUnfilledMandatoryServs(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IServService>().GetUnfilledMandatoryServs(baseParams);
            return result.Success ? new JsonNetResult(new { success = true, message = result.Message }) : JsonNetResult.Failure(result.Message);
        }

       /* private IDataTransaction BeginTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }

        public ActionResult UpdateTariffForConsummers()
        {
            // Собираем тарифы и группируем их по услугам
            var tariffConsumersDict = this.Container.Resolve<IDomainService<TariffForConsumers>>()
                .GetAll()
                .AsEnumerable()
                .GroupBy(x => x.BaseService.Id)
                .ToDictionary(x => x.Key, y => y.ToList());

            var baseService = this.Container.Resolve<IRepository<BaseService>>();

            var i = 0;

            using (var transaction = BeginTransaction())
            {
                try
                {
                    foreach (var item in tariffConsumersDict)
                    {
                        var tariffForConsumers = this.Container.Resolve<IServService>().GetTariffWithMaxDate(item.Value);

                        tariffForConsumers.BaseService.TariffForConsumers = tariffForConsumers.Cost;
                        tariffForConsumers.BaseService.TariffIsSetForDi = tariffForConsumers.TariffIsSetFor;
                        tariffForConsumers.BaseService.DateStartTariff = tariffForConsumers.DateStart;

                        baseService.Update(tariffForConsumers.BaseService);
                        i++;
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

            return new JsonNetResult(new { success = true });
        }*/
    }
}

