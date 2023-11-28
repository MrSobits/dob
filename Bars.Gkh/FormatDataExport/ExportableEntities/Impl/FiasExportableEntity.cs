namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    /// <summary>
    /// ФИАС
    /// </summary>
    [Obsolete("Устарел", true)]
    public class FiasExportableEntity : BaseExportableEntity<FiasAddress>
    {
        /// <inheritdoc />
        public override string Code => "FIAS";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.EntityRepository.GetAll()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.PlaceGuidId,
                        x.StreetGuidId,
                        x.AddressGuid,
                        x.PostCode,
                        x.AddressName,
                        x.PlaceAddressName,
                        x.PlaceName,
                        x.StreetName,
                        x.House,
                        x.Housing,
                        x.Building,
                        x.Letter,
                        x.Flat
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>()
            {
                "Уникальный код",
                "Классификационный код ФИАС",
                "Код улицы КЛАДР",
                "Код адреса ФИАС",
                "Почтовый индекс",
                "Полный адрес",
                "Адрес до Населенного пункта",
                "Наименование населенного пункта",
                "Наименование улицы",
                "Дом",
                "Корпус",
                "Строение",
                "Литера",
                "Квартира"
            };
        }
    }
}