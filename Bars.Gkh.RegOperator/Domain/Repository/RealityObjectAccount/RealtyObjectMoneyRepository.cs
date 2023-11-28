namespace Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Text;
    using B4;
    using B4.Utils;
    using B4.Utils.Annotations;

    using Entities;
    using Entities.ValueObjects;
    using Entities.Wallet;
    using Extensions;
    using FastMember;
    using Gkh.Entities;
    using NHibernate.Linq;

    public class RealtyObjectMoneyRepository : IRealtyObjectMoneyRepository
    {
        private readonly IDomainService<RealityObjectTransfer> transferRepo;
        private readonly IDomainService<RealityObjectPaymentAccount> accountRepo;
        private readonly IDomainService<RealityObjectLoan> loanDomain;

        public RealtyObjectMoneyRepository(
            IDomainService<RealityObjectTransfer> transferRepo, 
            IDomainService<RealityObjectPaymentAccount> accountRepo, 
            IDomainService<RealityObjectLoan> loanDomain)
        {
            this.transferRepo = transferRepo;
            this.accountRepo = accountRepo;
            this.loanDomain = loanDomain;
        }

        /// <inheritdoc />
        public IQueryable<TransferDto> GetDebtTransfers(IQueryable<RealityObjectPaymentAccount> paymentAccounts)
        {
            List<string> guids = this.GetWalletGuids(paymentAccounts);
            var ids = paymentAccounts.Select(x => x.Id).ToArray();

            return this.transferRepo.GetAll()
                .Where(x => ids.Contains(x.Owner.Id))
                .Where(x => (guids.Contains(x.TargetGuid) && (x.Operation.CanceledOperation == null || x.IsLoan))
                            || (guids.Contains(x.SourceGuid) && (x.Operation.CanceledOperation != null || x.IsReturnLoan)))
                .TranslateToDto();
        }

        /// <inheritdoc />
        public IQueryable<TransferDto> GetCreditTransfers(IQueryable<RealityObjectPaymentAccount> paymentAccounts)
        {
            List<string> guids = this.GetWalletGuids(paymentAccounts);
            var ids = paymentAccounts.Select(x => x.Id).ToArray();

            return this.transferRepo.GetAll()
                .Where(x => ids.Contains(x.Owner.Id))
                .Where(x => !x.IsLoan && !x.IsReturnLoan)
                .Where(x => (guids.Contains(x.SourceGuid) && x.Operation.CanceledOperation == null)
                            || (guids.Contains(x.TargetGuid) && x.Operation.CanceledOperation != null ))
                .TranslateToDto();
        }

        /// <inheritdoc />
        public IQueryable<RealityObjectLoanDto> GetRealityObjectLoanSum(IQueryable<RealityObject> realityObjects, bool anyOperations = true)
        {
            return this.loanDomain.GetAll()
                .WhereIf(anyOperations, x => x.Operations.Any())
                .Where(x => realityObjects.Any(r => r == x.LoanTaker.RealityObject))
                .Select(
                    x => new RealityObjectLoanDto
                    {
                        RealityObjectId = x.LoanTaker.RealityObject.Id,
                        LoanSum = x.LoanSum,
                        LoanReturnedSum = x.LoanReturnedSum
                    });
        }

        /// <inheritdoc />
        public IQueryable<RealtyObjectBalanceDto> GetRealtyBalances(IQueryable<RealityObject> realityObjects)
        {
            return this.accountRepo.GetAll()
                .Where(x => realityObjects.Any(r => r == x.RealityObject))
                .Select(x => new RealtyObjectBalanceDto
                {
                    RealtyObjectId = x.RealityObject.Id,
                    Credit = x.CreditTotal,
                    Debt = x.DebtTotal,
                    Loan = x.Loan,
                    Lock = x.MoneyLocked
                });
        }

        /// <inheritdoc />
        public IQueryable<TransferDto> GetSubsidyTransfers(IQueryable<RealityObjectPaymentAccount> paymentAccounts)
        {
            return this.transferRepo.GetAll()
                .Fetch(x => x.Operation)
                .Where(z => paymentAccounts.Any(x =>
                    x.FundSubsidyWallet.WalletGuid == z.SourceGuid
                    || x.FundSubsidyWallet.WalletGuid == z.TargetGuid
                    || x.RegionalSubsidyWallet.WalletGuid == z.SourceGuid
                    || x.RegionalSubsidyWallet.WalletGuid == z.TargetGuid
                    || x.StimulateSubsidyWallet.WalletGuid == z.SourceGuid
                    || x.StimulateSubsidyWallet.WalletGuid == z.TargetGuid
                    || x.TargetSubsidyWallet.WalletGuid == z.SourceGuid
                    || x.TargetSubsidyWallet.WalletGuid == z.TargetGuid))
                .TranslateToDto();
        }

        /// <inheritdoc />
        public IQueryable<TransferDto> GetSubsidyTransfers(RealityObjectPaymentAccount paymentAccount)
        {
            ArgumentChecker.NotNull(paymentAccount, "paymentAccount");

            var walletGuids = paymentAccount.GetSubsidyWalletGuids();

            return this.transferRepo.GetAll()
                .Where(x => walletGuids.Contains(x.SourceGuid) || walletGuids.Contains(x.TargetGuid))
                .Where(x =>
                        (!x.Reason.ToLower().Contains("займ")) || (x.Operation != null && !x.Operation.Reason.ToLower().Contains("займ")))
                .Where(x => // фильтруем "Оплата акта" и "Отмена оплаты акта"
                    ((!x.Reason.ToLower().Contains("оплата акта")) || (x.Operation != null && !x.Operation.Reason.ToLower().Contains("оплата акта"))) &&
                    ((!x.Reason.ToLower().Contains("оплаты акта")) || (x.Operation != null && !x.Operation.Reason.ToLower().Contains("оплаты акта"))))
                .Select(x => new TransferDto
                {
                    Id = x.Id,
                    Reason = x.Reason ?? x.Originator.Reason,
                    OperationDate = x.PaymentDate,
                    Amount = walletGuids.Contains(x.TargetGuid) ? x.Amount : -1 * x.Amount
                });
        }

        private List<string> GetWalletGuids(IQueryable<RealityObjectPaymentAccount> paymentAccounts)
        {
            var walletProps =
                typeof (RealityObjectPaymentAccount).GetProperties()
                    .Where(x => x.PropertyType == typeof (Wallet))
                    .Select(x => x.Name);

            var query = new StringBuilder("new(");

            var counter = 0;
            foreach (var walletProp in walletProps)
            {
                counter++;

                query.Append(walletProp).Append(".WalletGuid as ").Append("w_" + walletProp);
                if (counter != walletProps.Count())
                {
                    query.Append(", ");
                }
            }

            query.Append(")");

            var guids = new List<string>();

            var queryString = query.ToString().TrimEnd(',');
            var result = paymentAccounts.Select(queryString);

            foreach (var item in result)
            {
                var accessor = ObjectAccessor.Create(item);

                foreach (var walletProp in walletProps)
                {
                    guids.Add(accessor["w_" + walletProp].ToStr());
                }
            }

            return guids;
        }
    }
}