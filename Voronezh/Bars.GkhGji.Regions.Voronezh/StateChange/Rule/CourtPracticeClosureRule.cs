namespace Bars.GkhGji.Regions.Voronezh.StateChanges
{
    using B4.Modules.States;
    using Bars.B4;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Voronezh.Entities;
    using Bars.GkhGji.Regions.Voronezh.Enums;
    using Castle.Windsor;
    using System;
    using System.Linq;

    public class CourtPracticeClosureRule : IRuleChangeStatus
    {
        public virtual IWindsorContainer Container { get; set; }

        public virtual IDomainService<CourtPractice> CourtPracticeDomain { get; set; }
        public virtual IDomainService<Resolution> ResolutionDomain { get; set; }
        public virtual IDomainService<ResolutionDispute> ResolutionDisputeDomain { get; set; }
        public virtual IDomainService<TypeCourtGji> TypeCourtDomain { get; set; }
        public virtual IDomainService<CourtVerdictGji> CourtVerdictDomain { get; set; }

        public string Id
        {
            get { return "CourtPracticeClosureRule"; }
        }

        public string Name { get { return "Закрытие судебной практики"; } }
        public string TypeId { get { return "courtpractice"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса отправляет данные в постановление и проставляет дату закрытия";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var courtPractice = statefulEntity as CourtPractice;

            if (courtPractice == null)
            {
                return ValidateResult.No("Сущность не является судебной практикой");
            }

            if (newState.FinalState)
            {
                this.Action(courtPractice);
            }

            return ValidateResult.Yes();
        }

        protected void Action(CourtPractice courtPractice)
        {
            var resolution = ResolutionDomain.Get(courtPractice.DocumentGji.Id);

            ResolutionDisputeDomain.Save(new ResolutionDispute
            {
                Resolution = resolution,
                Court = GetTypeCourtFromJurInstitution(courtPractice.JurInstitution),
                Instance = courtPractice.InstanceGji,
                CourtVerdict = GetCourtVerdictFrom(courtPractice.CourtMeetingResult),
                DocumentDate = courtPractice.InLawDate,
                DocumentNum = courtPractice.DocumentNumber,
                Description = courtPractice.Discription,
                Appeal = GkhGji.Enums.ResolutionAppealed.Law,
                File = courtPractice.FileInfo
            });

            SetResolutionStatus(resolution, courtPractice.CourtMeetingResult);

            courtPractice.ClosureDate = DateTime.Now;
            CourtPracticeDomain.Update(courtPractice);
        }

        private TypeCourtGji GetTypeCourtFromJurInstitution(JurInstitution institution)
        {
            string typeName = "";
            switch (institution.CourtType)
            {
                case Gkh.Modules.ClaimWork.Enums.CourtType.Magistrate:
                    typeName = "Мировой";
                    break;

                case Gkh.Modules.ClaimWork.Enums.CourtType.Arbitration:
                    typeName = "Арбитражный";
                    break;

                case Gkh.Modules.ClaimWork.Enums.CourtType.District:
                    typeName = "Районный";
                    break;

                default:
                    typeName = "";
                    break;
            }

            if (typeName != "")
            {
                return TypeCourtDomain.GetAll()
                .Where(x => x.Name == typeName)
                .FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private CourtVerdictGji GetCourtVerdictFrom(CourtMeetingResult result)
        {
            string typeName = "";
            switch (result)
            {
                case CourtMeetingResult.Denied:
                    typeName = "Оставлено без изменения";
                    break;

                case CourtMeetingResult.PartiallySatisfied:
                    typeName = "Изменено";
                    break;

                case CourtMeetingResult.CompletelySatisfied:
                    typeName = "Отменено";
                    break;

                case CourtMeetingResult.Returned:
                    typeName = "Возвращено";
                    break;

                default:
                    typeName = "";
                    break;
            }

            if (typeName != "")
            {
                return CourtVerdictDomain.GetAll()
                .Where(x => x.Name == typeName)
                .FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private void SetResolutionStatus(Resolution res, CourtMeetingResult meetingResult)
        {
            switch (meetingResult)
            {
                case CourtMeetingResult.PartiallySatisfied:
                    res.DischargedByCourt = false;
                    res.SentToNewConcederation = false;
                    res.ChangedByCourt = true;
                    break;

                case CourtMeetingResult.CompletelySatisfied:
                    res.DischargedByCourt = true;
                    res.SentToNewConcederation = false;
                    res.ChangedByCourt = false;
                    break;

                case CourtMeetingResult.Returned:
                    res.DischargedByCourt = false;
                    res.SentToNewConcederation = true;
                    res.ChangedByCourt = false;
                    break;

                default:
                    break;
            }

            ResolutionDomain.Update(res);
        }
    }
}
