namespace Bars.B4.Modules.Analytics.Reports.Permissions
{
    public class ReportsPermissionMap : PermissionMap
    {
        public ReportsPermissionMap()
        {
            Namespace("B4.Analytics.Reports", "Конструктор отчетов");
            Permission("B4.Analytics.Reports.ReportPanel.View", "Панель отчетов - Просмотр");
            Permission("B4.Analytics.Reports.ReportCustoms.View", "Замена шаблонов - Просмотр");
            Permission("B4.Analytics.Reports.StoredReports.View", "Справочник отчетов - Просмотр");
            Permission("B4.Analytics.Reports.StoredReports.Connection", "Подключение к базе");
        }
    }
}