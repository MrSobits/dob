﻿namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisNotificationAddressee"
    /// </summary>
    public class RisNotificationAddresseeMap : BaseRisEntityMap<RisNotificationAddressee>
    {
        public RisNotificationAddresseeMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisNotificationAddressee", "RIS_NOTIFICATION_ADDRESSEE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.House, "House").Column("HOUSE_ID").Fetch();
            this.Reference(x => x.Notification, "Notification").Column("NOTIFICATION_ID").Fetch();
        }
    }
}
