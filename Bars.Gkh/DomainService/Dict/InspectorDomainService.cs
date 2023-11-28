namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class InspectorViewModel : BaseViewModel<Inspector>
    {
        public override IDataResult List(IDomainService<Inspector> domain, BaseParams baseParams)
        {
            var zonalInspectionInspectorDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var inspectorZjiNamesDict = new Dictionary<long, string>();

            var loadParams = this.GetLoadParam(baseParams);

            var ids = baseParams.Params.GetAs("Id", string.Empty);

            var zonalInspectionIds = baseParams.Params.GetAs("zonalInspectionIds", string.Empty);
            var zonalInspectionList = !string.IsNullOrEmpty(zonalInspectionIds) ? zonalInspectionIds.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var zonalInspectionInspectorsList = zonalInspectionInspectorDomain.GetAll()
                .Where(x => zonalInspectionList.Contains(x.ZonalInspection.Id))
                .Select(x => x.Inspector.Id).ToList();

            var headOnly = baseParams.Params.GetAs("headOnly", false);
            var onlyActive = baseParams.Params.GetAs<bool>("onlyActive",false); 
            var excludeInpectorId = baseParams.Params.GetAs<long>("excludeInpectorId"); 
            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
            //Если в метод переданы ID инспекторов - то должна быть возможность загрузить их в том же порядке, в котором ID переданы в метод.
            //Для этого создаем Dictionry<ID инспектора, порядковый номер>, но поле для Order by должно быть передано с клиента в loadParams
            var order = new Dictionary<long, int>();
            for (var i = 0; i < listIds.Length; i++)
            {
                order.Add(listIds[i], i);
            }

            using (Container.Using(zonalInspectionInspectorDomain))
            {
                inspectorZjiNamesDict = zonalInspectionInspectorDomain.GetAll()
                .WhereIf(listIds.Length > 0, x => listIds.Contains(x.Inspector.Id))
                .WhereIf(zonalInspectionInspectorsList.Count > 0, x => zonalInspectionInspectorsList.Contains(x.Inspector.Id))
                .WhereIf(headOnly, x => x.Inspector.IsHead)
                .WhereIf(excludeInpectorId > 0, x => x.Inspector.Id != excludeInpectorId)             
                .Select(x => new { x.Inspector.Id, x.ZonalInspection.Name })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    x.Select(y => y.Name)
                    .AsEnumerable()
                    .Aggregate(
                        "", (i, j) => i + (!string.IsNullOrEmpty(i) ? ", " + j : j)));
            }

            var tmpData = domain.GetAll()
                .WhereIf(listIds.Length > 0, x => listIds.Contains(x.Id))
                .WhereIf(zonalInspectionInspectorsList.Count > 0, x => zonalInspectionInspectorsList.Contains(x.Id))
                .WhereIf(headOnly, x => x.IsHead)
                .WhereIf(excludeInpectorId > 0, x => x.Id != excludeInpectorId)
                .WhereIf(onlyActive, x => x.Active)
                .Select(x => new
                    {
                        x.Id,
                        x.Fio,
                        x.Position,
                        NotMemberPosition = x.NotMemberPosition != null? x.NotMemberPosition.Name:"",
                        x.ShortFio,
                        x.Email,
                        x.Phone,
                        x.IsHead,
                        x.Description,
                        x.TypeCommissionMember,
                        x.Code,
                        Subdivision = x.Subdivision.Name
                })
                .OrderBy(x=> x.Fio)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Fio,
                    x.Position,
                    x.NotMemberPosition,
                    x.ShortFio,
                    x.Email,
                    x.Phone,
                    x.IsHead,
                    x.Description,
                    x.Code,
                    x.TypeCommissionMember,
                    ZonalInspection = inspectorZjiNamesDict.ContainsKey(x.Id) ? inspectorZjiNamesDict[x.Id] : string.Empty,
                    //Поле используется для сортировки по порядку ID, переданных в метод
                    //Order = order.ContainsKey(x.Id) ? order[x.Id] : 0
                })
                .AsQueryable()
                .Filter(loadParams, Container)
                .ToList();

            var totalCount = tmpData.Count();

            if (loadParams.Order.Length > 0)
            {

                var data = tmpData
                    .AsQueryable()
                    .Order(loadParams)
                    .Paging(loadParams)
                    .ToList();
                return new ListDataResult(data, totalCount);
            }
            else
            {

                var data = tmpData
                    .AsQueryable()
                    .Paging(loadParams)
                    .ToList();
                return new ListDataResult(data, totalCount);
            }

          
        }
    }
}