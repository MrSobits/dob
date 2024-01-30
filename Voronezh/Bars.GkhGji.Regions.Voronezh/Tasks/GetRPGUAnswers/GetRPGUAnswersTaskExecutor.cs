using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Entities;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers
{
    /// <summary>
    /// Задача на опрос и обработку ответов из СМЭВ
    /// </summary>
    public class GetRPGUAnswersTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGmp> GisGmpDomain { get; set; }
        public IDomainService<PayRegRequests> PayRegRequestsDomain { get; set; }

        private ISMEV3Service _SMEV3Service;
        private IGISGMPService _GISGMPService;
        private IPAYREGService _PAYREGService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public GetRPGUAnswersTaskExecutor(ISMEV3Service SMEV3Service, IGISGMPService gmp, IPAYREGService payreg)
        {            
            _SMEV3Service = SMEV3Service;
            _GISGMPService = gmp;
            _PAYREGService = payreg;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var processLog = new List<string>();
            SMEV3Library.Entities.GetResponseResponse.GetResponseResponse responseResult;
            try
            {
                long gisGmpCount = 0;
                long payRegCount = 0;

                do
                {
                    indicator?.Report(null, 0, $"Запрос ответа");

                    responseResult = _SMEV3Service.GetResponseAsyncSGIO("urn:GetResponse", null, null, true).GetAwaiter().GetResult();

                    //Если сервер прислал ошибку, возвращаем как есть
                    //if (responseResult.FaultXML != null)
                    //    break;

                    //Если результатов пока нет, возврат
                    //if (!responseResult.isAnswerPresent)
                    //    break;

                    indicator?.Report(null, 0, $"Обработка ответа");

                    string processedResult = null;

                    if (TryGISGMPAnswerProcessed(responseResult, ref processedResult))
                    {
                        gisGmpCount++; 
                    }

                    if (TryPAYREGAnswerProcessed(responseResult, ref processedResult))
                    {
                        payRegCount++;
                    }

                    if (DateTime.Now.Hour == 20)
                        break;
                }
                while (true);

                processLog.Add($"{gisGmpCount} сообщений обработано как начисления ГИС ГМП");
                processLog.Add($"{payRegCount} сообщений обработано как оплаты ГИС ГМП");
                return new BaseDataResult(processLog);
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

        private bool TryGISGMPAnswerProcessed(SMEV3Library.Entities.GetResponseResponse.GetResponseResponse responseResult, ref string result)
        {
            GisGmp requestData = GisGmpDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData != null)
            {
                var success = _GISGMPService.TryProcessResponse(requestData, responseResult, null);
                result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

                if (success)
                {
                    try
                    {
                        _GISGMPService.SendExportRequest(requestData);
                    }
                    catch (Exception e)
                    {

                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет сообщение, что оно - оплаты из ГИС ГМП, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryPAYREGAnswerProcessed(SMEV3Library.Entities.GetResponseResponse.GetResponseResponse responseResult, ref string result)
        {
            var success = _PAYREGService.TryProcessResponse(responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": ";

            if (success)
            {
                return true;
            }
            else return false;
        }
        #endregion
    }
}
