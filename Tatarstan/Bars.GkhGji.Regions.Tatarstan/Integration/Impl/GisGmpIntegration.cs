namespace Bars.GkhGji.Regions.Tatarstan.Integration.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using DomainService;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using Gkh.Import;
    using Gkh.Utils;
    using GkhGji.Entities;
    using global::Quartz.Util;
    using Newtonsoft.Json;

    public class GisGmpIntegration : IGisGmpIntegration
    {
        public IWindsorContainer Container { get; set; }

        #region Upload

        private static readonly object UploadSyncLock = new object();

        public IDataResult UploadCharges()
        {
            if (!Monitor.TryEnter(UploadSyncLock))
            {
                return new BaseDataResult(false, "Отправка начислений уже выполняется");
            }

            var messages = new StringBuilder();

            try
            {
                var config = GetConfig();

                //интеграция отключена
                if (!config.GetAs<bool>("GisGmpEnable"))
                {
                    return new BaseDataResult(false, "Отправка начислений отключена");
                }

                var uri = GetUri(config, "Upload");

                var domain = Container.ResolveDomain<GisChargeToSend>();
                var resolutionDomain = Container.ResolveDomain<Resolution>();

                using (var webClient = GetWebClient(config))
                {
                    try
                    {
                        foreach (var charge in GetCharges(domain))
                        {
                            var json = GetUploadJson(charge, config);

                            var response = webClient.UploadValues(uri, "POST", new NameValueCollection
                            {
                                { "request", json }
                            });

                            var responseJson = Encoding.UTF8.GetString(response);

                            var responseObject = (ResponseJson)JsonConvert.DeserializeObject(responseJson, typeof (ResponseJson));

                            if (responseObject.ImportCharges.Data.Error.IsEmpty())
                            {
                                var charges = (ResponseJsonResult[])JsonConvert.DeserializeObject(
                                        responseObject.ImportCharges.Data.Result,
                                        typeof(ResponseJsonResult[]));

                                foreach (var importCharge in charges)
                                {
                                    if (importCharge.Error.IsEmpty())
                                    {
                                        if (!importCharge.SupplierBillId.IsEmpty())
                                        {
                                            charge.Resolution.GisUin = importCharge.SupplierBillId;

                                            resolutionDomain.Update(charge.Resolution);
                                        }

                                        charge.IsSent = true;
                                        charge.DateSend = DateTime.Now;

                                        domain.Update(charge);
                                    }
                                    else
                                    {
                                        messages.AppendLine("При отправке начисления постановления №{0} от {1} произошла ошибка: {2}".FormatUsing(
                                            charge.Resolution.DocumentNumber,
                                            charge.Resolution.DocumentDate.ToDateString(),
                                            importCharge.Error));
                                    }
                                }
                            }
                            else
                            {
                                messages.AppendLine("При отправке начисления постановления №{0} от {1} произошла ошибка: {2}"
                                    .FormatUsing(charge.Resolution.DocumentNumber,
                                        charge.Resolution.DocumentDate.ToDateString(),
                                        responseObject.ImportCharges.Data.Error));
                            }
                        }

                        return new BaseDataResult(true, messages.ToString());
                    }
                    catch (WebException e)
                    {
                        return new BaseDataResult(false, e.Message);
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Monitor.Exit(UploadSyncLock);
            }
        }

        private IEnumerable<GisChargeToSend> GetCharges(IDomainService<GisChargeToSend> domain)
        {
            return domain.GetAll()
                .Where(x => !x.IsSent)
                .ToList();
        }

        private string GetUploadJson(GisChargeToSend charge, DynamicDictionary config)
        {
            var systemCode = config.GetAs<string>("GisGmpSystemCode");

            var dummyObject = new
            {
                import_charges = new
                {
                    @params = new
                    {
                        json = new
                        {
                            system_code = systemCode,
                            charges = new[]
                            {
                                charge.JsonObject
                            }
                        }
                    },
                    type = "Action"
                }
            };

            return JsonConvert.SerializeObject(dummyObject);
        }

        #endregion Upload

        #region Load

        private static readonly object LoadSyncLock = new object();

        public IDataResult LoadPayments()
        {
            if (!Monitor.TryEnter(LoadSyncLock))
            {
                return new BaseDataResult(false, "Загрузка оплат из ГИС ГМП уже выполняется");
            }

            try
            {
                var config = GetConfig();

                //интеграция отключена
                if (!config.GetAs<bool>("GisGmpEnable"))
                {
                    return new BaseDataResult(false, "Загрузка оплат из ГИС ГМП отключена");
                }

                var inn = config.GetAs<string>("GisGmpPayeeInn");
                var kpp = config.GetAs<string>("GisGmpPayeeKpp");

                if (inn.IsEmpty())
                {
                    return new BaseDataResult(false, "Не указан ИНН МЖФ");
                }

                if (kpp.IsEmpty())
                {
                    return new BaseDataResult(false, "Не указан КПП МЖФ");
                }

                var uri = GetUri(config, "Load");

                // Всегда берем данные с самого начала, потому что начисления могут проводится задним числом
                var dateStart = DateTime.MinValue;

                using (var webClient = GetWebClient(config))
                {
                    try
                    {
                        var json = GetLoadJson(config, dateStart, DateTime.Today, inn, kpp);

                        var response = webClient.UploadValues(uri, "POST", new NameValueCollection
                        {
                            { "request", json }
                        });

                        var responseText = Encoding.UTF8.GetString(response);

                        var responseObject = (ResponseJsonLoad)JsonConvert.DeserializeObject(responseText, typeof(ResponseJsonLoad));

                        if (responseObject == null)
                        {
                            return new BaseDataResult(false, "Неверный формат ответа");
                        }

                        if (!responseObject.GetPayments.Data.Error.IsEmpty())
                        {
                            return new BaseDataResult(
                                false,
                                "При загрузке оплат произошла ошибка: " + responseObject.GetPayments.Data.Error);
                        }

                        var payments = (GisPaymentJson[])JsonConvert.DeserializeObject(
                                responseObject.GetPayments.Data.Payments,
                                typeof(GisPaymentJson[]));

                            ProcessPayments(payments);
                    }
                    catch (WebException e)
                    {
                        return new BaseDataResult(false, e.Message);
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Monitor.Exit(LoadSyncLock);
            }

            return new BaseDataResult();
        }

        private void ProcessPayments(GisPaymentJson[] payments)
        {
            var logImport = Container.Resolve<ILogImport>();
            var logManager = Container.Resolve<ILogImportManager>();

            var now = DateTime.Now;

            logManager.FileNameWithoutExtention = string.Format("gis_gmp_load");
            logManager.UploadDate = now;

            logImport.SetFileName("gis_gmp_load.json");
            logImport.ImportKey = "Загрузка оплат из ГИС ГМП";

            var gisPaymentToSave = new List<GisPayment>();
            var itemsToCreate = new List<ResolutionPayFine>();
            var itemsToUpdate = new List<ResolutionPayFine>();
            var itemsToDelete = new List<ResolutionPayFine>();

            var uins = payments.Where(x => x.Uin != null).Select(x => x.Uin).Distinct().ToList();
            var paymentsIds = payments.Where(x => x.PaymentId != null).Select(x => x.PaymentId).Distinct().ToList();

            var queryResolution = Container.ResolveDomain<Resolution>().GetAll()
                .Where(x => uins.Contains(x.GisUin));

            var payfineDomain = Container.ResolveDomain<ResolutionPayFine>();

            var cachePayfine = payfineDomain.GetAll()
                .Where(y => queryResolution.Any(x => x.Id == y.Resolution.Id))
                .Where(x => x.DocumentNum != null)
                .Where(x => paymentsIds.Contains(x.DocumentNum))
                .GroupBy(x => x.DocumentNum)
                .ToDictionary(x => x.Key, y => y.First());

            var cacheResolutions = queryResolution
                .GroupBy(x => x.GisUin)
                .ToDictionary(x => x.Key, y => y.First());

            foreach (var payment in payments)
            {
                this.ProcessOnePayment(
                    payment,
                    logImport,
                    cachePayfine,
                    cacheResolutions,
                    itemsToCreate,
                    itemsToUpdate,
                    itemsToDelete,
                    gisPaymentToSave);
            }

            try
            {
                var gisPaymentDomain = Container.ResolveDomain<GisPayment>();

                Container.InTransaction(() =>
                {
                    Save(payfineDomain, itemsToCreate);
                    Update(payfineDomain, itemsToUpdate);
                    Delete(payfineDomain, itemsToDelete);

                    Save(gisPaymentDomain, gisPaymentToSave);
                });
            }
            catch(Exception e)
            {
                logImport.Error("Ошибка сохранения", e.Message);
            }
            finally
            {
                logManager.Add(new MemoryStream(
                    Encoding.GetEncoding(1251).GetBytes(JsonConvert.SerializeObject(payments))),
                    "gis_gmp_load.json",
                    logImport);
                logManager.Save();
            }
        }

        private void ProcessOnePayment(
            GisPaymentJson payment,
            ILogImport logImport,
            Dictionary<string, ResolutionPayFine> cachePayfine,
            Dictionary<string, Resolution> cacheResolutions,
            List<ResolutionPayFine> itemsToCreate,
            List<ResolutionPayFine> itemsToUpdate,
            List<ResolutionPayFine> itemsToDelete,
            List<GisPayment> gisPaymentToSave)
        {
            var uip = payment.Uip;
            var paymentId = payment.PaymentId;
            var now = DateTime.Now;

            if (uip.IsEmpty())
            {
                logImport.Warn(
                    "Оплата за {0} на сумму {1}".FormatUsing(
                        payment.PaymentDate.ToDateTime(),
                        payment.Amount.ToDecimal()),
                    "Не указан УИП");
                return;
            }

            if (paymentId.IsEmpty())
            {
                logImport.Warn(
                    "Оплата за {0} на сумму {1}".FormatUsing(
                        payment.PaymentDate.ToDateTime(),
                        payment.Amount.ToDecimal()),
                    "Не указан уникальный идентификатор платежа");
                return;
            }

            // Платеж анулирован, нужно удалять
            if (payment.ChangeStatus == "3")
            {
                var payfine = cachePayfine.Get(paymentId);

                if (payfine == null)
                {
                    logImport.Info("Оплата УИП: {0}".FormatUsing(uip), "Оплата не существует");
                    return;
                }

                itemsToDelete.Add(payfine);

                logImport.CountChangedRows++;
            }
            else
            {
                var payfine = cachePayfine.Get(paymentId);

                // Cоздаем
                if (payfine == null)
                {
                    if (payment.Uin.IsEmpty())
                    {
                        logImport.Warn("Оплата УИП: {0}".FormatUsing(uip), "Не указан УИН");
                        return;
                    }

                    var resolution = cacheResolutions.Get(payment.Uin);

                    if (resolution == null)
                    {
                        logImport.Warn(
                            "Оплата УИП: {0}".FormatUsing(uip),
                            "Не удалось получить постановление с УИН {0}".FormatUsing(payment.Uin));
                        return;
                    }

                    payfine = new ResolutionPayFine
                    {
                        Amount = payment.Amount.ToDecimal(),
                        Resolution = resolution,
                        DocumentDate = payment.PaymentDate.ToDateTime(),
                        GisUip = uip,
                        TypeDocumentPaid = TypeDocumentPaidGji.PaymentGisGmp,
                        DocumentNum = payment.PaymentId
                    };

                    var description = "Добавлена оплата для постановления {0} от {1} на сумму {2}".FormatUsing(
                        resolution.DocumentNumber,
                        resolution.DocumentDate.ToDateString(),
                        payment.Amount);
                    logImport.Info("Оплата УИП: {0}".FormatUsing(uip), description);

                    itemsToCreate.Add(payfine);
                    logImport.CountAddedRows++;
                }
                else
                {
                    payfine.Amount = payment.Amount.ToDecimal();
                    payfine.DocumentDate = payment.PaymentDate.ToDateTime();
                    payfine.TypeDocumentPaid = TypeDocumentPaidGji.PaymentGisGmp;
                    payfine.DocumentNum = payment.PaymentId;

                    var description = "Добавлена оплата для постановления {0} от {1} на сумму {2}".FormatUsing(
                        payfine.Resolution.DocumentNumber,
                        payfine.Resolution.DocumentDate.ToDateString(),
                        payment.Amount);
                    logImport.Info("Оплата УИП: {0}".FormatUsing(uip), description);

                    itemsToUpdate.Add(payfine);
                    logImport.CountChangedRows++;
                }

                logImport.Info("Оплата УИП: {0}".FormatUsing(uip), "Добавлена информация о загруженной оплате");

                gisPaymentToSave.Add(new GisPayment
                {
                    JsonObject = payment,
                    Uip = payment.Uip,
                    DateRecieve = now,
                    PayFine = payfine
                });

                logImport.CountAddedRows++;
            }
        }

        private static void Save<T>(IDomainService<T> domain, IEnumerable<T> list) where T: IEntity
        {
            foreach (var item in list)
            {
                domain.Save(item);
            }
        }

        private static void Update<T>(IDomainService<T> domain, IEnumerable<T> list) where T : IEntity
        {
            foreach (var item in list)
            {
                domain.Update(item);
            }
        }

        private static void Delete<T>(IDomainService<T> domain, IEnumerable<T> list) where T : IEntity
        {
            foreach (var item in list)
            {
                domain.Delete(item.Id);
            }
        }

        private static string GetLoadJson(DynamicDictionary config, DateTime dateBegin, DateTime dateEnd, string inn, string kpp)
        {
            var systemCode = config.GetAs<string>("GisGmpSystemCode");

            var dummyObject = new
            {
                get_payments = new
                {
                    @params = new
                    {
                        json = new
                        {
                            system_code = systemCode,
                            date_begin = dateBegin.ToShortDateString(),
                            date_end = dateEnd.ToShortDateString(),
                            payee = new
                            {
                                payee_inn = inn,
                                payee_kpp = kpp
                            }
                        }
                    },
                    type = "Action"
                }
            };

            return JsonConvert.SerializeObject(dummyObject);
        }

        #endregion Load
        
        private static WebClient GetWebClient(DynamicDictionary config)
        {
            var wc = new WebClient();

            var proxy = config.GetAs<string>("GisGmpProxy");

            if (!proxy.IsNullOrWhiteSpace())
            {
                wc.Proxy = new WebProxy(new Uri(proxy, UriKind.RelativeOrAbsolute));

                var proxyUser = config.GetAs<string>("GisGmpProxyUser");
                var proxyPassword = config.GetAs<string>("GisGmpProxyPassword");

                if (!proxyUser.IsNullOrWhiteSpace() && !proxyPassword.IsNullOrWhiteSpace())
                {
                    wc.Proxy.Credentials = new NetworkCredential(proxyUser, proxyPassword);
                }
            }

            return wc;
        }

        private static Uri GetUri(DynamicDictionary config, string method)
        {
            var uri = config.GetAs<string>("GisGmpUri" + method);

            if (uri.IsEmpty())
            {
                throw new ArgumentNullException("GisGmpUri" + method, @"Не указан адрес");
            }

            uri = Uri.UnescapeDataString(uri);

            return new Uri(uri, UriKind.RelativeOrAbsolute);
        }

        private DynamicDictionary GetConfig()
        {
            return Container.Resolve<IGjiTatParamService>().GetConfig();
        }

        private class ResponseJson
        {
            [JsonProperty("import_charges")]
            public ResponseJsonImportBills ImportCharges { get; set; }
        }

        private class ResponseJsonImportBills
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("data")]
            public ResponseJsonData Data { get; set; }
        }

        //указаны только нужные теги, остальные за ненадобностью опущены
        private class ResponseJsonData
        {
            [JsonProperty("error")]
            public string Error { get; set; }

            [JsonProperty("result")]
            public string Result { get; set; }
        }

        /// <summary>
        /// Контейнер "result"
        /// </summary>
        private class ResponseJsonResult
        {
            [JsonProperty("supplier_billd")]
            public string SupplierBillId { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
        }

        #region Payments JSON Response

        private class ResponseJsonLoad
        {
             [JsonProperty("get_payments")]
            public ResponseJsonGetPayments GetPayments { get; set; }
        }

        private class ResponseJsonGetPayments
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("data")]
            public ResponseJsonPayData Data { get; set; }
        }

        private class ResponseJsonPayData
        {
            [JsonProperty("error")]
            public string Error { get; set; }

            [JsonProperty("payments")]
            public string Payments { get; set; }
        }
        #endregion
    }
}