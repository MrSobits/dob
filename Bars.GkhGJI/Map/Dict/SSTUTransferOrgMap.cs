namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Маппинг для "Идентификаторы ССТУ"
    /// </summary>
    public class SSTUTransferOrgMap : BaseEntityMap<SSTUTransferOrg>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public SSTUTransferOrgMap() :
                base("Адресаты перенаправления обращений", "GJI_DICT_SSTU_TRANSFER")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(500).NotNull();
            this.Property(x => x.Guid, "Гуид").Column("SSTU_GUID").Length(300);

        }
    }
}