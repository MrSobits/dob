namespace Bars.GkhGji.InspectionAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class BaseStatementAction : IBaseStatementAction
    {
        public IWindsorContainer Container { get; set; }

        public void Create(IEntity entity)
        {
            var baseStatementAppealCits = Container.Resolve<IDomainService<InspectionAppealCits>>().Get(entity.Id.ToLong());
            if (baseStatementAppealCits == null) return;

            var appealCits = baseStatementAppealCits.AppealCits;
            if (appealCits == null) return;

            if (appealCits.State.StartState)
            {
                var stateProvider = Container.Resolve<IStateProvider>();
                var newState = this.Container.Resolve<IDomainService<State>>().GetAll()
                    .FirstOrDefault(x => x.TypeId == appealCits.State.TypeId && x.Code == "В работе");
                if (newState != null)
                {
                    stateProvider.ChangeState(appealCits.Id, newState.TypeId, newState, string.Empty, true);
                }
            }
        }
    }
}
