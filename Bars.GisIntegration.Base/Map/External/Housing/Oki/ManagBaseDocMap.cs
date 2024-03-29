﻿namespace Bars.GisIntegration.Base.Map.External.Housing.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.ManagBaseDoc
    /// </summary>
    public class ManagBaseDocMap : BaseEntityMap<ManagBaseDoc>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ManagBaseDocMap() :
            base("MANAG_BASE_DOC")
        {
            //Устанавливаем схему РИС
            this.Schema("HOUSING");

            this.Id(x => x.Id, m =>
            {
                m.Column("MANAG_BASE_DOC_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.OkiObject, "OKI_OBJECT_ID");
            this.References(x => x.Attachment, "ATTACHMENT_ID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
