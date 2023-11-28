namespace Bars.Gkh.Overhaul.Services.Impl
{
    using System.ServiceModel.Activation;

    using Castle.Windsor;

    using IService = Bars.Gkh.Overhaul.Services.ServiceContracts.IService;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        public IWindsorContainer Container { get; set; }
    }
}