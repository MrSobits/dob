namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Bars.GkhGji.Entities;
    using B4;
    using B4.DataAccess;
    using Entities;

    using Castle.Windsor;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;

    public class ResolutionArticleLawService : IResolutionArticleLawService
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        public IWindsorContainer Container { get; set; }      

        public IDataResult GetListResolution(BaseParams baseParams)
        {
            var inspectorId = UserManager.GetActiveOperator().Inspector.Id;
            var zonalId = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                .Where(x => x.Inspector.Id == inspectorId)
                .Select(x => x.ZonalInspection.Id)
                .FirstOrDefault();

            var loadParams = baseParams.GetLoadParam();
            var gisgmpdocsServ = this.Container.Resolve<IDomainService<GisGmp>>();
            var docidList = gisgmpdocsServ.GetAll()
                .Where(x => x.Protocol != null)
                .Select(x => x.Protocol.Id).Distinct().ToList();
            var resolutionList = ResolutionDomain.GetAll()
                .Where(x => x.ObjectCreateDate > DateTime.Now.AddYears(-2))
                .Where(x => x.PenaltyAmount.HasValue && x.TypeInitiativeOrg == GkhGji.Enums.TypeInitiativeOrgGji.HousingInspection)
                .Select(x=> x.Id).Distinct().ToList();
            var avalProt = DocumentGjiChildrenDomain.GetAll()
                .Where(x => resolutionList.Contains(x.Children.Id))
                .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Protocol
                || x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Protocol197
                || x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ProtocolMhc
                || x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ResolutionProsecutor)
                .Where(x => x.Parent.ComissionMeeting.ZonalInspection.Id == zonalId)
                .Select(x => x.Parent.Id).Distinct().ToList();


            var data = DocumentGjiDomain.GetAll()
                .Where(x => !x.State.StartState)
                .Where(x=> !docidList.Contains(x.Id))
                .Where(x=> avalProt.Contains(x.Id))             
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.TypeDocumentGji
                })
                //.OrderBy(x=>x.Id).Distinct()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }


        public IDataResult GetListDisposal(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var giserpdocsServ = this.Container.Resolve<IDomainService<GISERP>>();
            var docidList = giserpdocsServ.GetAll()
                .Where(x => x.Disposal != null)
                .Select(x => x.Disposal.Id).Distinct().ToList();

            var data = DocumentGjiDomain.GetAll()
                .Where(x=> x.Inspection.Contragent != null)
                .Where(x=> x.ObjectCreateDate>= DateTime.Now.AddMonths(-3))
                .Where(x => !x.State.StartState)
                .Where(x => !docidList.Contains(x.Id))
                .Where(x => x.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Disposal)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.TypeDocumentGji,
                    State = x.State.Name,
                })
                //              .OrderBy(x=>x.Id).Distinct()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}