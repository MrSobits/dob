using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.GkhGji.Regions.Voronezh.Enums;
using System;

namespace Bars.GkhGji.Regions.Voronezh.Entities.ASFK
{
    /// <summary>
    /// Обмен информацией с АСФК (фед. казначейство)
    /// </summary>
    public class ASFK : BaseEntity
    {
        /// <summary>
        /// Версионный номер (FK)
        /// </summary>
        public virtual string NumVer { get; set; }

        /// <summary>
        /// Сформировано (кем, чем) (FK)
        /// </summary>
        public virtual string Former { get; set; }

        /// <summary>
        /// Версия формирующей программы (FK)
        /// </summary>
        public virtual string FormVer { get; set; }

        /// <summary>
        /// Нормативный документ (FK)
        /// </summary>
        public virtual string NormDoc { get; set; }

        /// <summary>
        /// Код ТОФК (FROM)
        /// </summary>
        public virtual string KodTofkFrom { get; set; }

        /// <summary>
        /// Наименование ТОФК (FROM)
        /// </summary>
        public virtual string NameTofkFrom { get; set; }

        /// <summary>
        /// Уровень бюджета (TO)
        /// </summary>
        public virtual ASFKBudgetLevel BudgetLevel { get; set; }

        /// <summary>
        /// Код УБП (TO)
        /// </summary>
        public virtual string KodUbp { get; set; }

        /// <summary>
        /// Наименование УБП (TO)
        /// </summary>
        public virtual string NameUbp { get; set; }

        /// <summary>
        /// ГУИД (VT)
        /// </summary>
        public virtual Guid GuidVT { get; set; }

        /// <summary>
        /// Номер лицевого счета (VT)
        /// </summary>
        public virtual string LsAdb { get; set; }

        /// <summary>
        /// Дата выписки (VT)
        /// </summary>
        public virtual DateTime DateOtch { get; set; }

        /// <summary>
        /// Дата предыдущей выписки (VT)
        /// </summary>
        public virtual DateTime? DateOld { get; set; }

        /// <summary>
        /// Признак промежуточного отчёта (VT)
        /// </summary>
        public virtual ASFKReportType VidOtch { get; set; }

        /// <summary>
        /// Код ТОФК (VT)
        /// </summary>
        public virtual string KodTofkVT { get; set; }

        /// <summary>
        /// Наименование ТОФК (VT)
        /// </summary>
        public virtual string NameTofkVT { get; set; }

        /// <summary>
        /// Код АДБ (VT)
        /// </summary>
        public virtual string KodUbpAdb { get; set; }

        /// <summary>
        /// Администратор доходов бюджета (VT)
        /// </summary>
        public virtual string NameUbpAdb { get; set; }

        /// <summary>
        /// Код ГАДБ (VT)
        /// </summary>
        public virtual string KodGadb { get; set; }

        /// <summary>
        /// Главный администратор доходов бюджета (VT)
        /// </summary>
        public virtual string NameGadb { get; set; }

        /// <summary>
        /// Наименование бюджета (VT)
        /// </summary>
        public virtual string NameBud { get; set; }

        /// <summary>
        /// Код по ОКТМО (VT)
        /// </summary>
        public virtual string Oktmo { get; set; }

        /// <summary>
        /// Код по ОКПО (VT)
        /// </summary>
        public virtual string OkpoFo { get; set; }

        /// <summary>
        /// Наименование финансового органа (VT)
        /// </summary>
        public virtual string NameFo { get; set; }

        /// <summary>
        /// Должность ответственного исполнителя (VT)
        /// </summary>
        public virtual string DolIsp { get; set; }

        /// <summary>
        /// ФИО ответственного исполнителя (VT)
        /// </summary>
        public virtual string NameIsp { get; set; }

        /// <summary>
        /// Телефон ответственного исполнителя (VT)
        /// </summary>
        public virtual string TelIsp { get; set; }

        /// <summary>
        /// Дата формирования (VT)
        /// </summary>
        public virtual DateTime? DatePod { get; set; }

        /// <summary>
        /// Итоговая сумма поступлений (раздел 2 выписки) (VT)
        /// </summary>
        public virtual decimal SumInItogV { get; set; }

        /// <summary>
        /// Итоговая сумма возвратов (раздел 2 выписки) (VT)
        /// </summary>
        public virtual decimal SumOutItogV { get; set; }

        /// <summary>
        /// Итоговая сумма зачетов (раздел 2 выписки) (VT)
        /// </summary>
        public virtual decimal SumZachItogV { get; set; }

        /// <summary>
        /// Итоговая сумма неисполненных возвратов (раздел 3 выписки) (VT)
        /// </summary>
        public virtual decimal SumNOutItogV { get; set; }

        /// <summary>
        /// Итоговая сумма неисполненных зачетов (раздел 3 выписки) (VT)
        /// </summary>
        public virtual decimal SumNZachItogV { get; set; }

        /// <summary>
        /// Сумма поступлений на начало дня (VTSUM)
        /// </summary>
        public virtual decimal SumBeginIn { get; set; }

        /// <summary>
        /// Сумма возвратов на начало дня (VTSUM)
        /// </summary>
        public virtual decimal SumBeginOut { get; set; }

        /// <summary>
        /// Сумма зачетов на начало дня (VTSUM)
        /// </summary>
        public virtual decimal SumBeginZach { get; set; }

        /// <summary>
        /// Сумма неисполненных возвратов на начало дня (VTSUM)
        /// </summary>
        public virtual decimal SumBeginNOut { get; set; }

        /// <summary>
        /// Сумма неисполненных зачетов на начало дня (VTSUM)
        /// </summary>
        public virtual decimal SumBeginNZach { get; set; }

        /// <summary>
        /// Сумма поступлений на конец дня (VTSUM)
        /// </summary>
        public virtual decimal SumEndIn { get; set; }

        /// <summary>
        /// Сумма возвратов на конец дня (VTSUM)
        /// </summary>
        public virtual decimal SumEndOut { get; set; }

        /// <summary>
        /// Сумма зачетов на конец дня (VTSUM)
        /// </summary>
        public virtual decimal SumEndZach { get; set; }

        /// <summary>
        /// Сумма неисполненных возвратов на конец дня (VTSUM)
        /// </summary>
        public virtual decimal SumEndNOut { get; set; }

        /// <summary>
        /// Сумма неисполненных зачетов на конец дня (VTSUM)
        /// </summary>
        public virtual decimal SumEndNZach { get; set; }
    }
}
