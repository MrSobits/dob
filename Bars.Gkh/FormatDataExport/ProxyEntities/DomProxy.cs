namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// Прокси для Дом
    /// </summary>
    public class DomProxy :IHaveId
    {
        /// <summary>
        /// 1. Уникальный код дома в системе отправителя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Город/район
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 3. Населенный пункт
        /// </summary>
        public string Settlement { get; set; }

        /// <summary>
        /// 4. Улица
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 5. Номер дома
        /// </summary>
        public string House { get; set; }

        /// <summary>
        /// 6. Строение (секция)
        /// </summary>
        public string Building { get; set; }

        /// <summary>
        /// 7. Корпус
        /// </summary>
        public string Housing { get; set; }

        /// <summary>
        /// 8. Литера
        /// </summary>
        public string Letter { get; set; }

        /// <summary>
        /// 9. Код контрагента, с которым заключен договор на управление домом
        /// </summary>
        public long? ContragentId { get; set; }

        /// <summary>
        /// 10. Категория благоустроенности
        /// </summary>
        public int? ComfortСategory { get; set; }

        /// <summary>
        /// 11. Максимальная этажность
        /// </summary>
        public int? MaximumFloors { get; set; }

        /// <summary>
        /// 12. Год постройки (указывается 1 число года, например 01.01.1900)
        /// </summary>
        public int? BuildYear { get; set; }

        /// <summary>
        /// 13. Общая площадь (по техническому паспорту для расчета распределения расходов по площади дома)
        /// </summary>
        public decimal? AreaMkd { get; set; }

        /// <summary>
        /// 14. Площадь мест общего пользования
        /// </summary>
        public decimal? AreaCommonUsage { get; set; }

        /// <summary>
        /// 15. Полезная (отапливаемая площадь)
        /// </summary>
        public decimal? HeatingArea { get; set; }

        /// <summary>
        /// 16. Количество строк – лицевой счет
        /// </summary>
        public int? PersonalAccountCount { get; set; }

        /// <summary>
        /// 17. Код Улицы КЛАДР
        /// </summary>
        public string KladrCode { get; set; }

        /// <summary>
        /// 18. Код Улицы ФИАС
        /// </summary>
        public string StreetGuid { get; set; }

        /// <summary>
        /// 19. Код дома ФИАС
        /// </summary>
        public string HouseGuid { get; set; }

        /// <summary>
        /// 20. Кадастровый номер в ГКН
        /// </summary>
        public string CadastralHouseNumber { get; set; }

        /// <summary>
        /// 21. Условный номер ЕГРП
        /// </summary>
        public string EgrpNumber { get; set; }

        /// <summary>
        /// 22. Состояние дома
        /// </summary>
        public int? ConditionHouseId { get; set; }

        /// <summary>
        /// 23. Тип дома
        /// </summary>
        public int? TypeHouse { get; set; }

        /// <summary>
        /// 24. Общая площадь жилых помещений по паспорту помещения
        /// </summary>
        public decimal? AreaLiving { get; set; }

        /// <summary>
        /// 25. Год ввода в эксплуатацию
        /// </summary>
        public DateTime? CommissioningYear { get; set; }

        /// <summary>
        /// 26. Способ формирования фонда капитального ремонта
        /// </summary>
        public int? AccountFormationVariant { get; set; }

        /// <summary>
        /// 27. Количество подземных этажей
        /// </summary>
        public int UndergroundFloorCount { get; set; }

        /// <summary>
        /// 28. Количество этажей наименьшее
        /// </summary>
        public int? MinimumFloors { get; set; }

        /// <summary>
        /// 29. Часовая зона по Olsоn
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// 30. Наличие у дома статуса объекта культурного наследия
        /// </summary>
        public int? IsCulturalHeritage { get; set; }

        /// <summary>
        /// 31. Способ управления домом
        /// </summary>
        public int? TypeManagement { get; set; }

        /// <summary>
        /// 32. Код ОКТМО
        /// </summary>
        public string OktmoCode { get; set; }

        /// <summary>
        /// 33. Стадия жизненного цикла
        /// </summary>
        public string LifeCycleStage { get; set; }

        /// <summary>
        /// 34. Год проведения реконструкции
        /// </summary>
        public string ReconstructionYear { get; set; }

        /// <summary>
        /// Идентификатор расчетного счета
        /// </summary>
        public long? CalcAccountId { get; set; }
    }
}