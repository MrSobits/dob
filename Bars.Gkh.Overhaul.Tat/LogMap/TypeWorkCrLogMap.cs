namespace Bars.Gkh.Overhaul.Tat.LogMap
{
    using System;
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using GkhCr.Entities;

    public class TypeWorkCrLogMap : AuditLogMap<TypeWorkCr>
    {
        public TypeWorkCrLogMap()
        {
            Name("График выполнения работ");

            Description(v => String.Format("{0} - {1}", v.ObjectCr.RealityObject.Address, v.Work.Name));

            MapProperty(v => v.Work.Name, "WorkName", "Вид работы");
            MapProperty(v => v.FinanceSource.Name, "FinanceSourceName", "Разрез финансирования");
            MapProperty(v => v.Work.UnitMeasure.Name, "WorkUnitMeasureName", "Единица измерения");
            MapProperty(v => v.DateStartWork, "DateStartWork", "Дата начала работ", v => v.Return(d => d.Value.ToString("dd.MM.yyyy")));
            MapProperty(v => v.DateEndWork, "DateEndWork", "Дата окончания работ", v => v.Return(d => d.Value.ToString("dd.MM.yyyy")));
        }
    }
}
