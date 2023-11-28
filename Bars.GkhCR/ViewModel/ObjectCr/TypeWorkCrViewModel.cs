namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Enums;

    using Entities;
    using Enums;
    using Gkh.Domain;

    /// <summary>
    /// The ViewModel
    /// </summary>
    public class TypeWorkCrViewModel : BaseViewModel<TypeWorkCr>
    {
        public IOverhaulViewModels OverhaulViewModels { get; set; }

        public override IDataResult List(IDomainService<TypeWorkCr> domainService, BaseParams baseParams)
        {
            var objectCrDomain = Container.ResolveDomain<Entities.ObjectCr>();
            var controlDate = Container.ResolveDomain<ControlDate>();
            
            try
            {
                if (OverhaulViewModels != null)
                {
                    return OverhaulViewModels.TypeWorkCrList(domainService, baseParams);
                }

                var loadParams = GetLoadParam(baseParams);
                var objectCrId = baseParams.Params.GetAsId("objectCrId");
                var twId = baseParams.Params.GetAsId("twId");
                var onlyServices = baseParams.Params.GetAs<bool>("onlyServices");

                if (objectCrId == 0)
                {
                    objectCrId = loadParams.Filter.GetAs("objectCrId", 0l);
                }

                if (twId == 0)
                {
                    twId = loadParams.Filter.GetAs("twId", 0l);
                }


                var programCrId = objectCrDomain.GetAll()
                    .Where(x => x.Id == objectCrId)
                    .Select(x => (long?) x.ProgramCr.Id)
                    .FirstOrDefault() ?? 0L;

                var federalLaw185 = baseParams.Params.GetAs("federalLaw185", false);

                var dictControlDate = controlDate.GetAll()
                    .Where(y => y.ProgramCr.Id == programCrId)
                    .Select(x => new {x.Work.Id, x.Date})
                    .ToDictionary(x => x.Id, x => x.Date);

                var data = domainService.GetAll()
                    .Where(x => x.ObjectCr.Id == objectCrId)
                    .WhereIf(twId > 0, x => x.Id == twId)
                    .WhereIf(federalLaw185, x => x.FinanceSource.TypeFinance == TypeFinance.FederalLaw)
                    .WhereIf(onlyServices, x => x.Work.TypeWork == TypeWork.Service)
                    .Select(x => new
                    {
                        x.Id,
                        WorkName = x.Work.Name,
                        x.Work.TypeWork,
                        UnitMeasureName = x.Work.UnitMeasure.Name,
                        FinanceSourceName = x.FinanceSource.Name,
                        x.HasPsd,
                        x.Volume,
                        x.SumMaterialsRequirement,
                        x.Sum,
                        x.Description,
                        x.DateStartWork,
                        x.DateEndWork,
                        x.IsEmergrncy,
                        x.VolumeOfCompletion,
                        x.ManufacturerName,
                        x.PercentOfCompletion,
                        x.CostSum,
                        x.CountWorker,
                        StageWorkCrName = x.StageWorkCr.Name,
                        x.AdditionalDate,
                        x.IsActive,
                        x.IsDpkrCreated,
                        x.YearRepair,
                        ControlDate = dictControlDate.ContainsKey(x.Work.Id) ? dictControlDate[x.Work.Id] : null,
#warning Разрулить на клиенте.
                        WorkFinSourceName = x.Work.Name + " (" + x.FinanceSource.Name + ")" //поле нужно для акта выполненных работ
                    })
                    .Filter(loadParams, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                Container.Release(objectCrDomain);
                Container.Release(controlDate);
            }
        }

        public override IDataResult Get(IDomainService<TypeWorkCr> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId());

            return new BaseDataResult(new
            {
                obj.Id,
                obj.Work,
                RealityObject = new
                {
                    obj.ObjectCr.RealityObject.Id,
                    obj.ObjectCr.RealityObject.Address
                },
                ProgramCr = new
                {
                    obj.ObjectCr.ProgramCr.Id,
                    obj.ObjectCr.ProgramCr.Name
                },
                obj.FinanceSource,
                obj.YearRepair,
                obj.AdditionalDate,
                obj.CostSum,
                obj.CountWorker,
                obj.DateEndWork,
                obj.DateStartWork,
                obj.Description,
                obj.HasPsd,
                obj.IsActive,
                obj.IsEmergrncy,
                obj.IsDpkrCreated,
                obj.ManufacturerName,
                obj.PercentOfCompletion,
                StageWorkCr = obj.StageWorkCr.Return(x => x.Id),
                obj.Sum,
                obj.SumMaterialsRequirement,
                obj.Volume,
                obj.VolumeOfCompletion
            });
        }
    }
}