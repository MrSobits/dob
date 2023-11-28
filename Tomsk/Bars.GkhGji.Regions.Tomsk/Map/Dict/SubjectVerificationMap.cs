namespace Bars.GkhGji.Regions.Tomsk.Map.Dict
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    /// <summary>
    /// Предмет проверки
    /// </summary>
    class SubjectVerificationMap : BaseEntityMap<SubjectVerification>
    {
        public SubjectVerificationMap()
            : base("GJI_DICT_VERIF_SUBJECT")
        {
            Map(x => x.Name, "NAME").Length(300).Not.Nullable();
            Map(x => x.Code, "CODE").Length(300);
        }
    }
}
