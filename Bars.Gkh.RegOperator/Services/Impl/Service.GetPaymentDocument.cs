namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Services.DataContracts.GetChargePeriods;
    using ServiceStack.Common;

    public partial class Service
    {
        public PaymentDocumentResponse GetPaymentDocument(string accountNumber, string periodId, bool saveSnapshot)
        {
            var service = this.Container.Resolve<IPaymentDocumentService>();
            var accountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();
            var snapshotDomain = this.Container.ResolveDomain<PaymentDocumentSnapshot>();

            try
            {
                using (this.Container.Using(service, accountDomain, periodDomain))
                {
                    var account = accountDomain.GetAll()
                        .Where(x=> x.State.StartState).FirstOrDefault(x => x.PersonalAccountNum == accountNumber);
                    var period = periodDomain.GetAll().FirstOrDefault(x => x.Id == periodId.ToLong());

                    if (account == null)
                    {
                        return PaymentDocumentResponse.Fail("Не найден лицевой счет на счете регоператора");
                    }

                    if (period == null)
                    {
                        return PaymentDocumentResponse.Fail("Период не найден");
                    }

                    if (!period.IsClosed)
                    {
                        return
                            PaymentDocumentResponse.Fail(
                                "Период не закрыт. По данному периоду печать платежного документа невозможна.");
                    }

                    var snap = snapshotDomain.GetAll()
                        .Where(x => x.PaymentDocumentType == Enums.PaymentDocumentType.Individual && x.HolderType == PaymentDocumentData.AccountHolderType
                        && x.HolderId == account.Id && x.Period == period).FirstOrDefault();

                    if (snap == null)
                    {
                        snap = new PaymentDocumentSnapshot
                        {
                            DocNumber = "",
                            TotalCharge = 0
                        };
                    }

                    var stream = service.GetPaymentDocument(account, period, saveSnapshot, false);
                    stream.Seek(0, SeekOrigin.Begin);
                    var bytes = stream.ToBytes();

                    return PaymentDocumentResponse.Result(Convert.ToBase64String(bytes), snap.DocNumber, snap.TotalCharge);
                }
            }
            catch (Exception)
            {
                return PaymentDocumentResponse.Fail("Произошла непредвиденная ошибка.");
            }
        }
    }
}
