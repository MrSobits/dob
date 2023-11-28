namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor;

    using Castle.Windsor;

    using Entities.Owner;

    using Gkh.Modules.ClaimWork.Entities;

    using Entities;
    using Entities.ValueObjects;

    using Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using NHibernate.Linq;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.Debtors;

    using Sobits.RosReg.Tasks.ExtractParse;

    /// <inheritdoc />
    /// <summary>
    /// Сервис "Собственник в исковом заявлении"
    /// </summary>
    [SuppressMessage("ReSharper", "CommentTypo")]
    public class LawsuitOwnerInfoService : ILawsuitOwnerInfoService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<LawsuitOwnerInfo> LawsuitOwnerInfoDomain { get; set; }

        public IDomainService<PersonalAccountOwnerInformation> PersonalAccountOwnerInformationDomain { get; set; }

        public IDomainService<Lawsuit> LawsuitDomain { get; set; }

        public IDomainService<ClaimWorkAccountDetail> AccountDetailDomain { get; set; }

        public IDebtPeriodCalcService DebtPeriodCalcService { get; set; }

        private IDomainService<ChargePeriod> PeriodDomain { get; set; }

        private IDomainService<PaysizeRecord> paysizeRepository { get; set; }

        private IDomainService<LawsuitReferenceCalculation> RefCalcDomain { get; set; }

        public struct SumsBySummary
        {
            public  decimal baseTarifDebtSum;
            public decimal decisionDebtSum;
            public decimal penaltyDebtSum;

            public SumsBySummary(decimal baseTarifDebtSum, decimal decisionDebtSum, decimal penaltyDebtSum)
            {
                this.baseTarifDebtSum = baseTarifDebtSum;
                this.decisionDebtSum = decisionDebtSum;
                this.penaltyDebtSum = penaltyDebtSum;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Получение по Лс собственников для искового заявления
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Собственники в исковом заявлении</returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var lawsuitId = baseParams.Params.GetAs<long>("Lawsuit");
            DateTime documentDate = baseParams.Params.GetAs("DocumentDate", DateTime.MinValue);

            LoadParam loadParam = baseParams.GetLoadParam();

            var claimWorkIds = this.LawsuitDomain.GetAll()
                .Where(x => x.Id == lawsuitId)
                .Select(x => x.ClaimWork.Id);

            var accountDetailQuery = this.AccountDetailDomain.GetAll()
                .Where(x => claimWorkIds.Contains(x.ClaimWork.Id));

            var data = this.PersonalAccountOwnerInformationDomain.GetAll()
                .Where(x => accountDetailQuery.Any(y => y.PersonalAccount.Id == x.BasePersonalAccount.Id))
                .Where(x => !x.EndDate.HasValue || x.EndDate.Value.Date >= documentDate.Date)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Owner,
                        x.AreaShare,
                        x.EndDate
                    });

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }

        /// <inheritdoc />
        public IDataResult DebtCalculate(BaseParams baseParams)
        {
            var selectedIds = baseParams.Params.GetAs("ids", new long[0]);
            var lawsuitOwnerInfoList = this.LawsuitOwnerInfoDomain.GetAll()
                .WhereContains(x => x.Id, selectedIds)
                .ToList();

            if (lawsuitOwnerInfoList.IsEmpty())
            {
                return BaseDataResult.Error("Не найдена информация о переданных собственниках");
            }

            this.DebtPeriodCalcService.Calculate(lawsuitOwnerInfoList.Select(x => x.PersonalAccount.Id).Distinct());

            foreach (LawsuitOwnerInfo lawsuitOwnerInfo in lawsuitOwnerInfoList)
            {
                this.CalculateProcess(lawsuitOwnerInfo);
            }

            TransactionHelper.InsertInManyTransactions(this.Container, lawsuitOwnerInfoList, useStatelessSession: true);

            return new BaseDataResult(selectedIds);
        }

        public IDataResult DebtStartDateCalculate(BaseParams baseParams)
        {
            //Проверяем документ
            var docId = baseParams.Params.GetAs<long>("docId");
            if (docId == 0)
            {
                return BaseDataResult.Error("Не найдена информация о документе");
            }
            
            Lawsuit lawsuit = this.LawsuitDomain.Get(docId);
            
            List<ClaimWorkAccountDetail> claimWorkAccountDetail = this.AccountDetailDomain.GetAll()
               .Where(x => x.ClaimWork == lawsuit.ClaimWork)
               .ToList();

            if (claimWorkAccountDetail.Count <= 10)
                return this.CalcLegalWithReferenceCalc(lawsuit, claimWorkAccountDetail);
            var taskManager = this.Container.Resolve<ITaskManager>();
                    
            var Params = new DynamicDictionary();
            Params["docId"] = docId;
            taskManager.CreateTasks(new DebtStartCalculateTaskProvider(), new BaseParams() { Params = Params});
                
            return new BaseDataResult(
                new
                {
                    message =
                        @"Количество ЛС абонента больше 10. Задача поставлена на сервер расчетов."+
                        " После вылолнения задачи необходимо закрыть дело и открыть его снова."
                });

        }

        public IDataResult DebtStartDateCalculateChelyabinsk(BaseParams baseParams)
        {
            this.PeriodDomain = this.Container.ResolveDomain<ChargePeriod>();
            this.RefCalcDomain = this.Container.ResolveDomain<LawsuitReferenceCalculation>();

            //Проверяем документ
            var docId = baseParams.Params.GetAs<long>("docId");
            if (docId == 0)
            {
                return BaseDataResult.Error("Не найдена информация о документе");
            }

            //Удаляем старый рассчет
            this.ClearReferenceCalculation(docId);

            DocumentClw documentClw = this.Container.Resolve<IDomainService<DocumentClw>>().Get(docId);
            DateTime docDate = documentClw.DocumentDate ?? DateTime.MinValue;

            ClaimWorkAccountDetail claimWorkAccountDetail = this.AccountDetailDomain.GetAll()
                .FirstOrDefault(x => x.ClaimWork == documentClw.ClaimWork);

            ChargePeriod lastPeriod = this.GetLastReferenceCalculationPeriod(docDate);
            Lawsuit lawsuit = this.LawsuitDomain.Get(documentClw.Id);
            
            //если применен срок исковой давности то начинаем от него
            DateTime openDate = lawsuit.IsLimitationOfActions ? lawsuit.DateLimitationOfActions : claimWorkAccountDetail.PersonalAccount.OpenDate ;
            DateTime endDate = lastPeriod.EndDate ?? DateTime.MinValue;

            var chargePeriods = this.PeriodDomain.GetAll()
                .Where(x => x.EndDate >= openDate && x.EndDate <= endDate)
                .OrderBy(x => x.StartDate)
                .ToList();

            decimal roomArea = claimWorkAccountDetail.PersonalAccount.Room.Area;
            decimal areaShare = claimWorkAccountDetail.PersonalAccount.AreaShare;
            string accountNumber = claimWorkAccountDetail.PersonalAccount.PersonalAccountNum;
            long persAccId = claimWorkAccountDetail.PersonalAccount.Id;
                       
            Dictionary<ChargePeriod, decimal> paysizeByPeriod = GetTariffByPeriod(claimWorkAccountDetail, chargePeriods);
                       
            var entityLogLightDomain = this.Container.ResolveDomain<EntityLogLight>();

            var allHistory = entityLogLightDomain.GetAll()
                  .Where(x => x.ClassName == "BasePersonalAccount" && x.ParameterName == "area_share")
                  .Where(x => x.EntityId == claimWorkAccountDetail.PersonalAccount.Id)
                  .ToList();

            //исключаем более ранние изменения с той же датой начала действия
            var filteredHistory = allHistory
                .GroupBy(x => new { x.EntityId, x.DateActualChange })
                .ToDictionary(x => x.Key)
                .Select(x => x.Value.OrderByDescending(u => u.DateApplied).FirstOrDefault())
                .ToList()
                .GroupBy(x => x.EntityId)
                .ToDictionary(x => x.Key);


            //Lawsuit lawsuit = this.LawsuitDomain.Get(docId);

            var firstPeriod = true;
            var refCalc = new List<LawsuitReferenceCalculation>();

            foreach (ChargePeriod period in chargePeriods)
            {
                refCalc.Add(
                    new LawsuitReferenceCalculation
                    {
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 0,
                        PeriodId = period.Id,
                        AccountNumber = accountNumber,
                        AreaShare = areaShare,
                        Lawsuit = lawsuit,
                        BaseTariff = paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0,
                        PersonalAccountId = persAccId,
                        RoomArea = roomArea,
                        TariffCharged = firstPeriod
                            ? LawsuitOwnerInfoService.CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShare, openDate)
                            : LawsuitOwnerInfoService.CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShare)
                    });
                firstPeriod = false;
            }

            //Все трансферы
            var persAccAllTransfers = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>().GetAll()
                .Where(x => x.Owner.Id == claimWorkAccountDetail.PersonalAccount.Id)
                .Where(x => x.Operation.IsCancelled != true)
                .OrderByDescending(x => x.PaymentDate); //Обратный порядок для итерации

            //Оплаты
            var persAccAllChargeTransfers = persAccAllTransfers.Where(
                x => x.Reason == "Оплата по базовому тарифу").ToList();
            //    x.Reason == "Оплата пени" ||
            //    x.Reason == "Оплата по тарифу решения")


            //Отмены оплат
            var persAccAllReturnTransfers = persAccAllTransfers.Where(
                x => x.Reason == "Отмена оплаты по базовому тарифу" ||
                    // x.Reason == "Отмена оплаты по тарифу решения" ||
                    //        x.Reason == "Отмена оплаты пени" ||
                    x.Reason == "Возврат взносов на КР").ToList();
            var persAccChargeTransfers = new List<PersonalAccountPaymentTransfer>(persAccAllChargeTransfers);
            var persAccReturnTransfers = new List<PersonalAccountPaymentTransfer>(persAccAllReturnTransfers);

            //Обработка возвратов оплат
            for (int chargeIndex = persAccChargeTransfers.Count - 1; chargeIndex >= 0; chargeIndex--)
            {
                for (int returnIndex = persAccReturnTransfers.Count - 1; returnIndex >= 0; returnIndex--)
                {
                    if (persAccChargeTransfers[chargeIndex].Amount != persAccReturnTransfers[returnIndex].Amount ||
                        persAccChargeTransfers[chargeIndex].PaymentDate > persAccReturnTransfers[returnIndex].PaymentDate)
                    {
                        continue;
                    }

                    persAccChargeTransfers.RemoveAt(chargeIndex);
                    persAccReturnTransfers.RemoveAt(returnIndex);
                    //При нахождении отмены оплаты прекращаем поиск по списку возвратов
                    break;
                }
            }

            //Если остались неопределенные возвраты, вычитаем из оплат с начала
            if (persAccReturnTransfers.Count > 0)
            {
                //TODO:Обработка неопределенных возвратов
            }

            decimal totalCharged = refCalc.Sum(x => x.TariffCharged);
            decimal totalPayment = persAccChargeTransfers.Sum(x => x.Amount);
            decimal resultDebt = totalCharged - totalPayment;
            decimal currentDebt = 0;

            //Расставляем даты и оплаты по месяцам
            persAccChargeTransfers = persAccChargeTransfers.OrderBy(x => x.Id).ToList();

            var paymIndex = 0;
            foreach (LawsuitReferenceCalculation referenceCalculation in refCalc)
            {
                currentDebt = currentDebt + referenceCalculation.TariffCharged;
                if (currentDebt > 0 && paymIndex < persAccChargeTransfers.Count)
                {
                    referenceCalculation.PaymentDate = persAccChargeTransfers[paymIndex].PaymentDate.ToShortDateString();
                    referenceCalculation.TarifPayment = persAccChargeTransfers[paymIndex].Amount;
                    currentDebt = currentDebt - referenceCalculation.TarifPayment;
                    paymIndex++;
                }
            }
            //refCalc[chargeIndex].PaymentDate = persAccChargeTransfers[chargeIndex].PaymentDate.ToShortDateString();
            //refCalc[chargeIndex].TarifPayment = persAccChargeTransfers[chargeIndex].Amount;


            //Инвертируем оплаты для отображения
            decimal remainingPayment = totalPayment * -1;
            decimal totalDebt = 0;
            var debtStarted = false;
            DateTime debtStartDate = openDate;
            string message = totalPayment == 0 ? "Задолженность начинается с даты открытия лицевого счёта." : "";
                        
            foreach (LawsuitReferenceCalculation referenceCalculation in refCalc)
            {
                remainingPayment = remainingPayment + referenceCalculation.TariffCharged;
                totalDebt = totalDebt + referenceCalculation.TariffCharged - referenceCalculation.TarifPayment;
                referenceCalculation.TarifDebt = totalDebt;
                if (remainingPayment >= 0 && !debtStarted)
                {
                    //Рассчет даты начала задолженности
                    ChargePeriod lastPaymentPeriod = this.PeriodDomain.Get(referenceCalculation.PeriodId);
                    debtStartDate = LawsuitOwnerInfoService.CalculateDebtStartDate(
                        referenceCalculation.BaseTariff,
                        //      LawsuitOwnerInfoService.GetTariff(lastPaymentPeriod.StartDate),
                        roomArea,
                        areaShare,
                        remainingPayment,
                        lastPaymentPeriod.StartDate);

                    lawsuit.DebtStartDate = debtStartDate;
                    lawsuit.DebtEndDate = endDate;

                    if (message == "")
                    {
                        if (remainingPayment == 0)
                            message = $"Задолженность начинается с {debtStartDate.ToShortDateString()}";
                        else
                        {
                            switch (debtStartDate.Day)
                            {
                                //Разный текст примечания в зависимости от дня начала задолженности
                                case 1:
                                    message = $"За предыдущий месяц переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString()}.";
                                    break;
                                case 2:
                                    message = $"За {debtStartDate.AddDays(-1).ToShortDateString()} переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString(CultureInfo.InvariantCulture)}.";
                                    break;
                                default:
                                    message =
                                        $"С {lastPaymentPeriod.StartDate.ToShortDateString()} " +
                                        $"по {debtStartDate.AddDays(-1).ToShortDateString()} переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString(CultureInfo.InvariantCulture)}.";
                                    break;
                            }

                            message += $" С {debtStartDate.ToShortDateString()} задолженность {remainingPayment.ToString(CultureInfo.InvariantCulture)}.";
                            referenceCalculation.Description = message;
                            lawsuit.Description = message;
                        }
                    }

                    debtStarted = true;
                }

                this.RefCalcDomain.Save(referenceCalculation);
            }

            //TODO: Учет тарифа решения
            lawsuit.DebtBaseTariffSum = resultDebt;
            lawsuit.DebtDecisionTariffSum = 0;
            lawsuit.DebtSum = resultDebt;

            IRepository lawsuitRepository = this.Container.ResolveRepository(typeof(Lawsuit));
            lawsuitRepository.Evict(lawsuit);
            this.Container.Release(lawsuitRepository);
            //this.LawsuitDomain.Update(lawsuit);

            return new BaseDataResult(
                new
                {
                    lawsuit.DebtDecisionTariffSum,
                    lawsuit.DebtBaseTariffSum,
                    lawsuit.DebtSum,
                    dateStartDebt = debtStartDate,
                    message,
                    lawsuit.DebtCalcMethod,
                    lawsuit.DebtEndDate
                });
        }

        private IDataResult CalcIndividual(DocumentClw documentClw, ClaimWorkAccountDetail claimWorkAccountDetail)
        {

            this.PeriodDomain = this.Container.ResolveDomain<ChargePeriod>();
            this.RefCalcDomain = this.Container.ResolveDomain<LawsuitReferenceCalculation>();


            DateTime docDate = documentClw.DocumentDate ?? DateTime.MinValue;

            ChargePeriod lastPeriod = this.GetLastReferenceCalculationPeriod(docDate);
            Lawsuit lawsuit = this.LawsuitDomain.Get(documentClw.Id);
            
            //если применен срок исковой давности то начинаем от него
            DateTime openDate = lawsuit.IsLimitationOfActions ? lawsuit.DateLimitationOfActions : claimWorkAccountDetail.PersonalAccount.OpenDate ;
            DateTime endDate = lastPeriod.EndDate ?? DateTime.MinValue;

            var chargePeriods = this.PeriodDomain.GetAll()
                .Where(x => x.EndDate >= openDate && x.EndDate <= endDate)
                .OrderBy(x => x.StartDate)
                .ToList();

            decimal roomArea = claimWorkAccountDetail.PersonalAccount.Room.Area;
            decimal areaShare = claimWorkAccountDetail.PersonalAccount.AreaShare;
            string accountNumber = claimWorkAccountDetail.PersonalAccount.PersonalAccountNum;
            long persAccId = claimWorkAccountDetail.PersonalAccount.Id;
            

            //получаем размер взноса за кр
            var domain = Container.ResolveDomain<PaysizeRecord>();

            var paysize = domain.GetAll()
                .Where(x => x.Municipality != null)
                .Where(x => x.Value != null)
                .Where(x => x.Municipality == claimWorkAccountDetail.PersonalAccount.Room.RealityObject.Municipality)
                .ToList();

            Dictionary<ChargePeriod, decimal> paysizeByPeriod = new Dictionary<ChargePeriod, decimal>();

            var decisionDomain = Container.ResolveDomain<MonthlyFeeAmountDecHistory>();

            var decisions = decisionDomain.GetAll()
                .Where(x => x.Protocol.RealityObject == claimWorkAccountDetail.PersonalAccount.Room.RealityObject)
                .ToList();

            chargePeriods.ForEach(x =>
            {
                if (decisions != null)
                {
                    foreach (MonthlyFeeAmountDecHistory mfdh in decisions)
                    {
                        foreach (PeriodMonthlyFee pmf in mfdh.Decision)
                        {
                            if (pmf.Value > 0 && pmf.From == x.StartDate && (!pmf.To.HasValue || pmf.To.Value >= x.EndDate))
                            {
                                if (!paysizeByPeriod.ContainsKey(x))
                                {
                                    paysizeByPeriod.Add(x, pmf.Value);
                                }

                            }

                        }
                    }
                }

                if (!paysizeByPeriod.ContainsKey(x))
                {
                    foreach (PaysizeRecord psr in paysize)
                    {
                        if (psr.Paysize.DateStart <= x.StartDate && (psr.Paysize.DateEnd >= x.EndDate || psr.Paysize.DateEnd == null))
                        {
                            decimal? paySizeVal = psr.Value;
                            paysizeByPeriod.Add(x, paySizeVal.HasValue ? paySizeVal.Value : 0);
                        }
                    }


                }

            });

            var firstPeriod = true;
            var refCalc = new List<LawsuitReferenceCalculation>();

            var entityLogLightDomain = this.Container.ResolveDomain<EntityLogLight>();

            var allHistory = entityLogLightDomain.GetAll()
                  .Where(x => x.ClassName == "BasePersonalAccount" && x.ParameterName == "area_share")
                  .Where(x => x.EntityId == claimWorkAccountDetail.PersonalAccount.Id)
                  .ToList();

            //исключаем более ранние изменения с той же датой начала действия
            var filteredHistory = allHistory
                .GroupBy(x => new { x.EntityId, x.DateActualChange })
                .ToDictionary(x => x.Key)
                .Select(x => x.Value.OrderByDescending(u => u.DateApplied).FirstOrDefault())
                .ToList();
            // .GroupBy(x => x.EntityId);
            // .ToDictionary(x => x.Key);

            foreach (ChargePeriod period in chargePeriods)
            {
                var share = filteredHistory.Where(x => x.DateActualChange <= period.EndDate).OrderByDescending(x => x.DateApplied).ThenByDescending(x => x.Id).FirstOrDefault();
                var areaShareByPeriod = share != null ? Convert.ToDecimal(share.PropertyValue.Replace('.', ',')) : areaShare;
                refCalc.Add(
                    new LawsuitReferenceCalculation
                    {
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 0,
                        PeriodId = period.Id,
                        AccountNumber = accountNumber,
                        AreaShare = areaShareByPeriod,
                        Lawsuit = lawsuit,
                        BaseTariff = paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0,
                        //  BaseTariff = LawsuitOwnerInfoService.GetTariff(period.StartDate), пока переводим расчет на общие размеры взносов
                        PersonalAccountId = persAccId,
                        RoomArea = roomArea,
                        TariffCharged = firstPeriod
                            ? LawsuitOwnerInfoService.CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShareByPeriod, openDate)
                            : LawsuitOwnerInfoService.CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShareByPeriod)
                        //TariffCharged = firstPeriod расчет осуществляется по основным размерам взносов
                        //    ?LawsuitOwnerInfoService.CalculateMonthCharge(LawsuitOwnerInfoService.GetTariff(period.StartDate), roomArea, areaShareByPeriod, openDate)
                        //    :LawsuitOwnerInfoService.CalculateMonthCharge(LawsuitOwnerInfoService.GetTariff(period.StartDate), roomArea, areaShareByPeriod)
                    });
                firstPeriod = false;
            }

            //Все трансферы
            var persAccAllTransfers = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>().GetAll()
                .Where(x => x.Owner.Id == claimWorkAccountDetail.PersonalAccount.Id)
                .Where(x => x.Operation.IsCancelled != true)
                .OrderByDescending(x => x.PaymentDate); //Обратный порядок для итерации

            //Оплаты
            var persAccAllChargeTransfers = persAccAllTransfers
                .WhereIf(lawsuit.IsLimitationOfActions, x => x.PaymentDate > lawsuit.DateLimitationOfActions)
                .Where(
                    x => x.Reason == "Оплата по базовому тарифу" ||
                    //    x.Reason == "Оплата пени" ||
                        x.Reason == "Возврат взносов на КР" ||
                        x.Reason == "Оплата по тарифу решения")
                .Select(x => new
                {
                    x.Id,
                    Amount = x.Reason == "Возврат взносов на КР" ? x.Amount * (-1) : x.Amount,
                    x.ChargePeriod,
                    x.IsAffect,
                    x.IsInDirect,
                    x.IsLoan,
                    x.IsReturnLoan,
                    x.Owner,
                    x.PaymentDate,
                    x.Reason
                }).ToList();

            //Отмены оплат
            var persAccAllReturnTransfers = persAccAllTransfers
                .WhereIf(lawsuit.IsLimitationOfActions, x => x.PaymentDate > lawsuit.DateLimitationOfActions)
                .Where(
                x => x.Reason == "Отмена оплаты по базовому тарифу" ||
                    x.Reason == "Отмена оплаты по тарифу решения" ||
                //    x.Reason == "Отмена оплаты пени" ||
                    x.Reason == "Возврат взносов на КР").ToList();
            //var persAccChargeTransfers = new List<PersonalAccountPaymentTransfer>(persAccAllChargeTransfers);
            //var persAccReturnTransfers = new List<PersonalAccountPaymentTransfer>(persAccAllReturnTransfers);

            //Обработка возвратов оплат пока комментим, похоже что в persAccAllChargeTransfers и так приходят все неотмененные

            //for (int chargeIndex = persAccChargeTransfers.Count - 1; chargeIndex >= 0; chargeIndex--)
            //{
            //    for (int returnIndex = persAccReturnTransfers.Count - 1; returnIndex >= 0; returnIndex--)
            //    {
            //        if (persAccChargeTransfers[chargeIndex].Amount != persAccReturnTransfers[returnIndex].Amount ||
            //            persAccChargeTransfers[chargeIndex].PaymentDate > persAccReturnTransfers[returnIndex].PaymentDate)
            //        {
            //            continue;
            //        }

            //        persAccChargeTransfers.RemoveAt(chargeIndex);
            //        persAccReturnTransfers.RemoveAt(returnIndex);
            //        //При нахождении отмены оплаты прекращаем поиск по списку возвратов
            //        break;
            //    }
            //}

            //Если остались неопределенные возвраты, вычитаем из оплат с начала
            //if (persAccReturnTransfers.Count > 0)
            //{
            //    //TODO:Обработка неопределенных возвратов
            //}

            decimal totalCharged = refCalc.Sum(x => x.TariffCharged);
            decimal totalPayment = persAccAllChargeTransfers.Sum(x => x.Amount);
            decimal resultDebt = totalCharged - totalPayment;

            //Расставляем даты и оплаты по месяцам
            persAccAllChargeTransfers = persAccAllChargeTransfers.OrderBy(x => x.Id).ToList();
            for (var chargeIndex = 0;
                chargeIndex < persAccAllChargeTransfers.Count && chargeIndex < refCalc.Count;
                chargeIndex++)
            {
                refCalc[chargeIndex].PaymentDate = persAccAllChargeTransfers[chargeIndex].PaymentDate.ToShortDateString();
                refCalc[chargeIndex].TarifPayment = persAccAllChargeTransfers[chargeIndex].Amount;
                if (persAccAllChargeTransfers[chargeIndex].Reason == "Возврат взносов на КР")
                {
                    refCalc[chargeIndex].Description = "Возврат взносов на КР";
                }

            }

            //foreach (LawsuitReferenceCalculation referenceCalculation in refCalc)
            //{
            //    foreach (var retTr in persAccReturnTransfers)
            //    {
            //        if (referenceCalculation.PeriodId == retTr.ChargePeriod.Id)
            //        {
            //            referenceCalculation.TarifPayment += retTr.Amount;
            //            referenceCalculation.Description = "Возврат взносов";
            //        }
            //    }
            //}

            //Инвертируем оплаты для отображения
            decimal remainingPayment = totalPayment * -1;
            decimal totalDebt = 0;
            var debtStarted = false;
            DateTime debtStartDate = openDate;
            string message = totalPayment == 0 
                ? lawsuit.IsLimitationOfActions 
                    ? "Задолженность начинается с даты применения срока исковой давности"
                    : "Задолженность начинается с даты открытия лицевого счёта." 
                : "";
            foreach (LawsuitReferenceCalculation referenceCalculation in refCalc)
            {
                remainingPayment = remainingPayment + referenceCalculation.TariffCharged;
                totalDebt = totalDebt + referenceCalculation.TariffCharged - referenceCalculation.TarifPayment;
                referenceCalculation.TarifDebt = totalDebt;
                if (remainingPayment >= 0 && !debtStarted)
                {
                    //Рассчет даты начала задолженности
                    ChargePeriod lastPaymentPeriod = this.PeriodDomain.Get(referenceCalculation.PeriodId);
                    debtStartDate = LawsuitOwnerInfoService.CalculateDebtStartDate(
                        referenceCalculation.BaseTariff,
                        //  LawsuitOwnerInfoService.GetTariff(lastPaymentPeriod.StartDate),
                        roomArea,
                        referenceCalculation.AreaShare,
                        remainingPayment,
                        lastPaymentPeriod.StartDate);

                    // Фикс несостыковки дат открытия аккаунта и начала задолженности из-за округлений
                    // (дата начала может быть указана на 1 день раньше из-за десятых-сотых копеек долга,
                    // если такое происходит и итоговая дата раньше даты открытия - заменяем на дату открытия)
                    debtStartDate = debtStartDate < openDate ? openDate : debtStartDate;

                    lawsuit.DebtStartDate = debtStartDate;
                    lawsuit.DebtEndDate = endDate;

                    if (message == "")
                    {
                        if (remainingPayment == 0)
                            message = $"Задолженность начинается с {debtStartDate.ToShortDateString()}";
                        else
                        {
                            switch (debtStartDate.Day)
                            {
                                //Разный текст примечания в зависимости от дня начала задолженности
                                case 1:
                                    message = $"За предыдущий месяц переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString()}.";
                                    break;
                                case 2:
                                    message = $"За {debtStartDate.AddDays(-1).ToShortDateString()} переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString(CultureInfo.InvariantCulture)}.";
                                    break;
                                default:
                                    message =
                                        $"С {lastPaymentPeriod.StartDate.ToShortDateString()} " +
                                        $"по {debtStartDate.AddDays(-1).ToShortDateString()} переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString(CultureInfo.InvariantCulture)}.";
                                    break;
                            }

                            message += $" С {debtStartDate.ToShortDateString()} задолженность {remainingPayment.ToString(CultureInfo.InvariantCulture)}.";
                            referenceCalculation.Description = message;
                            lawsuit.Description = message;
                        }
                    }

                    debtStarted = true;
                }

                this.RefCalcDomain.Save(referenceCalculation);
            }

            //TODO: Учет тарифа решения
            lawsuit.DebtBaseTariffSum = resultDebt;
            lawsuit.DebtDecisionTariffSum = 0;
            lawsuit.DebtSum = resultDebt;

            IRepository lawsuitRepository = this.Container.ResolveRepository(typeof(Lawsuit));
            lawsuitRepository.Evict(lawsuit);
            this.Container.Release(lawsuitRepository);
            //this.LawsuitDomain.Update(lawsuit);

            return new BaseDataResult(
                new
                {
                    lawsuit.DebtDecisionTariffSum,
                    lawsuit.DebtBaseTariffSum,
                    lawsuit.DebtSum,
                    Description = lawsuit.Description,
                    dateStartDebt = debtStartDate,
                    message,
                    lawsuit.DebtCalcMethod,
                    lawsuit.DebtEndDate
                });
        }
        
        private IDataResult CalcLegal(DocumentClw documentClw, List<ClaimWorkAccountDetail> claimWorkAccountDetailList)
        {
            this.PeriodDomain = this.Container.ResolveDomain<ChargePeriod>();
            this.RefCalcDomain = this.Container.ResolveDomain<LawsuitReferenceCalculation>();

            DateTime docDate = documentClw.DocumentDate ?? DateTime.MinValue;
            
            var lastPeriod = docDate.Day < 25 ? this.PeriodDomain.GetAll().FirstOrDefault(x => x.StartDate < docDate.AddMonths(-2).Date && x.EndDate > docDate.AddMonths(-2).Date)
                                         : this.PeriodDomain.GetAll().FirstOrDefault(x => x.StartDate < docDate.AddMonths(-1).Date && (x.EndDate ?? DateTime.MaxValue) > docDate.AddMonths(-1).Date);
            

            Lawsuit lawsuit = this.LawsuitDomain.Get(documentClw.Id);
            lawsuit.DebtSum = 0;
            lawsuit.DebtBaseTariffSum = 0;
            lawsuit.DebtDecisionTariffSum = 0;
            lawsuit.PenaltyDebt = 0;
            //устанвливаем дату начала задолжености на конец последнего периода.Далее отодвигаем назад вовремени в зависимоcти от ситуации на ЛС должника
            //концом задолжености считаем конец последнего пеоиода 
            lawsuit.DebtStartDate = lastPeriod.EndDate ?? DateTime.Now; //подставил now для теста. так как на локальной базе не закрыт нужный период
            lawsuit.DebtEndDate = lastPeriod.EndDate ?? DateTime.Now; //подставил now для теста. так как на локальной базе не закрыт нужный период
            string message = "";

            foreach (var claimWorkAccountDetail in claimWorkAccountDetailList)
            {
                var summuries = claimWorkAccountDetail.PersonalAccount.Summaries.Where(x => x.Period.Id >= lastPeriod.Id).ToList();

                decimal saldoIn = summuries[0].SaldoIn;
                decimal baseTarifDebt = summuries[0].BaseTariffDebt;
                decimal decisionDebt = summuries[0].DecisionTariffDebt;
                decimal penaltyDebt = summuries[0].PenaltyDebt;
                decimal paymentsBaseTarif = 0;
                decimal paymentsDecision = 0;
                decimal paymentsPenalty = 0;
                decimal recalcBaseTarif = 0;
                decimal recalcDecision = 0;
                decimal recalcPenalty = 0;

                //не ясно как поступать с изменениями сальдо в последующих периодах. Может стоит их прибавить 
                decimal saldoChanges = (decimal)summuries[0].BalanceChanges.Sum(x => x.CurrentValue - x.NewValue);
                decimal decisionChanges = (decimal)summuries[0].DecisionTariffChange;
                decimal penaltyChanges = (decimal)summuries[0].PenaltyChange;

                summuries.ForEach(x => {
                        paymentsBaseTarif += x.TariffPayment;
                        paymentsDecision += x.TariffDecisionPayment;
                        paymentsPenalty += x.PenaltyPayment;
                        recalcBaseTarif += x.RecalcByBaseTariff;
                        recalcPenalty += x.RecalcByPenalty;
                        recalcDecision += x.RecalcByDecisionTariff; 
                    }); 

                //считаем задолженость
                decimal totalDebt = saldoIn + saldoChanges + (recalcBaseTarif  + recalcDecision) + (recalcPenalty + penaltyChanges) - (paymentsBaseTarif + paymentsDecision + paymentsPenalty);
                
                baseTarifDebt += recalcBaseTarif + saldoChanges - paymentsBaseTarif;
                decisionDebt += recalcDecision + decisionChanges - paymentsDecision;
                penaltyDebt += recalcPenalty + penaltyChanges - paymentsPenalty;

                if (totalDebt > 0) lawsuit.DebtSum += totalDebt;
                if (baseTarifDebt > 0) lawsuit.DebtBaseTariffSum += baseTarifDebt;
                if (decisionDebt > 0) lawsuit.DebtDecisionTariffSum += decisionDebt;
                if (penaltyDebt > 0) lawsuit.PenaltyDebt += penaltyDebt;
            
                //считаем дату начала задолжености

                var chargePeriods = this.PeriodDomain.GetAll()
                    .Where(x => x.EndDate >= claimWorkAccountDetail.PersonalAccount.OpenDate && x.EndDate <= (lastPeriod.EndDate ?? DateTime.Now)) //подставил now для теста. так как на локальной базе не закрыт нужный период)
                    .OrderBy(x => x.StartDate)
                    .ToList();

                Dictionary<ChargePeriod, decimal> tarifByPeriods = GetTariffByPeriod(claimWorkAccountDetail, chargePeriods);

                decimal totalPayment = claimWorkAccountDetail.PersonalAccount.Summaries.Sum(x => x.TariffPayment + x.TariffDecisionPayment);

                foreach (ChargePeriod item in chargePeriods)
                {
                    if (tarifByPeriods.ContainsKey(item))
                    {
                        decimal monthCharge = CalculateMonthCharge(tarifByPeriods[item], 
                            claimWorkAccountDetail.PersonalAccount.Room.Area, 
                            claimWorkAccountDetail.PersonalAccount.AreaShare, 
                            claimWorkAccountDetail.PersonalAccount.OpenDate);
                        decimal debtMonth = monthCharge - totalPayment;
                        if (debtMonth > 0)
                        {                            
                            DateTime debtStart = CalculateDebtStartDate(tarifByPeriods[item],
                                claimWorkAccountDetail.PersonalAccount.Room.Area,
                                claimWorkAccountDetail.PersonalAccount.AreaShare,
                                debtMonth,
                                item.StartDate);
                            if (lawsuit.DebtStartDate > debtStart)
                            {
                                lawsuit.DebtStartDate = debtStart;
                                message = $"Дата начала задолжености { debtStart.ToShortDateString()}";
                            }
                            break;
                        }
                        totalPayment -= monthCharge;
                    }
                }
                
            }

            IRepository lawsuitRepository = this.Container.ResolveRepository(typeof(Lawsuit));
            lawsuitRepository.Evict(lawsuit);
            this.Container.Release(lawsuitRepository);            

            return new BaseDataResult(
                new
                {
                    DebtDecisionTariffSum = lawsuit.DebtDecisionTariffSum ?? 0,
                    DebtBaseTariffSum = lawsuit.DebtBaseTariffSum ?? 0,
                    DebtSum = lawsuit.DebtSum ?? 0,
                    Description = lawsuit.Description ?? "",
                    lawsuit.DebtCalcMethod,
                    dateStartDebt = lawsuit.DebtStartDate,
                    lawsuit.DebtEndDate,
                    PenaltyDebt = lawsuit.PenaltyDebt ?? 0,
                    message
                }) ;

           
        }

        public IDataResult CalcLegalWithReferenceCalc(Lawsuit lawsuit, List<ClaimWorkAccountDetail> claimWorkAccountDetailList)
        {
            //Удаляем старый рассчет
            this.ClearReferenceCalculation(lawsuit.Id);
            bool hasCancelledCharges = false;
            this.PeriodDomain = this.Container.ResolveDomain<ChargePeriod>();
            this.RefCalcDomain = this.Container.ResolveDomain<LawsuitReferenceCalculation>();
           
            var paySizeDomain = Container.ResolveDomain<PaysizeRecord>();  
            
            DateTime docDate = lawsuit.DocumentDate ?? DateTime.MinValue;
            ChargePeriod lastPeriod = this.GetLastReferenceCalculationPeriod(docDate);

            lawsuit.DebtSum = 0;
            lawsuit.DebtBaseTariffSum = 0;
            lawsuit.DebtDecisionTariffSum = 0;
            lawsuit.PenaltyDebt = 0;
            
            //устанвливаем дату начала задолжености на конец последнего периода.Далее отодвигаем назад вовремени в зависимоcти от ситуации на ЛС должника
            //концом задолжености считаем конец последнего пеоиода 
            lawsuit.DebtStartDate = lastPeriod.EndDate ?? DateTime.Now; //подставил now для теста. так как на локальной базе не закрыт нужный период
            lawsuit.DebtEndDate = lastPeriod.EndDate ?? DateTime.Now; //подставил now для теста. так как на локальной базе не закрыт нужный период
            var message = "";

            foreach (var claimWorkAccountDetail in claimWorkAccountDetailList)
            {
                

                //если применен срок исковой давности то начинаем от него
                DateTime openDate = lawsuit.IsLimitationOfActions ? lawsuit.DateLimitationOfActions : claimWorkAccountDetail.PersonalAccount.OpenDate ;
                DateTime endDate = lastPeriod.EndDate ?? DateTime.MinValue;

                var chargePeriods = this.PeriodDomain.GetAll()
                    .Where(x => x.EndDate >= openDate && x.EndDate <= endDate)
                    .OrderBy(x => x.StartDate)
                    .ToList();

                decimal roomArea = claimWorkAccountDetail.PersonalAccount.Room.Area;
                decimal areaShare = claimWorkAccountDetail.PersonalAccount.AreaShare;
                string accountNumber = claimWorkAccountDetail.PersonalAccount.PersonalAccountNum;
                long persAccId = claimWorkAccountDetail.PersonalAccount.Id;
                

                //получаем размер взноса за кр
                

                var paysize = paySizeDomain.GetAll()
                    .Where(x => x.Municipality != null)
                    .Where(x => x.Value != null)
                    .Where(x => x.Municipality == claimWorkAccountDetail.PersonalAccount.Room.RealityObject.Municipality)
                    .ToList();

                Dictionary<ChargePeriod, decimal> paysizeByPeriod = new Dictionary<ChargePeriod, decimal>();

                var decisionDomain = Container.ResolveDomain<MonthlyFeeAmountDecHistory>();

                var decisions = decisionDomain.GetAll()
                    .Where(x => x.Protocol.RealityObject == claimWorkAccountDetail.PersonalAccount.Room.RealityObject)
                    .ToList();

                chargePeriods.ForEach(x =>
                {
                    if (decisions != null)
                    {
                        foreach (MonthlyFeeAmountDecHistory mfdh in decisions)
                        {
                            foreach (PeriodMonthlyFee pmf in mfdh.Decision)
                            {
                                if (pmf.Value > 0 && pmf.From == x.StartDate && (!pmf.To.HasValue || pmf.To.Value >= x.EndDate))
                                {
                                    if (!paysizeByPeriod.ContainsKey(x))
                                    {
                                        paysizeByPeriod.Add(x, pmf.Value);
                                    }

                                }

                            }
                        }
                    }

                    if (!paysizeByPeriod.ContainsKey(x))
                    {
                        foreach (PaysizeRecord psr in paysize)
                        {
                            if (psr.Paysize.DateStart <= x.StartDate && (psr.Paysize.DateEnd >= x.EndDate || psr.Paysize.DateEnd == null))
                            {
                                decimal? paySizeVal = psr.Value;
                                paysizeByPeriod.Add(x, paySizeVal.HasValue ? paySizeVal.Value : 0);
                            }
                        }


                    }

                });

                var firstPeriod = true;
                var refCalc = new List<LawsuitReferenceCalculation>();

                var entityLogLightDomain = this.Container.ResolveDomain<EntityLogLight>();

                var allHistory = entityLogLightDomain.GetAll()
                      .Where(x => x.ClassName == "BasePersonalAccount" && x.ParameterName == "area_share")
                      .Where(x => x.EntityId == claimWorkAccountDetail.PersonalAccount.Id)
                      .ToList();

                //исключаем более ранние изменения с той же датой начала действия
                var filteredHistory = allHistory
                    .GroupBy(x => new { x.EntityId, x.DateActualChange })
                    .ToDictionary(x => x.Key)
                    .Select(x => x.Value.OrderByDescending(u => u.DateApplied).FirstOrDefault())
                    .ToList();
                // .GroupBy(x => x.EntityId);
                // .ToDictionary(x => x.Key);

                foreach (ChargePeriod period in chargePeriods)
                {
                    var share = filteredHistory.Where(x => x.DateActualChange <= period.EndDate).OrderByDescending(x => x.DateApplied)
                        .ThenByDescending(x => x.Id).FirstOrDefault();
                    var areaShareByPeriod = share != null ? Convert.ToDecimal(share.PropertyValue.Replace('.', ',')) : areaShare;
                    refCalc.Add(
                        new LawsuitReferenceCalculation
                        {
                            ObjectCreateDate = DateTime.Now,
                            ObjectEditDate = DateTime.Now,
                            ObjectVersion = 0,
                            PeriodId = period.Id,
                            AccountNumber = accountNumber,
                            AreaShare = areaShareByPeriod,
                            Lawsuit = lawsuit,
                            BaseTariff = paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0,
                            //  BaseTariff = LawsuitOwnerInfoService.GetTariff(period.StartDate), пока переводим расчет на общие размеры взносов
                            PersonalAccountId = persAccId,
                            RoomArea = roomArea,
                            TariffCharged = firstPeriod
                                ? LawsuitOwnerInfoService.CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShareByPeriod, openDate)
                                : LawsuitOwnerInfoService.CalculateMonthCharge(paysizeByPeriod.ContainsKey(period) ? paysizeByPeriod[period] : 0, roomArea, areaShareByPeriod)
                            //TariffCharged = firstPeriod расчет осуществляется по основным размерам взносов
                            //    ?LawsuitOwnerInfoService.CalculateMonthCharge(LawsuitOwnerInfoService.GetTariff(period.StartDate), roomArea, areaShareByPeriod, openDate)
                            //    :LawsuitOwnerInfoService.CalculateMonthCharge(LawsuitOwnerInfoService.GetTariff(period.StartDate), roomArea, areaShareByPeriod)
                        });
                    firstPeriod = false;
                }

                //Все трансферы
                var persAccAllTransfers = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>().GetAll()
                    .Where(x => x.Owner.Id == claimWorkAccountDetail.PersonalAccount.Id)
                    .Where(x => x.Operation.IsCancelled != true)
                    .OrderByDescending(x => x.PaymentDate); //Обратный порядок для итерации

                var persAccPenaltyChargesList = this.Container.Resolve<IDomainService<PersonalAccountChargeTransfer>>().GetAll()
                    .Where(x => x.Owner.Id == claimWorkAccountDetail.PersonalAccount.Id)
                   .WhereIf(lawsuit.IsLimitationOfActions, x => x.OperationDate > lawsuit.DateLimitationOfActions)
                   .Where(x => x.ChargePeriod.Id <= lastPeriod.Id)
                   .Where(x => x.Reason == "Начисление пени" ||
                    x.Reason == "Установка/изменение пени" ||
                    x.Reason == "Перерасчет пени" ||
                    x.Reason == "Отмена начисления пени")
                   .Select(x=> new
                   {
                       Amount = x.Reason == "Отмена начисления пени" ? x.Amount * (-1) : x.Amount,
                   }).ToList();

                decimal persAccPenaltyCharges = 0;
                if (persAccPenaltyChargesList.Count > 0)
                {
                    persAccPenaltyCharges = persAccPenaltyChargesList.Sum(x => x.Amount);
                }
                //отмены начислений 
                var canceledCharge = this.Container.Resolve<IDomainService<PersonalAccountChargeTransfer>>().GetAll()
                    .Where(x => x.Owner.Id == claimWorkAccountDetail.PersonalAccount.Id)
                   .WhereIf(lawsuit.IsLimitationOfActions, x => x.OperationDate > lawsuit.DateLimitationOfActions)
                   .Where(x => x.ChargePeriod.Id <= lastPeriod.Id)
                   .Where(x => x.Reason == "Отмена начислений по базовому тарифу").FirstOrDefault();
                if (canceledCharge != null)
                {
                    hasCancelledCharges = true;
                }

                //Оплаты
                var persAccAllChargeTransfers = persAccAllTransfers
                    .WhereIf(lawsuit.IsLimitationOfActions, x => x.PaymentDate > lawsuit.DateLimitationOfActions)
                    .Where(
                        x => x.Reason == "Оплата по базовому тарифу" ||
                       //     x.Reason == "Оплата пени" ||
                            x.Reason == "Возврат взносов на КР" ||
                            x.Reason == "Оплата по тарифу решения")
                    .Select(x => new
                    {
                        x.Id,
                        Amount = x.Reason == "Возврат взносов на КР" ? x.Amount * (-1) : x.Amount,
                        x.ChargePeriod,
                        x.IsAffect,
                        x.IsInDirect,
                        x.IsLoan,
                        x.IsReturnLoan,
                        x.Owner,
                        x.PaymentDate,
                        x.Reason
                    }).ToList();

                var persAccAllPenaltyTransfers = persAccAllTransfers
                    .WhereIf(lawsuit.IsLimitationOfActions, x => x.PaymentDate > lawsuit.DateLimitationOfActions)
                    .Where(
                        x => x.Reason == "Оплата пени" ||
                            x.Reason == "Возврат оплаты пени")
                    .Select(x => new
                    {
                        x.Id,
                        Amount = x.Reason == "Возврат оплаты пени" ? x.Amount * (-1) : x.Amount,
                        x.ChargePeriod,
                        x.IsAffect,
                        x.IsInDirect,
                        x.IsLoan,
                        x.IsReturnLoan,
                        x.Owner,
                        x.PaymentDate,
                        x.Reason
                    }).ToList();

                decimal penaltyPayed = persAccAllPenaltyTransfers.Count > 0 ? persAccAllPenaltyTransfers.Sum(x => x.Amount) : 0;

                //Отмены оплат
                // var persAccAllReturnTransfers = persAccAllTransfers
                //     .WhereIf(lawsuit.IsLimitationOfActions, x => x.PaymentDate > lawsuit.DateLimitationOfActions)
                //     .Where(
                //     x => x.Reason == "Отмена оплаты по базовому тарифу" ||
                //         x.Reason == "Отмена оплаты по тарифу решения" ||
                //         x.Reason == "Отмена оплаты пени" ||
                //         x.Reason == "Возврат взносов на КР").ToList();
                //var persAccChargeTransfers = new List<PersonalAccountPaymentTransfer>(persAccAllChargeTransfers);
                //var persAccReturnTransfers = new List<PersonalAccountPaymentTransfer>(persAccAllReturnTransfers);

                //Обработка возвратов оплат пока комментим, похоже что в persAccAllChargeTransfers и так приходят все неотмененные

                //for (int chargeIndex = persAccChargeTransfers.Count - 1; chargeIndex >= 0; chargeIndex--)
                //{
                //    for (int returnIndex = persAccReturnTransfers.Count - 1; returnIndex >= 0; returnIndex--)
                //    {
                //        if (persAccChargeTransfers[chargeIndex].Amount != persAccReturnTransfers[returnIndex].Amount ||
                //            persAccChargeTransfers[chargeIndex].PaymentDate > persAccReturnTransfers[returnIndex].PaymentDate)
                //        {
                //            continue;
                //        }

                //        persAccChargeTransfers.RemoveAt(chargeIndex);
                //        persAccReturnTransfers.RemoveAt(returnIndex);
                //        //При нахождении отмены оплаты прекращаем поиск по списку возвратов
                //        break;
                //    }
                //}

                //Если остались неопределенные возвраты, вычитаем из оплат с начала
                //if (persAccReturnTransfers.Count > 0)
                //{
                //    //TODO:Обработка неопределенных возвратов
                //}

                decimal totalCharged = refCalc.Sum(x => x.TariffCharged);
                decimal totalPayment = persAccAllChargeTransfers.Sum(x => x.Amount);
                decimal resultDebt = totalCharged - totalPayment;

                //Расставляем даты и оплаты по месяцам
                persAccAllChargeTransfers = persAccAllChargeTransfers.OrderBy(x => x.Id).ToList();
                for (var chargeIndex = 0;
                    chargeIndex < persAccAllChargeTransfers.Count && chargeIndex < refCalc.Count;
                    chargeIndex++)
                {
                    refCalc[chargeIndex].PaymentDate = persAccAllChargeTransfers[chargeIndex].PaymentDate.ToShortDateString();
                    refCalc[chargeIndex].TarifPayment = persAccAllChargeTransfers[chargeIndex].Amount;
                    if (persAccAllChargeTransfers[chargeIndex].Reason == "Возврат взносов на КР")
                    {
                        refCalc[chargeIndex].Description = "Возврат взносов на КР";
                    }

                }

                //foreach (LawsuitReferenceCalculation referenceCalculation in refCalc)
                //{
                //    foreach (var retTr in persAccReturnTransfers)
                //    {
                //        if (referenceCalculation.PeriodId == retTr.ChargePeriod.Id)
                //        {
                //            referenceCalculation.TarifPayment += retTr.Amount;
                //            referenceCalculation.Description = "Возврат взносов";
                //        }
                //    }
                //}

                //Инвертируем оплаты для отображения
                decimal remainingPayment = totalPayment * -1;
                decimal totalDebt = 0;
                var debtStarted = false;
                DateTime debtStartDate = openDate;
                message = totalPayment == 0 
                    ? lawsuit.IsLimitationOfActions 
                        ? "Задолженность начинается с даты применения срока исковой давности"
                        : "Задолженность начинается с даты открытия лицевого счёта." 
                    : "";
                foreach (LawsuitReferenceCalculation referenceCalculation in refCalc)
                {
                    remainingPayment = remainingPayment + referenceCalculation.TariffCharged;
                    totalDebt = totalDebt + referenceCalculation.TariffCharged - referenceCalculation.TarifPayment;
                    referenceCalculation.TarifDebt = totalDebt;
                    if (remainingPayment >= 0 && !debtStarted)
                    {
                        //Рассчет даты начала задолженности
                        ChargePeriod lastPaymentPeriod = this.PeriodDomain.Get(referenceCalculation.PeriodId);
                        debtStartDate = LawsuitOwnerInfoService.CalculateDebtStartDate(
                            referenceCalculation.BaseTariff,
                            //  LawsuitOwnerInfoService.GetTariff(lastPaymentPeriod.StartDate),
                            roomArea,
                            referenceCalculation.AreaShare,
                            remainingPayment,
                            lastPaymentPeriod.StartDate);

                        // Фикс несостыковки дат открытия аккаунта и начала задолженности из-за округлений
                        // (дата начала может быть указана на 1 день раньше из-за десятых-сотых копеек долга,
                        // если такое происходит и итоговая дата раньше даты открытия - заменяем на дату открытия)
                        debtStartDate = debtStartDate < openDate ? openDate : debtStartDate;

                        lawsuit.DebtStartDate = lawsuit.DebtStartDate > debtStartDate ? debtStartDate : lawsuit.DebtStartDate ;
                        lawsuit.DebtEndDate = lawsuit.DebtEndDate < endDate ? endDate : lawsuit.DebtEndDate;

                        if (message == "")
                        {
                            if (remainingPayment == 0)
                                message = $"Задолженность начинается с {debtStartDate.ToShortDateString()}";
                            else
                            {
                                switch (debtStartDate.Day)
                                {
                                    //Разный текст примечания в зависимости от дня начала задолженности
                                    case 1:
                                        message = $"За предыдущий месяц переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString()}.";
                                        break;
                                    case 2:
                                        message = $"За {debtStartDate.AddDays(-1).ToShortDateString()} переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString(CultureInfo.InvariantCulture)}.";
                                        break;
                                    default:
                                        message =
                                            $"С {lastPaymentPeriod.StartDate.ToShortDateString()} " +
                                            $"по {debtStartDate.AddDays(-1).ToShortDateString()} переплата {(referenceCalculation.TariffCharged - remainingPayment).ToString(CultureInfo.InvariantCulture)}.";
                                        break;
                                }

                                message += $" С {debtStartDate.ToShortDateString()} задолженность {remainingPayment.ToString(CultureInfo.InvariantCulture)}.";
                                referenceCalculation.Description = message;
                                lawsuit.Description = message;
                            }
                        }

                        debtStarted = true;
                    }

                    this.RefCalcDomain.Save(referenceCalculation);
                }

                //TODO: Учет тарифа решения

                var sumsBySummaries = this.GetSummarySums(claimWorkAccountDetail, lastPeriod);
                
                lawsuit.DebtBaseTariffSum += resultDebt < 0 ? 0: resultDebt ;
                lawsuit.DebtDecisionTariffSum += sumsBySummaries.decisionDebtSum  < 0 ? 0: sumsBySummaries.decisionDebtSum;
                lawsuit.DebtSum += resultDebt + sumsBySummaries.decisionDebtSum < 0 ? 0: resultDebt + sumsBySummaries.decisionDebtSum;
                lawsuit.PenaltyDebt += (persAccPenaltyCharges - penaltyPayed);

                // IRepository lawsuitRepository = this.Container.ResolveRepository(typeof(Lawsuit));
                // lawsuitRepository.Evict(lawsuit);
                // this.Container.Release(lawsuitRepository);
                
            }
            
            this.LawsuitDomain.Update(lawsuit);
            if (hasCancelledCharges)
            {
                message += " Внимание!!!! на ЛС есть отмены начислений. Возможно списание исковой давности";
            }
            return new BaseDataResult(
                new
                {
                    lawsuit.DebtDecisionTariffSum,
                    lawsuit.DebtBaseTariffSum,
                    lawsuit.DebtSum,
                    lawsuit.Description,
                    dateStartDebt =  lawsuit.DebtStartDate,
                    message,
                    lawsuit.DebtCalcMethod,
                    lawsuit.DebtEndDate,
                    PenaltyDebt = lawsuit.PenaltyDebt ?? 0
                });
        }

        public IDataResult CalcLegalWithReferenceCalc(long docId)
        {
            Lawsuit lawsuit = this.LawsuitDomain.Get(docId);
            List<ClaimWorkAccountDetail> claimWorkAccountDetail = this.AccountDetailDomain.GetAll()
                .Where(x => x.ClaimWork == lawsuit.ClaimWork)
                .ToList();

            return this.CalcLegalWithReferenceCalc(lawsuit, claimWorkAccountDetail);
        }

        private void ClearReferenceCalculation(long docId)
        {
            var lawsuitReferenceCalculationDomain = this.Container.Resolve<IDomainService<LawsuitReferenceCalculation>>();
            try
            {
                long lawsuitId = this.Container.Resolve<IDomainService<Lawsuit>>().Get(docId).Id;

                var currentCalc = lawsuitReferenceCalculationDomain.GetAll()
                    .Where(x => x.Lawsuit.Id == lawsuitId)
                    .Select(x => x.Id).ToList();

                foreach (long calcId in currentCalc)
                {
                    lawsuitReferenceCalculationDomain.Delete(calcId);
                }
            }
            finally
            {
                this.Container.Release(lawsuitReferenceCalculationDomain);
            }
        }

        private ChargePeriod GetLastReferenceCalculationPeriod(DateTime documentDate)
        {
            var periodDomain = this.Container.Resolve<IDomainService<ChargePeriod>>();
      
            bool documentDateAfter25Day = documentDate.Day > 25;

            var lastPeriod = !documentDateAfter25Day ? this.PeriodDomain.GetAll().FirstOrDefault(x => x.StartDate <= documentDate.AddMonths(-2).Date && x.EndDate >= documentDate.AddMonths(-2).Date)
                : this.PeriodDomain.GetAll().FirstOrDefault(x => x.StartDate <= documentDate.AddMonths(-1).Date && (x.EndDate ?? DateTime.MaxValue) >= documentDate.AddMonths(-1).Date);



            this.Container.Release(periodDomain);
            return lastPeriod;
        }

        private static decimal CalculateMonthCharge(decimal tariff, decimal area, decimal share, DateTime openDate = default(DateTime))
        {
            if (openDate == default(DateTime))
            {
                return (area * share * tariff).RoundDecimal(2);
            }

            int daysInMonth = DateTime.DaysInMonth(openDate.Year, openDate.Month);
            int daysCounted = (new DateTime(openDate.AddMonths(1).Year, openDate.AddMonths(1).Month, 1) - openDate).Days;
           
            //округление 0,5 в большую сторону
            //var tmp = decimal.Round((area * share * tariff * decimal.Divide(daysCounted, daysInMonth)), MidpointRounding.AwayFromZero);
            
            return (area * share * tariff * decimal.Divide(daysCounted, daysInMonth)).RoundDecimal(2);
            
        }

        private static DateTime CalculateDebtStartDate(decimal tariff, decimal area, decimal share, decimal debt, DateTime month)
        {
            int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            decimal totalMonthCharge = tariff * area * share;
            decimal day = 0;
            if (totalMonthCharge > 0)
            {
                day = (totalMonthCharge - debt) * daysInMonth / totalMonthCharge;
            }
            else
            {
                day = 0;
            }

            DateTime resutDate = new DateTime(month.Year, month.Month, 1).AddDays((int)day);
            return resutDate;
        }

        //TODO:Использовать данные из базы, если возможно, для разделения рассчета по регионам вместо хардкодинга значений
        private static decimal GetTariff(DateTime date)
        {
            return date.Year == 2014?6.2m:6.6m;
        }

        private void CalculateProcess(LawsuitOwnerInfo lawsuitOwnerInfo)
        {
            this.SetDebtSum(lawsuitOwnerInfo);

            lawsuitOwnerInfo.DebtBaseTariffSum = LawsuitOwnerInfoService.Round(lawsuitOwnerInfo.DebtBaseTariffSum * lawsuitOwnerInfo.AreaShare);
            lawsuitOwnerInfo.DebtDecisionTariffSum = LawsuitOwnerInfoService.Round(lawsuitOwnerInfo.DebtDecisionTariffSum * lawsuitOwnerInfo.AreaShare);
            lawsuitOwnerInfo.PenaltyDebt = LawsuitOwnerInfoService.Round(lawsuitOwnerInfo.PenaltyDebt * lawsuitOwnerInfo.AreaShare);
        }

        private void SetDebtSum(LawsuitOwnerInfo lawsuitOwnerInfo)
        {
            List<DebtPeriodInfo> sumInfoList = new List<DebtPeriodInfo>();
            if (!this.DebtPeriodCalcService.DebtDict.TryGetValue(lawsuitOwnerInfo.PersonalAccount.Id, out sumInfoList))
            {
                return;
            }

            DebtPeriodInfo oldDebt = sumInfoList
                .FirstOrDefault(x => x.Period.Id == lawsuitOwnerInfo.StartPeriod.Id);

            DebtPeriodInfo newDebt = sumInfoList
                .WhereIf(lawsuitOwnerInfo.EndPeriod.IsClosed, x => x.Period.StartDate > lawsuitOwnerInfo.EndPeriod.EndDate)
                .WhereIf(lawsuitOwnerInfo.EndPeriod.IsClosed, x => x.Period.StartDate < lawsuitOwnerInfo.EndPeriod.EndDate.Value.AddDays(3))
                .WhereIf(!lawsuitOwnerInfo.EndPeriod.IsClosed, x => x.Period.Id == lawsuitOwnerInfo.EndPeriod.Id)
                .FirstOrDefault();

            lawsuitOwnerInfo.DebtBaseTariffSum = newDebt.BaseTariffSum - oldDebt.BaseTariffSum;
            lawsuitOwnerInfo.DebtDecisionTariffSum = newDebt.DecisionTariffSum - oldDebt.DecisionTariffSum;
            lawsuitOwnerInfo.PenaltyDebt = newDebt.PenaltySum - oldDebt.PenaltySum;
        }

        private static decimal Round(decimal val) => Math.Truncate(val * 100) / 100;

        private Dictionary<ChargePeriod, decimal> GetTariffByPeriod(ClaimWorkAccountDetail claimWorkAccountDetail, List<ChargePeriod> chargePeriods)
        {
            var domain = Container.ResolveDomain<PaysizeRecord>();
           
            var paysize = domain.GetAll()
               .Where(x => x.Municipality != null)
               .Where(x => x.Value != null)
               .Where(x => x.Municipality == claimWorkAccountDetail.PersonalAccount.Room.RealityObject.Municipality)
               .ToList();

            Dictionary<ChargePeriod, decimal> paysizeByPeriod = new Dictionary<ChargePeriod, decimal>();

            var decisionDomain = Container.ResolveDomain<MonthlyFeeAmountDecHistory>();

            var decisions = decisionDomain.GetAll()
                .Where(x => x.Protocol.RealityObject == claimWorkAccountDetail.PersonalAccount.Room.RealityObject)
                .ToList();

            chargePeriods.ForEach(x =>
            {
                if (decisions != null)
                {
                    foreach (MonthlyFeeAmountDecHistory mfdh in decisions)
                    {
                        foreach (PeriodMonthlyFee pmf in mfdh.Decision)
                        {
                            if (pmf.Value > 0 && pmf.From == x.StartDate && (!pmf.To.HasValue || pmf.To.Value >= x.EndDate))
                            {
                                if (!paysizeByPeriod.ContainsKey(x))
                                {
                                    paysizeByPeriod.Add(x, pmf.Value);
                                }

                            }

                        }
                    }
                }

                if (!paysizeByPeriod.ContainsKey(x))
                {
                    foreach (PaysizeRecord psr in paysize)
                    {
                        if (psr.Paysize.DateStart <= x.StartDate && (psr.Paysize.DateEnd ?? DateTime.Now) >= x.EndDate)
                        {
                            decimal? paySizeVal = psr.Value;
                            paysizeByPeriod.Add(x, paySizeVal.HasValue ? paySizeVal.Value : 0);
                        }
                    }


                }

            });

            return paysizeByPeriod;

        }
        public IDataResult GetDebtStartDateCalculate(BaseParams baseParams)
        {
            this.RefCalcDomain = this.Container.ResolveDomain<LawsuitReferenceCalculation>();
            //Проверяем документ
            var docId = baseParams.Params.GetAs<long>("docId");
            if (docId == 0)
            {
                return BaseDataResult.Error("Не найдена информация о документе");
            }
            this.ClearReferenceCalculation(docId);

            string message = "";

            DocumentClw documentClw = this.Container.Resolve<IDomainService<DocumentClw>>().Get(docId);
            try
            {
                Lawsuit lawsuitCO = this.LawsuitDomain.GetAll()
                    .FirstOrDefault(x => x.ClaimWork == documentClw.ClaimWork && x.DocumentType == Gkh.Modules.ClaimWork.Enums.ClaimWorkDocumentType.CourtOrderClaim);
                Lawsuit lawsuit = this.LawsuitDomain.Get(docId);

                RefCalcDomain.GetAll()
                    .Where(x => x.Lawsuit == lawsuitCO).ToList()
                    .ForEach(x =>
                    {
                        LawsuitReferenceCalculation lrc = new LawsuitReferenceCalculation
                        {
                            Lawsuit = lawsuit,
                            AccountNumber = x.AccountNumber,
                            AreaShare = x.AreaShare,
                            BaseTariff = x.BaseTariff,
                            Description = x.Description,
                            PaymentDate = x.PaymentDate,
                            PeriodId = x.PeriodId,
                            PersonalAccountId = x.PersonalAccountId,
                            RoomArea = x.RoomArea,
                            TarifDebt = x.TarifDebt,
                            TariffCharged = x.TariffCharged,
                            TarifPayment = x.TarifPayment,
                            ObjectCreateDate = x.ObjectCreateDate,
                            ObjectEditDate = x.ObjectEditDate,
                            ObjectVersion = x.ObjectVersion
                        };
                        RefCalcDomain.Save(lrc);

                    });

                return new BaseDataResult(
                new
                {
                    lawsuitCO.DebtDecisionTariffSum,
                    lawsuitCO.DebtBaseTariffSum,
                    lawsuitCO.DebtSum,
                    lawsuitCO.Description,
                    lawsuitCO.DebtCalcMethod,
                    dateStartDebt = lawsuitCO.DebtStartDate,
                    lawsuitCO.DebtEndDate,
                    lawsuitCO.PenaltyDebt,
                    message
                });
            }
            catch (Exception e)
            {
                message = "При переносе данных произошла ошибка: " + e.Message;
                return new BaseDataResult(
              new
              {
                  
                  DebtDecisionTariffSum = 0,
                  DebtBaseTariffSum = 0,
                  DebtSum = 0,
                  Description = 0,
                  DebtCalcMethod = 10,
                  dateStartDebt = DateTime.MinValue,
                  DebtEndDate = DateTime.MinValue,
                  PenaltyDebt = 0,
                  message
              });
            }

        }

        private SumsBySummary  GetSummarySums(ClaimWorkAccountDetail claimWorkAccountDetail, ChargePeriod lastPeriod)
        {
            var summuries = claimWorkAccountDetail.PersonalAccount.Summaries.Where(x => x.Period.Id >= lastPeriod.Id).ToList();

            decimal saldoIn = summuries[0].SaldoIn;
            decimal baseTarifDebt = summuries[0].BaseTariffDebt;
            decimal decisionDebt = summuries[0].DecisionTariffDebt;
            decimal penaltyDebt = summuries[0].PenaltyDebt;
            decimal paymentsBaseTarif = 0;
            decimal paymentsDecision = 0;
            decimal paymentsPenalty = 0;
            decimal recalcBaseTarif = 0;
            decimal recalcDecision = 0;
            decimal recalcPenalty = 0;

            //не ясно как поступать с изменениями сальдо в последующих периодах. Может стоит их прибавить 
            decimal saldoChanges = (decimal)summuries[0].BalanceChanges.Sum(x => x.CurrentValue - x.NewValue);
            decimal decisionChanges = (decimal)summuries[0].DecisionTariffChange;
            decimal penaltyChanges = (decimal)summuries[0].PenaltyChange;

            summuries.ForEach(x => {
                paymentsBaseTarif += x.TariffPayment;
                paymentsDecision += x.TariffDecisionPayment;
                paymentsPenalty += x.PenaltyPayment;
                recalcBaseTarif += x.RecalcByBaseTariff;
                recalcPenalty += x.RecalcByPenalty;
                recalcDecision += x.RecalcByDecisionTariff; 
            }); 

            //считаем задолженость
            decimal totalDebt = saldoIn + saldoChanges + (recalcBaseTarif  + recalcDecision) + (recalcPenalty + penaltyChanges) - (paymentsBaseTarif + paymentsDecision + paymentsPenalty);
                
            baseTarifDebt = recalcBaseTarif + saldoChanges - paymentsBaseTarif;
            decisionDebt = recalcDecision + decisionChanges - paymentsDecision;
            penaltyDebt = recalcPenalty + penaltyChanges - paymentsPenalty;
            
            return new SumsBySummary(baseTarifDebt,decisionDebt,penaltyDebt);
            

        }

    }


}