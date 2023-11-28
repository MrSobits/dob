namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="PremisesProxy"/>
    /// </summary>
    public class PremisesSelectorService : BaseProxySelectorService<PremisesProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, PremisesProxy> GetCache()
        {
            var roomRepository = this.Container.ResolveRepository<Room>();

            using (this.Container.Using(roomRepository))
            {
                var roomDict = this.FilterService
                    .FilterByRealityObject(roomRepository.GetAll(), x => x.RealityObject)
                    .WhereNotEmptyString(x => x.ChamberNum.Trim())
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.RoomNum,
                        x.Area,
                        x.LivingArea
                    })
                    .AsEnumerable()
                    .GroupBy(x => $"{x.RoId}|{x.RoomNum}", x => new { x.Area, x.LivingArea })
                    .ToDictionary(x => x.Key,
                        x => new
                        {
                            Area = x.SafeSum(y => y.Area),
                            LivingArea = x.SafeSum(y => y.LivingArea ?? 0)
                        });

                return this.FilterService
                    .FilterByRealityObject(roomRepository.GetAll(), x => x.RealityObject)
                    .WhereEmptyString(x => x.ChamberNum)
                    .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        EntranceId = (long?) x.Entrance.Id,
                        x.RoomNum,
                        x.Type,
                        x.OwnershipType,
                        TypeHouse = (TypeHouse?) x.RealityObject.TypeHouse,
                        x.RealityObject.CadastralHouseNumber,
                        x.Floor,
                        x.Area,
                        x.LivingArea,
                        x.RoomsCount
                    })
                    .AsEnumerable()
                    .Select(x => new PremisesProxy
                    {
                        Id = x.Id,
                        RealityObjectId = x.RoId,
                        EntranceId = x.EntranceId,
                        RoomNum = x.RoomNum,
                        Type = x.Type,
                        IsCommonProperty = x.Type == RoomType.NonLiving ? 1 : (int?)null,
                        TypeHouse = x.TypeHouse,

                        CadastralHouseNumber = x.CadastralHouseNumber,
                        Floor = x.Floor,
                        RoomsCount = x.RoomsCount,

                        Area = roomDict.Get($"{x.RoId}|{x.RoomNum}")?.Area ?? x.Area,
                        LivingArea = roomDict.Get($"{x.RoId}|{x.RoomNum}")?.LivingArea ?? x.LivingArea,
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}