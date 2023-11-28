namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using System.Web.Mvc;
    using Entities;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Voronezh.Tasks.MVDSendInformationRequest;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using System;
    //using System.IO;
    using System.Linq;
    using Enums;
    using System.Text;
    using System.Web.Mvc;
    using Bars.B4.Modules.States;
    using System.Net;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;

    public class SMEVMVDExecuteController : BaseController
    {
        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private IDomainService<FileInfo> _fileDomain;

        public SMEVMVDExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        public IDomainService<SMEVMVD> SMEVMVDDomain { get; set; }

        public IDomainService<SMEVMVDFile> SMEVMVDFileDomain { get; set; }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVMVDDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            try
            {
                _taskManager.CreateTasks(new SendMVDRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных из МВД не удалось: " + e.Message);
            }
        }


        public ActionResult GetResponce(BaseParams baseParams, Int64 taskId)
        {
            //Из-за нехватки времени все проверки ответа запускают таску на проверку всех ответоп
            var smevRequestData = SMEVMVDDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.ResponseReceived)
                return JsFailure("Ответ уже получен");

            //if (!baseParams.Params.ContainsKey("taskId"))
            //    baseParams.Params.Add("taskId", taskId);

            try
            {
                _taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), baseParams);
                return JsSuccess("Задача поставлена в очередь задач");
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на проверку ответов не удалось: " + e.Message);
            }
        }

    }
}
