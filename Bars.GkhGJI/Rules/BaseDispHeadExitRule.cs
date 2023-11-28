namespace Bars.GkhGji.Rules
{
    using B4;
    using Entities;
    using Enums;
    using Castle.Windsor;

    public class BaseDispHeadExitRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"BaseDispHeadExitRule"; } }

        public string Name { get { return @"Проверка по поручению руководителя. Основание «Поручение руководителей…», Вид проверки «Выездная»"; } }

        public TypeCheck DefaultCode { get { return TypeCheck.NotPlannedExit; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            if (entity.Inspection.TypeBase != TypeBase.DisposalHead)
                return false;

            var dispHead = Container.Resolve<IDomainService<BaseDispHead>>().Load(entity.Inspection.Id);

            if (dispHead.TypeBaseDispHead != TypeBaseDispHead.ExecutivePower)
                return false;

            if (dispHead.TypeForm != TypeFormInspection.Exit)
                return false;

            return true;
        }
    }
}