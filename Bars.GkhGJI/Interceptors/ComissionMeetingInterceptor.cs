namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Entities.Dict;

    public class ComissionMeetingInterceptor : EmptyDomainInterceptor<ComissionMeeting>
    {
        public IDomainService<ComissionMeetingInspector> ComissionMeetingInspectorService { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<ComissionMeeting> service, ComissionMeeting entity)
        {
            if (entity.CommissionDate != null && !string.IsNullOrEmpty(entity.CommissionNumber))
            {
                entity.ComissionName = $"Комиссия №{entity.CommissionNumber} от {entity.CommissionDate.ToString("dd.MM.yyyy")}";
            }
            if (entity.State == null)
            {
                var stateProvider = Container.Resolve<IStateProvider>();
                stateProvider.SetDefaultState(entity);
            }
            return Success();
        }
        public override IDataResult BeforeUpdateAction(IDomainService<ComissionMeeting> service, ComissionMeeting entity)
        {
            if (entity.CommissionDate != null && !string.IsNullOrEmpty(entity.CommissionNumber))
            {
                entity.ComissionName = $"Комиссия №{entity.CommissionNumber} от {entity.CommissionDate.ToString("dd.MM.yyyy")}";
            }
            if (entity.State == null)
            {
                var stateProvider = Container.Resolve<IStateProvider>();
                stateProvider.SetDefaultState(entity);
            }
            return Success();
        }
        public override IDataResult BeforeDeleteAction(IDomainService<ComissionMeeting> service, ComissionMeeting entity)
        {
            try
            {
                var listToDel = ComissionMeetingInspectorService.GetAll().Where(x => x.ComissionMeeting.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var item in listToDel)
                {
                    ComissionMeetingInspectorService.Delete(item);
                }
                return Success();
            }
            catch (ValidationException exc)
            {
                return Failure("Существуют связанные записи в протоколах");
            }
            finally
            {
                Container.Release(ComissionMeetingInspectorService);
            }
        }
    }
}