namespace Bars.GkhCr.Services.Impl
{
    using System.ServiceModel.Activation;

    using Bars.GkhCr.Services.ServiceContracts;

    using Castle.Windsor;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        public IWindsorContainer Container { get; set; }
    }
}