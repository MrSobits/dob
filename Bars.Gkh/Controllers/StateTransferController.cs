namespace Bars.Gkh.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Контроллер перехода по статусам
    /// </summary>
    public class StateTransferController : B4.Modules.States.Controller.StateTransferController
    {
        public ILocalAdminRoleService LocalAdminRoleService { get; set; }
        public IDomainService<LocalAdminRoleRelations> LocalAdminRoleRelationsDomain { get; set; }
        public IDomainService<LocalAdminStatefulEntity> LocalAdminStatefulEntityDomain { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }

        /// <summary>
        /// Отфильтрованный список переходов по статусам
        /// </summary>
        /// <param name="storeParams"></param>
        /// <returns></returns>
        public ActionResult ListFiltredTransfers(StoreLoadParams storeParams)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var stateTransferDomain = this.Container.Resolve<IDomainService<StateTransfer>>();

            using (this.Container.Using(stateProvider, stateTransferDomain))
            {
                var userRole = this.GkhUserManager.GetActiveOperatorRoles().First();
                var childRoleList = this.LocalAdminRoleService.GetChildRoleList(userRole.Id)
                    .Select(x => x.Id)
                    .ToList();

                var allowTypeDict = this.LocalAdminStatefulEntityDomain.GetAll()
                    .Where(x => childRoleList.Contains(x.Role.Id))
                    .Select(x => new
                    {
                        RoleId = x.Role.Id,
                        x.TypeId
                    })
                    .AsEnumerable()
                    .Join(stateProvider.GetAllInfo(),
                        o => o.TypeId,
                        i => i.TypeId,
                        (o, i) => new
                        {
                            o.RoleId,
                            o.TypeId,
                            i.Name
                        })
                    .GroupBy(x => x.RoleId, x => new
                    {
                        x.TypeId,
                        x.Name
                    })
                    .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.TypeId, y => y.Name));

                var query = stateTransferDomain.GetAll();

                if (this.LocalAdminRoleService.IsThisUserLocalAdmin())
                {
                    var data = query.WhereContainsBulked(x => x.Role.Id, childRoleList)
                        .WhereContainsBulked(x => x.Role.Id, allowTypeDict.Keys)
                        .Select(x => new
                        {
                            x.Id,
                            RoleId = x.Role.Id,
                            RoleName = x.Role.Name,
                            x.TypeId,
                            CurrentStateName = x.CurrentState.Name,
                            NewStateName = x.NewState.Name
                        })
                        .AsEnumerable()
                        .Where(x => allowTypeDict[x.RoleId].ContainsKey(x.TypeId))
                        .Select(x => new
                        {
                            x.Id,
                            Name = $"{x.RoleName}, {allowTypeDict[x.RoleId][x.TypeId]}: {x.CurrentStateName} -> {x.NewStateName}",
                            Role = x.RoleName,
                            TypeName = allowTypeDict[x.RoleId][x.TypeId],
                            x.TypeId
                        })
                        .OrderBy(x => x.Name)
                        .ToList();

                    return new JsonListResult(data, data.Count);
                }
                else
                {
                    var dictTypes = stateProvider.GetAllInfo()
                        .Select(x => new { x.TypeId, x.Name })
                        .GroupBy(x => x.TypeId)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Name).FirstOrDefault());

                    var data = query
                        .Select(x => new
                        {
                            x.Id,
                            RoleId = x.Role.Id,
                            RoleName = x.Role.Name,
                            x.TypeId,
                            CurrentStateName = x.CurrentState.Name,
                            NewStateName = x.NewState.Name
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.Id,
                            Name =
                            x.RoleName + ", " + (dictTypes.ContainsKey(x.TypeId) ? dictTypes[x.TypeId] : "") +
                            ": " + x.CurrentStateName + " -> " + x.NewStateName,
                            Role = x.RoleName,
                            TypeName = dictTypes.ContainsKey(x.TypeId) ? dictTypes[x.TypeId] : "",
                            x.TypeId,
                        })
                        .OrderBy(x => x.Name)
                        .ToList();

                    //09.11.2017 окно правил перехода статусов все такое же убогое
                    return new JsonListResult(data, data.Count);
                }
            }
        }
    }
}
