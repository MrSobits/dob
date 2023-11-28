namespace Bars.GkhGji.Regions.Voronezh
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Entities;
    using System;
    using Bars.GkhGji.Enums;
    using Castle.Core.Internal;
    using System.Web.Mvc;

    class BDOPERInterceptor : EmptyDomainInterceptor<BDOPER>
    {
        public override IDataResult AfterCreateAction(IDomainService<BDOPER> service, BDOPER entity)
        {
            var bdoperDomain = this.Container.ResolveDomain<BDOPER>();
            var vtoperDomain = this.Container.ResolveDomain<VTOPER>();
            var resolDomain = this.Container.ResolveDomain<Resolution>();
            var resolPayFineDomain = this.Container.ResolveDomain<ResolutionPayFine>();
            var indPersonDomain = this.Container.ResolveDomain<IndividualPerson>();

            try
            {
                int stopper;
                var vtoper = vtoperDomain.GetAll()
                    .Where(x => x.GUID == entity.GUID)
                    .FirstOrDefault();

                long resolutionId = 0;
                long relatedPayerId = 0;
                long relatedViolatorId = 0;
                string relatedCaseNumber = "";
                DateTime? relatedViolDate = DateTime.MinValue;

                //Проверяем, адекватно ли распарсился документ
                if (vtoper != null)
                {
                    //Ищем ФИО нарушителя в назначении платежа
                    relatedViolatorId = indPersonDomain.GetAll()
                        .Where(x => entity.Purpose.ToLower().Contains(x.Fio.ToLower()))
                        .Select(x => x.Id)
                        .FirstOrDefault();

                    //Если в назначении платежа не нашли нарушителя, берем ФИО плательщика и будем сравнивать по нему
                    if (relatedViolatorId == 0 && !entity.NamePay.IsNullOrEmpty())
                    {
                        //Ищем номер дела в назначении платежа
                        stopper = entity.Purpose.ToLower().IndexOf("по делу");
                        if (stopper > 0)
                        {
                            relatedCaseNumber = resolDomain.GetAll()
                                .Where(x => entity.Purpose.Substring(stopper, 10).ToLower().Contains(x.CaseNumber.ToLower()))
                                .Select(x => x.CaseNumber)
                                .FirstOrDefault();
                        }

                        //Ищем док, где дата документа такая же, как в назначении платежа
                        stopper = entity.Purpose.ToLower().IndexOf("время");
                        if (stopper > 0)
                        {
                            relatedViolDate = resolDomain.GetAll()
                                .Where(x => entity.Purpose.Substring(0, stopper).ToLower().Contains(x.DocumentDate.ToString().ToLower()))
                                .Select(x => x.DocumentDate)
                                .FirstOrDefault();
                        }

                        //Ищем ФИО в наименовании плательщика
                        relatedPayerId = indPersonDomain.GetAll()
                            .Where(x => entity.NamePay.ToLower().Contains(x.Fio.ToLower()))
                            .Where(x => entity.InnPay == x.INN)
                            .Select(x => x.Id)
                            .FirstOrDefault();

                        //Пытаемся найти постановление по плательщику, номеру дела и дате документа
                        resolutionId = resolDomain.GetAll()
                            .Where(x => x.IndividualPerson.Id == relatedPayerId)
                            .WhereIf(relatedCaseNumber != "", x => x.CaseNumber == relatedCaseNumber)
                            .WhereIf(relatedViolDate != DateTime.MinValue, x => x.DocumentDate == relatedViolDate)
                            .Select(x => x.Id)
                            .FirstOrDefault();
                    }
                    else
                    {
                        //Ищем номер дела в назначении платежа
                        stopper = entity.Purpose.ToLower().IndexOf("ИД");
                        if (stopper > 0)
                        {
                            relatedCaseNumber = resolDomain.GetAll()
                                .Where(x => entity.Purpose.Substring(stopper, 15).ToLower().Contains(x.CaseNumber.ToLower()))
                                .Select(x => x.CaseNumber)
                                .FirstOrDefault();
                        }

                        //Ищем док, где дата документа такая же, как в назначении платежа
                        if (stopper > 0)
                        {
                            relatedViolDate = resolDomain.GetAll()
                                .Where(x => entity.Purpose.Substring(stopper, 15).ToLower().Contains(x.DocumentDate.ToString().ToLower()))
                                .Select(x => x.DocumentDate)
                                .FirstOrDefault();
                        }
                    }

                    //Проверяем, нет ли уже оплат по данному постановлению с такой датой, с таким номером,
                    //с такой суммой и с типом оплаты - "Оплата АСФК"
                    var payFineExists = resolPayFineDomain.GetAll()
                        .Where(x => x.Resolution.Id == resolutionId)
                        .Where(x => x.DocumentDate == vtoper.DateDoc)
                        .Where(x => x.DocumentNum == vtoper.NomDoc)
                        .Where(x => x.Amount == entity.Sum)
                        .Where(x => x.TypeDocumentPaid == TypeDocumentPaidGji.PaymentASFK)
                        .FirstOrDefault() != null;

                    if (!payFineExists)
                    {
                        //Ищем по нарушителю
                        if (relatedViolatorId != 0)
                        {
                            resolPayFineDomain.Save(new ResolutionPayFine
                            {
                                Resolution = resolDomain.Get(resolutionId),
                                DocumentDate = vtoper.DateDoc,
                                DocumentNum = vtoper.NomDoc,
                                Amount = entity.Sum,
                                TypeDocumentPaid = TypeDocumentPaidGji.PaymentASFK
                            });

                            entity.Resolution = resolDomain.Get(resolutionId);
                            entity.IsPayFineAdded = true;
                            bdoperDomain.Update(entity);
                            CalcBalance(resolDomain.Get(resolutionId));
                        }
                        //Ищем по плательщику, если нарушителя нет
                        else if (relatedPayerId != 0)
                        {
                            resolPayFineDomain.Save(new ResolutionPayFine
                            {
                                Resolution = resolDomain.Get(resolutionId),
                                DocumentDate = vtoper.DateDoc,
                                DocumentNum = vtoper.NomDoc,
                                Amount = entity.Sum,
                                TypeDocumentPaid = TypeDocumentPaidGji.PaymentASFK
                            });

                            entity.Resolution = resolDomain.Get(resolutionId);
                            entity.IsPayFineAdded = true;
                            bdoperDomain.Update(entity);
                            CalcBalance(resolDomain.Get(resolutionId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Container.Release(vtoperDomain);
                Container.Release(bdoperDomain);
                Container.Release(resolDomain);
                Container.Release(resolPayFineDomain);
                Container.Release(indPersonDomain);
            }

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<BDOPER> service, BDOPER entity)
        {
            var vtoperDomain = this.Container.ResolveDomain<VTOPER>();
            var resolDomain = this.Container.ResolveDomain<Resolution>();
            var resolPayFineDomain = this.Container.ResolveDomain<ResolutionPayFine>();
            var indPersonDomain = this.Container.ResolveDomain<IndividualPerson>();

            var vtoper = vtoperDomain.GetAll()
                .Where(x => x.GUID == entity.GUID)
                .FirstOrDefault();

            try
            {
                if (entity.IsPayFineAdded == true)
                {
                    var relatedPayFine = resolPayFineDomain.GetAll()
                    .Where(x => x.Resolution.Id == entity.Resolution.Id)
                    .Where(x => x.DocumentDate == vtoper.DateDoc)
                    .Where(x => x.DocumentNum == vtoper.NomDoc)
                    .Where(x => x.Amount == entity.Sum)
                    .Where(x => x.TypeDocumentPaid == TypeDocumentPaidGji.PaymentASFK)
                    .FirstOrDefault();

                    resolPayFineDomain.Delete(relatedPayFine.Id);
                    entity.Resolution = null;
                    entity.IsPayFineAdded = false;
                    service.Update(entity);
                }

                CalcBalance(resolDomain.Get(entity.Resolution.Id));
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Container.Release(vtoperDomain);
                Container.Release(resolDomain);
                Container.Release(resolPayFineDomain);
                Container.Release(indPersonDomain);
            }

            return this.Success();
        }

        private void CalcBalance(Resolution resolution)
        {
            var resolRepo = this.Container.ResolveRepository<Resolution>();
            var resolPayFineDomain = this.Container.ResolveDomain<ResolutionPayFine>();

            try
            {
                // Заполняем поле "Штраф оплачен (подробно)"
                var resolutionPayFineSum = resolPayFineDomain
                         .GetAll()
                         .Where(x => x.Resolution.Id == resolution.Id)
                         .Sum(x => x.Amount)
                         .ToDecimal();
                var maxpaymentdate = resolPayFineDomain
                         .GetAll()
                         .Where(x => x.Resolution.Id == resolution.Id)
                         .Max(x => x.DocumentDate);
                if (maxpaymentdate.HasValue)
                {
                    resolution.PaymentDate = maxpaymentdate;
                }

                if (resolution.PenaltyAmount.HasValue && resolutionPayFineSum > resolution.PenaltyAmount)
                {
                    resolution.PayStatus = ResolutionPaymentStatus.OverPaid;
                }
                else if (resolution.PenaltyAmount.HasValue && resolutionPayFineSum == resolution.PenaltyAmount)
                {
                    resolution.PayStatus = ResolutionPaymentStatus.Paid;
                }
                else if (resolution.PenaltyAmount.HasValue && resolutionPayFineSum > 0)
                {
                    resolution.PayStatus = ResolutionPaymentStatus.PartialPaid;
                }
                else
                {
                    resolution.PayStatus = ResolutionPaymentStatus.NotPaid;
                }
                resolRepo.Update(resolution);
            }
            catch (Exception e)
            {

            }
            finally
            {
                Container.Release(resolRepo);
                Container.Release(resolPayFineDomain);
            }
        }
    }
}
