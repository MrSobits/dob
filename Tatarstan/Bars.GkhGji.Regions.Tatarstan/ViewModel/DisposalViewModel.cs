namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DisposalViewModel : BaseViewModel<Disposal>
    {
        public override IDataResult Get(IDomainService<Disposal> domainService, BaseParams baseParams)
        {
            var serviceDocumentChildren = this.Container.ResolveDomain<DocumentGjiChildren>();

            using(this.Container.Using(serviceDocumentChildren))
            {
                var id = baseParams.Params.GetAsId();
                var hasCildrenActChack = serviceDocumentChildren.GetAll()
                    .Count(x => x.Parent.Id == id && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck) > 0;

                var obj = domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.IssuedDisposal,
                        x.ResponsibleExecution,
                        x.DateStart,
                        x.DateEnd,
                        x.TypeDisposal,
                        x.TypeAgreementProsecutor,
                        x.TypeAgreementResult,
                        x.KindCheck,
                        x.TypeDocumentGji,
                        x.Description,
                        x.ObjectVisitStart,
                        x.ObjectVisitEnd,
                        x.OutInspector,
                        x.DocumentDate,
                        x.DocumentNum,
                        x.DocumentNumber,
                        x.LiteralNum,
                        x.DocumentSubNum,
                        x.DocumentYear,
                        x.State,
                        x.NcNum,
                        x.NcDate,
                        x.NcNumLatter,
                        x.NcDateLatter,
                        x.NcObtained,
                        x.NcSent,

                        TypeBase = x.Inspection != null ? x.Inspection.TypeBase : TypeBase.Default,
                        InspectionId = x.Inspection != null ? x.Inspection.Id : 0,
                        HasChildrenActCheck = hasCildrenActChack,
                        TimeVisitStart = x.TimeVisitStart.HasValue ? x.TimeVisitStart.Value.ToString("HH:mm") : string.Empty,
                        TimeVisitEnd = x.TimeVisitEnd.HasValue ? x.TimeVisitEnd.Value.ToString("HH:mm") : string.Empty,
                    })
                    .FirstOrDefault();

                return new BaseDataResult(obj);

            }
        }
    }
}