﻿namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;
    using System.Linq;

    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Gkh.Utils;

    public class RoomAreaControlReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }
        public RoomAreaControlReport()
            : base(new ReportTemplateBinary(Properties.Resources.RoomAreaControl))
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

        /// <summary>
        /// Собирать по
        /// </summary>
        private DataSource _collectBy;

        public override void SetUserParams(BaseParams baseParams)
        {
            this._municipalityIds = baseParams.Params.GetAs<string>("municipalityIds").ToLongArray();
            this._conditionHouseIds = baseParams.Params.GetAs<string>("conditonHouseIds").ToLongArray();
            this._typeOwnershipIds = baseParams.Params.GetAs<string>("typeOwnershipIds").ToLongArray();
            this._collectBy = baseParams.Params.GetAs<DataSource>("collectBy");
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

            var realityObjects = roService.GetAll();
            IQueryable<long> correctionRealityObjectsIds = null;

            if (this._collectBy == DataSource.Dpkr)
            {

                var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
                var groupByRoPeriod = config.GroupByRoPeriod;
                var periodEndYear = config.ProgrammPeriodEnd;
                var domainService = Container.Resolve<IDomainService<DpkrCorrectionStage2>>();

                if (groupByRoPeriod == 0)
                {
                    correctionRealityObjectsIds = domainService.GetAll()
                        .WhereIf(_municipalityIds.Any(),
                            x =>
                                x.Stage2.Stage3Version.ProgramVersion.IsMain &&
                                _municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                        .Where(x => x.Stage2.Stage3Version.IndexNumber > 0 || x.Stage2.Stage3Version.Year < periodEndYear)
                        .Select(x => x.RealityObject.Id).Distinct();
                }
                else
                {
                    var versionRecordDomain = Container.Resolve<IDomainService<VersionRecord>>();

                    correctionRealityObjectsIds = versionRecordDomain.GetAll()
                        .WhereIf(_municipalityIds.Any(), x => x.ProgramVersion.IsMain && _municipalityIds.Contains(x.ProgramVersion.Municipality.Id))
                        .Where(x => domainService.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                        .Select(x => x.RealityObject.Id).Distinct();
                }

                Container.Release(domainService);
            }

            var query = realityObjects
                    .WhereIf(_municipalityIds.Any(), x => _municipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(_conditionHouseIds.Any(), x => _conditionHouseIds.Contains((int) x.ConditionHouse))
                    .WhereIf(_typeOwnershipIds.Any(), x => _typeOwnershipIds.Contains(x.TypeOwnership.Id))
                    .WhereIf(correctionRealityObjectsIds != null && correctionRealityObjectsIds.Any(), x => correctionRealityObjectsIds.Contains(x.Id));

            Container.Release(roService);
            return query;
        }

        /// <summary>
        /// Собирать по
        /// </summary>
        /// <!-- При изменении значений проверить, что значения здесь и на панели отчета идентичны -->
        protected enum DataSource
        {
            /// <summary>
            /// Реестр жилых домов
            /// </summary>
            Reestr = 0,

            /// <summary>
            /// Долгосрочная программа
            /// </summary>
            Dpkr = 1
        }
    }
}