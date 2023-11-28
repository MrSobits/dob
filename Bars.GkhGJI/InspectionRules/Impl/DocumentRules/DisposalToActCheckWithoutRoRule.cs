namespace Bars.GkhGji.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Enums;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;
    /// <summary>
    /// Правило создание документа 'Акт проверки' из документа 'Распоряжение' (в случае если в проверке нет домов)
    /// Существуют проверки в которых нет домов соответсвенно и нарушения идут не на дома а на организации
    /// и данное правило создает акт бездомов
    /// </summary>
    public class DisposalToActCheckWithoutRoRule : IDocumentGjiRule
    {
        #region injection
        public IWindsorContainer Container { get; set; }
        #endregion

        public virtual string CodeRegion
        {
            get { return "Tat"; }
        }

        public virtual string Id
        {
            get { return "DisposalToActCheckWithoutRoRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создания документа 'Протокол' из материалов проверки"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.ProtocolGji"; }
        }

        public virtual string ResultName
        {
            get { return "Протокол"; }
        }

        public virtual TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Disposal; }
        }

        public virtual TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Protocol; }
        }

        // тут надо принять параметры если таковые имеютя
        public virtual void SetParams(BaseParams baseParams)
        {
            // не ожидаем ни каких параметров
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            var ActCheckDomain = Container.Resolve<IDomainService<ActCheck>>();
            var DisposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var ProtocolDomain = Container.Resolve<IDomainService<Protocol>>();
            var InspectionStageDomain = Container.Resolve<IDomainService<InspectionGjiStage>>();
            var DocumentInspectorDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var ActCheckRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var ChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var DisposalProvDocDomain = Container.Resolve<IDomainService<DisposalProvidedDoc>>();

            var disposal = DisposalDomain.Get(document.Id);


            try
            {
                #region Формируем протокол
                var protocol = new Protocol()
                {
                    Inspection = document.Inspection,
                    TypeDocumentGji = TypeDocumentGji.Protocol,
                    Contragent = document.Inspection.Contragent
                };
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
                var inspectorIds = DocumentInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == document.Id)
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
                #region Сохранение
                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        if (newStage != null)
                        {
                            InspectionStageDomain.Save(newStage);
                        }

                        ProtocolDomain.Save(protocol);

                        ChildrenDomain.Save(parentChildren);

                        listInspectors.ForEach(x => DocumentInspectorDomain.Save(x));                  

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
            finally
            {
                Container.Release(ProtocolDomain);
                Container.Release(ActCheckDomain);
                Container.Release(InspectionStageDomain);
                Container.Release(DocumentInspectorDomain);
                Container.Release(ActCheckRoDomain);
                Container.Release(ChildrenDomain);
                Container.Release(DisposalProvDocDomain);
            }

        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            /*
             тут проверяем, Если Распоряжение не Основное то недаем выполнить формирование
            */

            var DisposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var InspectionRoDomain = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            try
            {
                if (document != null)
                {
                    var disposal = DisposalDomain.FirstOrDefault(x => x.Id == document.Id);

                    if (disposal == null)
                    {
                        return new BaseDataResult(false, string.Format("Не удалось получить распоряжение {0}", document.Id));
                    }
                }

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(DisposalDomain);
                Container.Release(InspectionRoDomain);
            }

        }
    }
}
