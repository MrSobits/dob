namespace Bars.Gkh.Report
{
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;

    using Castle.Windsor;

    public class RoomAreaControlReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }
        public RoomAreaControlReport()
            : base(new ReportTemplateBinary(Resources.RoomAreaControl))
        {
        }

        /// <summary>
        /// Идентификаторы муниципальных образований
        /// </summary>
        private long[] _municipalityIds;

        /// <summary>
        /// Идентификаторы состояний дома
        /// </summary>
        private long[] _conditionHouseIds;

        /// <summary>
        /// Идентификаторы записей справочника Форма собственности
        /// </summary>
        private long[] _typeOwnershipIds;

        public override void SetUserParams(BaseParams baseParams)
        {
            this._municipalityIds = baseParams.Params.GetAs<string>("municipalityIds").ToLongArray();
            this._conditionHouseIds = baseParams.Params.GetAs<string>("conditonHouseIds").ToLongArray();
            this._typeOwnershipIds = baseParams.Params.GetAs<string>("typeOwnershipIds").ToLongArray();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var roomService = Container.Resolve<IDomainService<Room>>();

            var rooms = roomService.GetAll()
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Area));

            var housesQuery = this.GetHousesData();

            var houses = housesQuery
                .Select(x => new
                {
                    x.Id,
                    MunicipalityName = x.Municipality != null ? x.Municipality.Name : string.Empty,
                    x.Address,
                    x.AreaLivingNotLivingMkd
                }).ToList();

            var i = 1;
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            
            foreach (var house in houses)
            {
                var areaMkd = house.AreaLivingNotLivingMkd.HasValue ? house.AreaLivingNotLivingMkd.Value : 0;
                var roomsArea = rooms.ContainsKey(house.Id) ? rooms[house.Id] : 0;
                section.ДобавитьСтроку();
                section["Num"] = i;
                section["Municipality"] = house.MunicipalityName;
                section["Address"] = house.Address;
                section["Area"] = areaMkd;
                section["RoomArea"] = roomsArea;
                section["Error"] = roomsArea != areaMkd ? "Нет" : "Да";
                i++;
            }

            Container.Release(roomService);
        }

        public override string Name
        {
            get
            {
                return "Контроль площадей помещений";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Контроль площадей помещений";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональный фонд";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.RoomAreaControl";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return string.Empty;
            }
        }

        private IQueryable<RealityObject> GetHousesData()
        {
            var roService = Container.Resolve<IDomainService<RealityObject>>();

            var query = roService.GetAll()
                    .WhereIf(_municipalityIds.Any(), x => _municipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(_conditionHouseIds.Any(), x => _conditionHouseIds.Contains((int) x.ConditionHouse))
                    .WhereIf(_typeOwnershipIds.Any(), x => _typeOwnershipIds.Contains(x.TypeOwnership.Id));

            Container.Release(roService);
            return query;
        }
    }
}