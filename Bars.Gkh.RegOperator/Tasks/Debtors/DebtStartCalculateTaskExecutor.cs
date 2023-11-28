namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.Logging;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;

    using Bars.B4.Application;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Выполняет задачу рассчета даты начала долга и суммы лога
    /// </summary>
    public class DebtStartCalculateTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public string ExecutorCode { get; private set; }

        public IDataResult Execute(BaseParams baseParams,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var container = ApplicationContext.Current.Container;
                ct.ThrowIfCancellationRequested();
            
            var lawsuitService = container.Resolve<ILawsuitOwnerInfoService>();
            var lawsuitOwnerService = container.Resolve<IDomainService<ClaimWorkAccountDetail>>();
            
            indicator.Report(null, 5, "Начат расчет");
            var docId = baseParams.Params.GetAs<long>("docId");
            lawsuitService.CalcLegalWithReferenceCalc(docId);

            return new BaseDataResult();
        }
    }
}