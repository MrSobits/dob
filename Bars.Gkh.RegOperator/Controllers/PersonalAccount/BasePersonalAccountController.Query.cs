namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Web.Mvc;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ActionFilters;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Converter = Bars.B4.Modules.Analytics.Utils.Converter;

    internal partial class BasePersonalAccountController
    {
        /// <summary>
        /// Формирование дерева папок по лицевым счетам при Предпросмотре документов на оплату
        /// </summary>
        /// <param name="params">параметры клиента</param>
        /// <returns></returns>
        [Compress]
        public ActionResult GetPaymentDocumentsHierarchyPreview(BaseParams @params)
        {
            var accountIds =
                Converter.ToLongArray(@params.Params.GetAs("accountIds", ignoreCase: true, defaultValue: string.Empty));

            var helper = new PaymentDocPreviewHelper(this.Container);
            
            return new JsonNetResult(helper.GetPreview(accountIds, @params));
        }

        /// <summary>
        /// Пункт меню Предпросмотр документов на оплату из реестра лицевых счетов
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult GetPaymentDocument(BaseParams baseParams)
        {
            var persAccRepo = this.Container.ResolveRepository<BasePersonalAccount>();
            var chargePeriodDomain = this.Container.ResolveDomain<ChargePeriod>();

            try
            {
                var accId = baseParams.Params.GetAs<long>("accountId", ignoreCase: true);
                var periodId = baseParams.Params.GetAs<long>("periodId", ignoreCase: true);
                var mode = baseParams.Params.GetAs("mode", AccountRegistryMode.Common);

                var account = persAccRepo.Get(accId);
                var period = chargePeriodDomain.Get(periodId);

                // не сохраняем слепки данных при печати квитанций (3 параметр false)
                var stream = this.PaymentDocService.GetPaymentDocument(account, period, false, mode == AccountRegistryMode.PayDoc);
                stream.Position = 0;

                this.Response.AddHeader("Content-Disposition", "inline; filename=report.pdf");
                this.Response.AddHeader("Content-Length", stream.Length.ToString());
                return this.File(stream, "application/pdf");
            }
            finally
            {
                this.Container.Release(persAccRepo);
                this.Container.Release(chargePeriodDomain);
            }
        }
    }
}