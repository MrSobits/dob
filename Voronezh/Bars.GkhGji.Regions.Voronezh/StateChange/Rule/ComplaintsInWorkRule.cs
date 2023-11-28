namespace Bars.GkhGji.Regions.Voronezh.StateChanges
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using B4.DataAccess;
    using B4.Modules.States;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.Voronezh.ASDOU;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendPaymentRequest;
    using Castle.Windsor;
    using Entities;

    public class ComplaintsInWorkRule : IRuleChangeStatus
    {
        /// <summary>
        /// Домен сервис <see cref="LogOperation"/>
        /// </summary>
        public IDomainService<LogOperation> LogOperationDomainService { get; set; }
        public IDomainService<SMEVComplaintsRequest> SMEVComplaintsRequestDomain { get; set; }
        public IDomainService<SMEVComplaints> SMEVComplaintsDomainService { get; set; }
        public IDomainService<SMEVComplaintsExecutant> SMEVComplaintsExecutantDomain { get; set; }

        public virtual IWindsorContainer Container { get; set; }

        public IComplaintsService ComplaintsService { get; set; }

        public IFileManager FileManager { get; set; }

        public string Id
        {
            get { return "ComplaintsInWorkRule"; }
        }

        public string Name { get { return "Прием жалобы в работу и назначение проверяющего"; } }
        public string TypeId { get { return "gji_smev_complaints"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет отправлен отчет о назначении исполнителя и переводу жалобы в работу";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var taskManager = Container.Resolve<ITaskManager>();
            var complaint = statefulEntity as SMEVComplaints;

            var complaintExecutor = SMEVComplaintsExecutantDomain.GetAll()
                  .Where(x => x.IsResponsible && x.SMEVComplaints.Id == complaint.Id).FirstOrDefault();
            KndRequest voidRequest = new KndRequest
            {
                Item = new sendComplaintEventType
                {
                    id = complaint.ComplaintId,
                    Item = new sendComplaintEventTypeAssignStage
                    {
                        Item = new voidType()
                    },
                    eventTime = DateTime.Now,
                    unit = new unitType
                    {
                        id = "111111",
                        Value = "подразделение"
                    }                    
                }
            };
            KndRequest executorRequest = null;
            if (complaintExecutor != null)
            {
                //отправляем этап назначения исполнителя и приемки в работу
                executorRequest = new KndRequest
                {
                    Item = new sendComplaintEventType
                    {
                        id = complaint.ComplaintId,
                        Item = new sendComplaintEventTypeAssignStage
                        {
                            Item = new sendComplaintEventTypeAssignStageAssign
                            {
                                executor = new userType
                                {
                                    name = complaintExecutor.Executant.Fio
                                }
                            }
                        },
                        eventTime = DateTime.Now,
                        unit = new unitType
                        {
                            id = "111111",
                            Value = "подразделение"
                        }
                    }
                };
            }
            if (voidRequest != null)
            {
                var voidRequestElement = ToXElement<KndRequest>(voidRequest);
                SMEVComplaintsRequest voidRequestRequest = new SMEVComplaintsRequest
                {
                    CalcDate = DateTime.Now,
                    TypeComplainsRequest = BaseChelyabinsk.Enums.TypeComplainsRequest.Void,
                    TextReq = voidRequestElement.ToString(),
                    ComplaintId = complaint.Id
                };
                SMEVComplaintsRequestDomain.Save(voidRequestRequest);
                var baseParams = new BaseParams();

                if (!baseParams.Params.ContainsKey("taskId"))
                    baseParams.Params.Add("taskId", voidRequestRequest.Id.ToString());

                taskManager.CreateTasks(new SendComplaintsCustomRequestTaskProvider(Container), baseParams);
            }

            if (executorRequest != null)
            {
                var executorRequestElement = ToXElement<KndRequest>(executorRequest);
                SMEVComplaintsRequest executorRequestRequest = new SMEVComplaintsRequest
                {
                    CalcDate = DateTime.Now,
                    TypeComplainsRequest = BaseChelyabinsk.Enums.TypeComplainsRequest.Executor,
                    TextReq = executorRequestElement.ToString(),
                    ComplaintId = complaint.Id
                };
                SMEVComplaintsRequestDomain.Save(executorRequestRequest);
                var baseParams = new BaseParams();

                if (!baseParams.Params.ContainsKey("taskId"))
                    baseParams.Params.Add("taskId", executorRequestRequest.Id.ToString());

                taskManager.CreateTasks(new SendComplaintsCustomRequestTaskProvider(Container), baseParams);
            }

            return ValidateResult.Yes();

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

    }
}
