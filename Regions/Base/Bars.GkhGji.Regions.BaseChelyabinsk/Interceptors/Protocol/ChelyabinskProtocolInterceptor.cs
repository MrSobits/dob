namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.Protocol
{
    using System.Linq;
    using System;
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Authentification;
    using Bars.B4.Modules.States;

    public class ChelyabinskProtocolInterceptor : ProtocolServiceInterceptor<ChelyabinskProtocol>
    {
        public IGkhUserManager UserManager { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<ChelyabinskProtocol> service, ChelyabinskProtocol entity)
        {
            var longTextService = this.Container.Resolve<IDomainService<ProtocolLongText>>();

            try
            {
                var longIds = longTextService.GetAll().Where(x => x.Protocol.Id == entity.Id).ToList();

                foreach (var id in longIds)
                {
                    longTextService.Delete(id);
                }

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                this.Container.Release(longTextService);
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ChelyabinskProtocol> service, ChelyabinskProtocol entity)
        {
            if (entity.DateOfProceedings.HasValue)
            {
                entity.DocumentDate = entity.DateOfProceedings.Value;
            }
            if (entity.FiasPlaceAddress != null)
            {
                Utils.SaveFiasAddress(this.Container, entity.FiasPlaceAddress);
             //   return base.BeforeUpdateAction(service, entity);
            }
            var servStateProvider = this.Container.Resolve<IStateProvider>();
            var ComissionMeetingDomain = this.Container.Resolve<IDomainService<ComissionMeeting>>();
            var ZonalInspectionInspectorDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
                return Failure("Создание документов комиссии доступно только членам комиссии");

            var inspector = thisOperator.Inspector;
            var zonal = ZonalInspectionInspectorDomain.GetAll()
                .FirstOrDefault(x => x.Inspector == inspector)?.ZonalInspection;
            if (zonal == null)
            {
                return Failure("Создание документов комиссии доступно только членам комиссии");
            }
            //проставляем комиссию, либо создаем новую
            if (entity.ComissionMeeting == null && entity.DateOfProceedings.HasValue)
            {
                var comission = ComissionMeetingDomain.GetAll()
                    .FirstOrDefault(x => x.CommissionDate == entity.DateOfProceedings && x.ZonalInspection == zonal);
                if (comission != null)
                {
                    entity.ComissionMeeting = comission;
                }
                else
                {

                    comission = new ComissionMeeting
                    {
                        CommissionDate = entity.DateOfProceedings.Value,
                        ComissionName = $"Комиссия от {entity.DateOfProceedings.Value.ToString("dd.MM.yyyy")}",
                        Description = "Создано из протокола",
                        ZonalInspection = zonal,
                        CommissionNumber = "б/н"
                    };
                    servStateProvider.SetDefaultState(comission);
                    ComissionMeetingDomain.Save(comission);
                    entity.ComissionMeeting = comission;
                }
            }
            else if (entity.ComissionMeeting != null)
            {
                entity.DateOfProceedings = entity.ComissionMeeting.CommissionDate;
            }

        
            if (entity.PlaceOffense == PlaceOffense.AddressUr)
            {
                if (entity.Inspection.Contragent != null)
                {
                    entity.FiasPlaceAddress = entity.Inspection.Contragent.FiasJuridicalAddress;
                }
            }
            else if (entity.PlaceOffense == PlaceOffense.PlaceFact)
            {
                var data = Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                     .Where(x => x.Document.Id == entity.Id)
                     .Select(x => new
                     {
                         x.InspectionViolation.RealityObject
                     }).FirstOrDefault();
                if (data != null)
                {
                    entity.FiasPlaceAddress = data.RealityObject.FiasAddress;
                }

            }
            if (entity.FiasPlaceAddress != null) 
            {
                var violationHouse = entity.FiasPlaceAddress.House;
                char[] violationHouseChar = violationHouse.ToCharArray();
                for (int i = 0; i < violationHouse.Length; i++)
                {
                    if (!char.IsDigit(violationHouseChar[i]))
                    {
                        violationHouse = violationHouse.TrimEnd(violationHouseChar[i]);
                    }
                }
                //автоподстановка суд.участка по адресу места совершения правонарушения
                Int32 violationHouseInt = 0;
                try
                {
                    violationHouseInt = int.Parse(violationHouse);
                }
                catch
                {
                    
                }
                Utils.SaveFiasAddress(this.Container, entity.FiasPlaceAddress);
                int checkedMath = 0;
                long? jurId = null;
                Container.Resolve<IDomainService<JurInstitutionRealObj>>().GetAll()
                .Where(x => x.RealityObject.FiasAddress.StreetGuidId == entity.FiasPlaceAddress.StreetGuidId)
                .OrderBy(x => x.RealityObject.FiasAddress.House)
                .Select(x => new
                {
                    x.RealityObject.FiasAddress.House,
                    x.JurInstitution.Id
                })
                .ToList().ForEach(x =>
                {
                    var house = x.House;
                    char[] houseChar = house.ToCharArray();
                    for (int i = 0; i < houseChar.Length; i++)
                    {
                        if (!char.IsDigit(houseChar[i]))
                        {
                            house = house.Remove(i, 1);

                        }
                    }
                    var houseCharInt = int.Parse(house);
                    var module = Math.Abs(houseCharInt - violationHouseInt);
                    if (houseCharInt != violationHouseInt)
                    {
                        if (checkedMath == 0)
                        {
                            checkedMath = module;
                        }
                        else
                        {
                            if (checkedMath > module)
                            {
                                checkedMath = module;
                                jurId = x.Id;
                            }
                        }
                    }
                    else
                    {
                        jurId = x.Id;
                    }
                });
                if (jurId.HasValue)
                    entity.JudSector = new JurInstitution { Id = jurId.Value };
            }
               
            return base.BeforeUpdateAction(service, entity);
        }
    }
}
