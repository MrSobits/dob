namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// Справочник работ/услуг
    /// </summary>
    public class WorkUslugaProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код работы/услуги
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Наименование работы/услуги
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 3. Базовая работа/услуга организации
        /// </summary>
        public int? BaseService { get; set; }

        /// <summary>
        /// 4. Вид работ
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 5. Код ОКЕИ
        /// </summary>
        public string OkeiCode { get; set; }

        /// <summary>
        /// 6. Другая единица измерения
        /// </summary>
        public string AnotherUnit { get; set; }

        /// <summary>
        /// 7. Родительская работа/услуга <see cref="DictUslugaProxy"/>
        /// </summary>
        public long? ParentServiceId { get; set; }

        //#region WORKREQUIRED
        ///// <summary>
        ///// WORKREQUIRED 2. Классификатор видов работ (услуг)
        ///// </summary>
        //public long? WorkClassificationId { get; set; }
        //#endregion

        //#region WORKLIST
        ///// <summary>
        ///// WORKLIST 2. Дом
        ///// </summary>
        //public long? RealityObjectId { get; set; }

        ///// <summary>
        ///// WORKLIST 3. Договор управления
        ///// </summary>
        //public long? ContractId { get; set; }

        ///// <summary>
        ///// WORKLIST 4. Период «С»
        ///// </summary>
        //public DateTime? StartDate { get; set; }

        ///// <summary>
        ///// WORKLIST 5. Период «По»
        ///// </summary>
        //public DateTime? EndDate { get; set; }

        ///// <summary>
        ///// WORKLIST 6. Статус
        ///// </summary>
        //public int Status { get; set; }
        //#endregion
    }
}