namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Version;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Bars.Gkh.Overhaul.Hmao.Helpers;
    using Bars.Gkh.Utils;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Контроллер версии программы
    /// </summary>
    public class ProgramVersionController : B4.Alt.DataController<ProgramVersion>
    {
        #region Properties

        public IMakeKPKRFromDPKRService KPKRService { get; set; }

        public IFakePublicationService FakePublicationService { get; set; }

        public ICostService CostService { get; set; }

        public IAddVersionRecord AddVersionRecordService { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<ProgramVersion> VersionDomain { get; set; }

        public IDomainService<VersionActualizeLog> VersionActualizeLogDomain { get; set; }

        /// <summary>
        /// Лог сервис
        /// </summary>
        public IActualizeVersionLogService LogService { get; set; }


        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public IDomainService<VersionRecordStage1> VersionRecordStage1Domain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RealityObjectStructuralElementDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgramm> stage1Service { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage2> stage2Service { get; set; }

        public IDomainService<StructuralElementGroup> StructuralElementGroupDomain { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> RealityObjectStructuralElementInProgrammStage3Domain { get; set; }



        #endregion

        #region Public methods

        public ActionResult MoveTypeWork(BaseParams baseParams, Int64 programId, Int64 typeworkToMoveId)
        {
           

            try
            {
                return KPKRService.MoveTypeWork(baseParams, programId, typeworkToMoveId).ToJsonResult();
            }
            finally
            {
                //this.Container.Release(service);
            }
        }

        /// <summary>
        /// Изменить данные версии
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ChangeVersionData(BaseParams baseParams)
        {
            var programVersionService = this.Container.Resolve<IProgramVersionService>();

            try
            {
                return programVersionService.ChangeVersionData(baseParams).ToJsonResult("text/html; charset=utf-8");
            }
            finally
            {
                this.Container.Release(programVersionService);
            }
        }

        /// <summary>
        /// Экспорт
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("VersionRecordsExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally
            {
                this.Container.Release(export);
            }
        }

        /// <summary>
        /// Получить дату расчета ДПКР
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetDateCalcDpkr(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVersionDateCalcService>();
            try
            {
                var result = service.GetDateCalcDpkr(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить дату расчета показателей собираемости
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetDateCalcOwnerCollection(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVersionDateCalcService>();

            try
            {
                var result = service.GetDateCalcOwnerCollection(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить дату расчета корректировки
        /// </summary>
        /// <param name="baseParams">Получить дату опубликования</param>
        /// <returns></returns>
        public ActionResult GetDateCalcCorrection(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVersionDateCalcService>();

            try
            {
                var result = service.GetDateCalcCorrection(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить дату опубликования
        /// </summary>
        /// <param name="baseParams">Получить дату опубликования</param>
        /// <returns></returns>
        public ActionResult GetDateCalcPublished(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVersionDateCalcService>();

            try
            {
                var result = service.GetDateCalcPublished(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Создать новую версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult MakeNewVersion(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = service.MakeNewVersion(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Создать новую версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult MakeNewVersionAll(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = this.Container.Resolve<IProgramVersionService>().MakeNewVersionAll(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Добавить новые записи
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult AddNewRecords(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.AddNewRecords(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать стоимость
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizeSum(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeSum(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать год
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizeYear(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeYear(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать год для Ставрополя
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizeYearForStavropol(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeYearForStavropol(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать очередность
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizePriority(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizePriority(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Акутализировать с КПКР
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizeFromShortCr(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeFromShortCr(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить предупреждение
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetWarningMessage(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.GetWarningMessage(baseParams);
                return result.Success ? this.JsSuccess(result.Message) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список записей на удаление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetDeletedEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetDeletedEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список записей на добавление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetAddEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetAddEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список записей на актуализацию стоимости
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetActualizeSumEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetActualizeSumEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список записей на актуализацию года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetActualizeYearEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetActualizeYearEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список записей на актуализацию изменения года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetActualizeYearChangeEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetActualizeYearChangeEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Список для массового изменения года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ListForMassChangeYear(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.ListForMassChangeYear(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Массовое изменение года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult MassChangeYear(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = service.MassChangeYear(baseParams);
                return result.Success ? this.JsSuccess(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// формирование КПКР из ДПКР
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult MakeKPKR(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            var StartYear = baseParams.Params.GetAs<short>("StartYear");
            var YearCount = baseParams.Params.GetAs<byte>("YearCount");
            var FirstYearPSD = baseParams.Params.GetAs<bool>("FirstYearPSD");
            var FirstYearWithoutWork = baseParams.Params.GetAs<bool>("FirstYearWithoutWork");
            var EathWorkPSD = baseParams.Params.GetAs<bool>("EathWorkPSD");
            var SKWithWorks = baseParams.Params.GetAs<bool>("SKWithWorks");
            var PSDWithWorks = baseParams.Params.GetAs<bool>("PSDWithWorks");
            var OneProgramCR = baseParams.Params.GetAs<bool>("OneProgramCR");
            var PSDNext3 = baseParams.Params.GetAs<bool>("PSDNext3");

            try
            {
                KPKRService.MakeKPKR(version, StartYear, YearCount, FirstYearPSD, FirstYearWithoutWork, SKWithWorks, PSDWithWorks, PSDNext3, EathWorkPSD, OneProgramCR);
                return JsSuccess();
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// формирование подпрограммы КПКР из ДПКР
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult MakeSubKPKR(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            var StartYear = baseParams.Params.GetAs<short>("StartYear");
            var YearCount = baseParams.Params.GetAs<byte>("YearCount");
            var FirstYearPSD = baseParams.Params.GetAs<bool>("FirstYearPSD");
            var FirstYearWithoutWork = baseParams.Params.GetAs<bool>("FirstYearWithoutWork");
            var SelectedKE = baseParams.Params.GetAs<string>("SelectedKE").ToLongArray();

            try
            {
                if (!KPKRService.CheckCostsByYear(version, SelectedKE))
                    return JsonNetResult.Failure("Превышена максимальная стоимость");

                KPKRService.MakeSubKPKR(version, StartYear, YearCount, FirstYearPSD, FirstYearWithoutWork, SelectedKE);
                return JsSuccess();
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// список стоимостей по годам для подпрограммы КПКР
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        public ActionResult GetCostsByYear(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            var StartYear = baseParams.Params.GetAs<short>("StartYear");
            var YearCount = baseParams.Params.GetAs<byte>("YearCount");
            var FirstYearPSD = baseParams.Params.GetAs<bool>("FirstYearPSD");
            var FirstYearWithoutWork = baseParams.Params.GetAs<bool>("FirstYearWithoutWork");
            var SelectedKE = baseParams.Params.GetAs<string>("SelectedKE").ToLongArray();

            try
            {
                var list = KPKRService.GetCostsByYear(version, SelectedKE);

                return new JsonListResult(list.Order(baseParams).Paging(baseParams), list.Count);
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// список конструктивных элементов из работ
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        public ActionResult GetKE(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            var StartYear = baseParams.Params.GetAs<short>("StartYear");
            var YearCount = baseParams.Params.GetAs<byte>("YearCount");

            try
            {
                var list = KPKRService.GetKE(version, StartYear, YearCount).Select(x => new
                {
                    x.Id,
                    KE = x.CommonEstateObject.Name,
                    x.RealityObject.Address,
                    x.Sum
                }).Filter(baseParams).ToList();

                return new JsonListResult(list.Order(baseParams).Paging(baseParams), list.Count);
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        public ActionResult GetCosts(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            var SelectedKE = baseParams.Params.GetAs<string>("SelectedKE").ToLongArray();

            try
            {
                return new JsonNetResult(KPKRService.GetCosts(version, SelectedKE));
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizeDeletedEntries(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeDeletedEntries(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Копировать версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult CopyVersion(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = service.CopyVersion(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult RoofCorrection(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.RoofCorrection(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult CopyCorrectedYears(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.CopyCorrectedYears(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult DeleteRepeatedWorks(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.DeleteRepeatedWorks(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Опубликовать как есть
        /// </summary>
        public ActionResult PublishAsIs(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var id = baseParams.Params.GetAs<long>("versionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            try
            {
                FakePublicationService.CreateDpkrForPublish(version);
                return JsonNetResult.Success;
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// Пересчитать стоимости
        /// </summary>
        public ActionResult CalculateCosts(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var id = baseParams.Params.GetAs<long>("versionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            try
            {
                CostService.CalculateVersion(version);
                return JsonNetResult.Success;
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// Удалить запись
        /// </summary>
        public ActionResult Hide(BaseParams baseParams, Int64 stage3Id)
        {
            try
            {
                VersionRecordStage1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.Id == stage3Id)
                    .ForEach(x =>
                    {
                        x.VersionRecordState = Hmao.Enum.VersionRecordState.NonActual;
                        x.StateChangeDate = DateTime.Now;
                        VersionRecordStage1Domain.Save(x);
                    });
                Operator thisOperator = UserManager.GetActiveOperator();
                VersionRecordDomain.GetAll()
                    .Where(x => x.Id == stage3Id)
                    .ForEach(x =>
                    {
                        x.Show = false;
                        VersionRecordDomain.Save(x);                       

                        var log = new VersionActualizeLog();
                        log.ActualizeType = VersionActualizeType.ActualizeDeletedEntries;
                        log.DateAction = DateTime.Now;
                        log.Municipality = x.RealityObject.Municipality;
                        log.ProgramVersion = x.ProgramVersion;
                        log.UserName = thisOperator.User.Name;
                        log.CountActions = 1;

                        var logRecords = new List<ActualizeVersionLogRecord>();
                        var logRecord = new ActualizeVersionLogRecord
                        {
                            TypeAction = VersionActualizeType.ActualizeDeletedEntries,
                            Action = "Удаление",
                            Description = "Ручное удаление записи",
                            Address = x.RealityObject.Address,
                            Ceo = x.CommonEstateObjects,
                            PlanYear = x.Year                           
                        };

                        logRecords.Add(logRecord);
                        if (logRecords.Any())
                        {
                            log.CountActions = logRecords.Count;
                            log.LogFile = this.LogService.CreateLogFile(logRecords.OrderBy(y => y.Address).OrderBy(y => y.Number), baseParams);
                        }
                        VersionActualizeLogDomain.Save(log);
                    });

                

                return JsonNetResult.Success;
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        public ActionResult RemoveFromSubProgramm(BaseParams baseParams, Int64 stage3Id)
        {
            try
            {
                var stage2Container = this.Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage2>>();
                var stageRecord = RealityObjectStructuralElementInProgrammStage3Domain.Get(stage3Id);
                var stage2stageRecords = stage2Container.GetAll()
                    .Where(x => x.Stage3.Id == stage3Id)
                    .ToList();
                foreach (var rec in stage2stageRecords)
                {
                    rec.Stage3 = null;
                    stage2Container.Update(rec);
                }
                RealityObjectStructuralElementInProgrammStage3Domain.Delete(stage3Id);

                return JsonNetResult.Success;
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// Восстановить запись
        /// </summary>
        public ActionResult Restore(BaseParams baseParams, Int64 stage3Id)
        {
            try
            {             

                VersionRecordStage1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.Id == stage3Id)
                    .ForEach(x =>
                    {
                        x.VersionRecordState = Hmao.Enum.VersionRecordState.Actual;
                        x.StateChangeDate = DateTime.Now;
                        VersionRecordStage1Domain.Save(x);
                    });
                Operator thisOperator = UserManager.GetActiveOperator();
                VersionRecordDomain.GetAll()
                    .Where(x => x.Id == stage3Id)
                    .ForEach(x =>
                    {
                        x.Show = true;
                        VersionRecordDomain.Save(x);

                        var log = new VersionActualizeLog();
                        log.ActualizeType = VersionActualizeType.ActualizeNewRecords;
                        log.DateAction = DateTime.Now;
                        log.Municipality = x.RealityObject.Municipality;
                        log.ProgramVersion = x.ProgramVersion;
                        log.UserName = thisOperator.User.Name;
                        log.CountActions = 1;

                        var logRecords = new List<ActualizeVersionLogRecord>();
                        var logRecord = new ActualizeVersionLogRecord
                        {
                            TypeAction = VersionActualizeType.ActualizeDeletedEntries,
                            Action = "Восстановление",
                            Description = "Ручное восстановление записи",
                            Address = x.RealityObject.Address,
                            Ceo = x.CommonEstateObjects,
                            PlanYear = x.Year
                        };

                        logRecords.Add(logRecord);
                        if (logRecords.Any())
                        {
                            log.CountActions = logRecords.Count;
                            log.LogFile = this.LogService.CreateLogFile(logRecords.OrderBy(y => y.Address).OrderBy(y => y.Number), baseParams);
                        }
                        VersionActualizeLogDomain.Save(log);
                    });

                return JsonNetResult.Success;
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// Перенести в подпрограмму
        /// </summary>
        public ActionResult InSubDPKR(BaseParams baseParams, Int64 stage3Id)
        {
            try
            {
                var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
                var startYear = config.ProgrammPeriodStart;
                var endYear = config.ProgrammPeriodEnd;

                VersionRecord vr = VersionRecordDomain.Get(stage3Id);
                var realityObjectRepo = this.Container.Resolve<IRepository<RealityObject>>();

                RealityObject gro = realityObjectRepo.Get(vr.RealityObject.Id);
                vr.SubProgram = true;
                vr.Year = 2050;
                //vr.Year = endYear;
                gro.IsSubProgram = true;
                realityObjectRepo.Update(gro);
                VersionRecordDomain.Update(vr);
                Operator thisOperator = UserManager.GetActiveOperator();
                var log = new VersionActualizeLog();
                log.ActualizeType = VersionActualizeType.InSubDPKR;
                log.DateAction = DateTime.Now;
                log.Municipality = vr.RealityObject.Municipality;
                log.ProgramVersion = vr.ProgramVersion;
                log.UserName = thisOperator.User.Name;
                log.CountActions = 1;

                var logRecords = new List<ActualizeVersionLogRecord>();
                var logRecord = new ActualizeVersionLogRecord
                {
                    TypeAction = VersionActualizeType.InSubDPKR,
                    Action = "Перенос в подпрограмму",
                    Description = "Ручной перенос в подпрограмму",
                    Address = vr.RealityObject.Address,
                    Ceo = vr.CommonEstateObjects,
                    PlanYear = vr.Year
                };

                logRecords.Add(logRecord);
                if (logRecords.Any())
                {
                    log.CountActions = logRecords.Count;
                    log.LogFile = this.LogService.CreateLogFile(logRecords.OrderBy(y => y.Address).OrderBy(y => y.Number), baseParams);
                }
                VersionActualizeLogDomain.Save(log);
                return JsonNetResult.Success;
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// Вернуть из в подпрограммы
        /// </summary>
        public ActionResult ReInSubDPKR(BaseParams baseParams, Int64 stage3Id)
        {
            try
            {

                VersionRecord vr = VersionRecordDomain.Get(stage3Id);
                var realityObjectRepo = this.Container.Resolve<IRepository<RealityObject>>();

                RealityObject gro = realityObjectRepo.Get(vr.RealityObject.Id);
                vr.SubProgram = false;
                var oOiInSubVersion = VersionRecordDomain.GetAll()
                    .Where(x => x.RealityObject != null && x.RealityObject.Id == vr.RealityObject.Id && x.SubProgram && x.Show)
                    .Where(x => x.Id != stage3Id).Select(x=> x.RealityObject.Id).FirstOrDefault();
                if (oOiInSubVersion > 0)
                {
                }
                else
                {
                    gro.IsSubProgram = false;
                }
                
                realityObjectRepo.Update(gro);
                VersionRecordDomain.Update(vr);

                Operator thisOperator = UserManager.GetActiveOperator();
                var log = new VersionActualizeLog();
                log.ActualizeType = VersionActualizeType.InDPKR;
                log.DateAction = DateTime.Now;
                log.Municipality = vr.RealityObject.Municipality;
                log.ProgramVersion = vr.ProgramVersion;
                log.UserName = thisOperator.User.Name;
                log.CountActions = 1;

                var logRecords = new List<ActualizeVersionLogRecord>();
                var logRecord = new ActualizeVersionLogRecord
                {
                    TypeAction = VersionActualizeType.InDPKR,
                    Action = "Возврат в программу",
                    Description = "Ручной возврат в программу",
                    Address = vr.RealityObject.Address,
                    Ceo = vr.CommonEstateObjects,
                    PlanYear = vr.Year
                };

                logRecords.Add(logRecord);
                if (logRecords.Any())
                {
                    log.CountActions = logRecords.Count;
                    log.LogFile = this.LogService.CreateLogFile(logRecords.OrderBy(y => y.Address).OrderBy(y => y.Number), baseParams);
                }
                VersionActualizeLogDomain.Save(log);
                return JsonNetResult.Success;
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// Возвращает список КЭ дома
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetKEforAdd(BaseParams baseParams)//, Int64 houseId)
        {
            try
            {
                var loadParams = baseParams.GetLoadParam();

                var houseId = baseParams.Params.GetAs<long>("houseId");

                //var query = RealityObjectStructuralElementDomain.GetAll()
                //    .Where(x => x.RealityObject.Id == houseId)
                //    .Where(x => x.State.FinalState)
                //    .Select(x => x.StructuralElement)
                //    .Filter(loadParams, Container);

                var query = RealityObjectStructuralElementDomain.GetAll()
                  .Where(x => x.RealityObject.Id == houseId)
                  .Where(x => x.State.StartState)
                  .Select(x => new
                  {
                      x.StructuralElement.Id,
                      x.StructuralElement.Name,
                      OOI = x.StructuralElement.Group != null?  x.StructuralElement.Group.CommonEstateObject.Name: ""
                })
                  .Filter(loadParams, Container);

                return new JsonListResult(
                query
                .Order(loadParams)
                .Paging(loadParams),
                query.Count()
                );
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ActionResult GetRealityObjectforAdd(BaseParams baseParams)
        {
            try
            {
                var loadParams = baseParams.GetLoadParam();

                var versionId = baseParams.Params.GetAs<long>("versionId");
                var version = VersionDomain.Get(versionId);

                var query = RealityObjectDomain.GetAll()
                    .WhereIf(version != null, x => x.Municipality.Id == version.Municipality.Id)
                    .Select(x => new
                    {
                        x.Id,
                        x.Address
                    })
                    .Filter(loadParams, Container);

                return new JsonListResult(
                query
                .Order(loadParams)
                .Paging(loadParams),
                query.Count()
                );
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        /// <summary>
        /// Добавление элемента
        /// </summary>
        public ActionResult Add(BaseParams baseParams)
        {
            try
            {
                var loadParams = baseParams.GetLoadParam();

                var versionId = baseParams.Params.GetAs<long>("versionId");
                var version = VersionDomain.Get(versionId);
                if (version == null)
                    return JsonNetResult.Failure("Версия программы не определена в запросе");

                var houseId = baseParams.Params.GetAs<long>("houseId");
                var keId = baseParams.Params.GetAs<long>("keId");
                var sum = baseParams.Params.GetAs<decimal>("sum");
                var volume = baseParams.Params.GetAs<decimal>("volume");
                var year = baseParams.Params.GetAs<short>("year");

                var rostructel = RealityObjectStructuralElementDomain.GetAll()
                    .Where(x => x.RealityObject.Id == houseId)
                    .Where(x => x.StructuralElement.Id == keId)
                    .Where(x => x.State.FinalState)
                    .FirstOrDefault();

                if (rostructel == null)
                    return JsonNetResult.Failure("Не найден такой актуальный конструктивный элемент у дома");

                AddVersionRecordService.Add(version, rostructel, sum, volume, year);

                return JsonNetResult.Success;
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        private class R
        {
            public int CurrentCost { get; set; }
            public int CurrentLeft { get; set; }
            public int CurrentLimit { get; set; }
        }

        #endregion
    }
}