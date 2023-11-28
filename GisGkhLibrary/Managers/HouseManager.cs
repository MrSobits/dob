using System;
using System.Collections.Generic;
using System.Linq;
using GisGkhLibrary.Entities.HouseMgmt.House;
using GisGkhLibrary.Enums.HouseMgmt;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.HouseManagement;
using GisGkhLibrary.Services;

namespace GisGkhLibrary.Managers
{
    /// <summary>
    /// Человекоюзабельная обертка для методов адресных объектов
    /// </summary>
    public static class HouseManager
    {
        /// <summary>
        /// Импорт данных об адресных объектах
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="operationType">Тип операции с домом</param>
        /// <param name="NonResidentialPremiseToCreate">Нежилые помещения, которые нужно добавить</param>
        /// <param name="NonResidentialPremiseToUpdate">Нежилые помещения, которые нужно изменить</param>
        /// <param name="ResidentialPremiseToCreate">Жилые помещения, которые нужно добавить</param>
        /// <param name="ResidentialPremiseToUpdate">Жилые помещения, которые нужно изменить</param>
        /// <param name="EntranceToCreate">Подъезды, которые нужно добавить</param>
        /// <param name="EntranceToUpdate">Подъезды, которые нужно изменить</param>
        /// <param name="inheritMissingValues">Если флаг указан для запроса с обновлением данных, то отсутствующие в дельте значения будут подгружены из предыдущей версии сущности. 
        /// В противном случае отсутствующие значения будут сброшены в NULL.</param>
        public static void ImportHouseRSOData(ApartmentHouse house, 
            AddOrUpdate operationType, 
            IEnumerable<NonResidentialPremise> NonResidentialPremiseToCreate,
            IEnumerable<NonResidentialPremise> NonResidentialPremiseToUpdate,
            IEnumerable<ResidentialPremise> ResidentialPremiseToCreate,
            IEnumerable<ResidentialPremise> ResidentialPremiseToUpdate,
            IEnumerable<Entrance> EntranceToCreate,
            IEnumerable<Entrance> EntranceToUpdate,
            bool? inheritMissingValues = null)
        {
            HouseManagementService.ImportHouseRSOData(new importHouseRSORequestApartmentHouse
            {
                Item = GetItem(house, operationType),
                NonResidentialPremiseToCreate = NonResidentialPremiseToCreate.Select(x => ConvertNonResidentialPremiseToCreate(x)).ToArray(),
                NonResidentialPremiseToUpdate = NonResidentialPremiseToUpdate.Select(x => ConvertNonResidentialPremiseToUpdate(x)).ToArray(),
                ResidentialPremises = GetResidentialPremises(ResidentialPremiseToCreate, ResidentialPremiseToUpdate),
                EntranceToCreate = EntranceToCreate.Select(x => ConvertEntranceToCreate(x)).ToArray(),
                EntranceToUpdate = EntranceToUpdate.Select(x => ConvertEntranceToUpdate(x)).ToArray(),
            },
            inheritMissingValues);
        }

        private static object GetItem(ApartmentHouse house, AddOrUpdate operationType)
        {
            var searchCriteriasDictionary = GetSearchCriterias(house);

            switch (operationType)
            {
                case AddOrUpdate.Add:                   

                    return new importHouseRSORequestApartmentHouseApartmentHouseToCreate
                    {
                        TransportGUID = Guid.NewGuid().ToString(),
                        BasicCharacteristicts = new HouseBasicRSOType
                        {
                            FIASHouseGuid = house.FIAS.ToString(),
                            OKTMO = new OKTMORefType
                            {
                                code = house.OKTMO,
                                name = house.OKTMOName
                            },                            
                            OlsonTZ = house.OlsonTZ.GetHouseManagementNsiRef(),
                            ItemsElementName = searchCriteriasDictionary.Select(x => x.Key).ToArray(),
                            Items = searchCriteriasDictionary.Select(x => x.Value).ToArray(),                            
                        }
                    };
                case AddOrUpdate.Update:
                    return new importHouseRSORequestApartmentHouseApartmentHouseToUpdate
                    {
                        TransportGUID = Guid.NewGuid().ToString(),
                        BasicCharacteristicts = new HouseBasicUpdateRSOType
                        {
                            FIASHouseGuid = house.FIAS.ToString(),
                            OKTMO = new OKTMORefType
                            {
                                code = house.OKTMO,
                                name = house.OKTMOName
                            },
                            OlsonTZ = house.OlsonTZ.GetHouseManagementNsiRef(),
                            ItemsElementName = searchCriteriasDictionary.Select(x => x.Key).ToArray(),
                            Items = searchCriteriasDictionary.Select(x => x.Value).ToArray(),
                        }
                    };
                default:
                    throw new GISGKHRequestException($"Неизвестный тип операции: {operationType}");
            }
        }

        private static importHouseRSORequestApartmentHouseNonResidentialPremiseToCreate ConvertNonResidentialPremiseToCreate(NonResidentialPremise premise)
        {
            var searchCriteriasDictionary = GetSearchCriterias(premise);
            return new importHouseRSORequestApartmentHouseNonResidentialPremiseToCreate
            {
                TransportGUID = Guid.NewGuid().ToString(),
                ItemsElementName = searchCriteriasDictionary.Select(x => x.Key).ToArray(),
                Items = searchCriteriasDictionary.Select(x => x.Value).ToArray(),
                PremisesNum = premise.RoomNumber,
                FIASChildHouseGuid = premise.FIASChildHouse.ToString(),
                TotalAreaSpecified = premise.TotalArea.HasValue,
                TotalArea = premise.TotalArea.GetValueOrDefault(),
            };
        }

        private static importHouseRSORequestApartmentHouseNonResidentialPremiseToUpdate ConvertNonResidentialPremiseToUpdate(NonResidentialPremise premise)
        {
            var searchCriteriasDictionary = GetSearchCriterias(premise);
            return new importHouseRSORequestApartmentHouseNonResidentialPremiseToUpdate
            {
                TransportGUID = Guid.NewGuid().ToString(),
                PremisesGUID = premise.PremisesGUID.ToString(),
                ItemsElementName = searchCriteriasDictionary.Select(x => x.Key).ToArray(),
                Items = searchCriteriasDictionary.Select(x => x.Value).ToArray(),
                PremisesNum = premise.RoomNumber,
                FIASChildHouseGuid = premise.FIASChildHouse.ToString(),
                TotalAreaSpecified = premise.TotalArea.HasValue,
                TotalArea = premise.TotalArea.GetValueOrDefault(),                
                AnnulmentReason = premise.AnnulmentReason.GetHouseManagementNsiRef(),
                AnnulmentInfo = premise.AnnulmentInfo
            };
        }

        private static importHouseRSORequestApartmentHouseResidentialPremises[] GetResidentialPremises(IEnumerable<ResidentialPremise> residentialPremiseToCreate, 
            IEnumerable<ResidentialPremise> residentialPremiseToUpdate)
        {
            var result = new List<importHouseRSORequestApartmentHouseResidentialPremises>(residentialPremiseToCreate.Count() + residentialPremiseToUpdate.Count());

            foreach(var premise in residentialPremiseToCreate)
            {
                var searchCriteriasDictionary = GetSearchCriterias(premise);
                result.Add(new importHouseRSORequestApartmentHouseResidentialPremises
                {
                    Item = new importHouseRSORequestApartmentHouseResidentialPremisesResidentialPremisesToCreate
                    {
                        TransportGUID = Guid.NewGuid().ToString(),
                        ItemsElementName = searchCriteriasDictionary.Select(x => x.Key).ToArray(),
                        Items = searchCriteriasDictionary.Select(x => x.Value).ToArray(),
                        Item = !String.IsNullOrEmpty(premise.EntranceNum) ? (object)premise.EntranceNum : false,
                        FIASChildHouseGuid = premise.FIASChildHouse.ToString(),
                        PremisesCharacteristic = premise.PremisesCharacteristic.GetHouseManagementNsiRef(),
                        TotalAreaSpecified = premise.TotalArea.HasValue,
                        TotalArea = premise.TotalArea.GetValueOrDefault(),
                        PremisesNum = premise.RoomNumber,
                    },
                    LivingRoomToCreate = premise.LivingRoomToCreate.Select(x => ConvertLivingRoomToCreate(x)).ToArray(),
                    LivingRoomToUpdate = premise.LivingRoomToUpdate.Select(x => ConvertLivingRoomToUpdate(x)).ToArray(),
                });
            }

            foreach (var premise in residentialPremiseToUpdate)
            {
                var searchCriteriasDictionary = GetSearchCriterias(premise);
                result.Add(new importHouseRSORequestApartmentHouseResidentialPremises
                {
                    Item = new importHouseRSORequestApartmentHouseResidentialPremisesResidentialPremisesToUpdate
                    {
                        TransportGUID = Guid.NewGuid().ToString(),
                        PremisesGUID = premise.PremisesGUID.ToString(),
                        ItemsElementName = searchCriteriasDictionary.Select(x => x.Key).ToArray(),
                        Items = searchCriteriasDictionary.Select(x => x.Value).ToArray(),
                        Item = !String.IsNullOrEmpty(premise.EntranceNum) ? (object)premise.EntranceNum : false,
                        FIASChildHouseGuid = premise.FIASChildHouse.ToString(),
                        PremisesCharacteristic = premise.PremisesCharacteristic.GetHouseManagementNsiRef(),
                        TotalAreaSpecified = premise.TotalArea.HasValue,
                        TotalArea = premise.TotalArea.GetValueOrDefault(),
                        PremisesNum = premise.RoomNumber,
                        AnnulmentReason = premise.AnnulmentReason.GetHouseManagementNsiRef(),
                        AnnulmentInfo = premise.AnnulmentInfo
                    },
                    LivingRoomToCreate = premise.LivingRoomToCreate.Select(x => ConvertLivingRoomToCreate(x)).ToArray(),
                    LivingRoomToUpdate = premise.LivingRoomToUpdate.Select(x => ConvertLivingRoomToUpdate(x)).ToArray(),
                });
            }

            return result.ToArray();
        }

        private static importHouseRSORequestApartmentHouseResidentialPremisesLivingRoomToCreate ConvertLivingRoomToCreate(LivingRoom room)
        {
            var searchCriteriasDictionary = GetSearchCriterias(room);
            return new importHouseRSORequestApartmentHouseResidentialPremisesLivingRoomToCreate
            {
                TransportGUID = Guid.NewGuid().ToString(),
                ItemsElementName = searchCriteriasDictionary.Select(x => x.Key).ToArray(),
                Items = searchCriteriasDictionary.Select(x => x.Value).ToArray(),
                RoomNumber = room.RoomNumber,
                Square = room.Square.GetValueOrDefault(),
                SquareSpecified = room.Square.HasValue,
            };
        }

        private static importHouseRSORequestApartmentHouseResidentialPremisesLivingRoomToUpdate ConvertLivingRoomToUpdate(LivingRoom room)
        {
            var searchCriteriasDictionary = GetSearchCriterias(room);
            return new importHouseRSORequestApartmentHouseResidentialPremisesLivingRoomToUpdate
            {
                TransportGUID = Guid.NewGuid().ToString(),
                LivingRoomGUID = room.LivingRoomGUID.ToString(),
                ItemsElementName = searchCriteriasDictionary.Select(x => x.Key).ToArray(),
                Items = searchCriteriasDictionary.Select(x => x.Value).ToArray(),
                RoomNumber = room.RoomNumber,
                Square = room.Square.GetValueOrDefault(),
                SquareSpecified = room.Square.HasValue,
                AnnulmentReason = room.AnnulmentReason.GetHouseManagementNsiRef(),
                AnnulmentInfo = room.AnnulmentInfo
            };
        }

        private static Dictionary<ItemsChoiceType18, object> GetSearchCriterias(HouseIdentifiersBase identifiers)
        {
            var result = new Dictionary<ItemsChoiceType18, object>();

            if (!String.IsNullOrWhiteSpace(identifiers.CadastralNumber))
                result.Add(ItemsChoiceType18.CadastralNumber, identifiers.CadastralNumber);

            if (!String.IsNullOrWhiteSpace(identifiers.ConditionalNumber))
                result.Add(ItemsChoiceType18.CadastralNumber, identifiers.ConditionalNumber);

            if (identifiers.No_RSO_GKN_EGRP_Data)
                result.Add(ItemsChoiceType18.No_RSO_GKN_EGRP_Data, identifiers.No_RSO_GKN_EGRP_Data);

            if (identifiers.No_RSO_GKN_EGRP_Registered)
                result.Add(ItemsChoiceType18.No_RSO_GKN_EGRP_Registered, identifiers.No_RSO_GKN_EGRP_Registered);

            if (identifiers.Restrictions != null)
            {
                foreach (var restriction in identifiers.Restrictions)
                {
                    result.Add(ItemsChoiceType18.RightOrEncumbrance, new HouseManagement.RightOrEncumbrance
                    {
                        Type = restriction.Type == Enums.HouseMgmt.RightOrEncumbrance.Encumbrance ? RightOrEncumbranceType.E : RightOrEncumbranceType.R,
                        RegNumber = restriction.RegNumber,
                        RegDate = restriction.RegDate,
                    });
                }
            }
            return result;
        }

        private static importHouseRSORequestApartmentHouseEntranceToCreate ConvertEntranceToCreate(Entrance entrance)
        {
            return new importHouseRSORequestApartmentHouseEntranceToCreate
            {
                TransportGUID = Guid.NewGuid().ToString(),
                EntranceNum = entrance.EntranceNum,
                FIASChildHouseGuid = entrance.FIASChildHouse.ToString(),
            };
        }

        private static importHouseRSORequestApartmentHouseEntranceToUpdate ConvertEntranceToUpdate(Entrance entrance)
        {
            return new importHouseRSORequestApartmentHouseEntranceToUpdate
            {
                TransportGUID = Guid.NewGuid().ToString(),
                EntranceGUID = entrance.EntranceGUID.ToString(),
                EntranceNum = entrance.EntranceNum,
                FIASChildHouseGuid = entrance.FIASChildHouse.ToString(),
                AnnulmentReason = entrance.AnnulmentReason.GetHouseManagementNsiRef(),
                AnnulmentInfo = entrance.AnnulmentInfo
            };
        }
    }
}
