namespace Bars.Gkh.ViewModel.Dict
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

	public class MunicipalityFiasOktmoViewModel : BaseViewModel<MunicipalityFiasOktmo>
    {
		public IDomainService<Fias> FiasDomainService { get; set; }
		public IRepository<Municipality> MunicipalityRepository { get; set; }
		public IFiasRepository FiasRepository { get; set; }

		public override IDataResult List(IDomainService<MunicipalityFiasOktmo> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
			var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

			var data = domainService.GetAll()
				.WhereIf(municipalityId > 0, x => x.Municipality.Id == municipalityId)
				.Select(
					x => new
					{
						x.Id,
						x.FiasGuid,
						x.Oktmo,
						Municipality = x.Municipality.Name
					})
				.Filter(loadParams, Container)
				.ToList();

			var fiasGuids = data.Select(x => x.FiasGuid).ToArray();
			var fiasOffNameDict = FiasDomainService.GetAll()
				.Where(x => fiasGuids.Contains(x.AOGuid))
				.Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
				.Select(
					x => new
					{
						x.AOGuid,
						x.OffName
					})
				.AsEnumerable()
				.GroupBy(x => x.AOGuid)
				.ToDictionary(x => x.Key, x => x.First().OffName);

			var result = data.Select(
				x => new
				{
					x.Id,
					x.FiasGuid,
					x.Oktmo,
					OffName = fiasOffNameDict.ContainsKey(x.FiasGuid) ? fiasOffNameDict.Get(x.FiasGuid) : "",
					x.Municipality
				})
				.AsQueryable();
				
            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
        }

        public override IDataResult Get(IDomainService<MunicipalityFiasOktmo> domainService, BaseParams baseParams)
        {
            var entity = domainService.Get(baseParams.Params.GetAsId());
			if (entity != null)
			{
				var fias = FiasDomainService.GetAll()
					.FirstOrDefault(x => x.AOGuid == entity.FiasGuid && x.ActStatus == FiasActualStatusEnum.Actual);

                return new BaseDataResult(new
                {
                    entity.Id,
					FiasGuid = fias,
                    entity.Oktmo,
					entity.Municipality
                });
            }

            return base.Get(domainService, baseParams);
        }
    }
}
