using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;
using Castle.Windsor;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Helpers;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class PAYREGService : IPAYREGService
    {
        #region Constants

        static XNamespace bdiNamespace = @"http://roskazna.ru/gisgmp/xsd/BudgetIndex/2.5.0";
        static XNamespace orgNamespace = @"http://roskazna.ru/gisgmp/xsd/Organization/2.5.0";
        static XNamespace comNamespace = @"http://roskazna.ru/gisgmp/xsd/Common/2.5.0";
        static XNamespace chgNamespace = @"http://roskazna.ru/gisgmp/xsd/Charge/2.5.0";
        static XNamespace pkgNamespace = @"http://roskazna.ru/gisgmp/xsd/Package/2.5.0";
        static XNamespace reqNamespace = @"urn://roskazna.ru/gisgmp/xsd/services/import-charges/2.5.0";
        static XNamespace rfdNamespace = @"http://roskazna.ru/gisgmp/xsd/Refund/2.5.0";
        static XNamespace pmntNamespace = @"http://roskazna.ru/gisgmp/xsd/Payment/2.5.0";
        static XNamespace ns0Namespace = @"urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0";
        static XNamespace scNamespace = @"http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0";
        static XNamespace faNamespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0";

        #endregion

        #region Properties              

        public IDomainService<PayRegFile> PayRegFileDomain { get; set; }
        public IDomainService<PayReg> PayRegDomain { get; set; }
        public IDomainService<GisGmp> GisGmpDomain { get; set; }
        public IDomainService<ZonalInspection> ZonalInspectionDomain { get; set; }
        public IDomainService<PayRegRequests> PayRegRequestsDomain { get; set; }
        public IDomainService<Resolution> ResolutionDomain { get; set; }
        public IRepository<Resolution> ResolutionRepo { get; set; }
        public IDomainService<ResolutionPayFine> ResolutionPayFineDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;

        #endregion

        #region Constructors

        public PAYREGService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods
               
        /// <summary>
        /// Запрос информации о платежах (все комиссии)
        /// </summary>
        public bool SendPaymentRequest(PayRegRequests requestData, IProgressIndicator indicator = null)
        {
           List<string> originators = new List<string>();
            originators = ZonalInspectionDomain.GetAll().Where(x => x.GisGmpId != "" && x.GisGmpId != null)
                .Select(x=> x.GisGmpId).Distinct().ToList();
            foreach (string orig in originators)
            {
                SendRequest(requestData, orig);
            }
            return true;
        }

        /// <summary>
        /// Запрос информации о платежах по конкретной комиссии
        /// </summary>
        private void SendRequest(PayRegRequests requestData, string originatorId)
        {
            try
            {
                //Очищаем список файлов
                PayRegFileDomain.GetAll().Where(x => x.PayRegRequests == requestData).ToList().ForEach(x => PayRegFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                XElement request;
                if (requestData.PayRegPaymentsType == GisGmpPaymentsType.AllInTime)
                {
                    //тестовая отправка
                    request = GetPaymentRequestXML(requestData, originatorId);
                }
                else
                {
                    request = GetPaymentRequestXML(requestData, originatorId);
                }

                ChangeState(requestData, RequestState.Formed);

                string messageId = GetTimeStampUuid();

                var requestResult = _SMEV3Service.SendRequestAsyncSGIO(request, messageId, null, true).GetAwaiter().GetResult();
                requestData.MessageId = messageId;
                PayRegRequestsDomain.Update(requestData);
             
                //Больше не сохраняем, т.к. их будет слишком много
                //SaveFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                //SaveFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

            
                if (requestResult.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                    SaveException(requestData, requestResult.Error.Exception);
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveFile(requestData, requestResult.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в файле Fault.xml");
                }
                else
                {
                    //изменяем статус
                    //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани
                    requestData.RequestState = RequestState.Queued;
                    requestData.Answer = "Поставлено в очередь";
                    PayRegRequestsDomain.Update(requestData);
                }
            }
            catch (HttpRequestException)
            {
                //ошибки связи прокидываем в контроллер
                throw;
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendPaymentRequest exception: " + e.Message);
            }
        }  

        /// <summary>
        /// Проверить наличие ответа
        /// </summary>
        /// <param name="requestData">Запрос</param>
        /// <returns>ответ, если есть ответ, иначе null</returns>
        public GetResponseResponse CheckResponse(PayRegRequests requestData)
        {
            //запрашиваем ответ
            return Smev3Helper.TryGetResponseAsync(_SMEV3Service, null, null, requestData.MessageId, true).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData">Запрос</param>
        /// <param name="response">Ответ</param>
        /// <param name="indicator">Индикатор прогресса для таски</param>
        public bool TryProcessResponse(GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //сохранение данных
                indicator?.Report(null, 40, "Сохранение данных для отладки");
             
                indicator?.Report(null, 70, "Обработка результата");

                //ошибки наши
                if (response.Error != null)
                {                
                    return false;
                }
                //сервер отклонил запрос?
                else if (response.RequestRejected != null)
                {
                    return false;
                }
                else
                {
                    //ответ пустой?
                    if (response.MessagePrimaryContent == null)
                    {                        
                        return false;
                    }

                    if (response.MessagePrimaryContent.Element(ns0Namespace + "ExportPaymentsResponse") != null)
                    {
                        //ACK - ставим вдумчиво - чтобы можно было считать повторно, если это наш косяк
                        _SMEV3Service.GetAckAsyncSGIO(response.MessageId, true).GetAwaiter().GetResult();

                        var paymentsElements = response.MessagePrimaryContent.Element(ns0Namespace + "ExportPaymentsResponse")?.Elements();

                        foreach (XElement element in paymentsElements)
                        {
                            var payId = element.Attribute("paymentId")?.Value;
                            var UIN = element.Attribute("supplierBillID")?.Value;
                            GisGmp gisGmp = null;
                            if (!string.IsNullOrEmpty(UIN))
                            {
                                gisGmp = GisGmpDomain.GetAll()
                                    .Where(x => x.UIN == UIN).FirstOrDefault();
                            }
                            if ((PayRegDomain.GetAll()
                                .Where(x => x.PaymentId == payId).Count() == 0))
                            {
                                try
                                {
                                    PayReg newPayment = new PayReg
                                    {
                                        Amount = element.Attribute("amount") != null ? Convert.ToDecimal(element.Attribute("amount").Value) / 100 : 0,
                                        Kbk = element.Attribute("kbk")?.Value,
                                        OKTMO = element.Attribute("oktmo")?.Value,
                                        PaymentDate = NullableDateParse(element.Attribute("paymentDate")?.Value),
                                        PaymentId = payId,
                                        SupplierBillID = element.Attribute("supplierBillID")?.Value,
                                        Purpose = element.Attribute("purpose")?.Value,
                                        PaymentOrg = ParsePaymentOrg(element.Element(pmntNamespace + "PaymentOrg")),
                                        PaymentOrgDescr = ParsePaymentOrgDescr(element.Element(pmntNamespace + "PaymentOrg")),
                                        PayerId = element.Element(pmntNamespace + "Payer")?.Attribute("payerIdentifier")?.Value,
                                        PayerAccount = element.Element(pmntNamespace + "Payer")?.Attribute("payerAccount")?.Value,
                                        PayerName = element.Element(pmntNamespace + "Payer")?.Attribute("payerName")?.Value,
                                        BdiStatus = element.Element(pmntNamespace + "BudgetIndex")?.Attribute("status")?.Value,
                                        BdiPaytReason = element.Element(pmntNamespace + "BudgetIndex")?.Attribute("paytReason")?.Value,
                                        BdiTaxPeriod = element.Element(pmntNamespace + "BudgetIndex")?.Attribute("taxPeriod")?.Value,
                                        BdiTaxDocNumber = element.Element(pmntNamespace + "BudgetIndex")?.Attribute("taxDocNumber")?.Value,
                                        BdiTaxDocDate = element.Element(pmntNamespace + "BudgetIndex")?.Attribute("taxDocDate")?.Value,
                                        AccDocDate = NullableDateParse(element.Element(pmntNamespace + "AccDoc")?.Attribute("accDocDate")?.Value),
                                        AccDocNo = element.Element(pmntNamespace + "AccDoc")?.Attribute("accDocNo")?.Value,
                                        Status = byte.Parse(element.Element(comNamespace + "ChangeStatusInfo")?.Element(comNamespace + "Meaning")?.Value),
                                        GisGmp = gisGmp,
                                        // проставляем флаг квитирования платежам, если нашлось начисление с таким же УИНом, т.к. такие платежи автоматически сквитированы в ГИС ГМП
                                        Reconcile = element.Attribute("supplierBillID") == null ? Gkh.Enums.YesNoNotSet.NotSet : (element.Attribute("supplierBillID").Value != "0") && (gisGmp != null) ? Gkh.Enums.YesNoNotSet.Yes : Gkh.Enums.YesNoNotSet.No
                                    };
                                    PayRegDomain.Save(newPayment);
                                }
                                catch (Exception e)
                                {
                                }
                            };
                        }
                        return true;
                    }
                    //разбираем xml, которую прислал сервер
                    var code = response.MessagePrimaryContent.Element(reqNamespace + "ImportChargesResponse")?.Element(comNamespace + "ImportProtocol")?.Attribute("code")?.Value;

                    if (code == "0")
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {               
                return false;
            }
        }

        public GisGmp FindGisGmp(string UIN, string purpose)
        {
            var DocumentGjiChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var DocumentGjiDomain = Container.Resolve<IDomainService<DocumentGji>>();
            var ResolutionDomain = Container.Resolve<IDomainService<Resolution>>();
            var ResolutionProsecutorDomain = Container.Resolve<IDomainService<ResolPros>>();
            var ResolutionRospotrebnadzorDomain = Container.Resolve<IDomainService<ResolutionRospotrebnadzor>>();
            if (UIN != null && UIN != "")
            {
                GisGmp gisGmp = GisGmpDomain.GetAll()
                                .Where(x => x.UIN == UIN).FirstOrDefault();
                if (gisGmp != null)
                {
                    return gisGmp;
                }
            }
            List<string> dates = new List<string>();
            if (purpose != null && purpose != "")
            {
                purpose = Regex.Replace(purpose, @"(\d\d).(\d\d).(\d\d\d\d)", "$1.$2.$3", RegexOptions.IgnoreCase);
                MatchCollection datesTxt = Regex.Matches(purpose, @"\d\d[.]\d\d[.]\d\d\d\d");
                foreach (Match date in datesTxt)
                {
                    try
                    {
                        dates.Add(DateTime.ParseExact(date.Value, "dd.MM.yyyy", CultureInfo.InvariantCulture).ToShortDateString());
                    }
                    catch { }
                }
            }
            if (dates.Any())
            {
                // все начисления, имеющие ссылку на документ, кроме относящихся к лицензированию
                var gisGmpCollection = GisGmpDomain.GetAll()
                    .Where(x => x.Protocol != null)
                    .Where(x => x.TypeLicenseRequest == TypeLicenseRequest.NotSet)
                    .ToList();
                foreach (GisGmp charge in gisGmpCollection)
                {
                    var document = DocumentGjiDomain.GetAll()
                            .Where(y => y == charge.Protocol).FirstOrDefault();
                    if (document != null)
                    {

                        if (document.TypeDocumentGji == TypeDocumentGji.Protocol ||
                            document.TypeDocumentGji == TypeDocumentGji.Protocol197 ||
                            //document.TypeDocumentGji == TypeDocumentGji.ProtocolGZHI ||
                            document.TypeDocumentGji == TypeDocumentGji.ProtocolMhc ||
                            document.TypeDocumentGji == TypeDocumentGji.ProtocolMvd ||
                            //document.TypeDocumentGji == TypeDocumentGji.ProtocolProsecutor ||
                            document.TypeDocumentGji == TypeDocumentGji.ProtocolRSO ||
                            document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ||
                            document.TypeDocumentGji == TypeDocumentGji.ResolutionRospotrebnadzor)
                        {
                            var docChildrenParent = DocumentGjiChildrenDomain.GetAll()
                                .Where(y => y.Parent == document).FirstOrDefault();
                            if (docChildrenParent != null)
                            {
                                var resolution = ResolutionDomain.GetAll()
                                    .Where(y => y == docChildrenParent.Children).FirstOrDefault();
                                if (resolution != null)
                                {
                                    if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                                    {
                                        if (purpose.Contains(resolution.DocumentNumber))
                                        {
                                            foreach (string date in dates)
                                            {
                                                if (date == resolution.DocumentDate.Value.ToShortDateString())
                                                {
                                                    return charge;
                                                }
                                            }
                                        }
                                    }
                                    if ((resolution.DecisionNumber != null) && (resolution.DecisionNumber != ""))
                                    {
                                        if (purpose.Contains(resolution.DecisionNumber))
                                        {
                                            foreach (string date in dates)
                                            {
                                                if (date == resolution.DecisionDate.Value.ToShortDateString())
                                                {
                                                    return charge;
                                                }
                                            }
                                        }
                                    }
                                    if ((resolution.ExecuteSSPNumber != null) && (resolution.ExecuteSSPNumber != ""))
                                    {
                                        if (purpose.Contains(resolution.ExecuteSSPNumber))
                                        {
                                            foreach (string date in dates)
                                            {
                                                if (date == resolution.DateExecuteSSP.Value.ToShortDateString())
                                                {
                                                    return charge;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (document.TypeDocumentGji == TypeDocumentGji.Resolution)
                        {
                            var resolution = ResolutionDomain.GetAll()
                                .Where(y => y == document).FirstOrDefault();
                            if (resolution != null)
                            {
                                if ((resolution.DocumentNumber != null) && (resolution.DocumentNumber != ""))
                                {
                                    if (purpose.Contains(resolution.DocumentNumber))
                                    {
                                        foreach (string date in dates)
                                        {
                                            if (date == resolution.DocumentDate.Value.ToShortDateString())
                                            {
                                                return charge;
                                            }
                                        }
                                    }
                                }
                                if ((resolution.DecisionNumber != null) && (resolution.DecisionNumber != ""))
                                {
                                    if (purpose.Contains(resolution.DecisionNumber))
                                    {
                                        foreach (string date in dates)
                                        {
                                            if (date == resolution.DecisionDate.Value.ToShortDateString())
                                            {
                                                return charge;
                                            }
                                        }
                                    }
                                }
                                if ((resolution.ExecuteSSPNumber != null) && (resolution.ExecuteSSPNumber != ""))
                                {
                                    if (purpose.Contains(resolution.ExecuteSSPNumber))
                                    {
                                        foreach (string date in dates)
                                        {
                                            if (date == resolution.DateExecuteSSP.Value.ToShortDateString())
                                            {
                                                return charge;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // если нету дат в назначении платежа, не привязываем такой платёж только по номеру документа
            return null;
        }

        public IDataResult ListPayments(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("GisGmpId", 0L);
            var data = PayRegDomain.GetAll()
            .Where(x => x.GisGmp != null && x.GisGmp.Id == id)
           .Select(x => new
           {
               x.Id,
               x.Amount,
               x.Kbk,
               x.OKTMO,
               x.GisGmp,
               x.PaymentDate,
               x.PaymentId,
               x.Purpose,
               x.SupplierBillID,
               PaymentOrg = x.PaymentOrg != null ? x.PaymentOrg : "Неизвестно",
               PaymentOrgDescr = x.PaymentOrgDescr != null ? x.PaymentOrgDescr : "",
               PayerId = x.PayerId != null ? x.PayerId : "",
               PayerAccount = x.PayerAccount != null ? x.PayerAccount : "",
               PayerName = x.PayerName != null ? x.PayerName : "",
               BdiStatus = x.BdiStatus != null ? x.BdiStatus : "",
               BdiPaytReason = x.BdiPaytReason != null ? x.BdiPaytReason : "",
               BdiTaxPeriod = x.BdiTaxPeriod != null ? x.BdiTaxPeriod : "",
               BdiTaxDocNumber = x.BdiTaxDocNumber != null ? x.BdiTaxDocNumber : "",
               BdiTaxDocDate = x.BdiTaxDocDate != null ? x.BdiTaxDocDate : "",
               AccDocDate = x.AccDocDate != null ? x.AccDocDate : System.DateTime.MinValue,
               AccDocNo = x.AccDocNo != null ? x.AccDocNo : "",
               Status = x.Status != null ? x.Status : 0,
               x.Reconcile,

           })
           .AsQueryable();

            var data2 = data.Select(x=> new
            {
                x.Id,
                x.Amount,
                x.Kbk,
                x.OKTMO,
                x.PaymentDate,
                x.PaymentId,
                x.Purpose,
                x.SupplierBillID,
                x.PaymentOrg,
                x.PaymentOrgDescr,
                x.PayerId,
                x.PayerAccount,
                x.PayerName,
                x.BdiStatus,
                x.GisGmp,
                x.BdiPaytReason,
                x.BdiTaxPeriod,
                x.BdiTaxDocNumber,
                x.BdiTaxDocDate,
                x.AccDocDate,
                x.AccDocNo,
                x.Status,
                x.Reconcile
               
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data2.Order(loadParams).Paging(loadParams).ToList(), data2.Count());
        }

        public IDataResult ListPaymentsForPayFine(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            //var id = loadParams.Filter.GetAs("GisGmpId", 0L);
            var data = PayRegDomain.GetAll()
            .Where(x => x.GisGmp == null)
            .Where(x => x.Reconcile != Gkh.Enums.YesNoNotSet.Yes)
           .Select(x => new
           {
               x.Id,
               x.Amount,
               x.PaymentDate,
               x.PaymentId,
               x.Purpose,
               x.SupplierBillID,
               PayerId = x.PayerId != null ? x.PayerId : "",
               PayerName = x.PayerName != null ? x.PayerName : "",
               Status = x.Status != null ? x.Status : 0
           })
           .AsQueryable();

            var data2 = data.Select(x=> new
            {
                x.Id,
                x.Amount,
                x.PaymentDate,
                x.PaymentId,
                x.Purpose,
                x.SupplierBillID,
                x.PayerId,
                x.PayerName,
                x.Status
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data2.Order(loadParams).Paging(loadParams).ToList(), data2.Count());
        }

        public IDataResult AddPayFine(BaseParams baseParams, long resolutionId, long payRegId)
        {
            var resolution = ResolutionDomain.GetAll()
                                            .Where(x => x.Id == resolutionId).FirstOrDefault();
            //var resolution = ResolutionDomain.Get(resolutionId);
            var payment = PayRegDomain.Load(payRegId);

            // gisGmp по постановлению
            var gisGmp = GisGmpDomain.GetAll()
                .Where(x => x.Protocol.Id == resolutionId).FirstOrDefault();
            if (resolution != null && gisGmp == null)
            {
                // документ протокола
                var protocolDoc = DocumentGjiChildrenDomain.GetAll()
                    .Where(x => x.Children == resolution)
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol ||
                        x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol197 ||
                        //x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolGZHI ||
                        x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolMhc ||
                        x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolMvd ||
                        //x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolProsecutor ||
                        x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolRSO ||
                        x.Parent.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ||
                        x.Parent.TypeDocumentGji == TypeDocumentGji.ResolutionRospotrebnadzor)
                        .Select(x => x.Parent).FirstOrDefault();
                if (protocolDoc != null)
                {
                    // gisGmp по протоколу
                    gisGmp = GisGmpDomain.GetAll()
                        .Where(x => x.Protocol == protocolDoc).FirstOrDefault();
                }
            }
            if (gisGmp == null)
            {
                return new BaseDataResult { Success = false, Message = "Отсутствует начисление ГИС ГМП по данному постановлению или протоколу" };
            }
            else if (resolution != null)
            {
                if (payment.GisGmp != null && payment.GisGmp != gisGmp)
                {
                    return new BaseDataResult { Success = false, Message = "Данный платёж уже привязан к другому постановлению" };
                }
                else
                {
                    if (ResolutionPayFineDomain.GetAll().Where(x => x.DocumentNum == payment.PaymentId).Any())
                    {
                        return new BaseDataResult { Success = false, Message = "Данный платёж уже привязан к этому постановлению" };
                    }
                    else
                    {
                        ResolutionPayFine payFine = new ResolutionPayFine();
                        payFine.TypeDocumentPaid = TypeDocumentPaidGji.PaymentGisGmp;
                        payFine.Amount = payment.Amount;
                        payFine.DocumentNum = payment.PaymentId;
                        payFine.DocumentDate = payment.PaymentDate;
                        payFine.GisUip = payment.PayerId;
                        payFine.Resolution = resolution;
                        ResolutionPayFineDomain.Save(payFine);
                        payment.GisGmp = gisGmp;
                        payment.IsPayFineAdded = true;
                        PayRegDomain.Update(payment);
                        CalcBalance(resolution);
                        //ResolutionDomain.Update(resolution);
                        return new BaseDataResult { Success = true };
                    }
                }
            }
            else
            {
                return new BaseDataResult { Success = false, Message = "Не найдено постановление" };
            }
        }

        #endregion

        #region Private methods

        private string GetTimeStampUuid()
        {
            return GUIDHelper.GenerateTimeBasedGuid().ToString();
        }

        private void CalcBalance(Resolution resolution)
        {
            try
            {
                // Заполняем поле "Штраф оплачен (подробно)"
                var resolutionPayFineSum = ResolutionPayFineDomain
                         .GetAll()
                         .Where(x => x.Resolution.Id == resolution.Id)
                         .Sum(x => x.Amount)
                         .ToDecimal();
                var maxpaymentdate = ResolutionPayFineDomain
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
                ResolutionRepo.Update(resolution);
            }
            catch (Exception e)
            {

            }
        }

        private DateTime? NullableDateParse(string value)
        {
            if (value == null)
                return null;

            DateTime result;

            return (DateTime.TryParse(value, out result) ? result : (DateTime?)null);
        }

        private string ParsePaymentOrg(XElement element)
        {
            string org = "";
            if (element.Element(orgNamespace + "Bank") != null)
            {
                org = "Банк";
            }
            else
            {
                if (element.Element(orgNamespace + "UFK") != null)
                {
                    org = "ТОФК или другая организация";
                };
                if (element.Element(orgNamespace + "Other") != null)
                {
                    org = org == "" ? "Иной способ" : org + ", иной способ";
                }
            }
            return org;
        }

        private string ParsePaymentOrgDescr(XElement element)
        {
            string descr = "";
            if (element.Element(orgNamespace + "Bank") != null)
            {
                descr = element.Element(orgNamespace + "Bank").Attribute("bik").Value;
            }
            else
            {
                if (element.Element(orgNamespace + "UFK") != null)
                {
                    descr = element.Element(orgNamespace + "UFK").Value;
                };
                if (element.Element(orgNamespace + "Other") != null)
                {
                    descr = descr == "" ? element.Element(orgNamespace + "Other").Value : descr + ", " + element.Element(orgNamespace + "Other").Value;
                }
            }
            return descr;
        }
               
        /// <summary>
        /// Генерация XML запроса о платежах с TimeInterval
        /// </summary>
        private XElement GetPaymentRequestXML(PayRegRequests requestData, string originatorId)
        {
            var identifiers = GetIdentifiersFromConfig();

            var paymentsRequest = new ExportPaymentsProxy.ExportPaymentsRequest
            {
                Id = $"ID_{ requestData.Id }",
                timestamp = DateTime.Now,
                senderIdentifier = identifiers.SenderIdentifier,
                originatorId = originatorId,
                senderRole = identifiers.SenderRole,
                Paging = new ExportPaymentsProxy.PagingType
                {
                    pageNumber = "1",
                    pageLength = "1000"
                },
                PaymentsExportConditions = new ExportPaymentsProxy.PaymentsExportConditions
                {
                    kind = requestData.PayRegPaymentsKind.ToString().Replace('_', '-'),
                    ItemElementName = ExportPaymentsProxy.ItemChoiceType.TimeConditions,
                    Item = new ExportPaymentsProxy.TimeConditionsType
                    {
                        TimeInterval = new ExportPaymentsProxy.TimeIntervalType
                        {
                            startDate = requestData.GetPaymentsStartDate.HasValue ? requestData.GetPaymentsStartDate.Value : DateTime.Now.AddDays(-10),
                            endDate = requestData.GetPaymentsEndDate.HasValue ? requestData.GetPaymentsEndDate.Value : DateTime.Now.AddDays(-10)
                        }
                    }
                }
            };

            return ToXElement<ExportPaymentsProxy.ExportPaymentsRequest>(paymentsRequest);
        }
               
        /// <summary>
        /// Получает УРН участника-отправителя запроса из конфига
        /// </summary>
        private Identifiers GetIdentifiersFromConfig()
        {
            var configProvider = Container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("Bars.GkhGji.Regions.Voronezh");
            var identifiers = new Identifiers
            {
                SenderIdentifier = config.GetAs("SenderIdentifier", (string)null, true),
                SenderRole = config.GetAs("SenderRole", (string)null, true),
                OriginatorID = config.GetAs("OriginatorID", (string)null, true),
            };

            if (String.IsNullOrEmpty(identifiers.SenderIdentifier))
                throw new ApplicationException("Не найден SenderIdetifier в конфиге модуля Bars.GkhGji.Regions.Voronezh");
            if (String.IsNullOrEmpty(identifiers.SenderRole))
                throw new ApplicationException("Не найден SenderRole в конфиге модуля Bars.GkhGji.Regions.Voronezh");
            if (String.IsNullOrEmpty(identifiers.OriginatorID))
                throw new ApplicationException("Не найден OriginatorID в конфиге модуля Bars.GkhGji.Regions.Voronezh");

            return identifiers;
        }

        private int GetMeaning(GisGmpChargeType chargeType)
        {
            switch (chargeType)
            {
                case GisGmpChargeType.First:
                    return 1;
                case GisGmpChargeType.Refinement:
                    return 2;
                case GisGmpChargeType.Cancellation:
                    return 3;
                case GisGmpChargeType.ReCancellation:
                    return 4;
                default:
                    throw new Exception($"PAYREGService: не найдено значение Meaning для {chargeType}");
            }
        }

        private void SaveFile(PayRegRequests request, byte[] data, string fileName)
        {
            if (data == null)
                return;

            //сохраняем отправленный пакет
            PayRegFileDomain.Save(new PayRegFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                PayRegRequests = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(PayRegRequests request, XElement data, string fileName)
        {
            if (data == null)
                return;

            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            PayRegFile faultRec = new PayRegFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                PayRegRequests = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            };

            PayRegFileDomain.Save(faultRec);
        }

        private void SaveException(PayRegRequests request, Exception exception)
        {
            if (exception == null)
                return;

            PayRegFileDomain.Save(new PayRegFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                PayRegRequests = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }

        private void ChangeState(PayRegRequests requestData, RequestState state)
        {
            requestData.RequestState = state;
            PayRegRequestsDomain.Update(requestData);
        }

        private void SetErrorState(PayRegRequests requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            PayRegRequestsDomain.Update(requestData);
        }

        private XElement ToXElement<T>(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private T Deserialize<T>(XElement element)
        where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(element.ToString()))
                return (T)ser.Deserialize(sr);
        }

        #endregion

        #region Nested classes
        internal class Identifiers
        {
            internal string SenderIdentifier;
            internal string SenderRole;
            internal string OriginatorID;
        }

        #endregion
    }
}
