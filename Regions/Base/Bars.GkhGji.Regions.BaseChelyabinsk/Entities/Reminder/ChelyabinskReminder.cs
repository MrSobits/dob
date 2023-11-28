namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Reminder
{
    using AppealCits;
    using GkhGji.Entities;

    public class ChelyabinskReminder : Reminder
    {
        /// <summary>
        /// Мероприятие комиссии
        /// </summary>
        public virtual AppealCitsExecutant AppealCitsExecutant { get; set; }
    }
}