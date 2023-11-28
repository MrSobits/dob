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

    public class ComplaintsAcceptRule : IRuleChangeStatus
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
            get { return "ComplaintsAcceptRule"; }
        }

        public string Name { get { return "Удовлетворение жалобы"; } }
        public string TypeId { get { return "gji_smev_complaints"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет отправлен отчет об удволетворении заявки на досудебное обжалование";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var taskManager = Container.Resolve<ITaskManager>();
            var complaint = statefulEntity as SMEVComplaints;

            var complaintExecutor = SMEVComplaintsExecutantDomain.GetAll()
                  .Where(x => x.IsResponsible && x.SMEVComplaints.Id == complaint.Id).FirstOrDefault();
            KndRequest acceptRequest = new KndRequest
            {
                Item = new sendComplaintEventType
                {
                    id = complaint.ComplaintId,
                    Item = new sendComplaintEventTypeResolutionStage
                    {
                      Item = new sendComplaintEventTypeResolutionStageResult
                      {
                          resolution = complaint.Answer,
                          reason = complaint.Answer,
                          signer = new userType
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
           
            if (acceptRequest != null)
            {
                var voidRequestElement = ToXElement<KndRequest>(acceptRequest);
                SMEVComplaintsRequest voidRequestRequest = new SMEVComplaintsRequest
                {
                    CalcDate = DateTime.Now,
                    TypeComplainsRequest = BaseChelyabinsk.Enums.TypeComplainsRequest.Decision,
                    TextReq = voidRequestElement.ToString(),
                    ComplaintId = complaint.Id
                };
                SMEVComplaintsRequestDomain.Save(voidRequestRequest);
                var baseParams = new BaseParams();

                if (!baseParams.Params.ContainsKey("taskId"))
                    baseParams.Params.Add("taskId", voidRequestRequest.Id.ToString());

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
