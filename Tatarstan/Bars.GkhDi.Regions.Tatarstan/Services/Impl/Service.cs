namespace Bars.GkhDi.Regions.Tatarstan.Services
{
    using System.ServiceModel.Activation;

    using Castle.Windsor;


    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class Service : IService
    {
        public IWindsorContainer Container { get; set; }
    }
}