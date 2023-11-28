namespace Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Dicts;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Протокола' из документа 'Постановление'
    /// </summary>
    public class ResolutionToChelyabinskProtocolRule : ResolutionToProtocolRule
    {
        public IWindsorContainer Container { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<Resolution> ResolutionDomain { get; set; }
        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }
        public IDomainService<Protocol197> Protocol197Domain { get; set; }
        public IDomainService<PhysicalPersonDocType> PhysicalPersonDocTypeDomain { get; set; }
        public IDomainService<ZonalInspectionPrefix> ZonalInspectionPrefixDomain { get; set; }
        public IDomainService<ChelyabinskProtocol> ProtocolDomain { get; set; }
        
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }
        public IDomainService<ArticleLawGji> ArticleLawGjiDomain { get; set; }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomain { get; set; }
        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }
        

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "ResolutionToProtocolRule"; }
        }

        public string Description
        {
            get { return "Правило создания документа 'Протокола' из документа 'Постановление'"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.ProtocolGji"; }
        }

        public string ResultName
        {
            get { return "Протокол"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Resolution; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Protocol; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            //никаких параметров неожидаем
        }

        public override IDataResult CreateDocument(DocumentGji document)
        {
            DateTime? protDate = document.ObjectCreateDate > DateTime.Now ? document.ObjectCreateDate : DateTime.MinValue;
            #region Формируем протокол
            var resolution = ResolutionDomain.GetAll()
                                    .Where(x => x.Id == document.Id)
                                    .Select(x => new {Id = x.Id, x.Executant, x.Contragent, x.PhysicalPerson, x.PhysicalPersonInfo, x.IndividualPerson , x.Protocol205Date, x.FirstName, x.Surname, x.Patronymic,x.Inspection, x.Stage})
                                    .FirstOrDefault();
            document = (DocumentGji)ResolutionDomain.Get(resolution.Id);


            if (resolution == null)
            {
                throw new Exception("Неудалось получить постановление");
            }

            var protocol197doc = DocumentGjiChildrenDomain.GetAll().FirstOrDefault(x => x.Children.Id == document.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol197)?.Parent;
            if (protocol197doc == null)
            {
                throw new Exception("Неудалось получить протокол на рассмотрение");
            }
            Protocol197 p197 = Protocol197Domain.Get(protocol197doc.Id);

            ChelyabinskProtocol protocol = new ChelyabinskProtocol();

            if (resolution.IndividualPerson != null)
            {
                protocol = new ChelyabinskProtocol()
                {
                    DocumentDate = resolution.Protocol205Date,
                    Inspection = document.Inspection,
                    TypeDocumentGji = TypeDocumentGji.Protocol,
                    Contragent = p197.Contragent,
                    Executant = resolution.Executant,
                    ActuallyResidence = resolution.IndividualPerson.IsActuallyResidenceOutState ? resolution.IndividualPerson.ActuallyResidenceOutState : resolution.IndividualPerson.ActuallyResidence,
                    ActuallyResidenceOutState = resolution.IndividualPerson.ActuallyResidenceOutState,
                    IsActuallyResidenceOutState = resolution.IndividualPerson.IsActuallyResidenceOutState,
                    BirthPlace = resolution.IndividualPerson.BirthPlace,
                    DateBirth = resolution.IndividualPerson.DateBirth,
                    DateIssue = resolution.IndividualPerson.DateIssue,
                    FamilyStatus = resolution.IndividualPerson.FamilyStatus,
                    Job = resolution.IndividualPerson.Job,
                    PhysicalPersonDocType = p197.PhysicalPersonDocType,
                    IsPlaceResidenceOutState = resolution.IndividualPerson.IsPlaceResidenceOutState,
                    PlaceResidence = resolution.IndividualPerson.IsPlaceResidenceOutState ? resolution.IndividualPerson.PlaceResidenceOutState : resolution.IndividualPerson.PlaceResidence,
                    PhysicalPerson = resolution.IndividualPerson.Fio,
                    PhysicalPersonDocumentNumber = resolution.IndividualPerson.PassportNumber,
                    PhysicalPersonDocumentSerial = resolution.IndividualPerson.PassportSeries,
                    PlaceResidenceOutState = resolution.IndividualPerson.PlaceResidenceOutState,
                    PhysicalPersonInfo = resolution.PhysicalPersonInfo,
                    IndividualPerson = resolution.IndividualPerson,
                    DateViolation = resolution.Protocol205Date,
                    DateOfViolation = resolution.Protocol205Date,
                    AddressPlace = resolution.IndividualPerson.ActuallyResidence,
                    FiasPlaceAddress = resolution.IndividualPerson.FiasFactAddress,
                    PersonPosition = p197.PersonPosition,
                    PhysicalPersonPosition = p197.PersonPosition,
                    HourOfViolation = 0,
                    MinuteOfViolation = 1
                };
            }
            else
            {
                protocol = new ChelyabinskProtocol()
                {
                    DocumentDate = resolution.Protocol205Date,
                    Inspection = document.Inspection,
                    TypeDocumentGji = TypeDocumentGji.Protocol,
                    Contragent = p197.Contragent,
                    Executant = resolution.Executant,
                    PhysicalPerson = resolution.PhysicalPerson,                  
                    PhysicalPersonInfo = resolution.PhysicalPersonInfo,
                    PhysicalPersonDocType = p197.PhysicalPersonDocType,
                    AddressPlace = resolution.Contragent != null? resolution.Contragent.JuridicalAddress:"",
                    FiasPlaceAddress = resolution.Contragent != null ? resolution.Contragent.FiasJuridicalAddress :null,
                    DateViolation = resolution.Protocol205Date,
                    DateOfViolation = resolution.Protocol205Date,
                    HourOfViolation = 0,
                    MinuteOfViolation = 1
                };
            }

            switch (p197.Executant.Code)
            {
                case "8":
                    protocol.AddressPlace = resolution.IndividualPerson.ActuallyResidence;
                    protocol.FiasPlaceAddress = resolution.IndividualPerson.FiasFactAddress;
                    break;
                case "0":
                    protocol.AddressPlace = resolution.Contragent != null ? resolution.Contragent.JuridicalAddress : "";
                    protocol.FiasPlaceAddress = resolution.Contragent != null ? resolution.Contragent.FiasJuridicalAddress : null;
                    break;
                default:
                    protocol.AddressPlace = resolution.IndividualPerson.ActuallyResidence;
                    protocol.FiasPlaceAddress = resolution.IndividualPerson.FiasFactAddress;
                    break;
            }

            if (protDate != DateTime.MinValue)
            {
                protocol.DocumentDate = protDate;
                protocol.DateOfProceedings = protDate;
                protocol.HourOfProceedings = 9;
                protocol.MinuteOfProceedings = 30;
                protocol.FormatHour = 0;
                protocol.FormatMinute = 1;
            }

          
            #endregion

            #region Формируем этап протокола
            // Если у родительского документа есть этап у которого есть родитель
            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Protocol);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.Protocol,
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

            protocol.Stage = currentStage;
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = protocol
            };
            #endregion

            #region Формируем Инспекторов тянем их из родительского документа
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.DocumentInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == p197.Id)
                    .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var id in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = protocol,
                    Inspector = new Inspector { Id = id }
                });
            }
            #endregion

            #region Формируем Статью закона

            var artlaw = this.ArticleLawGjiDomain.GetAll().Where(x => x.Name.Contains("20.25"))
                    .FirstOrDefault();
            #endregion

            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
                throw new Exception("Создание документов доступно только для членов комиссии");

            var inspector = thisOperator.Inspector;
            var zonal = ZonalInspectionInspectorDomain.GetAll()
                .FirstOrDefault(x => x.Inspector == inspector)?.ZonalInspection;

            string pref = ZonalInspectionPrefixDomain.GetAll().FirstOrDefault(x => x.ZonalInspection == zonal)?.ProtocolPrefix;
            if (!string.IsNullOrEmpty(pref))
            {
                var protNum = GetNextNumber();
                protocol.DocumentNum = protNum;
                protocol.DocumentNumber = $"{pref}20.25/{protNum}";
            }

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    if (newStage != null)
                    {
                        this.InspectionStageDomain.Save(newStage);
                    }

                    this.ProtocolDomain.Save(protocol);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    if (artlaw != null)
                    {
                        ProtocolArticleLawDomain.Save(new ProtocolArticleLaw
                        {
                            ArticleLaw = artlaw,
                            Protocol = protocol,
                            Description = "Сформировано из постановления"
                        });
                    }

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = protocol.Id, inspectionId = document.Inspection.Id });
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult();
        }

        private int GetNextNumber()
        {
            Operator thisOperator = UserManager.GetActiveOperator();
            var inspector = thisOperator.Inspector;
            var zonal = ZonalInspectionInspectorDomain.GetAll()
                .FirstOrDefault(x => x.Inspector == inspector)?.ZonalInspection;
            string pref = ZonalInspectionPrefixDomain.GetAll().FirstOrDefault(x => x.ZonalInspection == zonal)?.ProtocolPrefix;

            var maxnuber = ProtocolDomain.GetAll()
                .Where(x => x.DocumentNumber.StartsWith($"{pref}20.25/"))
                .Where(x => x.DocumentNum.HasValue)
                .Max(x => x.DocumentNum);

            if (maxnuber.HasValue && maxnuber > 0)
            {
                return maxnuber.Value + 1;
            }

            else
            {
                return 1;
            }
        }
    }
}
