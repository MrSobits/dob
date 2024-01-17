namespace Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Постановления' из документа 'Протокол МЖК'
    /// </summary>
    public class Protocol197ToResolutionRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<Protocol197> Protocol197Domain { get; set; }

        public IDomainService<ResolutionDispute> ResolutionDisputeDomain { get; set; }
        public IDomainService<ConcederationResult> ConcederationResultDomain { get; set; }
        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "Protocol197ToResolutionRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Постановления' из документа 'Протокола 19.7'"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.Resolution"; }
        }

        public string ResultName
        {
            get { return "Постановление"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Protocol197; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Resolution; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // никаких параметров неожидаем
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем постановление
            var protocol197 = Protocol197Domain.GetAll()
                                    .Where(x => x.Id == document.Id)                                 
                                    .FirstOrDefault();

            if (protocol197 == null)
            {
                throw new Exception("Неудалось получить протокол");
            }
            Resolution resolution = null;

            //учитывать номер
            int docnum = protocol197.Inspection.InspectionNum.HasValue ? protocol197.Inspection.InspectionNum.Value : GetNextNumber();
            int num = 0;
            if (protocol197.IndividualPerson != null)
            {
                resolution = new Resolution()
                {
                    Inspection = document.Inspection,
                    TypeDocumentGji = TypeDocumentGji.Resolution,
                    GisUin = protocol197.UIN,
                    Contragent = protocol197.Contragent,
                    Executant = protocol197.Executant,
                    Position = protocol197.PersonPosition,
                    Surname = protocol197.IndividualPerson.Fio.Split(' ').Length > 2 ? protocol197.IndividualPerson.Fio.Split(' ')[0] : "",
                    FirstName = protocol197.IndividualPerson.Fio.Split(' ').Length > 2 ? protocol197.IndividualPerson.Fio.Split(' ')[1] : "",
                    Patronymic = protocol197.IndividualPerson.Fio.Split(' ').Length > 2 ? protocol197.IndividualPerson.Fio.Split(' ')[2] : "",
                    Fio = protocol197.IndividualPerson?.Fio,
                    PhysicalPerson = protocol197.IndividualPerson?.Fio,
                    PhysicalPersonInfo = protocol197.PhysicalPersonInfo,
                    PhysicalPersonDocType = protocol197.PhysicalPersonDocType,
                    PhysicalPersonDocumentNumber = protocol197.PhysicalPersonDocumentNumber,
                    PhysicalPersonDocumentSerial = protocol197.PhysicalPersonDocumentSerial,
                    PhysicalPersonIsNotRF = protocol197.PhysicalPersonIsNotRF,
                    PlaceResidence = protocol197.PlaceResidence,
                    ActuallyResidence = protocol197.ActuallyResidence,
                    BirthPlace = protocol197.BirthPlace,
                    DateBirth = protocol197.DateBirth,
                    PassportNumber = Int32.TryParse(protocol197.PhysicalPersonDocumentNumber, out num) ? Convert.ToInt32(protocol197.PhysicalPersonDocumentNumber) : 0,
                    PassportSeries = Int32.TryParse(protocol197.PhysicalPersonDocumentSerial, out num) ? Convert.ToInt32(protocol197.PhysicalPersonDocumentSerial) : 0,
                    PassportIssued = protocol197.PassportIssued,
                    INN = protocol197.INN,
                    DepartmentCode = protocol197.DepartmentCode,
                    DateIssue = protocol197.DateIssue,
                    TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
                    Sanction = new SanctionGji { Id = 2, Name = "Административный штраф" },
                    Paided = YesNoNotSet.NotSet,
                    DocumentDate = protocol197.ComissionMeeting != null ? protocol197.ComissionMeeting.CommissionDate : DateTime.Now,
                    DocumentNum = protocol197.Inspection.InspectionNum,
                    DocumentNumber = protocol197.Inspection.InspectionNum.ToString(),
                    FineMunicipality = GetFineMO(document.Id),
                    Job = protocol197.Job,
                    FamilyStatus = protocol197.FamilyStatus,
                    IndividualPerson = protocol197.IndividualPerson,
                    ConcederationResult = ConcederationResultDomain.GetAll().FirstOrDefault(x=> x.Name == "постановление о назначении административного наказания")
                };
            }
            else
            {
                resolution = new Resolution()
                {
                    Inspection = document.Inspection,
                    TypeDocumentGji = TypeDocumentGji.Resolution,
                    GisUin = protocol197.UIN,
                    Contragent = protocol197.Contragent,
                    Executant = protocol197.Executant,                  
                    PhysicalPersonInfo = protocol197.PhysicalPersonInfo,
                    PhysicalPersonDocType = protocol197.PhysicalPersonDocType,
                    PhysicalPersonDocumentNumber = protocol197.PhysicalPersonDocumentNumber,
                    PhysicalPersonDocumentSerial = protocol197.PhysicalPersonDocumentSerial,
                    PhysicalPersonIsNotRF = protocol197.PhysicalPersonIsNotRF,
                    PlaceResidence = protocol197.PlaceResidence,
                    ActuallyResidence = protocol197.ActuallyResidence,
                    BirthPlace = protocol197.BirthPlace,
                    DateBirth = protocol197.DateBirth,
                    PassportNumber = Int32.TryParse(protocol197.PhysicalPersonDocumentNumber, out num) ? Convert.ToInt32(protocol197.PhysicalPersonDocumentNumber) : 0,
                    PassportSeries = Int32.TryParse(protocol197.PhysicalPersonDocumentSerial, out num) ? Convert.ToInt32(protocol197.PhysicalPersonDocumentSerial) : 0,
                    PassportIssued = protocol197.PassportIssued,
                    INN = protocol197.INN,
                    DepartmentCode = protocol197.DepartmentCode,
                    DateIssue = protocol197.DateIssue,
                    Job = protocol197.Job,
                    FamilyStatus = protocol197.FamilyStatus,
                    TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
                    Sanction = new SanctionGji {Id = 2, Name = "Административный штраф" },
                    Paided = YesNoNotSet.NotSet,
                    FineMunicipality = GetFineMO(document.Id),
                    DocumentDate = protocol197.ComissionMeeting != null ? protocol197.ComissionMeeting.CommissionDate : DateTime.Now,
                    DocumentNum = protocol197.Inspection.InspectionNum,
                    DocumentNumber = protocol197.Inspection.InspectionNum.ToString(),
                    ConcederationResult = ConcederationResultDomain.GetAll().FirstOrDefault(x => x.Name == "постановление о назначении административного наказания")
                };
            }
            #endregion

            #region Формируем этап проверки
            // Если у родительского документа есть этап у которого есть родитель
            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.Resolution,
                    Parent = parentStage,
                    Position = 1
                };
                var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == document.Inspection.Id)
                                     .OrderByDescending(x => x.Position).FirstOrDefault();

                if (stageMaxPosition != null)
                {
                    currentStage.Position = stageMaxPosition.Position + 1;
                }

                // Фиксируем новый этап чтобы потом незабыть сохранить 
                newStage = currentStage;
            }

            resolution.Stage = currentStage;
            #endregion

            #region формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = resolution
            };
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    if (newStage != null)
                    {
                        this.InspectionStageDomain.Save(newStage);
                    }

                    this.ResolutionDomain.Save(resolution);

                    this.ChildrenDomain.Save(parentChildren);

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = resolution.Id, inspectionId = document.Inspection.Id });
        }

        public IDataResult ValidationRule(DocumentGji document)
        {
            // Короче в РТ такой замут который я до сих пор понять не могу
            // Если у последнего Постановления в субтабличке Рассмотрение есть запись с кодом '3'
            // То есть возвращено на новое рассмотрение то тогда формировать новое постановление можно
            // иначе нельзя

            // Получаем последнее постановление дочернее для данного Постановления прокуратуры


            //var lastResolutionId = ChildrenDomain.GetAll()
            //    .Where(x => x.Parent.Id == document.Id && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
            //    .OrderByDescending(x => x.Children.DocumentDate)
            //    .Select(x => x.Children.Id)
            //    .FirstOrDefault();

            //if (lastResolutionId > 0)
            //{
            //    // получив последнее постановление выясняем отправлено оно на новое рассмотрение или нет
            //    if (!ResolutionDisputeDomain.GetAll()
            //        .Any(x => x.Resolution.Id == lastResolutionId && x.CourtVerdict.Code == "3"))
            //    {
            //        return new BaseDataResult(false, "Чтобы сформировать новое постановление, необходимо чтобы текущее постановление было возвращено на новое постановление");
            //    }
            //}

            return new BaseDataResult();
        }

        private int GetNextNumber()
        {
            var maxNumber = ResolutionDomain.GetAll()
                .Max(x => x.DocumentNum);
            if (maxNumber.HasValue)
            {
                return maxNumber.Value + 1;
            }
            return 1;
        }


        private Municipality GetFineMO(long protocolId)
        {
            var articleLawType = Container.Resolve<IDomainService<Protocol197ArticleLaw>>().GetAll()
             .Where(x => x.Protocol197.Id == protocolId)
             .Select(x => x.ArticleLaw.OmsRegion).FirstOrDefault();

            if (articleLawType == OmsRegionBelonging.OMS)
            {
                var zonalInspId = Container.Resolve<IDomainService<ComissionMeetingDocument>>().GetAll()
                .Where(x => x.DocumentGji.Id == protocolId)
                .Select(x => x.ComissionMeeting.ZonalInspection.Id).FirstOrDefault();

                var municipality = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                 .Where(x => x.ZonalInspection.Id == zonalInspId)
                 .Select(x => x.Municipality).FirstOrDefault();

                return municipality;
            }
            else
            {
                return null;
            }
        }
    }
}
