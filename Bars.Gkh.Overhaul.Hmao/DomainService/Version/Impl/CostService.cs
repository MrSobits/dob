using Bars.B4;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.CommonEstateObject;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities.Version;
using Bars.Gkh.Overhaul.Hmao.Enum;
using Bars.B4.Modules.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using Bars.Gkh.Enums;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version.Impl
{
    internal class CostService : ICostService
    {
        public IFileManager FileManager { get; set; }

        public IDomainService<CostLimit> CostLimitDomain { get; set; }

        public IDomainService<CostLimitOOI> CostLimitOOIDomain { get; set; }

        public IDomainService<VersionRecordStage1> VersionRecordStage1Domain { get; set; }

        public IDomainService<VersionRecordStage2> VersionRecordStage2Domain { get; set; }

        public IDomainService<VersionRecord> VersionRecordStage3Domain { get; set; }

        public IDomainService<VersionActualizeLog> VersionActualizeLogDomain { get; set; }

        public IUserIdentity UserIdentity { get; set; }

        /// <summary>
        /// Сервис ДПКР
        /// </summary>
        public ILongProgramService LongProgramService { get; set; }

        /// <summary>
        /// Пересчитать стоимости всех элементов версии
        /// </summary>
        public void CalculateVersion(ProgramVersion version)
        {
            StringBuilder log = new StringBuilder();
            int count = 0;
            log.Append($"Тип сущности;Id;Адрес;ООИ;Старая сумма;Новая сумма;\n");

            var allStages3 = VersionRecordStage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.VersionRecordState != VersionRecordState.NonActual)
                .GroupBy(x => x.Stage2Version.Stage3Version)
                .ToDictionary(x => x.Key, y => y.ToList());

            foreach (var stage3 in allStages3)
            {
                //пересчитываем стоимости всех stage1
                decimal stage3cost = 0;
                foreach (var stage1 in stage3.Value)
                {

                    var sum = this.LongProgramService.GetDpkrCost(
                        stage1.RealityObject.Municipality.Id,
                        0,
                        (int)stage1.Year,
                        stage1.StructuralElement.StructuralElement.Id,
                        stage1.RealityObject.Id,
                        stage1.RealityObject.CapitalGroup != null ? stage1.RealityObject.CapitalGroup.Id : 0,
                        stage1.StructuralElement.StructuralElement.CalculateBy,
                        stage1.StructuralElement.Volume,
                        stage1.RealityObject.AreaLiving,
                        stage1.RealityObject.AreaMkd,
                        stage1.RealityObject.AreaLivingNotLivingMkd);

                   // var sum = GetCost(stage1.RealityObject, stage1.Stage2Version.CommonEstateObject, (short)stage1.Year, stage1.StructuralElement.Volume, stage1.StructuralElement.StructuralElement.CalculateBy);
                    if(sum.HasValue && stage1.Sum != sum.Value)
                    {
                        log.Append($"Stage1;{stage1.Id};{stage1.RealityObject.Address};{stage1.Stage2Version.CommonEstateObject.Name};{stage1.Sum};{sum.Value};\n");

                        stage1.Sum = sum.Value;
                        VersionRecordStage1Domain.Save(stage1);

                        count++;
                    }

                    stage3cost += stage1.Sum;
                }

                //пересчитываем стоимости всех stage3
                if (stage3.Key.Sum != stage3cost)
                {
                    log.Append($"Stage3;{stage3.Key.Id};{stage3.Key.RealityObject.Address};{stage3.Key.CommonEstateObjects};{stage3.Key.Sum};{stage3cost};\n");

                    stage3.Key.Sum = stage3cost;
                    VersionRecordStage3Domain.Save(stage3.Key);

                    count++;
                }
            }

            //Сохраняем лог
            VersionActualizeLogDomain.Save(new VersionActualizeLog
            {
                ProgramVersion = version,
                Municipality = version.Municipality,
                UserName = UserIdentity.Name,
                DateAction = DateTime.Now,
                ActualizeType = VersionActualizeType.ActualizeSum,
                CountActions = count,
                LogFile = FileManager.SaveFile($"CalculateCostLog_{DateTime.Now.ToString("dd_MMM_yyyy-HH_mm_ss")}.csv", Encoding.UTF8.GetBytes(log.ToString())),
            });
        }


        List<CostLimitOOI> cache = null;
        public decimal? GetCost(RealityObject house, CommonEstateObject ooi, short yearRepair, decimal volume, PriceCalculateBy calcBy)
        {
            if (calcBy == PriceCalculateBy.Volume && volume==0)
                return 0;

            if (cache == null)
                cache = CostLimitOOIDomain.GetAll().ToList();

            var cost = cache
                .Where(x => x.CommonEstateObject.Id == ooi.Id)
                .Where(x => x.Municipality == null || x.Municipality.Id == house.Municipality.Id)
                .Where(x => x.DateStart == null || x.DateStart.Value.Year <= yearRepair)
                .Where(x => x.DateEnd == null || x.DateEnd.Value.Year >= yearRepair)
                .Where(x => x.FloorStart == null || x.FloorStart <= house.Floors)
                .Where(x => x.FloorEnd == null || x.FloorEnd >= house.Floors)
                .Select(x => x.Cost)
                .ToList();

            if (cost.Count > 1)
                throw new ApplicationException($"Под условия отбора предельной стоимости {house.Address}:{ooi.Name}:{yearRepair} подошли несколько стоимостей. Пожалуйста удалите лишние");
            else if(cost.Count == 0)
                return null;
            else
                return cost.First() * volume;
        }

        public decimal? GetCost(RealityObject house, Work work, short yearRepair, decimal volume)
        {
            if (volume == 0)
                return 0;

            var cost = CostLimitDomain.GetAll()
                .Where(x => x.Work.Id == work.Id)
                .Where(x => x.Municipality == null || x.Municipality.Id == house.Municipality.Id)
                .Where(x => x.DateStart == null || x.DateStart.Value.Year <= yearRepair)
                .Where(x => x.DateEnd == null || x.DateEnd.Value.Year >= yearRepair)
                .Where(x => x.FloorStart == null || x.FloorStart <= house.Floors)
                .Where(x => x.FloorEnd == null || x.FloorEnd >= house.Floors)
                .Select(x => x.Cost)
                .ToList();

            if (cost.Count > 1)
                throw new ApplicationException($"Под условия отбора предельной стоимости {house.Address}:{work.Name}:{yearRepair} подошли несколько стоимостей. Пожалуйста удалите лишние");
            else if (cost.Count == 0)
                return null;
            else
                return cost.First() * volume;
        }

        private IEnumerable<VersionRecordStage1> GetAllStages1(VersionRecord stage3)
        {
            return VersionRecordStage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.Id == stage3.Id)
                .ToList();
        }
    }
}
