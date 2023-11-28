namespace Bars.Gkh.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Контроллер правил перехода по статусам
    /// </summary>
    public class StateTransferRuleController : B4.Modules.States.Controller.StateTransferRuleController
    {
        public ILocalAdminRoleService LocalAdminRoleService { get; set; }
        public IDomainService<LocalAdminRoleRelations> LocalAdminRoleRelationsDomain { get; set; }
        public IDomainService<LocalAdminStatefulEntity> LocalAdminStatefulEntityDomain { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }

        /// <inheritdoc />
        public override ActionResult List(StoreLoadParams baseParams)
        {
            var query = this.DomainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.RuleId,
                    x.StateTransfer.TypeId,
                    NewState = x.StateTransfer.NewState.Name,
                    CurrentState = x.StateTransfer.CurrentState.Name,
                    Role = x.StateTransfer.Role.Name,
                    RoleId = x.StateTransfer.Role.Id
                });

            if (this.LocalAdminRoleService.IsThisUserLocalAdmin())
            {
                var userRole = this.GkhUserManager.GetActiveOperatorRoles().First();
                var childRoleList = this.LocalAdminRoleService.GetChildRoleList(userRole.Id)
                    .Select(x => x.Id)
                    .ToList();

                var allowTypeList = this.LocalAdminStatefulEntityDomain.GetAll()
                    .Where(x => childRoleList.Contains(x.Role.Id))
                    .Select(x => x.TypeId)
                    .ToList();

                query = query.Where(x => childRoleList.Contains(x.RoleId))
                    .Where(x => allowTypeList.Contains(x.TypeId));
            }

            var ruleDict = this.Container.ResolveAll<IRuleChangeStatus>()
                .ToDictionary(x => x.Id, x => new
                {
                    x.Name,
                    x.Description
                });

            var typeDict = this.Container.Resolve<IStateProvider>().GetAllInfo()
                .ToDictionary(x => x.TypeId, x => x.Name);

            return query
                .WhereContainsBulked(x => x.RuleId, ruleDict.Keys)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    RuleName = ruleDict.Get(x.RuleId)?.Name,
                    RuleDescription = ruleDict.Get(x.RuleId)?.Description,
                    x.TypeId,
                    TypeName = typeDict.Get(x.TypeId),
                    x.NewState,
                    x.CurrentState,
                    x.Role
                })
                .ToListDataResult(baseParams)
                .ToJsonResult();
        }

        /// <inheritdoc />
        public override ActionResult Get(long id)
        {
            if (this.LocalAdminRoleService.IsThisUserLocalAdmin())
            {
                var userRole = this.GkhUserManager.GetActiveOperatorRoles().First();
                var childRoleList = this.LocalAdminRoleService.GetChildRoleList(userRole.Id)
                    .Select(x => x.Id)
                    .ToList();

                var allowTypeList = this.LocalAdminStatefulEntityDomain.GetAll()
                    .Where(x => childRoleList.Contains(x.Role.Id))
                    .Select(x => x.TypeId)
                    .ToList();

                var isAllow = this.DomainService.GetAll()
                    .Where(x => childRoleList.Contains(x.StateTransfer.Role.Id))
                    .Any(x => allowTypeList.Contains(x.StateTransfer.TypeId));

                if (!isAllow)
                {
                    throw new AuthorizationException("У текущей роли нет доступа");
                }
            }

            return base.Get(id);
        }
    }
}
