using GisGkhLibrary.Entities.Dictionaries;
using GisGkhLibrary.Entities.HouseMgmt;
using GisGkhLibrary.Entities.HouseMgmt.ObjectAddress;
using GisGkhLibrary.Entities.HouseMgmt.Owners;
using GisGkhLibrary.Entities.HouseMgmt.Person;
using GisGkhLibrary.Entities.HouseMgmt.Person.Identifiers;
using GisGkhLibrary.Entities.HouseMgmt.Volume;
using GisGkhLibrary.Enums.HouseMgmt;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.HouseManagement;
using GisGkhLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GisGkhLibrary.Managers
{
    /// <summary>
    /// Человекоюзабельная обертка для методов импорта договора ресурсоснабжения с РСО
    /// </summary>
    public static class ImportSupplyResourceContractDataManager
    {
        /// <summary>
        /// Создание/изменение договора ресурсоснабжения с РСО
        /// </summary>
        /// <param name="contract">Договор</param>
        /// <param name="contractGuid">Идентификатор договора ресурсоснабжения в ГИС ЖКХ</param>
        /// <param name="lastVersionGuid">true - идентификатор последней версии, false - идентификатор базовой версии</param>
        public static void NewContract(Guid? contractGuid, SupplyResourceContract contract, bool lastVersionGuid = false)
        {
            var items = GetItems(contract);

            var contracts = new importSupplyResourceContractRequestContract[]
            {
                new importSupplyResourceContractRequestContract
                {
                    TransportGUID = Guid.NewGuid().ToString(),
                    ItemElementName = lastVersionGuid ? ItemChoiceType16.ContractGUID : ItemChoiceType16.ContractRootGUID,
                    Item = null,//contractGuid.ToString(),
                    Item1 = new SupplyResourceContractType
                    {
                        Item = GetSupplyResourceContractType(contract),
                        Items = items.items,
                        ItemsElementName = items.itemsName,
                        //Period = GetPeriod(contract.ChargePeriod),
                        ContractBase = contract.Reasons.Select(x => x.GetHouseManagementNsiRef()).ToArray(),
                        Item1 = GetOwner(contract.Owner),
                        IsPlannedVolume = contract.IsPlannedVolume,
                        //PlannedVolumeTypeSpecified = contract.PlannedVolumeType.HasValue,
                        //PlannedVolumeType = !contract.PlannedVolumeType.HasValue ? default(SupplyResourceContractTypePlannedVolumeType) : (contract.PlannedVolumeType.Value == Context.Dogovor ? SupplyResourceContractTypePlannedVolumeType.D : SupplyResourceContractTypePlannedVolumeType.O),
                        ContractSubject = contract.ContractSubjects.Select(x => new SupplyResourceContractTypeContractSubject
                        {
                            TransportGUID = x.PairKey.ToString(),
                            ServiceType = x.ServiceType.GetContractSubjectTypeServiceType(),
                            MunicipalResource = x.RatedResource.GetContractSubjectTypeMunicipalResource(),
                            StartSupplyDate = x.StartSupplyDate,
                            EndSupplyDate= x.EndSupplyDate ?? default(DateTime),
                            EndSupplyDateSpecified= x.EndSupplyDate.HasValue,
                            PlannedVolume= ConvertPlannedVolume(x.PlannedVolume)
                        }).ToArray(),
                        //
                        //CountingResourceSpecified = contract.CountingResource.HasValue,
                        //CountingResource = !contract.CountingResource.HasValue ? default(SupplyResourceContractTypeCountingResource) : (contract.CountingResource.Value ==  CountingResource.Proprietor ? SupplyResourceContractTypeCountingResource.P : SupplyResourceContractTypeCountingResource.R),
                        //
                        SpecifyingQualityIndicators = contract.SpecifyingQualityIndicator == Context.Dogovor ? SupplyResourceContractTypeSpecifyingQualityIndicators.D : SupplyResourceContractTypeSpecifyingQualityIndicators.O,
                        OtherQualityIndicator = contract.OtherQualityIndicators.Select(x => ConvertOtherQualityIndicator(x)).ToArray(),
                        ObjectAddress = contract.ObjectAddresses.Select(x => ConvertObjectAddress(x)).ToArray(),
                        Quality = contract.Qualities.Select(x => ConvertQuality(x)).ToArray(),
                        TemperatureChart = contract.TemperatureCharts.Select(x => ConvertTemperatureChart(x)).ToArray(),
                        BillingDate = ConvertBillingDate(contract.BillingDate),
                        PaymentDate = ConvertPaymentDate(contract.PaymentDate),
                        //ProvidingInformationDate = ConvertProvidingInformationDate(contract.ProvidingInformationDate),
                        //
                        MeteringDeviceInformationSpecified = contract.MeteringDeviceInformation.HasValue,
                        MeteringDeviceInformation = contract.MeteringDeviceInformation ?? default(bool),
                        //
                        VolumeDependsSpecified = contract.VolumeDepends.HasValue,
                        VolumeDepends = contract.VolumeDepends ?? default(bool),
                        //
                        OneTimePaymentSpecified = contract.OneTimePayment.HasValue,
                        OneTimePayment = contract.OneTimePayment ?? default(bool),
                        //
                        AccrualProcedureSpecified = contract.AccrualProcedure.HasValue,
                        AccrualProcedure = contract.AccrualProcedure.HasValue ? (contract.AccrualProcedure.Value == Context.Dogovor ? SupplyResourceContractTypeAccrualProcedure.D : SupplyResourceContractTypeAccrualProcedure.O) : default(SupplyResourceContractTypeAccrualProcedure),
                        //
                        Tariff = contract.Tariffs.Select(x => ConvertTariff(x)).ToArray(),
                        Norm = contract.Norms.Select(x => ConvertNorm(x)).ToArray()
                    }
                }
            };

            var result = HouseManagementService.ImportSupplyResourceContractData(contracts);
            ImportResultCommonResult item = (ImportResultCommonResult)result.Items[0];
            foreach(var subitem in item.Items)
            {
                if(subitem is CommonResultTypeError)
                {
                    throw new GISGKHAnswerException($"{((CommonResultTypeError)subitem).Description}");
                }
            }            
        }


        /// <summary>
        /// Расторжение договора ресурсоснабжения с РСО
        /// </summary>
        /// <param name="reason">Причина расторжения договора</param>
        /// <param name="terminateDate">Дата расторжения, прекращения действия устава</param>
        /// <param name="contractGuid">Идентификатор договора ресурсоснабжения в ГИС ЖКХ</param>
        /// <param name="lastVersionGuid">true - идентификатор последней версии, false - идентификатор базовой версии</param>
        public static void TerminateContract(Guid contractGuid, ContractTerminationReason reason, DateTime terminateDate, bool lastVersionGuid = false)
        {
            var contracts = new importSupplyResourceContractRequestContract[]
            {
                new importSupplyResourceContractRequestContract
                {
                    TransportGUID = Guid.NewGuid().ToString(),
                    ItemElementName = lastVersionGuid ? ItemChoiceType16.ContractGUID : ItemChoiceType16.ContractRootGUID,
                    Item = contractGuid.ToString(),
                    Item1 = new importSupplyResourceContractRequestContractTerminateContract
                    {
                        ReasonRef = reason.GetHouseManagementNsiRef(),
                        Terminate = terminateDate
                    }
                }
            };

            var result = HouseManagementService.ImportSupplyResourceContractData(contracts);            
        }

        /// <summary>
        /// Аннулирование договора ресурсоснабжения с РСО
        /// </summary>
        /// <param name="contractGuid">Идентификатор договора ресурсоснабжения в ГИС ЖКХ</param>
        /// <param name="reason">Причина аннулирования договора</param>
        /// <param name="lastVersionGuid">true - идентификатор последней версии, false - идентификатор базовой версии</param>
        public static void AnnulmentContract(Guid contractGuid, string reason, bool lastVersionGuid = false)
        {

            var contracts = new importSupplyResourceContractRequestContract[]
            {
                new importSupplyResourceContractRequestContract
                {
                    TransportGUID = Guid.NewGuid().ToString(),
                    ItemElementName = lastVersionGuid ? ItemChoiceType16.ContractGUID : ItemChoiceType16.ContractRootGUID,
                    Item = contractGuid.ToString(),
                    Item1 = new AnnulmentType
                    {
                        ReasonOfAnnulment = reason
                    }
                }
            };

            var result = HouseManagementService.ImportSupplyResourceContractData(contracts);
        }

        /// <summary>
        /// Пролонгация договора ресурсоснабжения с РСО
        /// </summary>
        /// <param name="contractGuid">Идентификатор договора ресурсоснабжения в ГИС ЖКХ</param>
        /// <param name="rollOverDate">Дата окончания пролонгации</param>
        /// <param name="lastVersionGuid">true - идентификатор последней версии, false - идентификатор базовой версии</param>
        public static void RollOverContract(Guid contractGuid, DateTime rollOverDate, bool lastVersionGuid = false)
        {
            var contracts = new importSupplyResourceContractRequestContract[]
            {
                new importSupplyResourceContractRequestContract
                {
                    TransportGUID = Guid.NewGuid().ToString(),
                    ItemElementName = lastVersionGuid ? ItemChoiceType16.ContractGUID : ItemChoiceType16.ContractRootGUID,
                    Item = contractGuid.ToString(),
                    Item1 = new importSupplyResourceContractRequestContractRollOverContract
                    {
                        RollOverDate = rollOverDate,
                    }
                }
            };

            var result = HouseManagementService.ImportSupplyResourceContractData(contracts);
        }

        private static SupplyResourceContractTypeNorm ConvertNorm(Norm norm)
        {
            if (norm == null)
                return null;

            return new SupplyResourceContractTypeNorm
            {
                Items = norm.AddressObjectKey.HasValue ? new object[] { norm.AddressObjectKey.Value.ToString() } : new object[] { true },
                PairKey = norm.PairKey.ToString(),
                NormGUID = norm.NormGUID.ToString()
            };
        }

        private static SupplyResourceContractTypeTariff ConvertTariff(Tariff tariff)
        {
            if(tariff == null)
                return null;

            return new SupplyResourceContractTypeTariff
            {
                PairKey = tariff.PairKey.ToString(),
                PriceGUID = tariff.PriceGUID.ToString()
            };
        }

        private static SupplyResourceContractTypeProvidingInformationDate ConvertProvidingInformationDate(MonthDate date)
        {
            if (date == null)
                return null;

            return new SupplyResourceContractTypeProvidingInformationDate
            {
                Date = date.DayOfMonth,
                DateType = date.Type == MonthDateType.Current ? SupplyResourceContractTypeProvidingInformationDateDateType.C : SupplyResourceContractTypeProvidingInformationDateDateType.N
            };
        }

        private static SupplyResourceContractTypePaymentDate ConvertPaymentDate(MonthDate date)
        {
            if (date == null)
                return null;

            return new SupplyResourceContractTypePaymentDate
            {
                Date = date.DayOfMonth,
                DateType = date.Type == MonthDateType.Current ? SupplyResourceContractTypePaymentDateDateType.C : SupplyResourceContractTypePaymentDateDateType.N
            };
        }

        private static SupplyResourceContractTypeBillingDate ConvertBillingDate(MonthDate date)
        {
            if (date == null)
                return null;

            return new SupplyResourceContractTypeBillingDate
            {
                Date = date.DayOfMonth,
                DateType = date.Type == MonthDateType.Current ? SupplyResourceContractTypeBillingDateDateType.C : SupplyResourceContractTypeBillingDateDateType.N
            };
        }

        private static SupplyResourceContractTypeTemperatureChart ConvertTemperatureChart(TemperatureChart chart)
        {
            if (chart == null)
                return null;

            return new SupplyResourceContractTypeTemperatureChart
            {
                AddressObjectKey = chart.AddressObjectKey.ToString(),
                OutsideTemperature = chart.OutsideTemperature,
                FlowLineTemperature = chart.FlowLineTemperature,
                OppositeLineTemperature = chart.OppositeLineTemperature,
            };
        }

        private static SupplyResourceContractTypeQuality ConvertQuality(Quality quality)
        {
            if (quality == null)
                return null;

            var values = new Dictionary<ItemsChoiceType20, object>();
            if (quality.IndicatorValue.Correspond.HasValue)
                values.Add(ItemsChoiceType20.Correspond, quality.IndicatorValue.Correspond.Value);
            if (quality.IndicatorValue.EndRange.HasValue)
                values.Add(ItemsChoiceType20.EndRange, quality.IndicatorValue.EndRange.Value);
            if (quality.IndicatorValue.Number.HasValue)
                values.Add(ItemsChoiceType20.Number, quality.IndicatorValue.Number.Value);
            if (!String.IsNullOrEmpty(quality.IndicatorValue.OKEI))
                values.Add(ItemsChoiceType20.OKEI, quality.IndicatorValue.OKEI);
            if (quality.IndicatorValue.StartRange.HasValue)
                values.Add(ItemsChoiceType20.StartRange, quality.IndicatorValue.StartRange.Value);

            return new SupplyResourceContractTypeQuality
            {
                AddressObjectKey = quality.AddressObjectKey,
                PairKey = quality.PairKey,
                QualityIndicator = quality.QualityIndicator.GetHouseManagementNsiRef(),
                IndicatorValue = new SupplyResourceContractTypeQualityIndicatorValue
                {
                    Items = values.Select(x => x.Value).ToArray(),
                    ItemsElementName = values.Select(x => x.Key).ToArray()
                },
                AdditionalInformation = quality.AdditionalInformation
            };
        }

        private static SupplyResourceContractTypeObjectAddress ConvertObjectAddress(ObjectAddressContract objectAddress)
        {
            if (objectAddress == null)
                return null;

            return new SupplyResourceContractTypeObjectAddress
            {
                TransportGUID = Guid.NewGuid().ToString(),
                HouseTypeSpecified = objectAddress.HouseType.HasValue,
                HouseType = objectAddress.HouseType.HasValue ? ConvertHouseType(objectAddress.HouseType.Value) : default(ObjectAddressTypeHouseType),                
                FIASHouseGuid = objectAddress.FIASHouseGuid,
                ApartmentNumber = objectAddress.ApartmentNumber,
                RoomNumber = objectAddress.RoomNumber,
                //
                Pair = objectAddress.Pairs.Select(x => ConvertPair(x)).ToArray(),
                //PlannedVolume = objectAddress.PlannedVolumes.Select(x => ConvertSupplyContractPlannedVolume(x)).ToArray(),
                //
                CountingResource = objectAddress.CountingResource.HasValue ? (objectAddress.CountingResource.Value == CountingResource.Proprietor ? SupplyResourceContractTypeObjectAddressCountingResource.P : SupplyResourceContractTypeObjectAddressCountingResource.R) : default(SupplyResourceContractTypeObjectAddressCountingResource),
                CountingResourceSpecified = objectAddress.CountingResource.HasValue,
                //
                MeteringDeviceInformation = objectAddress.MeteringDeviceInformation ?? default(bool),
                MeteringDeviceInformationSpecified = objectAddress.MeteringDeviceInformation.HasValue,
            };
        }

        private static SupplyResourceContractTypeObjectAddressPlannedVolume ConvertSupplyContractPlannedVolume(SupplyContractPlannedVolume plannedVolume)
        {
            if (plannedVolume == null)
                return null;

            return new SupplyResourceContractTypeObjectAddressPlannedVolume
            {
                PairKey = plannedVolume.PairKey.ToString(),
                Volume = plannedVolume.Volume,
                Unit = "113",//((int)plannedVolume.Unit).ToString(),
                FeedingMode = plannedVolume.FeedingMode
            };
        }

        private static SupplyResourceContractTypeObjectAddressPair ConvertPair(Pair pair)
        {
            if (pair == null)
                return null;

            return new SupplyResourceContractTypeObjectAddressPair
            {
                PairKey = pair.PairKey.ToString(),
                StartSupplyDate = pair.StartSupplyDate,
                EndSupplyDateSpecified = pair.EndSupplyDate.HasValue,
                EndSupplyDate = pair.EndSupplyDate ?? default(DateTime),
                HeatingSystemType = ConvertHeatingSystem(pair.HeatingSystemType)
            };
        }

        private static SupplyResourceContractTypeObjectAddressPairHeatingSystemType ConvertHeatingSystem(HeatingSystemType heatingSystemType)
        {
            if (heatingSystemType == null)
                return null;

            return new SupplyResourceContractTypeObjectAddressPairHeatingSystemType
            {
                OpenOrNot = heatingSystemType.OpenOrNot == OpenOrNot.Opened ? SupplyResourceContractTypeObjectAddressPairHeatingSystemTypeOpenOrNot.Opened : SupplyResourceContractTypeObjectAddressPairHeatingSystemTypeOpenOrNot.Closed,
                CentralizedOrNot = heatingSystemType.CentralizedOrNot == CentralizedOrNot.Centralized ? SupplyResourceContractTypeObjectAddressPairHeatingSystemTypeCentralizedOrNot.Centralized : SupplyResourceContractTypeObjectAddressPairHeatingSystemTypeCentralizedOrNot.Decentralized
            };
        }

        private static ObjectAddressTypeHouseType ConvertHouseType(Enums.HouseMgmt.ObjectAddressType type)
        {
            switch(type)
            {
                case Enums.HouseMgmt.ObjectAddressType.MKD:
                    return ObjectAddressTypeHouseType.MKD;
                case Enums.HouseMgmt.ObjectAddressType.ZHD:
                    return ObjectAddressTypeHouseType.ZHD;
                case Enums.HouseMgmt.ObjectAddressType.ZHDBlockZastroyki:
                    return ObjectAddressTypeHouseType.ZHDBlockZastroyki;
                default:
                    throw new GISGKHRequestException($"Нет сопоставления для ObjectAddressType.{type.ToString()}");
            }
        }

        private static SupplyResourceContractTypeOtherQualityIndicator ConvertOtherQualityIndicator(OtherQualityIndicator indicator)
        {
            if (indicator == null)
                return null;

            var items = new Dictionary<ItemsChoiceType21, object>();
            if (!String.IsNullOrEmpty(indicator.OKEI))
                items.Add(ItemsChoiceType21.OKEI, indicator.OKEI);
            if (indicator.Correspond.HasValue)
                items.Add(ItemsChoiceType21.Correspond, indicator.Correspond.Value);
            if (indicator.Number.HasValue)
                items.Add(ItemsChoiceType21.Number, indicator.Number.Value);
            if (indicator.StartRange.HasValue)
                items.Add(ItemsChoiceType21.StartRange, indicator.StartRange.Value);
            if (indicator.EndRange.HasValue)
                items.Add(ItemsChoiceType21.EndRange, indicator.EndRange.Value);

            return new SupplyResourceContractTypeOtherQualityIndicator
            {
                AddressObjectKey = indicator.AddressObjectKey.ToString(),
                PairKey = indicator.PairKey.ToString(),
                IndicatorName = indicator.IndicatorName,
                AdditionalInformation = indicator.AdditionalInformation,
                ItemsElementName = items.Select(x => x.Key).ToArray(),
                Items = items.Select(x => x.Value).ToArray()
            };
    }

        private static ContractSubjectTypePlannedVolume ConvertPlannedVolume(PlannedVolume plannedVolume)
        {
            if (plannedVolume == null)
                return null;

            return new ContractSubjectTypePlannedVolume
            {
                Volume = plannedVolume.Volume,
                Unit = "113", //((int)plannedVolume.Unit).ToString(),
                FeedingMode = plannedVolume.FeedingMode
            };
        }

        //return type: ApartmentBuildingOwner, ApartmentBuildingRepresentativeOwner, ApartmentBuildingSoleOwner, LivingHouseOwner, Offer, Organization
        private static object GetOwner(OwnerBase owner)
        {
            if (owner == null)
                return null;
            else if (owner is OfferOwner)
                return true;
            else if (owner is ApartmentBuildingOwner)
                return new SupplyResourceContractTypeApartmentBuildingOwner
                {
                    Item = ConvertPerson(((ApartmentBuildingOwner)owner).Person)
                };
            else if (owner is ApartmentBuildingRepresentativeOwner)
                return new SupplyResourceContractTypeApartmentBuildingRepresentativeOwner
                {
                    Item = ConvertPerson(((ApartmentBuildingRepresentativeOwner)owner).Person)
                };
            else if (owner is ApartmentBuildingSoleOwner)
                return new SupplyResourceContractTypeApartmentBuildingSoleOwner
                {
                    Item = ConvertPerson(((ApartmentBuildingSoleOwner)owner).Person)
                };
            else if (owner is LivingHouseOwner)
                return new SupplyResourceContractTypeLivingHouseOwner
                {
                    Item = ConvertPerson(((LivingHouseOwner)owner).Person)
                };
            else if (owner is OrganizationOwner)
                return new SupplyResourceContractTypeOrganization
                {
                };
            else throw new GISGKHRequestException($"Не найдено преобразование из типа {owner.GetType().Name} в формат ГИС ГМП");
        }

        /// <summary>
        /// Преобразование типа лица
        /// </summary>
        private static object ConvertPerson(PersonBase person)
        {
            if (person == null)
                return null;
            else if (person is PhysicalPerson)
            {
                var physicalPerson = (PhysicalPerson)person;
                return new DRSOIndType
                {
                    FirstName = physicalPerson.FirstName,
                    Surname = physicalPerson.Surname,
                    Patronymic = physicalPerson.Patronymic,
                    //
                    SexSpecified = physicalPerson.Gender.HasValue,
                    Sex = !physicalPerson.Gender.HasValue ? default(DRSOIndTypeSex) : (physicalPerson.Gender.Value == Enums.Gender.Man ? DRSOIndTypeSex.M : DRSOIndTypeSex.F),
                    //
                    DateOfBirthSpecified = physicalPerson.DateOfBirth.HasValue,
                    DateOfBirth = physicalPerson.DateOfBirth.HasValue ? physicalPerson.DateOfBirth.Value : default(DateTime),
                    //
                    Item = GetId(physicalPerson.Identifier),
                    //
                    PlaceBirth = physicalPerson.PlaceBirth,
                };
            }
            else if (person is JuridicalPerson)
            {
                return new DRSORegOrgType
                {
                    orgRootEntityGUID = ((JuridicalPerson)person).OrgRootEntityGUID.ToString()
                };
            }
            else throw new GISGKHRequestException($"Не найдено преобразование из типа {person.GetType().Name} в формат ГИС ГМП");
        }

        /// <summary>
        /// Преобразование идентификатора личности
        /// </summary>
        /// <returns>ID, SNILS</returns>
        private static object GetId(IdentifierBase identifier)
        {
            if (identifier == null)
                return null;
            else if (identifier is SNILSIdentifier)
                return ((SNILSIdentifier)identifier).Number;
            else if (identifier is Identifier)
            {
                var id = (Identifier)identifier;
                return new ID
                {
                    Type = id.IdentifierType.GetHouseManagementNsiRef(),
                    Series = id.Series,
                    Number = id.Number,
                    IssueDate = id.IssueDate
                };
            }
            else throw new GISGKHRequestException($"Не найдено преобразование из типа {identifier.GetType().Name} в формат ГИС ГМП");
        }

        private static SupplyResourceContractTypePeriod GetPeriod(ChargePeriod chargePeriod)
        {
            if (chargePeriod == null)
                return null;

            return new SupplyResourceContractTypePeriod
            {
                Start = new SupplyResourceContractTypePeriodStart
                {
                    StartDate = chargePeriod.StartDate,
                    NextMonth = true,//chargePeriod.StartNextMonth.HasValue ? chargePeriod.StartNextMonth.Value : true,
                    NextMonthSpecified = chargePeriod.StartNextMonth.HasValue
                },
                End = new SupplyResourceContractTypePeriodEnd
                {
                    EndDate = chargePeriod.EndDate,
                    NextMonth = true,//chargePeriod.EndNextMonth.HasValue ? chargePeriod.EndNextMonth.Value : true,
                    NextMonthSpecified = chargePeriod.EndNextMonth.HasValue
                }
            };
        }

        //AutomaticRollOverOneYear, ComptetionDate, IndefiniteTerm
        private static (object[] items, ItemsChoiceType19[] itemsName) GetItems(SupplyResourceContract contract)
        {
            var count = 0;
            if (contract.IndefiniteTerm.HasValue)
                count++;
            if (contract.AutomaticRollOverOneYear.HasValue)
                count++;
            if (contract.ComptetionDate.HasValue)
                count++;

            var items = new object[count];
            var itemsName = new ItemsChoiceType19[count];

            var n = 0;
            if (contract.IndefiniteTerm.HasValue)
            {
                items[n] = contract.IndefiniteTerm.Value;
                itemsName[n++] = ItemsChoiceType19.IndefiniteTerm;
            }
            if (contract.AutomaticRollOverOneYear.HasValue)
            {
                items[n] = contract.AutomaticRollOverOneYear.Value;
                itemsName[n++] = ItemsChoiceType19.AutomaticRollOverOneYear;
            }
            if (contract.ComptetionDate.HasValue)
            {
                items[n] = contract.ComptetionDate.Value;
                itemsName[n++] = ItemsChoiceType19.ComptetionDate;
            }

            return (items, itemsName);
        }

        private static object GetSupplyResourceContractType(SupplyResourceContract contract)
        {
            if (contract.IsContract)
                return new SupplyResourceContractTypeIsContract
                {
                    ContractNumber = contract.ContractNumber,
                    SigningDate = contract.SigningDate.Value,
                    EffectiveDate = contract.EffectiveDate.Value,
                    ContractAttachment = contract.Attachments != null ? contract.Attachments.Select(x => ConvertAttachmant(x)).ToArray() : null
                };
            else
                return new SupplyResourceContractTypeIsNotContract
                {
                    ContractNumber = contract.ContractNumber,
                    SigningDate = contract.SigningDate ?? default(DateTime),
                    SigningDateSpecified = contract.SigningDate.HasValue,
                    EffectiveDate = contract.EffectiveDate ?? default(DateTime),
                    EffectiveDateSpecified = contract.EffectiveDate.HasValue,
                    ContractAttachment = contract.Attachments != null ? contract.Attachments.Select(x => ConvertAttachmant(x)).ToArray() : null
                };
        }

        private static HouseManagement.AttachmentType ConvertAttachmant(Entities.HouseMgmt.AttachmentType attachment)
        {
            if (attachment == null)
                return null;

            return new HouseManagement.AttachmentType
            {
                Name = attachment.Name,
                Description = attachment.Description,
                Attachment = new Attachment
                {
                    AttachmentGUID = attachment.Attachment.ToString()
                },
                AttachmentHASH = attachment.AttachmentHASH
            };
        }
    }
}
