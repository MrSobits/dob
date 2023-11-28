namespace Bars.GkhGji.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ComissionMeetingOperationService : IComissionMeetingOperationService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddMembers(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var commeetingId = baseParams.Params.GetAs<long>("commeetingId");
                    var inspectorIds = baseParams.Params.GetAs<List<long>>("inspectorIds");

                    var serviceComissionMeetingInspector = this.Container.Resolve<IDomainService<ComissionMeetingInspector>>();
                    var serviceInspector = this.Container.Resolve<IDomainService<Inspector>>();
                    var serviceComissionMeeting = this.Container.Resolve<IDomainService<ComissionMeeting>>();

                    // в этом списке будут id статей, которые уже связаны с этим предписанием
                    // (чтобы недобавлять несколько одинаковых документов в один и тотже протокол)
                    var listIds =
                        serviceComissionMeetingInspector.GetAll()
                            .Where(x => x.ComissionMeeting.Id == commeetingId)
                            .Select(x => x.Inspector.Id)
                            .Distinct()
                            .ToList();

                    var commeeting = serviceComissionMeeting.Load(commeetingId);

                    foreach (var id in inspectorIds)
                    {
                        // Если среди существующих статей уже есть такая статья, то пролетаем мимо
                        if (listIds.Contains(id))
                            continue;

                        // Если такой статьи еще нет, то добалвяем
                        var newObj = new ComissionMeetingInspector
                        {
                                ComissionMeeting = commeeting,
                                Inspector = serviceInspector.Load(id)
                            };

                        serviceComissionMeetingInspector.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
            }
        }
    }
}