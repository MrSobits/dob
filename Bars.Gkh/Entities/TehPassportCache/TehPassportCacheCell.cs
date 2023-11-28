namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Кэшированное значение технического пасспорта
    /// <para>Не хранимая сущность</para>
    /// </summary>
    public class TehPassportCacheCell
    {
        /// <summary>
        /// Идентификатор дома <see cref="RealityObject"/>
        /// </summary>
        public long RealityObjectId { get; set; }

        /// <summary>
        /// Код формы
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// Номер строки
        /// </summary>
        public int RowId { get; set; }

        /// <summary>
        /// Номер столбца
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Значение ячейки
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Код ячейки
        /// </summary>
        public string CellCode => $"{this.RowId}:{this.ColumnId}";
    }
}