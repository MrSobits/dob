using GisGkhLibrary.Entities.Dictionaries;
using GisGkhLibrary.Entities.HouseMgmt.ObjectAddress;
using GisGkhLibrary.Entities.HouseMgmt.Owners;
using GisGkhLibrary.Enums.HouseMgmt;
using System;
using System.Collections.Generic;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Договор ресурсоснабжения
    /// </summary>
    public class SupplyResourceContract
    {
        /// <summary>
        /// Ид договора в ГИС ЖКХ
        /// </summary>
        public Guid ContractGuid { get; set; }

        //Договор не является публичным и/или присутствует заключенный на бумажном носителе (электронной форме) и/или не заключен в отношении нежилых помещений в многоквартирных домах
        public bool IsContract { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// Дата заключения - обязательно, если IsNotContract = false
        /// </summary>
        public DateTime? SigningDate { get; set; }

        /// <summary>
        /// Дата вступления в силу - обязательно, если IsNotContract = false
        /// </summary>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Договор ресурсоснабжения и приложения к договору
        /// </summary>
        public List<AttachmentType> Attachments { get; set; }

        /// <summary>
        /// Период передачи текущих показаний по индивидуальным приборам учета. Обязателен для заполненияв следующих случаях: если вторая сторона отлична от "Управляющая организация" и в поле VolumeDepends = true ИЛИ если заполнено поле tns:MeteringDeviceInformation Не заполняется, если в поле VolumeDepends = false
        /// </summary>
        public ChargePeriod ChargePeriod { get; set; }

        /// <summary>
        /// Основания заключения договора
        /// </summary>
        public List<ContractConclusionReason> Reasons { get; set; }

        /// <summary>
        /// Собственник
        /// </summary>
        public OwnerBase Owner { get; set; }

        /// <summary>
        /// Автоматически пролонгировать договор на один год при наступлении даты окончания действия.
        /// </summary>
        public bool? AutomaticRollOverOneYear { get; set; }

        /// <summary>
        /// Дата окончания действия. Обязательно для заполнения, если указано значение в AutomaticRollOverOneYear
        /// </summary>
        public DateTime? ComptetionDate { get; set; }

        /// <summary>
        /// Договор заключен на неопределенный срок
        /// </summary>
        public bool? IndefiniteTerm { get; set; }

        /// <summary>
        /// Наличие в договоре планового объема и режима подачи поставки ресурсов
        /// </summary>
        public bool IsPlannedVolume { get; set; }

        /// <summary>
        /// Тип ведения планового объема и режима подачи. Заполняется при наличии в договоре планового объема и режима поставки ресурсов.
        /// </summary>
        public Context? PlannedVolumeType { get; set; }

        /// <summary>
        /// Предмет договора
        /// </summary>
        public List<ContractSubject> ContractSubjects { get; set; }

        /// <summary>
        /// Размещение информации о начислениях за коммунальные услуги осуществляет:
        ///R(SO)- РСО.
        ///P(roprietor)-Исполнитель коммунальных услуг.
        ///Заполняется, если порядок размещения информации о начислениях за коммунальные услуги ведется в разрезе договора
        /// </summary>
        public CountingResource? CountingResource { get; set; }

        /// <summary>
        /// Показатели качества коммунальных ресурсов и температурный график ведутся:
        /// D - в разрезе договора.
        /// O - в разрезе объектов жилищного фонда.
        /// </summary>
        public Context SpecifyingQualityIndicator { get; set; }

        /// <summary>
        /// Иной показатель качества коммунального ресурса (не содержащийся в справочнике показателей качества). Если показатели указываются в разрезе договора, то ссылка на ОЖФ не заполняется. Если показатели указываются в разрезе ОЖФ, то ссылка на ОЖФ обязательна.
        /// </summary>
        public List<OtherQualityIndicator> OtherQualityIndicators { get; set; }

        /// <summary>
        /// Данные об объекте жилищного фонда. При импорте договора должен быть добавлен как минимум один адрес объекта жилищного фонда
        /// </summary>
        public List<ObjectAddressContract> ObjectAddresses { get; set; }

        /// <summary>
        /// Показатель качества (содержащийся в справочнике показателей качества). Если показатели указываются в разрезе договора, то ссылка на ОЖФ не заполняется. Если показатели указываются в разрезе ОЖФ, то ссылка на ОЖФ обязательна.
        /// </summary>
        public List<Quality> Qualities { get; set; }

        /// <summary>
        /// Информация о температурном графике. Если показатели качества указываются в разрезе договора, то ссылка на ОЖФ в данном элементе не заполняется и элемент может заполняться только если предмете договора хотя бы раз встречается ресурс "Тепловая энергия". 
        /// Если показатели качества указываются в разрезе ОЖФ, то ссылка на ОЖФ обязательна и элемент заполняется только если в рамках ОЖФ встречается ресурс "Тепловая энергия".
        /// </summary>
        public List<TemperatureChart> TemperatureCharts { get; set; }

        /// <summary>
        /// Срок представления (выставления) платежных документов, не позднее. Является обязательным, если вторая сторона договора отличается от "Управляющая организация" ИЛИ если заполнено поле tns:MeteringDeviceInformation Не заполняется, если OneTimePayment = true
        /// </summary>
        public MonthDate BillingDate { get;  set; }

        /// <summary>
        /// Срок внесения платы, не позднее. Является обязательным, если вторая сторона договора отличается от "Управляющая организация" И договор не является публичным и/или присутствует заключенный на бумажном носителе или в электронной форме И в поле OneTimePayment = false. Не заполняется, если OneTimePayment = true
        /// </summary>
        public MonthDate PaymentDate { get; set; }

        /// <summary>
        /// Срок предоставления информации о поступивших платежах, не позднее. Является обязательным, если второй стороной договора является «Управляющая организация», «Размещение информации о начислениях за коммунальные услуги осуществляет» = «РСО» И договор не является публичным и/или присутствует заключенный на бумажном носителе или в электронной форме.
        /// </summary>
        public MonthDate ProvidingInformationDate { get; set; }

        /// <summary>
        /// Размещение информации об индивидуальных приборах учета и их показаниях осуществляет ресурсоснабжающая организация. Обязательно для заполнения, если в tns:CountingResource указано"РСО" . В остальных случаях не заполняется.
        /// </summary>
        public bool? MeteringDeviceInformation { get;  set; }

        /// <summary>
        /// Объем поставки ресурса(ов) определяется на основании прибора учета. Поле не заполняется, если вторая сторона договора = Управляющая организация ИЛИ в поле OneTimePayment = true
        /// </summary>
        public bool? VolumeDepends { get; set; }

        /// <summary>
        /// Оплата предоставленных услуг осуществляется единоразово при отгрузке указанных ресурсов без заведения лицевых счетов для потребителей. Доступно для заполнения, только если вторая сторона договора отлична от Управляющая организация.
        /// </summary>
        public bool? OneTimePayment { get; set; }

        /// <summary>
        /// Порядок размещения информации о начислениях за коммунальные услуги ведется
        /// D - в разрезе договора.
        /// O - в разрезе объектов жилищного фонда.
        /// Заполняется, если второй стороной договора является исполнитель коммунальных услуг
        /// </summary>
        public Context? AccrualProcedure { get; set; }

        /// <summary>
        /// Информация о применяемом тарифе
        /// </summary>
        public List<Tariff> Tariffs { get; set; }

        /// <summary>
        /// Информация о нормативе потребления коммунальной услуги
        /// </summary>
        public List<Norm> Norms { get; set; }
    }
}
