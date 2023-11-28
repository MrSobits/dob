using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Tasks.EGRULSendInformationRequest;
using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
using System;
using System.Collections.Generic;

using System.Linq;

namespace Bars.GkhGji.Regions.Voronezh.ExecutionAction
{
    /// <summary>
    /// Периодическый запрос оплат в СМЭВ
    /// </summary>
    public class SendEGRULRequestsAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        //сколько запрашивать выписок за раз
        static int numberOfRequests = 15;

        public override string Description => "Запрашивает выписки из ЕГРЮЛ по контрагентам" +
            "Задача запускает проверку по 15 контрагентам с наименее актуальными ЕГРЮЛ";

        public override string Name => "Запросить сведения из ЕГРЮЛ";

        public override Func<IDataResult> Action => SendEGRULRequests;

        //public bool IsNeedAction() => true;

        public IDomainService<SMEVEGRUL> EGRULDomain { get; set; }

        //public IDomainService<SMEVEGRIP> EGRIPDomain { get; set; }

        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }

        public IDomainService<ManagingOrganization> ManagingOrganizationDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<Operator> OperatorDomain { get; set; }

        private IDataResult SendEGRULRequests()
        {
            var taskManager = Container.Resolve<ITaskManager>();
           
            try
            {
                Operator thisOperator = OperatorDomain.GetAll().Where(x => x.User == this.User).FirstOrDefault();

                if (1 == 1)

                //if (thisOperator?.Inspector != null)

                {
                    //ОГРН контрагентов с активными и переоформленными лицензиями
                    List<string> contragentsWithActiveLicense = ManOrgLicenseDomain.GetAll()
                        .Where(x => x.State.Code == "002" || x.State.Code == "004")
                        .Select(x => x.Contragent.Ogrn).ToList();

                    ////ОГРН всех контрагентов
                    //List<string> contragentsORGN = ContragentDomain.GetAll().Where(x => x.Ogrn != null && x.Ogrn != "").Select(x => x.Ogrn).ToList();

                    //ОГРН контрагентов - ТСЖ и ЖСК
                    List<string> tsjjskOGRN = ManagingOrganizationDomain.GetAll().Where(x => x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK).Select(x => x.Contragent.Ogrn).ToList();

                    List<string> needOGRN = new List<string>();
                    needOGRN.AddRange(contragentsWithActiveLicense);
                    needOGRN.AddRange(tsjjskOGRN);
                    needOGRN.Distinct();

                    //уже запрошенные (и полученные(!!!)) выписки из ЕГРЮЛ по контрагентам
                    var egruls = EGRULDomain.GetAll()
                        .Where(x => needOGRN.Contains(x.OGRN))
                        .Select(x => new { x.OGRN, x.ObjectCreateDate });

                    //уже запрошенные (и полученные(!!!)) выписки из ЕГРИП по контрагентам
                    //var egrips = EGRIPDomain.GetAll()
                    //    .Where(x => needOGRN.Contains(x.OGRN))
                    //    .Select(x => new { x.OGRN, x.ObjectCreateDate });

                    //словарь ОГРН - дата последней выписки по контрагентам
                    Dictionary<string, DateTime> egrulDates = new Dictionary<string, DateTime>();

                    foreach (var egrul in egruls)
                    {
                        if (!egrulDates.ContainsKey(egrul.OGRN))
                        {
                            egrulDates.Add(egrul.OGRN, egrul.ObjectCreateDate);
                        }
                    }

                    //foreach (var egrip in egrips)
                    //{
                    //    if (!egrulDates.ContainsKey(egrip.OGRN))
                    //    {
                    //        egrulDates.Add(egrip.OGRN, egrip.ObjectCreateDate);
                    //    }
                    //}

                    foreach (string contragent in needOGRN)
                    {
                        if (!egrulDates.ContainsKey(contragent))
                        {
                            egrulDates.Add(contragent, DateTime.MinValue);
                        }
                    }

                    //сортируем по дате выписки и возвращаем numberOfRequests первых ОГРН
                    List<string> ogrns = egrulDates.OrderBy(x => x.Value)
                        .Select(x => x.Key).Take(numberOfRequests).ToList();

                    BaseParams baseParams = new BaseParams();
                    foreach (string ogrn in ogrns)
                    {
                        // Если ЮЛ
                        if (ogrn.Length == 13)
                        {
                            var oldEGRULIds = EGRULDomain.GetAll()
                                .Where(x => x.OGRN == ogrn).Select(x => x.Id);
                            foreach (var oldEGRULId in oldEGRULIds)
                            {
                                EGRULDomain.Delete(oldEGRULId);
                            }
                            SMEVEGRUL smevRequestData = new SMEVEGRUL();
                            smevRequestData.InnOgrn = Enums.InnOgrn.OGRN;
                            smevRequestData.INNReq = ogrn;
                            if (thisOperator?.Inspector == null)
                            {
                                smevRequestData.Inspector = InspectorDomain.GetAll().FirstOrDefault();
                            }
                            else
                            {
                                smevRequestData.Inspector = thisOperator.Inspector;
                            }
                            EGRULDomain.Save(smevRequestData);

                            baseParams.Params.Clear();
                            if (!baseParams.Params.ContainsKey("taskId"))
                                baseParams.Params.Add("taskId", smevRequestData.Id.ToString());

                            taskManager.CreateTasks(new SendInformationRequestTaskProvider(Container), baseParams);
                        }
                        // Если ИП
                        //else if (ogrn.Length == 15)
                        //{
                        //    var oldEGRIPIds = EGRIPDomain.GetAll()
                        //        .Where(x => x.OGRN == ogrn).Select(x => x.Id);
                        //    foreach (var oldEGRIPId in oldEGRIPIds)
                        //    {
                        //        EGRULDomain.Delete(oldEGRIPId);
                        //    }
                        //    SMEVEGRIP smevRequestData = new SMEVEGRIP();
                        //    smevRequestData.InnOgrn = Enums.InnOgrn.OGRN;
                        //    smevRequestData.INNReq = ogrn;
                        //    if (thisOperator?.Inspector == null)
                        //    {
                        //        smevRequestData.Inspector = InspectorDomain.GetAll().FirstOrDefault();
                        //    }
                        //    else
                        //    {
                        //        smevRequestData.Inspector = thisOperator.Inspector;
                        //    }
                        //    EGRIPDomain.Save(smevRequestData);

                        //    baseParams.Params.Clear();
                        //    if (!baseParams.Params.ContainsKey("taskId"))
                        //        baseParams.Params.Add("taskId", smevRequestData.Id.ToString());

                        //    taskManager.CreateTasks(new SendEGRIPRequestTaskProvider(Container), baseParams);
                        //}
                    }

                    //ставим задачу на проверку ответов
                    baseParams.Params.Clear();
                    taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), baseParams);
                    return new BaseDataResult(true, "Задачи успешно поставлены");
                }
                else
                {
                    return new BaseDataResult(false, "Обмен информацией со ГИС ГМП доступен только сотрудникам ГЖИ");
                }
                
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(taskManager);
            }
        }
    }
}
