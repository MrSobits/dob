namespace Bars.GkhDi.Services.Impl
{
    using DataContracts.GetPeriods;
    using Domain;

    public partial class Service
    {
        public virtual GetManOrgRealtyObjectInfoResponse GetManOrgRealtyObjectInfo(string houseId)
        {
            return Container.Resolve<IServiceDi>().GetManOrgRealtyObjectInfo(houseId);

        }
    }
}