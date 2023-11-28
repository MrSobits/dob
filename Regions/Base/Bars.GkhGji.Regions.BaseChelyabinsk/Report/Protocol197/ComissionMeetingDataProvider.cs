namespace Bars.GkhGji.Regions.BaseChelyabinsk.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;

    using Castle.Windsor;

    /// <summary>
    /// Расширенный поставщик данных для документа ПИР
    /// </summary>
    public class ComissionMeetingDataProvider : BaseCollectionDataProvider<CourtMeetingAllProxy>
    {
        public ComissionMeetingDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        public override string Name
        {
            get { return "ComissionMeetingData"; }
        }

        public override string Description
        {
            get { return this.Name; }
        }

        public string ClaimworkId { get; set; }
        public string LawsuitId { get; set; }
        public string OwnerId { get; set; }
        public string FIO { get; set; }
        public string Pos { get; set; }
        public bool Solidary { get; set; }
        public long[] DocumentIds { get; set; }

        protected override IQueryable<CourtMeetingAllProxy> GetDataInternal(BaseParams baseParams)
        {
            var records = new List<CourtMeetingAllProxy>();
            records.Add(new CourtMeetingAllProxy
            {
                ClwId = ClaimworkId.ToString(),
                LawId = LawsuitId.ToString(),
                RloiId = OwnerId.ToString(),
                Solidary = Solidary,
                FIO =FIO,
                DocumentIds = DocumentIds
            });

            return records.AsQueryable();         
        }
    }

    public class CourtMeetingAllProxy
    {
        public long [] DocumentIds { get; set; }
        public string ClwId { get; set; } //Claimwork ("CLW_CLAIMWORK")
        public string LawId { get; set; } //Lawsuit ("CLW_LAWSUIT")
        public string RloiId { get; set; } //Lawsuit Owner ("REGOP_LAWSUIT_OWNER_INFO")
        public bool Solidary { get; set; } //Lawsuit Owner ("REGOP_LAWSUIT_OWNER_INFO")
        public string FIO { get; set; } //Lawsuit Owner ("REGOP_LAWSUIT_OWNER_INFO")
    }
}