namespace Bars.GkhGji.Rules
{
    using B4;
    using Enums;
    using Entities;
    using Castle.Windsor;
    using System.Linq;
    using Bars.GkhGji.Contracts;

    public class DispPrescrCode1279Rule : IKindCheckRule
    {

        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"DispPrescrCode1279Rule"; } }

        public string Name { 
            get {

                var dispText = Container.Resolve<IDisposalText>();
                return string.Format("{0} на проверку предписания (код вида проверки первого {1} - 1,2,7,9)", 
                        dispText.SubjectiveCase,
                        dispText.GenetiveCase.ToLower()); 
            } 
        }

        public TypeCheck DefaultCode { get { return TypeCheck.NotPlannedExit; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.DocumentGji)
                return false;

            var mainDisposal = Container.Resolve<IDomainService<Disposal>>()
                                .GetAll()
                                .FirstOrDefault(x => x.Inspection.Id == entity.Inspection.Id && x.TypeDisposal == TypeDisposalGji.Base);

            if (mainDisposal == null)
                return false;

            if (mainDisposal.KindCheck != null &&  mainDisposal.KindCheck.Code != TypeCheck.PlannedExit
                && mainDisposal.KindCheck.Code != TypeCheck.NotPlannedExit)
                return false;

            return true;
        }
    }
}