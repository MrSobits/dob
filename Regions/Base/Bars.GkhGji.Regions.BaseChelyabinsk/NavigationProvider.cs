namespace Bars.GkhGji.Regions.BaseChelyabinsk
{
    using Bars.B4;
    using Bars.Gkh.TextValues;

    public class NavigationProvider : INavigationProvider
    {
        public IMenuItemText MenuItemText { get; set; }

        public void Init(MenuItem root)
        {
            var dicts = root.Add("Справочники").Add("ГЖИ");

            var item = dicts.Get(this.MenuItemText.GetText("Зональные жилищные инспекции"));
            if (item != null)
            {
                item.Caption = "Отделы";
            }

            root.Add("Административная комиссия")
                .Add("Управление задачами")
                .Add("Задачи по обращениям", "reminderappealcits")
                .AddRequiredPermission("GkhGji.ManagementTask.ReminderAppealCits.View")
                .WithIcon("reminderHead");

            //root.Add("Административная комиссия")
            //    .Add("Реестр уведомлений")
            //    .Add("Реестр уведомлений о смене способа управления МКД", "mkdchangenotification")
            //    .AddRequiredPermission("GkhGji.MkdChangeNotification.View")
            //    .WithIcon("mkdChangeNotificationHead");

            //root.Add("Административная комиссия")
            //    .Add("Лицензирование")
            //    .Add("Аннулирование и предоставление сведений о лицензиях", "licenseaction")
            //    .AddRequiredPermission("GkhGji.License.LicenseAction.View")
            //    .WithIcon("mkdChangeNotificationHead");

            root.Add("Административная комиссия")
                .Add("Межведомственное взаимодействие")
                .Add("Прием информации о сертификате ключа проверки электронной подписи", "certinfo")
                .AddRequiredPermission("GkhGji.SMEV.CertInfo.View")
                .WithIcon("mkdChangeNotificationHead");

            root.Add("Административная комиссия")
          .Add("Управление задачами")
          .Add("Календарь задач", "taskcalendar")
          .AddRequiredPermission("GkhGji.TaskCalendar.TaskCalendarPanel.View")
          .WithIcon("clnd1");
            root.Add("Административная комиссия").Add("Документы").Add("Протоколы на рассмоторение", "simpleprotocol").AddRequiredPermission("GkhGji.DocumentsGji.Protocol197.View").WithIcon("documentGji");
            root.Add("Административная комиссия").Add("Документы").Add("Реестр СЭД", "eds").AddRequiredPermission("GkhGji.EDS.EDSRegistry.View").WithIcon("baseInsCheck");
            root.Add("Административная комиссия").Add("Документы").Add("Запросы Комиссии", "requestregistry").AddRequiredPermission("GkhGji.EDS.EDSRegistry.View").WithIcon("baseInsCheck");
            root.Add("Административная комиссия").Add("Документы").Add("Реестр документов для подписи", "edssign").AddRequiredPermission("GkhGji.EDS.EDSRegistrySign.View").WithIcon("competition");
            root.Add("Административная комиссия").Add("Документы").Add("Досудебное обжалование", "complaints").AddRequiredPermission("GkhGji.SMEV.SMEVComplaints.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Запросы по досудебному обжалованию", "complaintsrequest").AddRequiredPermission("GkhGji.SMEV.SMEVComplaints.View");
        }

        public string Key => MainNavigationInfo.MenuName;

        public string Description => MainNavigationInfo.MenuDescription;
    }
}