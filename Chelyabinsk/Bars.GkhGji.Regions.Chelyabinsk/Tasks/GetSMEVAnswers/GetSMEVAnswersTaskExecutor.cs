using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Chelyabinsk.Tasks.GetSMEVAnswers
{
    /// <summary>
    /// Задача на опрос и обработку ответов из смэв
    /// </summary>
    public class GetSMEVAnswersTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGmp> GisGmpDomain { get; set; }
        public IDomainService<GISERP> GISERPDomain { get; set; }

        public IDomainService<SMEVCertInfo> SMEVCertInfoDomain { get; set; }
        public IDomainService<PayRegRequests> PayRegRequestsDomain { get; set; }
        public IDomainService<ExternalExchangeTestingFiles> ExternalExchangeTestingFilesDomain { get; set; }
        public IDomainService<SMEVEGRUL> SMEVEGRULDomain { get; set; }
        public IDomainService<SMEVEGRIP> SMEVEGRIPDomain { get; set; }
        public IDomainService<SMEVMVD> SMEVMVDDomain { get; set; }
        public IDomainService<SMEVEGRN> SMEVEGRNDomain { get; set; }

        private ISMEV3Service _SMEV3Service;
        private IGISGMPService _GISGMPService;
        private IPAYREGService _PAYREGService;
        private ISMEVEGRULService _SMEVEGRULService;
        private ISMEVEGRIPService _SMEVEGRIPService;
        private ISMEVMVDService _SMEVMVDService;
        private IGISERPService _GISERPService;
        private ISMEVEGRNService _SMEVEGRNService;
        private ISMEVCertInfoService _SMEVCertInfoService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public GetSMEVAnswersTaskExecutor(IGISGMPService GISGMPService, IPAYREGService PAYREGService, ISMEVEGRULService SMEVEGRULService, ISMEV3Service SMEV3Service, ISMEVEGRIPService SMEVEGRIPService, ISMEVMVDService SMEVMVDService, IGISERPService GISERPService, ISMEVEGRNService SMEVEGRNService, ISMEVCertInfoService SMEVCertInfoService)
        {
            _GISGMPService = GISGMPService;
            _PAYREGService = PAYREGService;
            _SMEVEGRULService = SMEVEGRULService;
            _SMEVEGRIPService = SMEVEGRIPService;
            _SMEVMVDService = SMEVMVDService;
            _SMEV3Service = SMEV3Service;
            _GISERPService = GISERPService;
            _SMEVEGRNService = SMEVEGRNService;
            _SMEVCertInfoService = SMEVCertInfoService;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var processLog = new List<string>();
            GetResponseResponse responseResult;
            uint number = 0;

            try
            {
                //ЕГРН
                do
                {
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");
                    responseResult = _SMEV3Service.GetResponseAsync(@"urn://x-artefacts-rosreestr-gov-ru/virtual-services/egrn-statement/1.1.2", @"Request", true).GetAwaiter().GetResult();

                    //Если сервер прислал ошибку, возвращаем как есть
                    if (responseResult.FaultXML != null)
                    {
                        processLog.Add($"Запрос ЕГРН {number} - ошибка: {responseResult.FaultXML}; ");
                        break;
                    }
                    //   return new BaseDataResult(false, responseResult.FaultXML.ToString());

                    //если результатов пока нет, возврат
                    if (!responseResult.isAnswerPresent)
                    {
                        processLog.Add($"Запрос ЕГРН {number} - ответ пуст; ");
                        break;
                    }
                    indicator?.Report(null, 0, $"Обработка ответа {number}");

                    string processedResult = null;

                    if (TryEGRNAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕГРН - {processedResult}");
                    }
                    else
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработчик не найден");
                    }
                }
                while (true);
                // остальное
                do
                {
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");

                    responseResult = _SMEV3Service.GetResponseAsync(null, null, true).GetAwaiter().GetResult();

                    //Если сервер прислал ошибку, возвращаем как есть
                    if (responseResult.FaultXML != null)
                        return new BaseDataResult(false, responseResult.FaultXML.ToString());

                    //если результатов пока нет, возврат
                    if (!responseResult.isAnswerPresent)
                        return new BaseDataResult(processLog);

                    indicator?.Report(null, 0, $"Обработка ответа {number}");

                    string processedResult = null;

                    if (TryGISGMPAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ГИС ГМП - {processedResult}");
                    }
                    else if (TryPAYREGAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как оплаты из ГИС ГМП - {processedResult}");
                    }
                    else if (TryEGRULAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕГРЮЛ - {processedResult}");
                    }
                    else if (TryEGRIPAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕГРИП - {processedResult}");
                    }
                    else if (TryMVDAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как МВД - {processedResult}");
                    }
                    else if (TryGetProcecutorOfficeAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как Справочник отделов прокуратур - {processedResult}");
                    }
                    else if (TryGISERPAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как проверка ГИС ЕРП - {processedResult}");
                    }
                    else if (TrySmevCertAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как отправка сертификата - {processedResult}");
                    }
                    else
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработчик не найден");
                    }
                }
                while (true);
            }
            catch (HttpRequestException e)
            {
                return new BaseDataResult(false, $"Ошибка связи: {e.InnerException}");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Проверяет сообщение, что оно ГИС ГМП, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryGISGMPAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            GisGmp requestData = GisGmpDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ГИС ГМП

            var success = _GISGMPService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        /// <summary>
        /// Проверяет сообщение, что оно ГИС ГМП, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryGISERPAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            GISERP requestData = GISERPDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ГИС ГМП

            var success = _GISERPService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        /// <summary>
        /// Проверяет сообщение, что оно ГИС ГМП, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TrySmevCertAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            var requestData = SMEVCertInfoDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ГИС ГМП

            var success = _SMEVCertInfoService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        /// <summary>
        /// Проверяет сообщение, что оно - оплаты из ГИС ГМП, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryPAYREGAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            PayRegRequests requestData = PayRegRequestsDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение с оплатами из ГИС ГМП

            var success = _PAYREGService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        /// <summary>
        /// Проверяет сообщение, что оно ЕГРЮЛ, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryEGRULAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVEGRULDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _SMEVEGRULService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryEGRIPAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVEGRIPDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРИП

            bool success = _SMEVEGRIPService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryMVDAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVMVDDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение МВД

            bool success = _SMEVMVDService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryGetProcecutorOfficeAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = ExternalExchangeTestingFilesDomain.GetAll()
                .Where(x=> x.ClassName == "ProsecutorOffice")
                .Where(x => x.ClassDescription == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение с офисами прокуратуры

            bool success = _GISERPService.TryProcessGetProsecutorOfficeResponse(responseResult);
            result = (success ? "успешно" : "ошибка");

            return true;
        }

        private bool TryEGRNAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVEGRNDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРН

            bool success = _SMEVEGRNService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        #endregion
    }
}
