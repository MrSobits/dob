namespace Bars.Gkh.Overhaul.Reports
{
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Properties;

    using Castle.Windsor;

	/// <summary>
	/// Отчет для Экспорт технического паспорта
	/// </summary>
    public class RealtyObjectDataReport : BasePrintForm
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

        private long roId;

		/// <summary>
		/// Конструктор
		/// </summary>
		public RealtyObjectDataReport()
            : base(new ReportTemplateBinary(Resources.RealtyObjectDataReport))
        {
        }

		/// <summary>
		/// Наименование
		/// </summary>
		public override string Name
        {
            get { return "RealtyObjectPassport"; }
        }

		/// <summary>
		/// Установить пользовательские параметры
		/// </summary>
		public override void SetUserParams(BaseParams baseParams)
        {
            this.roId = baseParams.Params.GetAs<long>("house");
        }

		/// <summary>
		/// Генератор отчета
		/// </summary>
		public override string ReportGenerator { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public override string Desciption
        {
            get { return string.Empty; }
        }

		/// <summary>
		/// Наименование группы
		/// </summary>
		public override string GroupName
        {
            get { return string.Empty; }
        }

		/// <summary>
		/// Клиентский контроллер
		/// </summary>
		public override string ParamsController
        {
            get { return string.Empty; }
        }

		/// <summary>
		/// Ограничение прав доступа
		/// </summary>
		public override string RequiredPermission
        {
            get { return string.Empty; }
        }

		/// <summary>
		/// Подготовить отчет
		/// </summary>
		public override void PrepareReport(ReportParams reportParams)
        {
            var realtyObjInfo = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => x.Id == this.roId)
                .Select(x => new
                {
                    Address = x.FiasAddress.AddressName,
                    x.TypeHouse,
                    x.ConditionHouse,
                    x.BuildYear,
                    x.DateCommissioning,
                    x.DateTechInspection,
                    x.PrivatizationDateFirstApartment,
                    x.FederalNum,
                    x.GkhCode,
                    TypeOwnership = x.TypeOwnership.Name,
                    x.PhysicalWear,
                    x.CadastreNumber,
                    x.TotalBuildingVolume,
                    x.AreaMkd,
                    x.AreaOwned,
                    x.AreaMunicipalOwned,
                    x.AreaGovernmentOwned,
                    x.AreaLivingNotLivingMkd,
                    x.AreaLiving,
                    x.AreaLivingOwned,
                    x.AreaNotLivingFunctional,
                    x.NecessaryConductCr,
                    x.NumberApartments,
                    x.NumberLiving,
                    x.NumberLifts,
                    RoofingMaterial = x.RoofingMaterial.Name,
                    WallMaterial = x.WallMaterial.Name,
                    x.TypeRoof,
                    x.HeatingSystem
                })
                .FirstOrDefault();

            if (realtyObjInfo == null)
            {
                return;
            }

            var realityObjectStructuralElements =
                this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>()
                    .GetAll()
                    .Where(x => x.RealityObject.Id == this.roId)
                    .Select(x => new
                        {
                            GroupName = x.StructuralElement.Group.Name,
                            x.StructuralElement.Name,
                            x.LastOverhaulYear,
                            x.Wearout,
                            x.Volume,
                            UnitMeasure = x.StructuralElement.UnitMeasure.Name,
							State = x.State.Name
                        })
                    .ToList();

            reportParams.SimpleReportParams["Address"] = realtyObjInfo.Address;
            reportParams.SimpleReportParams["TypeHouse"] = realtyObjInfo.TypeHouse.GetEnumMeta().Display;
            reportParams.SimpleReportParams["ConditionHouse"] = realtyObjInfo.ConditionHouse.GetEnumMeta().Display;
            reportParams.SimpleReportParams["BuildYear"] = realtyObjInfo.BuildYear;
            reportParams.SimpleReportParams["DateCommissioning"] = realtyObjInfo.DateCommissioning.HasValue ? realtyObjInfo.DateCommissioning.Value.ToString("dd.MM.yyyy") : string.Empty;
            reportParams.SimpleReportParams["DateTechInspection"] = realtyObjInfo.DateTechInspection.HasValue ? realtyObjInfo.DateTechInspection.Value.ToString("dd.MM.yyyy") : string.Empty;
            reportParams.SimpleReportParams["PrivatizationDateFirstApartment"] = realtyObjInfo.PrivatizationDateFirstApartment.HasValue ? realtyObjInfo.PrivatizationDateFirstApartment.Value.ToString("dd.MM.yyyy") : string.Empty;
            reportParams.SimpleReportParams["FederalNum"] = realtyObjInfo.FederalNum;
            reportParams.SimpleReportParams["Code"] = realtyObjInfo.GkhCode;
            reportParams.SimpleReportParams["TypeOwnership"] = realtyObjInfo.TypeOwnership;
            reportParams.SimpleReportParams["PhysicalWear"] = realtyObjInfo.PhysicalWear;
            reportParams.SimpleReportParams["CadastreNumber"] = realtyObjInfo.CadastreNumber;
            reportParams.SimpleReportParams["TotalBuildingVolume"] = realtyObjInfo.TotalBuildingVolume;
            reportParams.SimpleReportParams["AreaMkd"] = realtyObjInfo.AreaMkd;
            reportParams.SimpleReportParams["AreaOwned"] = realtyObjInfo.AreaOwned;
            reportParams.SimpleReportParams["AreaMunicipalOwned"] = realtyObjInfo.AreaMunicipalOwned;
            reportParams.SimpleReportParams["AreaGovernmentOwned"] = realtyObjInfo.AreaGovernmentOwned;
            reportParams.SimpleReportParams["AreaLivingNotLivingMkd"] = realtyObjInfo.AreaLivingNotLivingMkd;
            reportParams.SimpleReportParams["AreaLiving"] = realtyObjInfo.AreaLiving;
            reportParams.SimpleReportParams["AreaLivingOwned"] = realtyObjInfo.AreaLivingOwned;
            reportParams.SimpleReportParams["AreaNotLivingFunctional"] = realtyObjInfo.AreaNotLivingFunctional;
            reportParams.SimpleReportParams["NecessaryConductCr"] = realtyObjInfo.NecessaryConductCr.GetEnumMeta().Display;
            reportParams.SimpleReportParams["NumberApartments"] = realtyObjInfo.NumberApartments;
            reportParams.SimpleReportParams["NumberLiving"] = realtyObjInfo.NumberLiving;
            reportParams.SimpleReportParams["NumberLifts"] = realtyObjInfo.NumberLifts;
            reportParams.SimpleReportParams["RoofingMaterial"] = realtyObjInfo.RoofingMaterial;
            reportParams.SimpleReportParams["WallMaterial"] = realtyObjInfo.WallMaterial;
            reportParams.SimpleReportParams["TypeRoof"] = realtyObjInfo.TypeRoof.GetEnumMeta().Display;
            reportParams.SimpleReportParams["HeatingSystem"] = realtyObjInfo.HeatingSystem.GetEnumMeta().Display;


            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var num = 0;
            foreach (var structuralElement in realityObjectStructuralElements)
            {
                ++num;

                section.ДобавитьСтроку();

                section["num"] = num;
                section["grname"] = structuralElement.GroupName;
                section["name"] = structuralElement.Name;
                section["LastOvrhlYear"] = structuralElement.LastOverhaulYear;
                section["Wearout"] = structuralElement.Wearout;
                section["Volume"] = structuralElement.Volume;
                section["UMeas"] = structuralElement.UnitMeasure;
                section["State"] = structuralElement.State;
            }
        }
    }
}