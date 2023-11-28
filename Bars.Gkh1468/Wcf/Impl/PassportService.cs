namespace Bars.Gkh1468.Wcf
{
    using System.ServiceModel.Activation;

    using Castle.Windsor;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class PassportService : IPassportService
    {
        public IWindsorContainer Container { get; set; }
    }
}