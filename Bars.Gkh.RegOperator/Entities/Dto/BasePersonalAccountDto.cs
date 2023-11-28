namespace Bars.Gkh.RegOperator.Entities.Dto
{
    using System;

    using AutoMapper;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Хранимый DTO для <see cref="BasePersonalAccount"/>
    /// </summary>
    public class BasePersonalAccountDto : PersistentObject, IStatefulEntity
    {
        /// <summary>
        /// Идентификатор помещения
        /// </summary>
        public virtual long RoomId { get; set; }

        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public virtual long RoId { get; set; }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public virtual long OwnerId { get; set; }

        /// <summary>
        /// Категория льгот
        /// </summary>
        public virtual long? PrivilegedCategoryId { get; set; }

        /// <summary>
        /// Адрес дома
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Мунинципальный район (id)
        /// </summary>
        public virtual long MuId { get; set; }

        /// <summary>
        /// Муниципальное образование (id)
        /// </summary>
        public virtual long? SettleId { get; set; }

        /// <summary>
        /// Мунинципальный район
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string Settlement { get; set; }

        /// <summary>
        /// Адрес помещения
        /// </summary>
        public virtual string RoomAddress { get; set; }

        /// <summary>
        /// Номер помещения
        /// </summary>
        public virtual string RoomNum { get; set; }

        /// <summary>
        /// Номер комнаты
        /// </summary>
        public virtual string ChamberNum { get; set; }

        /// <summary>
        /// Имя владельца
        /// </summary>
        public virtual string AccountOwner { get; set; }

        /// <summary>
        /// Тип владельца
        /// </summary>
        public virtual PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// Площадь помещенния
        /// </summary>
        public virtual decimal Area { get; set; }

        /// <summary>
        /// Площадь МКД
        /// </summary>
        public virtual decimal? AreaMkd { get; set; }

        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public virtual string PersonalAccountNum { get; set; }

        /// <summary>
        /// Доля собственности
        /// </summary>
        public virtual decimal AreaShare { get; set; }

        /// <summary>
        /// Дата открытия
        /// </summary>
        public virtual DateTime OpenDate { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public virtual DateTime? CloseDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Номер ЛС во внешней системе
        /// </summary>
        public virtual string PersAccNumExternalSystems { get; set; }

        /// <summary>
        /// Имеет только одно помещение со статусом Открыто
        /// </summary>
        public virtual bool HasOnlyOneRoomWithOpenState { get; set; }

        /// <summary>
        /// Способ формирования фонда кр на текущий момент
        /// </summary>
        public virtual CrFundFormationType AccountFormationVariant { get; set; }

        /// <summary>
        /// Обнвить dto, согласно изменений ЛС
        /// </summary>
        /// <param name="account">Лицевой счёт</param>
        /// <returns>Объект</returns>
        public virtual BasePersonalAccountDto UpdateMe(BasePersonalAccount account)
        {
            this.MergeData(account);
            return this;
        }

        /// <summary>
        /// Создать dto, согласно ЛС
        /// </summary>
        /// <param name="account">Лицевой счёт</param>
        /// <returns>Объект</returns>
        public static BasePersonalAccountDto FromAccount(BasePersonalAccount account)
        {
            var accountDto = new BasePersonalAccountDto { Id = account.Id };
            return accountDto.MergeData(account);
        }

        private BasePersonalAccountDto MergeData(BasePersonalAccount account)
        {
            return Mapper.Map(account, this);
        }

        /// <summary>
        /// Инициализировать <see cref="Mapper"/>
        /// </summary>
        public static void ConfigureAutoMapper()
        {
            Mapper.CreateMap<BasePersonalAccount, BasePersonalAccountDto>()
                .ForMember(x => x.PersonalAccountNum, y => y.MapFrom(x => x.PersonalAccountNum))
                .ForMember(x => x.PersAccNumExternalSystems, y => y.MapFrom(x => x.PersAccNumExternalSystems))
                .ForMember(x => x.OpenDate, y => y.MapFrom(x => x.OpenDate))
                .ForMember(x => x.CloseDate, y => y.MapFrom(x => x.CloseDate != DateTime.MinValue ? x.CloseDate : (DateTime?)null))
                .ForMember(x => x.Area, y => y.MapFrom(x => x.AreaShare))
                .ForMember(x => x.RoomId, y => y.MapFrom(x => x.Room.Id))
                .ForMember(x => x.OwnerId, y => y.MapFrom(x => x.AccountOwner.Id))
                .ForMember(x => x.State, y => y.MapFrom(x => x.State))
                .ForMember(x => x.AccountOwner, y => y.MapFrom(x => x.AccountOwner.Name))
                .ForMember(x => x.OwnerType, y => y.MapFrom(x => x.AccountOwner.OwnerType))
                .ForMember(
                    x => x.PrivilegedCategoryId,
                    y => y.MapFrom(x => x.AccountOwner.PrivilegedCategory != null ? x.AccountOwner.PrivilegedCategory.Id : (long?)null))

                .ForMember(x => x.HasOnlyOneRoomWithOpenState, y => y.MapFrom(x => x.State.StartState && x.AccountOwner.ActiveAccountsCount == 1))

                .ForMember(x => x.RoId, y => y.MapFrom(x => x.Room.RealityObject.Id))
                .ForMember(
                    x => x.RoomAddress,
                    y => y.MapFrom(
                            x =>
                            x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum + (x.Room.ChamberNum != string.Empty && x.Room.ChamberNum != null ? ", ком. " + x.Room.ChamberNum : string.Empty)))

                .ForMember(x => x.RoomNum, y => y.MapFrom(x => x.Room.RoomNum))
                .ForMember(x => x.ChamberNum, y => y.MapFrom(x => x.Room.ChamberNum))
                .ForMember(x => x.Area, y => y.MapFrom(x => x.Room.Area))
                .ForMember(x => x.Address, y => y.MapFrom(x => x.Room.RealityObject.Address))
                .ForMember(x => x.AreaMkd, y => y.MapFrom(x => x.Room.RealityObject.AreaMkd))
                .ForMember(
                    x => x.AccountFormationVariant,
                    y => y.MapFrom(x => (x.Room.RealityObject.AccountFormationVariant ?? CrFundFormationType.Unknown)))

                .ForMember(x => x.Municipality, y => y.MapFrom(x => x.Room.RealityObject.Municipality.Name))
                .ForMember(x => x.MuId, y => y.MapFrom(x => x.Room.RealityObject.Municipality.Id))

                .ForMember(
                    x => x.Settlement,
                    y => y.MapFrom(x => x.Room.RealityObject.MoSettlement != null ? x.Room.RealityObject.MoSettlement.Name : null))

                .ForMember(
                    x => x.SettleId,
                    y => y.MapFrom(x => x.Room.RealityObject.MoSettlement != null ? x.Room.RealityObject.MoSettlement.Id : (long?)null));
        }
    }
}