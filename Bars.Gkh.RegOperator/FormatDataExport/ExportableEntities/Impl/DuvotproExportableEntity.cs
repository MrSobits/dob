namespace Bars.Gkh.RegOperator.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Протоколы голосования к договору управления
    /// </summary>
    [Obsolete("СА: Не выгружаем", true)]
    public class DuVotproExportableEntity : BaseExportableEntity<RealityObjectDecisionProtocol>
    {
        /// <inheritdoc />
        public override string Code => "DUVOTPRO";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Oms;

        /// <summary>
        /// Репозиторий договор управления - МКД
        /// </summary>
        public IRepository<ManOrgContractRealityObject> ManOrgContractRealityObjectRepository { get; set; }

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var contractIds = this.ProxySelectorFactory.GetSelector<DuProxy>().ProxyListCache.Keys;

            var contractList = this.FilterService
                .FilterByRealityObject(this.ManOrgContractRealityObjectRepository.GetAll(),
                    x => x.RealityObject)
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners)
                .WhereContainsBulked(x => x.ManOrgContract.Id, contractIds, 5000)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    ContractId = x.ManOrgContract.Id
                })
                .ToList();

            var id = 1L;

            return this.GetFiltred(x => x.RealityObject)
                .Select(x => new
                {
                    x.Id,
                    RoId = x.RealityObject.Id
                })
                .AsEnumerable()
                .Join(contractList,
                    i => i.RoId,
                    o => o.RoId,
                    (p, d) => new ExportableRow(id++,
                        new List<string>
                        {
                            p.Id.ToStr(),
                            d.ContractId.ToStr()
                        }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => this.GetAllFieldIds();

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Протокол голосования",
                "Договор управления"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "PROTOCOLOSS",
                "DU"
            };
        }
    }
}