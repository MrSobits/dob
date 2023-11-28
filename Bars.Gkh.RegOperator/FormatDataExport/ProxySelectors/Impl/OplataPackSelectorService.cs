namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Селектор Пачка оплат ЖКУ
    /// </summary>
    public class OplataPackSelectorService : BaseProxySelectorService<OplataPackProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, OplataPackProxy> GetCache()
        {
            var bankDocumentImportRepository = this.Container.ResolveRepository<BankDocumentImport>();
            var importedPaymentRepository = this.Container.ResolveRepository<ImportedPayment>();
            var bankAccountStatementRepository = this.Container.ResolveRepository<BankAccountStatement>();

            using (this.Container.Using(bankDocumentImportRepository, importedPaymentRepository, bankAccountStatementRepository))
            {
                var bankDocumentImports = bankDocumentImportRepository.GetAll()
                    .Where(x => x.PaymentConfirmationState != PaymentConfirmationState.Deleted)
                    .Select(
                        x => new OplataPackProxy
                        {
                            Id = x.ExportId,
                            Date = x.DocumentDate ?? x.ImportDate,
                            Number = x.DocumentNumber != null && x.DocumentNumber != string.Empty 
                                ? x.DocumentNumber 
                                : x.LogImport.File.Name,
                            Sum = x.ImportedSum ?? 0m,
                            Count = importedPaymentRepository.GetAll().Count(y => y.BankDocumentImport.Id == x.Id),
                            TransferGuid = x.TransferGuid,
                            PayerName = x.PaymentAgentCode ?? x.PaymentAgentName,
                            Destination = x.ImportType
                        })
                    .AsEnumerable();

                var bankAccountStatements = bankAccountStatementRepository.GetAll()
                    .Where(x => x.DistributeState != DistributionState.Deleted)
                    .Select(
                        x => new OplataPackProxy
                        {
                            Id = x.ExportId,
                            Date = x.DocumentDate,
                            Number = x.DocumentNum != null && x.DocumentNum != string.Empty ? x.DocumentNum : x.RecipientAccountNum,
                            Sum = x.Sum,
                            Count = 1,
                            TransferGuid = x.TransferGuid,
                            PayerName = x.PayerAccountNum ?? x.PayerName,
                            Destination = x.PaymentDetails
                        })
                    .AsEnumerable();

                return bankDocumentImports
                    .Union(bankAccountStatements)
                    .ToDictionary(x => x.Id);
            }
        }
    }
}