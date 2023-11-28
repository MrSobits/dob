namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using System.Web.Mvc;

    using B4;

    public interface IDisposalViolationsService
    {
        ActionResult GetListNotRemovedViolations(BaseParams baseParams);
    }
}
