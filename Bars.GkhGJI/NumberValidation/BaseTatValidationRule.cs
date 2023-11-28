﻿namespace Bars.GkhGji.NumberValidation
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;
    using ValidateResult = Bars.B4.Modules.States.ValidateResult;
    using Gkh.Domain.CollectionExtensions;

    /// <summary>
    /// Правило проставления номера документа ГЖИ РТ
    /// </summary>
    public class BaseTatValidationRule : INumberValidationRule
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id
        {
            get { return "gji_tat_doc_number_validation"; }
        }

        /// <summary>
        /// Название валидатора
        /// </summary>
        public string Name
        {
            get { return "Правило проставления номера документа РТ"; }
        }

        /// <summary>
        /// Установка актуального номера документа ГЖИ
        /// </summary>
        /// <param name="document">Документ для проверки</param>
        /// <returns>Результат операции</returns>
        public ValidateResult Validate(DocumentGji document)
        {
            if (string.IsNullOrEmpty(document.DocumentNumber))
            {
                return ValidateResult.Yes();
            }

            var documentService = this.Container.Resolve<IDomainService<DocumentGji>>();
            var documentChildrenService = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

            using (this.Container.Using(documentService, documentChildrenService))
            {
                this.SetDocumentNumber(document, documentService, documentChildrenService);

                if (documentService.GetAll().Any(x => x.Id != document.Id
                   && x.DocumentYear == document.DocumentYear
                   && x.DocumentNumber == document.DocumentNumber
                   && x.TypeDocumentGji == document.TypeDocumentGji))
                {
                    if (document.TypeDocumentGji == TypeDocumentGji.Resolution)
                    {
                        this.SetDocumentNumber(document, documentService, documentChildrenService, true);
                    }
                    else
                    {
                        return ValidateResult.No("Документ с таким номером уже существует");
                    }
                }
            }

            return ValidateResult.Yes();
        }

        private void SetDocumentNumber(DocumentGji document, IDomainService<DocumentGji> documentService, IDomainService<DocumentGjiChildren> documentChildrenService, bool ignoreOldNumber = false)
        {
            var muCode = "";
            switch (document.TypeDocumentGji)
            {
                case TypeDocumentGji.Protocol:
                case TypeDocumentGji.Prescription:
                case TypeDocumentGji.Resolution:
                    {
                        var parentDocument =
                            documentChildrenService.GetAll()
                                .Where(x => x.Children.Id == document.Id)
                                .Select(x => new { x.Parent.DocumentNumber, x.Parent.DocumentNum, x.Parent.TypeDocumentGji, x.Parent.Id })
                                .FirstOrDefault();

                        if (parentDocument != null && (document.DocumentNum == null || ignoreOldNumber))
                        {
                            if (document.TypeDocumentGji == TypeDocumentGji.Resolution && parentDocument.TypeDocumentGji == TypeDocumentGji.ProtocolMvd)
                            {
                                // т.к. для постановлений сформированных из протокола МВД должна быть сквозная нумерация через все распоряжения и постановления прокуратуры с начала года
                                var resolutionsByProtocolMvdQuery = documentChildrenService.GetAll()
                                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolMvd)
                                    .Select(x => x.Children.Id);

                                var maxNum = documentService.GetAll()
                                    .Where(x => x.DocumentYear == document.DocumentYear)
                                    .Where(x => x.Id != document.Id)
                                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal
                                                || x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor
                                                ||
                                                (x.TypeDocumentGji == TypeDocumentGji.Resolution &&
                                                 resolutionsByProtocolMvdQuery.Contains(x.Id)))
                                    .SafeMax(x => x.DocumentNum);

                                var num = maxNum.HasValue ? maxNum.Value + 1 : 1;

                                document.DocumentNum = num;

                                muCode = this.Container.Resolve<IDomainService<ProtocolMvdRealityObject>>().GetAll()
                                    .Where(x => x.ProtocolMvd.Id == parentDocument.Id)
                                    .Select(x => x.RealityObject.Municipality.Code)
                                    .FirstOrDefault();

                                document.DocumentNumber = string.IsNullOrEmpty(muCode) ? num.ToStr() : muCode + "-" + num;
                            }
                            else if (!string.IsNullOrEmpty(parentDocument.DocumentNumber))
                            {
                                document.DocumentNumber = parentDocument.DocumentNumber;
                                document.DocumentNum = parentDocument.DocumentNum;

                                var siblingsDocumentSubNum = documentChildrenService.GetAll()
                                    .Where(x => x.Parent.Id == parentDocument.Id)
                                    .Where(x => x.Children.Id != document.Id)
                                    .Where(x => x.Children.Stage.Id == document.Stage.Id)
                                    .Where(x => x.Children.TypeDocumentGji == document.TypeDocumentGji)
                                    .Select(x => x.Children.DocumentSubNum)
                                    .ToList();

                                if (siblingsDocumentSubNum.Any())
                                {
                                    document.DocumentSubNum = siblingsDocumentSubNum.Max().ToInt() + 1;
                                }
                            }
                        }
                    }

                    break;
                default:
                    {
                        if (document.Inspection != null)
                        {
                            var muService = this.Container.Resolve<IDomainService<Municipality>>();

                            if (document.Inspection.TypeBase == TypeBase.ProsecutorsResolution)
                            {
                                var resolPros = document as ResolPros;
                                if (resolPros != null)
                                {
                                    muCode = resolPros.ReturnSafe(x => x.Municipality.Code);
                                }
                            }
                            else
                            {
                                var serviceViewDisposal = this.Container.Resolve<IDomainService<ViewDisposal>>();

                                muCode = muService.GetAll()
                                    .Where(y => y.Id ==
                                        serviceViewDisposal.GetAll()
                                            .Where(x => x.InspectionId == document.Inspection.Id && x.TypeDisposal == TypeDisposalGji.Base)
                                            .Select(x => x.MunicipalityId)
                                            .FirstOrDefault())
                                    .Select(x => x.Code)
                                    .FirstOrDefault();
                            }
                        }

                        if (!string.IsNullOrEmpty(muCode))
                        {
                            document.DocumentNumber = muCode + "-" + document.DocumentNum.ToLong().ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            document.DocumentNumber = document.DocumentNum.ToLong().ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    break;
            }

            if (document.DocumentSubNum.ToLong() > 0)
            {
                int indexOfSlash = document.DocumentNumber.IndexOf("/", StringComparison.Ordinal);

                if (indexOfSlash > -1)
                {
                    document.DocumentNumber = document.DocumentNumber.Substring(0, indexOfSlash);
                }

                document.DocumentNumber += "/" + document.DocumentSubNum.ToLong().ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}