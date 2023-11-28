namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.Collections;
    using System.Web.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class ProgramVersionController : B4.Alt.DataController<ProgramVersion>
    {
        public IProgramVersionService Service { get; set; }

        public ActionResult CopyProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IProgramVersionService>().CopyProgram(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetMainVersionByMunicipality(BaseParams baseParams)
        {
            var result = Service.GetMainVersionByMunicipality(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ActualizeFromShortCr(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeFromShortCr(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ActualizeNewRecords(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeNewRecords(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ActualizeSum(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeSum(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ActualizeYear(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeYear(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult GetWarningMessage(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.GetWarningMessage(baseParams);
                return result.Success ? JsSuccess(result.Message) : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult GetDeletedEntriesList(BaseParams baseParams)
        {
            var service = Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetDeletedEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ActualizeDeletedEntries(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeDeletedEntries(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ActualizeGroup(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeGroup(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ActualizeOrder(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeOrder(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}