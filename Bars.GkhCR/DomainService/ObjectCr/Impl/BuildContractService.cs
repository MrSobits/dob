namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Enums;

    using Entities;
    using Castle.Windsor;
    using Gkh.DomainService.GkhParam;

    public class BuildContractService : IBuildContractService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTypeWorks(BaseParams baseParams)
        {
            var buildContractTypeWorkDomain = Container.ResolveDomain<BuildContractTypeWork>();
            var typeWorkDomain = Container.ResolveDomain<TypeWorkCr>();
            var buildContractDomain = Container.ResolveDomain<BuildContract>();
            try
            {
                var buildContractId = baseParams.Params.GetAs<long>("buildContractId");

                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',').Select(x => x.ToLong()).ToList();

                var exsistingTypeWorks = buildContractTypeWorkDomain.GetAll().Where(x => x.BuildContract.Id == buildContractId).Select(x => x.TypeWork.Id).ToList();

                foreach (var id in objectIds.Where(x => !exsistingTypeWorks.Contains(x)))
                {
                    var newBuildContractTypeWork = new BuildContractTypeWork { BuildContract = buildContractDomain.Load(buildContractId), TypeWork = typeWorkDomain.Load(id), };

                    buildContractTypeWorkDomain.Save(newBuildContractTypeWork);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(buildContractTypeWorkDomain);
                Container.Release(typeWorkDomain);
                Container.Release(buildContractDomain);
            }
        }

        public IDataResult ListAvailableBuilders(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var builderDomain = Container.ResolveDomain<Builder>();
            var lotBidDomain = Container.ResolveDomain<CompetitionLotBid>();
            var qualificationDomain = Container.ResolveDomain<Qualification>();
            var lotTypeWorkDomain = Container.ResolveDomain<CompetitionLotTypeWork>();
            try
            {
                var objectCrId = baseParams.Params.GetAs<long>("objectCrId");
                if (objectCrId == 0)
                {
                    return new BaseDataResult(false, "Не указан объект КР");
                }

                var builderSelection = Container.GetGkhConfig<GkhCrConfig>().General.BuilderSelection;
                var query = builderDomain.GetAll().Select(x => new { ContragentName = x.Contragent.Name, x.Id });
                IQueryable<long> builderSelector = null;
                if (builderSelection == TypeBuilderSelection.Competition)
                {
                    var queryTypeWorks = lotTypeWorkDomain.GetAll().Where(x => x.TypeWork.ObjectCr.Id == objectCrId);
                    builderSelector = lotBidDomain.GetAll().Where(x => queryTypeWorks.Any(y => y.Lot.Id == x.Lot.Id)).Select(x => x.Builder.Id);
                }
                else
                {
                    builderSelector = qualificationDomain.GetAll().Where(x => x.ObjectCr.Id == objectCrId).Select(x => x.Builder.Id);
                }

                if (builderSelector.Any())
                {
                    query = query.Where(x => builderSelector.Contains(x.Id));
                }

                query = query.Filter(loadParams, Container);
                return new ListDataResult(query.Order(loadParams).Paging(loadParams), query.Count());
            }
            finally
            {
                Container.Release(builderDomain);
                Container.Release(lotBidDomain);
                Container.Release(qualificationDomain);
            }
        }

        public virtual IDataResult GetForMap(BaseParams baseParams)
        {
            var bcDomain = this.Container.ResolveDomain<BuildControlTypeWorkSmr>();

            try
            {
                var id = baseParams.Params.GetAs<long>("id");
                var dt = bcDomain.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new { x = x.Latitude, y = x.Longitude }).FirstOrDefault();

                return new BaseDataResult(dt);
            }
            finally
            {
                this.Container.Release(bcDomain);
            }
        }
    }
}