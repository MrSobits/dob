namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class FillTpByRealityObjectData : BaseExecutionAction
    {
        public override string Description => "Копирование данных из общих сведений дома в техпаспорт";

        public override string Name => "Перенос сведений в ТехПаспорт";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var realityObjectService = this.Container.ResolveDomain<RealityObject>();
            var syncService = this.Container.Resolve<IRealityObjectTpSyncService>();
            var logManager = this.Container.Resolve<ILogManager>();

            using (this.Container.Using(realityObjectService, syncService, logManager))
            {
                var realityObjects = realityObjectService.GetAll()
                    .Where(x => x.ConditionHouse == ConditionHouse.Emergency || x.ConditionHouse == ConditionHouse.Serviceable)
                    .Where(x => x.TypeHouse == TypeHouse.ManyApartments);

                var count = realityObjects.Count();
                var take = 500;
                for (int i = 0; i < count; i += take)
                {
                    var robjects = realityObjects.OrderBy(x => x.Id).Skip(i).Take(take).ToList();

                    this.Container.InTransaction(
                        () =>
                        {
                            robjects.ForEach(x => syncService.Sync(x));
                        });

                    logManager.Debug($"Перенос сведений в ТехПаспорт. Количество: {i}");
                }

                return new BaseDataResult();
            }
        }
    }
}