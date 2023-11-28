namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;
    using Bars.B4.DataModels;

    /// <summary>
    /// Платежный документ
    /// </summary>
    public class EpdProxy : IHaveId
    {
        private const string Yes = "1";

        /// <summary>
        /// 1. Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Тип документа
        /// </summary>
        public string DocumentType => EpdProxy.Yes;

        /// <summary>
        /// 3. Начисления за капитальный ремонт
        /// </summary>
        public string ChargeFlag => EpdProxy.Yes;

        /// <summary>
        /// 4. Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 5. Лицевой счет
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// 6. Уникальный код расчетного счета получателя платежа
        /// </summary>
        public long? PaymentReceiverAccountCode { get; set; }

        /// <summary>
        /// 7. Количество проживающих
        /// </summary>
        public string ResidentNumber => string.Empty;

        /// <summary>
        /// 8. Жилая площадь
        /// </summary>
        public decimal? LivingArea { get; set; }

        /// <summary>
        /// 9. Отапливаемая площадь
        /// </summary>
        public string HeatedArea => string.Empty;

        /// <summary>
        /// 10. Общая площадь для ЛС
        /// </summary>
        public string TotalArea => string.Empty;

        /// <summary>
        /// 11. Статус
        /// </summary>
        public string StateFlag => EpdProxy.Yes;

        /// <summary>
        /// 12. Задолженность за предыдущие периоды, руб.
        /// </summary>
        public decimal? Debt { get; set; }

        /// <summary>
        /// 13. Аванс на начало расчетного периода, руб.
        /// </summary>
        public decimal? Overpayment { get; set; }

        /// <summary>
        /// 14. Сумма к оплате с учетом рассрочки платежа и процентов за рассрочку,руб.
        /// </summary>
        public decimal? TotalCharge => this.Charge + this.Recalc;

        #region EPDCAPITAL

        /// <summary>
        /// 2. Платежный документ
        /// </summary>
        public long SnapshotIdCapital => this.Id;

        /// <summary>
        /// 3. Дата
        /// </summary>
        public DateTime DateCapital => this.Date;

        /// <summary>
        /// 4. Размер взноса на кв. метр (руб.)
        /// </summary>
        public decimal? TariffCapital => this.Tariff;

        /// <summary>
        /// 5. Всего начислено за расчётный период (руб.)
        /// </summary>
        public decimal? ChargeCapital => this.Charge;

        /// <summary>
        /// 6. Перерасчеты, корректировки (руб.)
        /// </summary>
        public decimal? ChangeCapital => this.Correction + this.Recalc;

        /// <summary>
        /// 7. Льготы, субсидии, скидки (руб.)
        /// </summary>
        public decimal? BenefitCapital => this.Benefit;

        /// <summary>
        /// 8. Итого к оплате за расчётный период (руб.)
        /// </summary>
        public decimal? SaldoOutCapital => this.SaldoOut;

        #endregion

        #region EPDCHARGE

        /// <summary>
        /// 2. Платежный документ
        /// </summary>
        public long SnapshotIdCharge => this.Id;

        /// <summary>
        /// 3. Код услуги
        /// </summary>
        public string ServiceCode => string.Empty;

        /// <summary>
        /// 4. Тариф
        /// </summary>
        public decimal? TariffCharge => this.Tariff;

        /// <summary>
        /// 5. Итого к оплате за расчетный период
        /// </summary>
        public decimal? SaldoOutCharge => this.SaldoOut;

        /// <summary>
        /// 6. Всего начислено за расчетный период ( без перерасчетов и льгот)
        /// </summary>
        public decimal? ChargeCharge => this.Charge;

        /// <summary>
        /// 7. Перерасчеты, корректировки (руб.)
        /// </summary>
        public decimal? ChangeCharge => this.Correction + this.Recalc;

        /// <summary>
        /// 8. Льготы, субсидии, скидки (руб.)
        /// </summary>
        public decimal? BenefitCharge => this.Benefit;

        /// <summary>
        /// 9. Тип предоставления услуги
        /// </summary>
        public string ServiceType => string.Empty;

        /// <summary>
        /// 10. Сумма платы с учётом рассрочки платежа - от платы за расчётный период
        /// </summary>
        public string CurrentSum => string.Empty;

        /// <summary>
        /// 11. Сумма платы с учётом рассрочки платежа - от платы за предыдущие расчётные периоды
        /// </summary>
        public string PreviousSum => string.Empty;

        /// <summary>
        /// 12. Проценты за рассрочку, руб.
        /// </summary>
        public string PercentInRubles => string.Empty;

        /// <summary>
        /// 13. Проценты за рассрочку, %
        /// </summary>
        public string Percent => string.Empty;

        /// <summary>
        /// 14. Сумма к оплате с учётом рассрочки платежа и процентов за рассрочку
        /// </summary>
        public string SumWithPercent => string.Empty;

        /// <summary>
        /// 15. Основания перерасчётов
        /// </summary>
        public string RecalcReason => string.Empty;

        /// <summary>
        /// 16. Сумма перерасчета , руб.
        /// </summary>
        public decimal? RecalcCharge => this.Recalc;

        /// <summary>
        /// 17. Текущие показания приборов учёта коммунальных услуг - индивидуальное потребление
        /// </summary>
        public string IndividualMeteringDeviceMeasure => string.Empty;

        /// <summary>
        /// 18. Текущие показания приборов учёта коммунальных услуг - общедомовые нужды
        /// </summary>
        public string HouseMeteringDeviceMeasure => string.Empty;

        /// <summary>
        /// 19. Суммарный объём коммунальных услуг в доме - индивидуальное потребление
        /// </summary>
        public string SumIndividualMeteringDevice => string.Empty;

        /// <summary>
        /// 20. Суммарный объём коммунальных услуг в доме - общедомовые нужды
        /// </summary>
        public string SumHouseMeteringDevice => string.Empty;

        /// <summary>
        /// 21. Норматив потребления коммунальных услуг - общедомовые нужды
        /// </summary>
        public string HouseMeteringDeviceStandard => string.Empty;

        /// <summary>
        /// 22. Норматив потребления коммунальных услуг - индивидуальное потребление
        /// </summary>
        public string IndividualMeteringDeviceStandard => string.Empty;

        /// <summary>
        /// 23. К оплате за индивидуальное потребление коммунальной услуги, руб.
        /// </summary>
        public string IndividualMeteringDeviceCharge => string.Empty;

        /// <summary>
        /// 24. К оплате за общедомовое потребление коммунальной услуги, руб.
        /// </summary>
        public string HouseMeteringDeviceCharge => string.Empty;

        /// <summary>
        /// 25. Порядок расчётов
        /// </summary>
        public string CalculationOrder => string.Empty;

        /// <summary>
        /// 26. Способ определения объемов КУ
        /// </summary>
        public string VolumeMethod => string.Empty;

        #endregion 

        /// <summary>
        /// Тариф
        /// </summary>
        public decimal? Tariff { get; set; }
 
        /// <summary>
        /// Начислено за период
        /// </summary>
        public decimal? Charge { get; set; }

        /// <summary>
        /// Корректировки
        /// </summary>
        public decimal? Correction { get; set; }

        /// <summary>
        /// Льготы
        /// </summary>
        public decimal? Benefit { get; set; }       

        /// <summary>
        /// Перерасчеты
        /// </summary>
        public decimal? Recalc { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public decimal? SaldoOut { get; set; }
    }
}