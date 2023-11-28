namespace Bars.Gkh.RegOperator.Domain.Repository.Wallets
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;

    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;

    using Entities.ValueObjects;
    using Entities.Wallet;
    using Repositories;

    /// <summary>
    /// Репозиторий для Кошелек оплат
    /// </summary>
    public class WalletRepository : BaseDomainRepository<Wallet>, IWalletRepository
    {
        private readonly ISessionProvider sessions;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sessions">Провайдер сессий NHibernate</param>
        public WalletRepository(ISessionProvider sessions)
        {
            this.sessions = sessions;
        }

        /// <summary>
        /// Получить кошельки, к которых уходили деньги
        /// </summary>
        /// <param name="transfers">Трансферы</param>
        /// <returns>Запрос с кошельками</returns>
        public IQueryable<IWallet> GetSourceWalletsFor(IEnumerable<Transfer> transfers)
        {
            var sourcesGuids = transfers.Select(x => x.SourceGuid).ToList();
            return this.DomainService.GetAll().Where(w => sourcesGuids.Contains(w.TransferGuid));
        }

        /// <summary>
        /// Получить кошельки, на которые уходили деньги
        /// </summary>
        /// <param name="transfers">Трансферы</param>
        /// <returns>Запрос с кошельками</returns>
        public IQueryable<IWallet> GetTargetWalletsFor(IEnumerable<Transfer> transfers)
        {
            var targetsGuids = transfers.Select(x => x.TargetGuid).ToList();
            return this.DomainService.GetAll().Where(w => targetsGuids.Contains(w.TransferGuid));
        }

        /// <summary>
        /// Обновить баланс кошельков
        /// </summary>
        /// <param name="walletGuids">Идентификаторы кошельков</param>
        /// <param name="realityObject">Учитывать ли копии трансферов</param>
        public void UpdateWalletBalance(List<string> walletGuids, bool realityObject = false)
        {
            walletGuids = walletGuids.Distinct().ToList();

            using (var session = this.sessions.OpenStatelessSession())
            using (var tr = session.BeginTransaction())
            {
                var tableName = realityObject ? "regop_reality_transfer" : " regop_transfer";
                
                session.CreateSQLQuery(string.Format(@"UPDATE regop_wallet w2
                            SET balance = round(wallet.balance,2),
                                has_new_ops = :false
                            FROM
	                            (
		                            SELECT
			                            w. ID,
				                            ((select COALESCE (sum(amount*target_coef), 0) from {0} t where t.target_guid = w.wallet_guid and t.is_affect)-
			                                (select COALESCE (sum(t.amount*target_coef), 0) from {0} t where t.source_guid = w.wallet_guid and t.is_affect) -
		                                (select COALESCE (sum(amount), 0) from regop_money_lock where is_active and wallet_id = w.id)) as balance
		                            FROM
			                            regop_wallet w
                                    where w.wallet_guid in (:ids)
	                            ) AS wallet
                            WHERE
	                            wallet.ID = w2.ID", tableName))
                      .SetParameterList("ids", walletGuids)
                      .SetBoolean("false", false)
                      .ExecuteUpdate();

                try
                {
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
        }
    }
}
