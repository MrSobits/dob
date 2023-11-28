namespace Bars.GkhRf.Services.Impl
{
    using System.ServiceModel.Activation;

    using Bars.GkhRf.Services.ServiceContracts;

    using Castle.Windsor;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        public IWindsorContainer Container { get; set; }
    }
}