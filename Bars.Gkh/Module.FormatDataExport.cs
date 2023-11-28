namespace Bars.Gkh
{
    using System.Collections.Generic;

    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.Domain.Impl;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.ExportableFile;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Impl;
    using Bars.Gkh.FormatDataExport.FormatProvider;
    using Bars.Gkh.FormatDataExport.FormatProvider.Converter;
    using Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat;
    using Bars.Gkh.FormatDataExport.NetworkWorker;
    using Bars.Gkh.FormatDataExport.NetworkWorker.Impl;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.FormatDataExport.ProxySelectors.Impl;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.FormatDataExport.Scheduler;
    using Bars.Gkh.FormatDataExport.Scheduler.Impl;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel.Registration;

    public partial class Module
    {
        private void RegisterFormatDataExport()
        {
            this.Container.RegisterTransient<IExportFormatProviderBuilder, ExportFormatProviderBuilder>();
            this.Container.Register(Component.For<CsvFormatProvider>().LifestyleTransient());
            this.Container.Register(Component.For<NetCsvFormatProvider>().LifestyleTransient());
            this.Container.Register(Component.For<FormatDataExportJob>().LifestyleTransient());

            this.Container.RegisterTransient<IExportFormatConverter, ExportFormatConverter>();
            this.Container.RegisterTransient<IFormatDataTransferService, FormatDataTransferService>();
            this.Container.RegisterTransient<IProxySelectorFactory, ProxySelectorFactory>();
            this.Container.RegisterTransient<IFormatDataExportFilterService, FormatDataExportFilterService>();
            this.Container.RegisterTransient<IFormatDataExportSchedulerService, FormatDataExportSchedulerService>();
            this.Container.RegisterTransient<IFormatDataExportIncrementalService, FormatDataExportIncrementalService>();

            this.Container.RegisterService<IExportableEntityResolver, ExportableEntityResolver>();
            this.Container.RegisterService<IFormatDataExportRoleService, FormatDataExportRoleService>();

            ContainerHelper.RegisterFileInfoExportableEntity<FilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<InfoExportableEntity>(this.Container);

            ContainerHelper.RegisterExportableEntity<ContragentAreaExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<ContragentExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DictMeasureExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DictUslugaExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DomExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoAddressExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoObjectExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoObjectQualityExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoObjectOtherQualityExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DrsoUslugaResExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DuExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DuChargeExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DuFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DuOuExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<DuOuUslugaExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<EntranceExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<LiftExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<OgvExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<OmsExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<PoiExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<PremisesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<RoomExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<RsoExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<SotrudExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UoExportableEntity>(this.Container);
            //ContainerHelper.RegisterExportableEntity<UoProtocolossExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UstavExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UstavChargeExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UstavChargeFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UstavFilesExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UstavOuExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<UstavOuUslugaExportableEntity>(this.Container);
            ContainerHelper.RegisterExportableEntity<WorkTimeExportableEntity>(this.Container);

            ContainerHelper.RegisterProxySelectorService<ActualManOrgByRealityObject, ActualManOrgByRealityObjectSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<RealityObjectByContract, RealityObjectByContractSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DomProxy, DomSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DrsoAddressProxy, DrsoAddressSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DrsoObjectQualityProxy, DrsoObjectQualitySelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DuProxy, DuSelectorService>(this.Container);
            //ContainerHelper.RegisterProxySelectorService<DuChargeProxy, DuChargeSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DuOuProxy, DuOuSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DuOuUslugaProxy, DuOuUslugaSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<UoProxy, UoSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<UstavProxy, UstavSelectorService>(this.Container);
            //ContainerHelper.RegisterProxySelectorService<UstavChargeProxy, UstavChargeSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<UstavOuProxy, UstavOuSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<UstavOuUslugaProxy, UstavOuUslugaSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<DictUslugaProxy, DictUslugaSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<ContragentProxy, ContragentSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<PremisesProxy, PremisesSelectorService>(this.Container);
            ContainerHelper.RegisterProxySelectorService<RoomProxy, RoomSelectorService>(this.Container);

            this.RegisterEntityGroups();
        }

        private void RegisterEntityGroups()
        {
            this.RegisterEntityGroup("ActWorkDogovEntityGroup",
                "Акты выполненных работ по договору на выполнение работ (оказание услуг) по капитальному ремонту",
                new List<string>
                {
                    "ACTWORKDOGOV",
                    "ACTWORK",
                    "ACTWORKDOGOVFILES"
                },
                FormatDataExportProviderFlags.RegOpCr
                | FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("BankEntityGroup",
                "Банки",
                new List<string>
                {
                    "BANK"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("ContragentEntityGroup",
                "Контрагенты",
                new List<string>
                {
                    "CONTRAGENT"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("ContragentRschetEntityGroup",
                "Расчетные счета",
                new List<string>
                {
                    "CONTRAGENTRSCHET"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("DictMeasureEntityGroup",
                "Справочник единиц измерения",
                new List<string>
                {
                    "DICTMEASURE"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("DictParamValEntityGroup",
                "Справочные значения параметров",
                new List<string>
                {
                    "DICTPARAMVAL"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("DictUslugaEntityGroup",
                "Справочник услуг",
                new List<string>
                {
                    "DICTUSLUGA"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("DogovorKprEntityGroup",
                "Договоры на выполнение работ (оказание услуг) по капитальному ремонту",
                new List<string>
                {
                    "DOGOVORKPR",
                    "DOGOVORKPRFILES",
                    "RASTORGKPR",
                    "RASTORGKPRFILES",
                    "WORKDOGOV"
                },
                FormatDataExportProviderFlags.Ogv
                | FormatDataExportProviderFlags.RegOpCr
                | FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("DogPoiEntityGroup",
                "Договоры на пользование общим имуществом",
                new List<string>
                {
                    "DOGPOI",
                    "DOGPOIFILES",
                    "DOGPOIPROTOCOLOSS",
                    "DOGPOIPAYMENT",
                    "DOGPOIPAYMENTPERIOD",
                    "POI"
                },
                FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("DomEntityGroup",
                "Характеристики жилищного фонда",
                new List<string>
                {
                    "DOM",
                    "DOMDOC",
                    "DOMPARAM",
                    "ENTRANCE",
                    "PREMISES",
                    "ROOM",
                    "LIFT"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("DomInfoEntityGroup",
                "Сведения о домах",
                new List<string>
                {
                    "DOM",
                    "ENTRANCE",
                    "PREMISES",
                    "ROOM"
                },
                FormatDataExportProviderFlags.RegOpCr
                | FormatDataExportProviderFlags.Oms
                | FormatDataExportProviderFlags.Rso
                | FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("DrsoEntityGroup",
                "Договоры ресурсоснабжения",
                new List<string>
                {
                    "DRSO",
                    "DRSOFILES",
                    "DRSOOBJECT",
                    "DRSOADDRESS",
                    "DRSOUSLUGARES",
                    "DRSOOBJECTQUALITY",
                    "DRSOOBJECTOTHERQUALITY",
                    "DRSOTEMP"
                },
                FormatDataExportProviderFlags.Rso);

            this.RegisterEntityGroup("DuEntityGroup",
                "Договоры управления",
                new List<string>
                {
                    "DU",
                    "DUVOTPRO",
                    "DUFILES",
                    "DUCHARGE",
                    "DUOU",
                    "DUOUUSLUGA"
                },
                FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("IndEntityGroup",
                "Физические лица",
                new List<string>
                {
                    "IND"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("KapRemProtocolossEntityGroup",
                "Решения о выборе способа формирования фонда капитального ремонта",
                new List<string>
                {
                    "KAPREMPROTOCOLOSS",
                    "KAPREMPROTOCOLFILES"
                },
                FormatDataExportProviderFlags.RegOpCr
                | FormatDataExportProviderFlags.Oms
                | FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("KprEntityGroup",
                "Программы капитального ремонта",
                new List<string>
                {
                    "KPR",
                    "PLANKPR",
                    "WORKKPRTYPE",
                    "WORKKPR"
                },
                FormatDataExportProviderFlags.RegOpCr);

            this.RegisterEntityGroup("KvarEntityGroup",
                "Сведения о лицевых счетах",
                new List<string>
                {
                    "KVAR",
                    "KVARACCOM"
                },
                FormatDataExportProviderFlags.RegOpCr
                | FormatDataExportProviderFlags.Rso
                | FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("ProtocolossEntityGroup",
                "Протоколы общего собрания собственников",
                new List<string>
                {
                    //"PROTOCOLOSS",
                    "VOTPROCONT",
                    //"PROTOCOLOSSFILES",
                    "SOLUTIONOSS"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("AuditEntityGroup",
                "Проверки",
                new List<string>
                {
                    "AUDIT",
                    "AUDITOBJECT",
                    "AUDITEVENT",
                    "AUDITPLACE",
                    "AUDITFILES",
                    "AUDITDOC",
                    "AUDITRESULTFILES",
                    "AUDITRESULT",
                    "PRECEPTHOUSE",
                    "PRECEPTAUDIT",
                    "PRECEPTFILES",
                    "PROTOCOLAUDIT",
                    "PROTOCOLFILES",
                    "REVEALEDVIOL"
                },
                FormatDataExportProviderFlags.Gji);

            this.RegisterEntityGroup("AuditPlanEntityGroup",
                "Планы проверок",
                new List<string>
                {
                    "AUDITPLAN",
                    "AUDIT",
                    "AUDITOBJECT",
                    "AUDITPLACE",
                    "AUDITEVENT"
                },
                FormatDataExportProviderFlags.Gji);

            this.RegisterEntityGroup("RegOpAccountsEntityGroup",
                "Расчетные счета регионального оператора",
                new List<string>
                {
                    "REGOPSCHET"
                },
                FormatDataExportProviderFlags.RegOpCr);

            this.RegisterEntityGroup("RegOpEntityGroup",
                "Региональные операторы капитального ремонта",
                new List<string>
                {
                    "REGOP"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("RsoEntityGroup",
                "Ресурсоснабжающие организации",
                new List<string>
                {
                    "RSO"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("SpecialAccountsEntityGroup",
                "Специальные счета капитального ремонта",
                new List<string>
                {
                    "REGOPSCHET"
                },
                FormatDataExportProviderFlags.RegOpCr
                | FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("UoEntityGroup",
                "Управляющие организации",
                new List<string>
                {
                    "UO"
                },
                FormatDataExportProviderFlags.All);

            this.RegisterEntityGroup("UstavEntityGroup",
                "Уставы",
                new List<string>
                {
                    "USTAV",
                    "USTAVVOTPROT",
                    "USTAVFILES",
                    "USTAVOU",
                    "USTAVOUUSLUGA",
                    "USTAVCHARGE",
                    "USTAVCHARGEFILES"
                },
                FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("WorkActualPlanEntityGroup",
                "Актуальные планы по перечню работ/услуг",
                new List<string>
                {
                    "WORKACTUALPLAN",
                    "WORKPLAN",
                    "WORKPLANDATE"
                },
                FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("WorkListEntityGroup",
                "Сведения по перечню работ/услуг",
                new List<string>
                {
                    "WORKLIST",
                    "WORK"
                },
                FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("WorkUslugaEntityGroup",
                "Работы и услуги организации",
                new List<string>
                {
                    "WORKUSLUGA",
                    "WORKREQUIRED"
                },
                FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup("EpdEntityGroup",
                "Начисления / ЕПД",
                new List<string>
                {
                    "EPD",
                    "EPDCHARGE",
                    "EPDCAPITAL"
                },
                FormatDataExportProviderFlags.RegOpCr
                    | FormatDataExportProviderFlags.Rso 
                    | FormatDataExportProviderFlags.Uo);

            this.RegisterEntityGroup(
                "OplataEntityGroup",
                "Извещения о принятии к исполнению распоряжений",
                new List<string>
                {
                    "OPLATA",
                    "OPLATAPACK"
                },
                FormatDataExportProviderFlags.RegOpCr);

            this.RegisterEntityGroup(
                "KvaraccomEntityGroup",
                "Связь помещений плательщиков с лицевыми счетами",
                new List<string>
                {
                    "KVARACCOM"
                },
                FormatDataExportProviderFlags.RegOpCr);
        }

        private void RegisterEntityGroup(string code, string description, IList<string> inheritedEtities, FormatDataExportProviderFlags allowProviderFlags)
        {
            this.Container.Register(Component.For<IExportableEntityGroup>()
                .ImplementedBy<ExportableEntityGroup>()
                .UsingFactoryMethod(() => new ExportableEntityGroup(code, description, inheritedEtities, allowProviderFlags))
                .LifestyleSingleton()
                .Named(code));
        }
    }
}