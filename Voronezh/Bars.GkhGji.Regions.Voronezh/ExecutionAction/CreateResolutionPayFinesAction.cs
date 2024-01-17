using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.Entities;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Tasks;
using System;
using System.Collections.Generic;

using System.Linq;

namespace Bars.GkhGji.Regions.Voronezh.ExecutionAction
{
    /// <summary>
    /// Периодическый запрос оплат в СМЭВ
    /// </summary>
    public class CreateResolutionPayFinesAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        //сколько запрашивать выписок за раз
        static int numberOfRequests = 15;

        public override string Description => "Добавляет оплаты штрафов постановлениям на основании платежей, полученных из ГИС ГМП; добавляет оплаты пошлин за выдачу и обновление лицензий";

        public override string Name => "Заполнить оплаты штрафов и пошлин";

        public override Func<IDataResult> Action => CreateResolutionPayFines;

        //public bool IsNeedAction() => true;

        private IDataResult CreateResolutionPayFines()
        {
            var ResolutionDomain = Container.Resolve<IDomainService<Resolution>>();
            var ResolutionPayFineDomain = Container.Resolve<IDomainService<ResolutionPayFine>>();
            var CourtPracticeDomain = Container.Resolve<IDomainService<CourtPractice>>();
            var Protocol197Domain = Container.Resolve<IDomainService<Protocol197>>();
            var DocumentGjiChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            var resNotInLaw = ResolutionDomain.GetAll().Where(x => !x.InLawDate.HasValue && x.DocumentDate > DateTime.Now.AddYears(-1)).ToList();
            foreach (var res in resNotInLaw)
            {
                var prot197_id = DocumentGjiChildrenDomain.GetAll()
                    .Where(x => x.Children.Id == res.Id)
                    .Select(x => x.Parent.Id)
                    .FirstOrDefault();

                var prot197 = Protocol197Domain.GetAll()
                    .Where(x => x.Id == prot197_id)
                    .Select(x => x)
                    .FirstOrDefault();

                //проверяем на оспаривание
                DateTime? startDate = null;
                DateTime? dueDate = null;

                //var cp = CourtPracticeDomain.GetAll().FirstOrDefault(x => x.DocumentGji == res);
                //if (cp != null && (cp.CourtMeetingResult == Enums.CourtMeetingResult.Denied || cp.CourtMeetingResult == Enums.CourtMeetingResult.LeftWithoutConsideration))
                //{
                //    res.InLawDate = cp.InLawDate;
                //}

                if (res.Paided != Gkh.Enums.YesNoNotSet.NotNeed)
                {
                    if (res.DeliveryDate.HasValue)
                    {
                        startDate = res.DeliveryDate.Value;
                        var inlaw = res.DeliveryDate.Value.AddDays(10);
                        if (inlaw.Date <= DateTime.Now.Date)
                        {
                            res.InLawDate = inlaw;
                        }
                    }
                    else if (res.PostDeliveryDate.HasValue)
                    {
                        startDate = res.PostDeliveryDate.Value;
                        var inlaw = res.PostDeliveryDate.Value.AddDays(10);
                        if (inlaw.Date <= DateTime.Now.Date)
                        {
                            res.InLawDate = inlaw;
                        }
                    }
                }

                if (startDate.HasValue && res.InLawDate.HasValue)
                {
                    var prodCalendarContainer = this.Container.Resolve<IDomainService<ProdCalendar>>().GetAll()
                    .Where(x => x.ProdDate >= startDate.Value && x.ProdDate <= startDate.Value.AddDays(15)).Where(x => !x.WorkDay).Select(x => x.ProdDate).ToList();

                    var prodCalendarWorkContainer = this.Container.Resolve<IDomainService<ProdCalendar>>().GetAll()
                    .Where(x => x.ProdDate >= startDate.Value && x.ProdDate <= startDate.Value.AddDays(15)).Where(x => x.WorkDay).Select(x => x.ProdDate).ToList();

                    DateTime newControlDate = res.InLawDate.Value;

                    //int sartudaysCount = CountDays(DayOfWeek.Saturday, appDate.Value, appDate.Value.AddDays(28));
                    //int sundaysCount = CountDays(DayOfWeek.Sunday, appDate.Value, appDate.Value.AddDays(28));
                    //newControlDate = appDate.Value.AddDays(28 + sartudaysCount + sundaysCount);

                    if (prodCalendarContainer.Contains(newControlDate))
                    {
                        for (int i = 0; i <= prodCalendarContainer.Count; i++)
                        {
                            if (prodCalendarContainer.Contains(newControlDate))
                            {
                                newControlDate = newControlDate.AddDays(1);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (newControlDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        if (prodCalendarWorkContainer.Contains(newControlDate))
                        {
                            newControlDate = newControlDate.AddDays(0);
                        }
                        else
                            newControlDate = newControlDate.AddDays(2);
                    }
                    else if (newControlDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (prodCalendarWorkContainer.Contains(newControlDate))
                        {
                            newControlDate = newControlDate.AddDays(0);
                        }
                        else
                            newControlDate = newControlDate.AddDays(1);
                    }

                    if (prodCalendarContainer.Contains(newControlDate))
                    {
                        for (int i = 0; i <= prodCalendarContainer.Count; i++)
                        {
                            if (prodCalendarContainer.Contains(newControlDate))
                            {
                                newControlDate = newControlDate.AddDays(1);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    res.InLawDate = newControlDate.AddDays(1);
                    dueDate = res.InLawDate;
                }

                if (prot197 != null)
                {
                    if (prot197.DateOfViolation.HasValue && !res.DueDate.HasValue && res.PenaltyAmount > 0)
                    {
                        res.DueDate = prot197.DateOfViolation.Value.AddDays(60);
                    }
                }

                if (res.InLawDate.HasValue && res.Paided == Gkh.Enums.YesNoNotSet.NotSet)
                {
                    if (res.PenaltyAmount > 0 && DateTime.Now > res.InLawDate.Value.AddDays(60))
                    {
                        var payfineSum = ResolutionPayFineDomain.GetAll().Where(x => x.Resolution.Id == res.Id && x.Amount.HasValue).SafeSum(x => x.Amount.Value);
                        if (payfineSum < res.PenaltyAmount)
                        {
                            res.Paided = Gkh.Enums.YesNoNotSet.No;
                            res.Protocol205Date = res.InLawDate.Value.AddDays(61);
                        }
                        else if (payfineSum >= res.PenaltyAmount)
                        {
                            var payfinedate = ResolutionPayFineDomain.GetAll().Where(x => x.Resolution.Id == res.Id && x.Amount.HasValue).Max(x => x.DocumentDate);
                            if (payfinedate.HasValue)
                            {
                                res.Paided = Gkh.Enums.YesNoNotSet.Yes;
                                res.PaymentDate = payfinedate;
                            }
                        }

                    }
                }

                ResolutionDomain.Update(res);
            }

            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new CreateResolutionPayFinesTaskProvider(Container), new BaseParams());
                return new BaseDataResult(true, "Задача успешно поставлена");
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
