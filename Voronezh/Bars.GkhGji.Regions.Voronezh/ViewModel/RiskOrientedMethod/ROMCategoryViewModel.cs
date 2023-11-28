namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ROMCategoryViewModel : BaseViewModel<ROMCategory>
    {
        public override IDataResult List(IDomainService<ROMCategory> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var yearEnum = baseParams.Params.GetAs<string>("yearEnum");
            Dictionary<string, Enums.YearEnums> enumsDict = new Dictionary<string, Enums.YearEnums>();
            enumsDict.Add("2015", Enums.YearEnums.y2015);
            enumsDict.Add("2016", Enums.YearEnums.y2016);
            enumsDict.Add("2017", Enums.YearEnums.y2017);
            enumsDict.Add("2018", Enums.YearEnums.y2018);
            enumsDict.Add("2019", Enums.YearEnums.y2019);
            enumsDict.Add("2020", Enums.YearEnums.y2020);


            var data = domainService.GetAll()
                .Where(x=> x.YearEnums == enumsDict[yearEnum])
                .Select(x => new
                    {
                        x.Id,
                        Contragent = x.Contragent.Name,
                        ContragentINN = x.Contragent.Inn,
                        x.KindKND,
                        x.CalcDate,
                        x.Result,
                        Inspector = x.Inspector.Fio,
                        x.YearEnums,
                        x.RiskCategory,
                        x.State
                    })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}