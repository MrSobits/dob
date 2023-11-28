namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.ViewModel;

    public class ActCheckViewModel : ActCheckViewModel<ActCheck>
    {
        public override IDataResult Get(IDomainService<ActCheck> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(
                new
                {
                    obj.Id,
                    obj.ActCheckGjiRealityObject,
                    obj.ActToPres,
                    obj.Area,
                    obj.DateToProsecutor,
                    obj.DocumentDate,
                    obj.DocumentDateStr,
                    obj.DocumentNum,
                    obj.DocumentNumber,
                    obj.DocumentPlaceFias,
                    obj.LiteralNum,
                    obj.DocumentSubNum,
                    obj.DocumentYear,
                    obj.Flat,
                    obj.ParentDocumentsList,
                    obj.RealityObjectsList,
                    obj.ResolutionProsecutor,
                    obj.Stage,
                    obj.ToProsecutor,
                    obj.State,
                    obj.TypeActCheck,
                    obj.TypeDocumentGji,
                    DocumentTime = obj.DocumentTime.HasValue ? obj.DocumentTime.Value.ToString("HH:mm") : string.Empty,
                    obj.AcquaintState,
                    obj.AcquaintedDate,
                    obj.RefusedToAcquaintPerson,
                    obj.AcquaintedPerson
                }) : new BaseDataResult();
        }
    }
}