namespace Bars.Gkh.Navigation
{
    using B4;

    public class ContragentMenuProvider : INavigationProvider
    {
        public static string Key = "Contragent";

        string INavigationProvider.Key
        {
            get
            {
                return "Contragent";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки контрагента";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "contragentedit/{0}/edit").AddRequiredPermission("Gkh.Orgs.Contragent.View").WithIcon("icon-shield-rainbow");
            root.Add("Данные для плана проверок", "contragentedit/{0}/auditpurpose").AddRequiredPermission("Gkh.Orgs.Contragent.Register.AuditPurpose.View");
            root.Add("Муниципальные образования", "contragentedit/{0}/municipality").AddRequiredPermission("Gkh.Orgs.Contragent.Register.Municipality.View");
            root.Add("Контакты", "contragentedit/{0}/contact").AddRequiredPermission("Gkh.Orgs.Contragent.Register.Contact.View").WithIcon("icon-user");
            root.Add("Обслуживающие банки", "contragentedit/{0}/bank").AddRequiredPermission("Gkh.Orgs.Contragent.Register.Bank.View").WithIcon("icon-money-dollar");
            root.Add("Падежи", "contragentedit/{0}/casesedit").AddRequiredPermission("Gkh.Orgs.Contragent.Register.CasesEdit.View").WithIcon("icon-text-lowercase");
        }
    }
}