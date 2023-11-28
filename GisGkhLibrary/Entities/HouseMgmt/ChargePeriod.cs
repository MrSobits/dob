namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Период передачи текущих показаний по индивидуальным приборам учета
    /// </summary>
    public class ChargePeriod
    {
        /// <summary>
        /// Дата начала
        /// </summary>
        public sbyte StartDate { get; set; }

        /// <summary>
        /// Следующего месяца
        /// </summary>
        public bool? StartNextMonth { get; set; }

        /// <summary>
        /// Дата окончания. Если нужно указать значение "Последний день месяца", то поле заполняется значением "-1".
        /// </summary>
        public sbyte EndDate { get; set; }

        /// <summary>
        /// Следующего месяца
        /// </summary>
        public bool? EndNextMonth { get; set; }
    }
}
