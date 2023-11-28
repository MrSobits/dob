namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class InspectionGjiProvider : IInspectionGjiProvider
    {
        public IWindsorContainer Container { get; set; }

        public virtual string CodeRegion 
        {
            get { return "Tat"; }
        }

        /// <summary>
        /// Получаем правила формирования документов для ТипуДокументаГЖИ
        /// </summary>
        public virtual IDataResult GetRules(BaseParams baseParams)
        {
            var DocumentGjiDomain = Container.Resolve<IDomainService<DocumentGji>>();
            var InspectionGjiDomain = Container.Resolve<IDomainService<InspectionGji>>();

            try
            {
                var typeBase = baseParams.Params.GetAs("typeBase", 0);
                var typeDocument = baseParams.Params.GetAs("typeDocument", 0);
                var parentId = baseParams.Params.GetAs("parentId", 0);

                if (parentId == 0)
                {
                    return new BaseDataResult(false, "Для получения списка правил необходимо передать Id сущности инициатора");
                }

                if (typeDocument > 0)
                {
                    var document = DocumentGjiDomain.GetAll().FirstOrDefault(x => x.Id == parentId);

                    if (document == null)
                    {
                        return new BaseDataResult(false, string.Format("По Id {0} ненайден документ", parentId));
                    }
                    else if (typeDocument != (int)document.TypeDocumentGji)
                    {
                        return new BaseDataResult(
                            false,
                            string.Format(
                                "Запрашиваемый тип {0} не соответсвует типу документа {1}",
                                typeDocument,
                                (int)document.TypeDocumentGji));
                    }
                    else
                    {
                        var listRules = GetRules(document);
                        var dataResult = listRules.Select(x => new { x.Id, Name = x.ResultName, x.ActionUrl }).ToList();
                        return new BaseDataResult(dataResult);
                    }
                }
                else if (typeBase > 0)
                {
                    var inspection = InspectionGjiDomain.GetAll().FirstOrDefault(x => x.Id == parentId);

                    if (inspection == null)
                    {
                        return new BaseDataResult(false, string.Format("По Id {0} ненайдена проверка", parentId));
                    }
                    else if (typeBase != (int)inspection.TypeBase)
                    {
                        return new BaseDataResult(
                            false,
                            string.Format(
                                "Запрашиваемый тип {0} не соответсвует типу проверки {1}",
                                typeBase,
                                (int)inspection.TypeBase));
                    }
                    else
                    {
                        var listRules = GetRules(inspection);
                        var dataResult = listRules.Select(x => new { x.Id, Name = x.ResultName, x.ActionUrl }).ToList();
                        return new BaseDataResult(dataResult);
                    }
                }
                else
                {
                    return new BaseDataResult(
                        false,
                        "Необходимо указать либо тип проверки, либо тип документа для получения списка правил формирвоания документов");
                }
            }
            catch (Exception exc)
            {
                return new BaseDataResult(false, exc.Message);
            }
            finally
            {
                Container.Release(DocumentGjiDomain);
                Container.Release(InspectionGjiDomain);
            }
        }

        /// <summary>
        /// Получаем правила формирования документов для ТипаПроверки
        /// </summary>
        public virtual List<IInspectionGjiRule> GetRules(InspectionGji inspection)
        {
            var inspectionRulesService = Container.ResolveAll<IInspectionGjiRule>();

            try
            {
                var result = new List<IInspectionGjiRule>();

                var rules =
                    inspectionRulesService
                        .Where(x => x.TypeInspectionInitiator == inspection.TypeBase);

                foreach (var rule in rules)
                {
                    // Необходим опроверить Валидацию правила а только в случае 
                    // положительного результата доабвить его в список
                    var validation = rule.ValidationRule(inspection);

                    if (validation.Success)
                    {
                        result.Add(rule);
                    }
                }

                return result;

            }
            finally
            {
                Container.Release(inspectionRulesService);
            }
           
        }

        /// <summary>
        /// Получаем правила формирования документов для ТипаПроверки
        /// </summary>
        public virtual List<IDocumentGjiRule> GetRules(DocumentGji document)
        {
            var documentRulesService = Container.ResolveAll<IDocumentGjiRule>();

            try
            {
                var result = new List<IDocumentGjiRule>();

                var rules = documentRulesService
                        .Where(x => x.CodeRegion == CodeRegion
                                    && x.TypeDocumentInitiator == document.TypeDocumentGji)
                        .ToList();

                foreach (var rule in rules)
                {
                    // Необходим опроверить Валидацию правила а только в случае 
                    // положительного результата доабвить его в список
                    var validation = rule.ValidationRule(document);

                    if (validation.Success)
                    {
                        result.Add(rule);
                    }
                }

                return result;
            }
            finally
            {
                Container.Release(documentRulesService);
            }
        }

        /// <summary>
        /// Метод формирвоания дкоумента
        /// Документ ГЖИ может быть сформирвоан либо по основанию проверки, 
        /// либо по другому документу ГЖИ
        /// </summary>
        public virtual IDataResult CreateDocument(BaseParams baseParams)
        {
            var parentId = baseParams.Params.GetAs("parentId", 0);
            var ruleId = baseParams.Params.GetAs("ruleId", "");
            
            if (parentId == 0)
            {
                return new BaseDataResult(false, "Необходимо указать родителький документ");
            }

            if (string.IsNullOrEmpty(ruleId))
            {
                return new BaseDataResult(false, "Не указан идентификатор правила формирования документа");
            }

            var inspectionRulesService = Container.ResolveAll<IInspectionGjiRule>();
            var documentRulesService = Container.ResolveAll<IDocumentGjiRule>();
            var DocumentGjiDomain = Container.Resolve<IDomainService<DocumentGji>>();
            var InspectionGjiDomain = Container.Resolve<IDomainService<InspectionGji>>();

            try
            {
                var inspectionRule = inspectionRulesService.FirstOrDefault(x => x.CodeRegion == CodeRegion && x.Id == ruleId);
                var documentRule = documentRulesService.FirstOrDefault(x => x.CodeRegion == CodeRegion && x.Id == ruleId);

                if (inspectionRule == null && documentRule == null)
                {
                    return new BaseDataResult(false, string.Format("По идентификатору {0} не удалось определить правило формирования документа", ruleId));
                }

                if (documentRule != null)
                {
                    // Если правило по Документу, то и получаем по parentId документ ГЖИ
                    var document = DocumentGjiDomain.GetAll().FirstOrDefault(x => x.Id == parentId);

                    if (document == null)
                    {
                        return new BaseDataResult(false, string.Format("По Id {0} не найден документ", parentId));
                    }
                    else
                    {
                        // Сначала формируем входящие параметры
                        documentRule.SetParams(baseParams);

                        // Создаем документ из другого документа ГЖИ
                        return documentRule.CreateDocument(document);
                    }
                }
                else if (inspectionRule != null)
                {
                    var inspection = InspectionGjiDomain.GetAll().FirstOrDefault(x => x.Id == parentId);

                    if (inspection == null)
                    {
                        return new BaseDataResult(false, string.Format("По Id {0} не найден документ ГЖИ", parentId));
                    }
                    else
                    {
                        // Сначала формируем входящие параметры
                        inspectionRule.SetParams(baseParams);

                        // Создаем документ из другого документа ГЖИ
                        return inspectionRule.CreateDocument(inspection);
                    }
                }

                return new BaseDataResult(false, "Чтото пошло нетак" );
            }
            catch (Exception exc)
            {
                return new BaseDataResult(false, exc.Message);
            }
            finally
            {
                Container.Release(inspectionRulesService);
                Container.Release(documentRulesService);
                Container.Release(DocumentGjiDomain);
                Container.Release(InspectionGjiDomain);
            }
        }

        /// <summary>
        /// Метод создания документа по Основанию проверки - данный метод нужен для тех случаем когда Нет правил но документ надо создать из проверки
        /// Например какието специфичные проверки когда ЖЕстко в коде сказано что должна стоят ькнопка - по которой должен создаватся документ
        /// То есть не по правилам 
        /// </summary>
        public virtual IDataResult CreateDocument(InspectionGji inspection, TypeDocumentGji typeDocument)
        {
            var inspectionRulesService = Container.ResolveAll<IInspectionGjiRule>();
            try
            {
                var inspectionRule = inspectionRulesService.FirstOrDefault(x => x.CodeRegion == CodeRegion
                                        && x.TypeInspectionInitiator == inspection.TypeBase
                                        && x.TypeDocumentResult == typeDocument);

                if (inspectionRule == null)
                {
                    return new BaseDataResult(false, "Не найдено правило");
                }

                return inspectionRule.CreateDocument(inspection);
            }
            finally 
            {
                Container.Release(inspectionRulesService);
            }
        }
    }
}
