﻿namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Адреса объектов жилищного фонда к договору ресурсоснабжения
    /// </summary>
    public class DrsoAddressExportableEntity : BaseExportableEntity<PublicServiceOrgContractRealObj>
    {
        /// <inheritdoc />
        public override string Code => "DRSOADDRESS";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Rso;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DrsoAddressProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.DrsoId.ToStr(),
                        x.DomId.ToStr(),
                        x.PremisesId.ToStr(),
                        x.RoomId.ToStr(),
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Договор ресурсоснабжения",
                "Дом",
                "Номер  помещения (квартиры)",
                "Номер комнаты"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DRSO",
                "DOM",
                "PREMISES",
                "ROOM"
            };
        }
    }
}