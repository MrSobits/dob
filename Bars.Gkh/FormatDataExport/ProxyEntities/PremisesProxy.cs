namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Помещение
    /// </summary>
    public class PremisesProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код помещения в системе отправителя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор дома
        /// </summary>
        public long? RealityObjectId { get; set; }

        /// <summary>
        /// 3. Уникальный идентификатор подъезда
        /// </summary>
        public long? EntranceId { get; set; }

        /// <summary>
        /// 4. Номер помещения
        /// </summary>
        public string RoomNum { get; set; }

        /// <summary>
        /// 5. Тип помещения
        /// </summary>
        public RoomType Type { get; set; }

        /// <summary>
        /// 6. Нежилое помещение является общим имуществом в МКД
        /// </summary>
        public int? IsCommonProperty { get; set; }

        /// <summary>
        /// Вид дома (для 7. Характеристика жилого помещения)
        /// </summary>
        public TypeHouse? TypeHouse { get; set; }

        /// <summary>
        /// 8. Общая площадь помещения по паспорту помещения
        /// </summary>
        public decimal? Area { get; set; }

        /// <summary>
        /// 9. Жилая площадь помещения по паспорту помещения
        /// </summary>
        public decimal? LivingArea { get; set; }

        /// <summary>
        /// 10. Кадастровый номер в ГКН
        /// </summary>
        public string CadastralHouseNumber { get; set; }

        /// <summary>
        /// 12. Этаж
        /// </summary>
        public int? Floor { get; set; }

        #region Лицевые счета

        /// <summary>
        /// 13. Количество проживающих
        /// </summary>
        public int? AccountsNum { get; set; }

        /// <summary>
        /// 16. Количество комнат
        /// </summary>
        public int? RoomsCount { get; set; }

        #endregion


    }
}
