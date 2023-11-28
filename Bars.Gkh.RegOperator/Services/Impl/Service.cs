namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System.ServiceModel.Activation;

    using Bars.Gkh.RegOperator.Services.ServiceContracts;

    using Castle.Windsor;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        public IWindsorContainer Container { get; set; }
    }
}