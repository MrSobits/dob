﻿namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Entities.HouseManagement;
    using HouseManagementAsync;

    /// <summary>
    /// Задача подготовки данных по домам для управляющих организаций
    /// </summary>
    public partial class HouseUOPrepareDataTask : BaseHousePrepareDataTask<importHouseUORequest>
    {
        /// <summary>
        /// Переопределить параметры сбора данных
        /// </summary>
        /// <param name="parameters">Параметры сбора</param>
        protected override void OverrideExtractingParametes(DynamicDictionary parameters)
        {
            parameters.Add("uoId", this.Contragent.GkhId);
        }

        /// <summary>
        /// Проверка дома перед импортом
        /// </summary>
        /// <param name="house">Дом</param>
        /// <returns>Результат проверки</returns>
        protected override ValidateObjectResult CheckHouse(RisHouse house)
        {
            StringBuilder messages = new StringBuilder();

            if (house.FiasHouseGuid.IsEmpty())
            {
                messages.Append("FIASHouseGuid ");
            }

            if (house.OktmoCode.IsEmpty())
            {
                messages.Append("OktmoCode ");
            }

            if (house.OlsonTZCode.IsEmpty() || house.OlsonTZGuid.IsEmpty())
            {
                messages.Append("OlsonTZ ");
            }

            if (house.HouseType == HouseType.Apartment)
            {
                if (house.UndergroundFloorCount.IsEmpty())
                {
                    messages.Append("UndergroundFloorCount ");
                }
            }

            return new ValidateObjectResult
            {
                Id = house.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Дом"
            };
        }

        /// <summary>
        /// Проверка жилого помещения перед импортом
        /// </summary>
        /// <param name="premise">Жилое помещение</param>
        /// <returns>Результат проверки</returns>
        protected override ValidateObjectResult CheckResidentialPremise(ResidentialPremises premise)
        {
            StringBuilder messages = new StringBuilder();

            if (premise.PremisesNum.IsEmpty())
            {
                messages.Append("PremisesNum ");
            }

            if (premise.EntranceNum == null)
            {
                messages.Append("EntranceNum ");
            }

            if (premise.PremisesCharacteristicCode.IsEmpty() || premise.PremisesCharacteristicGuid.IsEmpty())
            {
                messages.Append("PremisesCharacteristic ");
            }

            if (premise.TotalArea == null)
            {
                messages.Append("TotalArea ");
            }

            if (premise.GrossArea == null)
            {
                messages.Append("GrossArea ");
            }

            return new ValidateObjectResult
            {
                Id = premise.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Жилое помещение"
            };
        }

        /// <summary>
        /// Проверка нежилого помещения перед импортом
        /// </summary>
        /// <param name="premise">Нежилое помещение</param>
        /// <returns>Результат проверки</returns>
        protected override ValidateObjectResult CheckNonResidentialPremise(NonResidentialPremises premise)
        {
            StringBuilder messages = new StringBuilder();

            if (string.IsNullOrEmpty(premise.PremisesNum))
            {
                messages.Append("PremisesNum ");
            }

            if (!premise.TotalArea.HasValue)
            {
                messages.Append("TotalArea ");
            }

            return new ValidateObjectResult
            {
                Id = premise.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Нежилое помещение"
            };
        }

        /// <summary>
        /// Проверка комнаты в жилом доме перед импортом
        /// </summary>
        /// <param name="livingRoom">Комната в жилом доме</param>
        /// <returns>Результат проверки</returns>
        protected override ValidateObjectResult CheckLivingRoom(LivingRoom livingRoom)
        {
            StringBuilder messages = new StringBuilder();

            if (livingRoom.RoomNumber.IsEmpty())
            {
                messages.Append("RoomNumber ");
            }

            if (livingRoom.Square == null)
            {
                messages.Append("Square ");
            }

            return new ValidateObjectResult
            {
                Id = livingRoom.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Комната"
            };
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importHouseUORequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importHouseUORequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);
                request.Id = Guid.NewGuid().ToString();

                result.Add(request, transportGuidDictionary);
            }
            return result;
        }

        /// <summary>
        /// Получить объект запроса
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта (в текущем методе в списке 1 объект)</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект запроса</returns>
        private importHouseUORequest GetRequestObject(IEnumerable<RisHouse> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var house = listForImport.First();
            object item = null;

            switch (house.HouseType)
            {
                case HouseType.Apartment:
                    item = this.CreateApartmentHouseRequest(house, transportGuidDictionary);
                    break;
                case HouseType.Living:
                    // case HouseType.Blocks:
                    item = this.CreateLivingHouseRequest(house, transportGuidDictionary);
                    break;
            }

            return new importHouseUORequest { Item = item };
        }

        /// <summary>
        /// Получить BasicCharacteristicts для раздела ApartmentHouseToCreate
        /// </summary>
        /// <param name="house">Дом</param>
        /// <returns>BasicCharacteristicts для раздела ApartmentHouseToCreate</returns>
        private T GetBasicCharacteristictsToCreate<T>(RisHouse house) where T : HouseBasicUOType, new()
        {
            var items = this.GetBasicCharacteristictsItem(house.CadastralNumber);

            return new T
            {
                Items = items,
                ItemsElementName = this.GetBasicCharacteristictsItemElementName(items),
                FIASHouseGuid = house.FiasHouseGuid,
                TotalSquare = decimal.Round(house.TotalSquare.GetValueOrDefault(), 1),
                TotalSquareSpecified = house.TotalSquare.HasValue,
                State = !house.StateCode.IsEmpty() && !house.StateGuid.IsEmpty() ? new nsiRef
                {
                    Code = house.StateCode,
                    GUID = house.StateGuid
                } : null,
                //ProjectSeries = house.ProjectSeries,
                //ProjectType = !string.IsNullOrEmpty(house.ProjectTypeCode) || !string.IsNullOrEmpty(house.ProjectTypeGuid) ?
                //        new nsiRef
                //        {
                //            Code = house.ProjectTypeCode,
                //            GUID = house.ProjectTypeGuid
                //        } : null,
                //BuildingYear = house.BuildingYear.GetValueOrDefault(),
                UsedYear = house.UsedYear.GetValueOrDefault(),
                UsedYearSpecified = house.UsedYear.HasValue,
                //TotalWear = Decimal.Round(house.TotalWear.GetValueOrDefault(), 2),
                FloorCount = house.FloorCount,
                //Energy = !string.IsNullOrEmpty(house.EnergyCategoryCode) || !string.IsNullOrEmpty(house.EnergyCategoryGuid) || house.EnergyDate.HasValue ?
                //    this.GetEnergyToCreate(house) : null,
                OKTMO = !house.OktmoCode.IsEmpty() ? new OKTMORefType
                {
                    code = house.OktmoCode
                } : null,
                OlsonTZ = new nsiRef
                {
                    Code = house.OlsonTZCode,
                    GUID = house.OlsonTZGuid
                },
                //ResidentialSquare = Decimal.Round(house.ResidentialSquare, 2),
                CulturalHeritage = house.CulturalHeritage,
                CulturalHeritageSpecified = true
            };
        }

        /// <summary>
        /// Получить BasicCharacteristicts для раздела Update
        /// </summary>
        /// <param name="house">Дом</param>
        /// <returns>BasicCharacteristicts для раздела Update</returns>
        private HouseBasicUpdateUOType GetBasicCharacteristictsToUpdate(RisHouse house)
        {
            var items = this.GetBasicCharacteristictsItem(house.CadastralNumber);

            return new HouseBasicUpdateUOType
            {
                Items = items,
                ItemsElementName = this.GetBasicCharacteristictsItemElementName(items),
                FIASHouseGuid = house.FiasHouseGuid,
                TotalSquare = decimal.Round(house.TotalSquare.GetValueOrDefault(), 1),
                TotalSquareSpecified = house.TotalSquare != null,
                State = (!house.StateCode.IsEmpty() && !house.StateGuid.IsEmpty()) ? new nsiRef
                {
                    Code = house.StateCode,
                    GUID = house.StateGuid
                } : null,
                //ProjectSeries = house.ProjectSeries,
                //ProjectType = !string.IsNullOrEmpty(house.ProjectTypeCode) || !string.IsNullOrEmpty(house.ProjectTypeGuid) ?
                //        new nsiRef
                //        {
                //            Code = house.ProjectTypeCode,
                //            GUID = house.ProjectTypeGuid
                //        } : null,
                //BuildingYear = house.BuildingYear.GetValueOrDefault(),
                UsedYear = house.UsedYear.GetValueOrDefault(),
                UsedYearSpecified = house.UsedYear != null,
                //TotalWear = Decimal.Round(house.TotalWear.GetValueOrDefault(), 2),
                FloorCount = house.FloorCount,
                OKTMO = !house.OktmoCode.IsEmpty() ? new OKTMORefType
                {
                    code = house.OktmoCode
                } : null,
                //Energy = !string.IsNullOrEmpty(house.EnergyCategoryCode) || !string.IsNullOrEmpty(house.EnergyCategoryGuid) || house.EnergyDate.HasValue ?
                //    this.GetEnergyToUpdate(house) : null,
                OlsonTZ = new nsiRef
                {
                    Code = house.OlsonTZCode,
                    GUID = house.OlsonTZGuid
                },
                // ResidentialSquare = Decimal.Round(house.ResidentialSquare, 2),
                CulturalHeritage = house.CulturalHeritage,
                CulturalHeritageSpecified = true
            };
        }
        /// <summary>
        /// Получить блок ItemElementName в разделе BasicCharacteristicts
        /// </summary>
        /// <param name="Items">Блок Items в разделе BasicCharacteristicts</param>
        /// <returns>Блок ItemElementName в разделе BasicCharacteristicts</returns>
        private ItemsChoiceType5[] GetBasicCharacteristictsItemElementName(object[] Items)
        {
            if (Items.Length == 0)
            {
                throw new Exception("Недопустимое количество элементов в блоке Items");
            }
            return (Items[0] is bool ? new[] { ItemsChoiceType5.No_RSO_GKN_EGRP_Registered } : new[] { ItemsChoiceType5.CadastralNumber });
        }

        //private HouseBasicUOTypeEnergy GetEnergyToCreate(RisHouse house)
        //{
        //    return new HouseBasicUOTypeEnergy
        //    {
        //        EnergyDate = house.EnergyDate.GetValueOrDefault(),
        //        EnergyCategory =
        //                   !string.IsNullOrEmpty(house.EnergyCategoryCode) || !string.IsNullOrEmpty(house.EnergyCategoryGuid)
        //                       ? new nsiRef { Code = house.EnergyCategoryCode, GUID = house.EnergyCategoryGuid }
        //                       : null
        //    };
        //}

        //private HouseBasicUpdateUOTypeEnergy GetEnergyToUpdate(RisHouse house)
        //{
        //    return new HouseBasicUpdateUOTypeEnergy
        //    {
        //        EnergyDate = house.EnergyDate.GetValueOrDefault(),
        //        EnergyCategory =
        //                   !string.IsNullOrEmpty(house.EnergyCategoryCode) || !string.IsNullOrEmpty(house.EnergyCategoryGuid)
        //                       ? new nsiRef { Code = house.EnergyCategoryCode, GUID = house.EnergyCategoryGuid }
        //                       : null
        //    };
        //}
    }
}
