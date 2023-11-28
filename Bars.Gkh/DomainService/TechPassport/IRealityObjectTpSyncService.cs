namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    public interface IRealityObjectTpSyncService
    {
        IDataResult Validate(TehPassportValue value);

        IDataResult Validate(RealityObject robject);

        IDataResult Sync(TehPassportValue value);

        IDataResult Sync(RealityObject robject);
    }
}