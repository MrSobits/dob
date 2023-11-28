namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Результаты проверки
    /// </summary>
    public class AuditResultExportableEntity : BaseExportableEntity<ActCheckViolation>
    {
        /// <inheritdoc />
        public override string Code => "AUDITRESULT";

        public IRepository<ActCheckRealityObject> ActCheckRealityObjectRepository { get; set; }
        public IRepository<DocumentGjiChildren> DocumentGjiChildrenRepository { get; set; }
        public IRepository<ActCheckWitness> ActCheckWitnessRepository { get; set; }
        public IRepository<InspectionGjiViolStage> InspectionGjiViolStageRepository { get; set; }
        public IRepository<TypeSurveyGoalInspGji> TypeSurveyGoalInspGjiRepository { get; set; }
        public IRepository<TypeSurveyTaskInspGji> TypeSurveyTaskInspGjiRepository { get; set; }
        public IRepository<DisposalTypeSurvey> DisposalTypeSurveyRepository { get; set; }
        public IRepository<Disposal> DisposalRepository { get; set; }
        public IRepository<DocumentGjiInspector> DocumentGjiInspectorRepository { get; set; }

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var disposalDict = this.GetDisposalDict();
            var actCheckDict = this.GetActCheckDict();
            var typeCheckDoc = 0;
            var protocolFromActCheckDocumentList = this.GetProtocolFromActCheckDocumentList();

            var violationsList = this.InspectionGjiViolStageRepository.GetAll()
                .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.ActCheck || x.Document.TypeDocumentGji == TypeDocumentGji.Protocol)
                .Select(x => x.Document.Inspection.Id)
                .Distinct()
                .ToList();

            return this.EntityRepository.GetAll()
                .Where(x => x.InspectionViolation != null)
                .Where(x => x.ActObject != null)
                .Select(x => new
                    {
                        x.Id,
                        ViolationId = x.InspectionViolation.Id,
                        InspectionId = x.ActObject.ActCheck.Inspection.Id,
                        ViolationGjiName = x.InspectionViolation.Violation.Name,
                        ViolationGjiDescription = x.InspectionViolation.Violation.Description
                    })
                .AsEnumerable()
                .GroupBy(x => x.InspectionId)
                .Select(x =>
                {
                        var disposal = disposalDict.Get(x.Key);
                        var actCheck = actCheckDict.Get(x.Key);
                        var introducedStatus = actCheckDict.Get(x.Key)?.IntroducedStatus;
                        var introducedDate = introducedStatus == 3 ? disposal?.DocumentDate : null;
                        var protocol = protocolFromActCheckDocumentList.FirstOrDefault(z => z.Id == (actCheck?.Id ?? 0));
                        var firstViolation = x.First();

                        return new ExportableRow(
                            firstViolation.Id,
                            new List<string>
                            {
                                x.Key.ToStr(), // 1. Проверка
                                this.Yes, // 2. Статус результата проверки
                                this.Yes, // 3. Вид документа результата проверки
                                typeCheckDoc == 2
                                    ? protocol?.DocumentNumber
                                    : actCheck?.Number, // 4. Номер документа результата проверки
                                typeCheckDoc == 2
                                    ? this.GetDate(protocol?.DocumentDate)
                                    : this.GetDate(actCheck?.Date), // 5. Дата составления документа результата проверки
                                violationsList.Contains(x.Key) ? this.Yes : this.No, // 6. Результат проверки

                                //Информация о выявленных нарушениях
                                x.AggregateWithSeparator(y => y.ViolationGjiDescription, ";").Cut(2000), // 7. Характер нарушения
                                string.Empty, // 8. Несоответствие поданных сведений о начале осуществления предпринимательской деятельности
                                x.AggregateWithSeparator(y => y.ViolationGjiName, ";").Cut(1000), //9. Положение нарушенного правового акта
                                string.Empty, // 10. Другие несоответствия поданных сведений
                                string.Empty, // 11. Список лиц допустивших нарушение
                                string.Empty, // 12. Орган, в который отправлены материалы о выявленных нарушениях
                                string.Empty, // 13. Дата отправления материалов в ОГВ
                                string.Empty, // 14. Перечень применённых мер обеспечения производства по делу об административном правонарушении
                                string.Empty, // 15. Информация о привлечении проверяемых лиц к административной ответственности
                                string.Empty, // 16. Информация об аннулировании или приостановлении документов, имеющих разрешительный характер
                                string.Empty, // 17. Информация об обжаловании решений органа контроля
                                this.GetDate(disposal?.StartDate), // 18. Дата начала проведения проверки
                                this.GetDate(disposal?.EndDate),   // 19. Дата окончания проведения проверки
                                actCheck?.ResolutionProsecutor?.Inspection?.Contragent?.TypeEntrepreneurship != TypeEntrepreneurship.Small
                                    ? disposal?.CountDays
                                    : string.Empty, //20. Продолжительность проведения проверки (дней)
                                actCheck?.ResolutionProsecutor?.Inspection?.Contragent?.TypeEntrepreneurship == TypeEntrepreneurship.Small
                                    ? disposal?.CountHours
                                    : string.Empty, //21. Продолжительность проведения проверки (часов)
                                actCheck?.Address.Cut(2000), // 22. Место проведения проверки
                                disposal?.Inspectors.Cut(2000), // 23. ФИО и должность лиц, проводивших проверку
                                actCheck?.Witness.Cut(2000),    // 24. ФИО и должность представителей субъекта проверки
                                actCheck?.Address.Cut(2000),    // 25. Место составления документа результата проверки
                                string.Empty, // 26. Дополнительная информация о результате проверки

                                //Информация об ознакомлении с результатом проверки
                                introducedStatus.ToStr(), // 27. Статус ознакомления с результатами проверки

                                //Информация об отказе
                                actCheck?.NotIntroduced.Cut(2000), // 28. ФИО должностного лица, отказавшегося от ознакомления с актом проверки

                                //Информация об ознакомлении
                                this.GetDate(introducedDate), // 29. Дата ознакомления
                                string.Empty, // 30. Наличие подписи
                                actCheck?.Introduced.Cut(2000), // 31. ФИО должностного лица, ознакомившегося с актом проверки

                                //Информация об отмене результата проверки
                                string.Empty, // 32. Причина отмены результата проверки
                                string.Empty, // 33. Дата отмены результата проверки
                                string.Empty, // 34. Номер решения об отмене результата проверки
                                string.Empty, // 35. Организация, принявшая решение об отмене результата проверки
                                string.Empty  // 36. Дополнительная информация об отмене результата проверки
                            });
                })
                .ToList();
        }
        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 2:
                case 3:
                case 4:
                case 5:
                case 17:
                case 18:
                case 21:
                case 22:
                case 23:
                case 24:
                case 26:
                case 27:
                case 28:
                case 30:
                case 31:
                case 32:
                case 34:
                    return row.Cells[cell.Key].IsEmpty();
                case 19:
                case 20:
                    return row.Cells[19].IsEmpty() && row.Cells[20].IsEmpty();
            }
            return false;
        };
       
        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Проверка",
                "Статус результата проверки",
                "Вид документа результата проверки",
                "Номер документа результата проверки",
                "Дата составления документа результата проверки",
                "Результат проверки",

                //Информация о выявленных нарушениях
                "Характер нарушения",
                "Несоответствие поданных сведений о начале осуществления предпринимательской деятельности",
                "Положение нарушенного правового акта",
                "Другие несоответствия поданных сведений",
                "Список лиц допустивших нарушение",
                "Орган, в который отправлены материалы о выявленных нарушениях",
                "Дата отправления материалов в ОГВ",
                "Перечень применённых мер обеспечения производства по делу об административном правонарушении",
                "Информация о привлечении проверяемых лиц к административной ответственности",
                "Информация об аннулировании или приостановлении документов, имеющих разрешительный характер",
                "Информация об обжаловании решений органа контроля",
                "Дата начала проведения проверки",
                "Дата окончания проведения проверки",
                "Продолжительность проведения проверки (дней)",
                "Продолжительность проведения проверки (часов)",
                "Место проведения проверки",
                "ФИО и должность лиц, проводивших проверку",
                "ФИО и должность представителей субъекта проверки",
                "Место составления документа результата проверки",
                "Дополнительная информация о результате проверки",

                //Информация об ознакомлении с результатом проверки
                "Статус ознакомления с результатами проверки",

                //Информация об отказе
                "ФИО должностного лица, отказавшегося от ознакомления с актом проверки",

                //Информация об ознакомлении
                "Дата ознакомления",
                "Наличие подписи",
                "ФИО должностного лица, ознакомившегося с актом проверки",

                //Информация об отмене результата проверки
                "Причина отмены результата проверки",
                "Дата отмены результата проверки",
                "Номер решения об отмене результата проверки",
                "Организация, принявшая решение об отмене результата проверки",
                "Дополнительная информация об отмене результата проверки"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "AUDITPLAN"
            };
        }

        private IList<AuditResultExportableEntity.ProtocolFromActCheckProxy> GetProtocolFromActCheckDocumentList()
        {
            return this.DocumentGjiChildrenRepository.GetAll()
                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                .Select(x => new AuditResultExportableEntity.ProtocolFromActCheckProxy
                        {
                        Id = x.Parent.Id,
                        DocumentNumber = x.Children.DocumentNumber,
                        DocumentDate = x.Children.DocumentDate
                        })
                .AsEnumerable()
                .Distinct(x => x.Id)
                .ToList();
        }

        private IDictionary<long, AuditResultExportableEntity.ActCheckProxy> GetActCheckDict()
        {
            var witnessDict = this.ActCheckWitnessRepository.GetAll()
                .Where(x => x.ActCheck != null)
                .Select(x => new
                    {
                        x.ActCheck.Id,
                        x.Fio,
                        x.Position,
                        x.IsFamiliar
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => new
                    {
                        IntroducedName = x.Where(y => y.IsFamiliar).Select(y => $"{y.Fio} {y.Position}").Distinct().AggregateWithSeparator(";"),
                        NotIntroducedName = x.Where(y => !y.IsFamiliar).Select(y => $"{y.Fio} {y.Position}").Distinct().AggregateWithSeparator(";"),
                    });

            return this.ActCheckRealityObjectRepository.GetAll()
                .WhereNotNull(x => x.RealityObject)
                .Select(x => new
                    {
                        ActCheckId = x.ActCheck.Id,
                        x.ActCheck.DocumentDate,
                        x.ActCheck.DocumentNumber,
                        x.ActCheck.Inspection.Id,
                        x.RealityObject.Address,
                    })
                .AsEnumerable()
                .Select(x =>
                {
                        var witness = witnessDict.Get(x.ActCheckId);
                        var introduced = witness?.IntroducedName;
                        var notIntroduced = witness?.NotIntroducedName;
                        var allWitness = !string.IsNullOrEmpty(introduced)
                            ? !string.IsNullOrEmpty(notIntroduced)
                                ? $"{introduced};{notIntroduced}"
                                : introduced
                            : !string.IsNullOrEmpty(notIntroduced)
                                ? notIntroduced
                                : string.Empty;

                        return new AuditResultExportableEntity.ActCheckProxy
                        {
                            Id = x.ActCheckId,
                            InspectionId = x.Id,
                            Date = x.DocumentDate,
                            Number = x.DocumentNumber,
                            Introduced = introduced,
                            NotIntroduced = notIntroduced,
                            Witness = allWitness,
                            Address = x.Address,
                            IntroducedStatus = !string.IsNullOrEmpty(introduced)
                                ? 3 // Ознакомлен
                                : !string.IsNullOrEmpty(notIntroduced)
                                    ? 2 // Отказ от ознакомления
                                    : 1 // Не ознакомлен
                        };
                })
                .GroupBy(x => x.InspectionId)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());
        }

        private IDictionary<long, DisposalProxy> GetDisposalDict()
        {
            // Цели проверок
            var purposeDict = this.TypeSurveyGoalInspGjiRepository.GetAll()
                .Select(x => new
                    {
                        x.TypeSurvey.Id,
                        x.SurveyPurpose.Name
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id, x => x.Name)
                .ToDictionary(x => x.Key, x => x.Distinct().AggregateWithSeparator(";"));

            // Задачи проверки
            var taskDict = this.TypeSurveyTaskInspGjiRepository.GetAll()
                .Select(x => new
                    {
                        x.TypeSurvey.Id,
                        x.SurveyObjective.Name
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id, x => x.Name)
                .ToDictionary(x => x.Key, x => x.Distinct().AggregateWithSeparator(";"));

            var typeSurveyGjiDict = this.DisposalTypeSurveyRepository.GetAll()
                .Select(x => new
                    {
                        x.Disposal.Id,
                        TypeSurveyId = x.TypeSurvey.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    x => new
                    {
                        Purposes = x.Select(y => purposeDict.Get(y.TypeSurveyId)).Distinct().AggregateWithSeparator(";"),
                        Tasks = x.Select(y => taskDict.Get(y.TypeSurveyId)).Distinct().AggregateWithSeparator(";"),
                    });

            var inspectorDict = this.DocumentGjiInspectorRepository.GetAll()
                .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal)
                .Where(x => x.Inspector != null)
                .Select(x => new
                    {
                        x.DocumentGji.Id,
                        x.Inspector.Fio,
                        x.Inspector.Position
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id, x => $"{x.Fio} {x.Position}")
                .ToDictionary(x => x.Key, x => x.Distinct().AggregateWithSeparator(";"));

            return this.DisposalRepository.GetAll()
                .WhereNotNull(x => x.Inspection)
                .Select(x => new
                    {
                        x.Id,
                        InspectionId = x.Inspection.Id,
                        x.DocumentDate,
                        x.TypeDisposal,
                        x.DateStart,
                        x.DateEnd,
                        x.TypeAgreementProsecutor,
                        x.TypeAgreementResult,
                        x.DocumentNumber,
                    })
                .AsEnumerable()
                .Select(x => new DisposalProxy
                    {
                        Id = x.Id,
                        InspectionId = x.InspectionId,
                        DocumentDate = x.DocumentDate,
                        Type = x.TypeDisposal,
                        TypeSurveyPurposes = typeSurveyGjiDict.Get(x.Id)?.Purposes,
                        TypeSurveyTasks = typeSurveyGjiDict.Get(x.Id)?.Tasks,
                        StartDate = x.DateStart,
                        EndDate = x.DateEnd,
                        DocumentNumber = x.DocumentNumber,
                        Inspectors = inspectorDict.Get(x.Id),
                        TypeAgreementResult = x.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement
                            ? x.TypeAgreementResult.GetDisplayName()
                            : string.Empty
                    })
                .GroupBy(x => x.InspectionId)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Id).FirstOrDefault());
        }

        private class ProtocolFromActCheckProxy
        {
            public long Id { get; set; }

            public string DocumentNumber { get; set; }

            public DateTime? DocumentDate { get; set; }
        }

        private class ActCheckProxy
        {
            public long Id { get; set; }

            public long InspectionId { get; set; }

            public DateTime? Date { get; set; }

            public string Number { get; set; }

            public string Introduced { get; set; }

            public string NotIntroduced { get; set; }

            public string Witness { get; set; }

            public string Address { get; set; }

            public int IntroducedStatus { get; set; }

            public DocumentGji ResolutionProsecutor { get; set; }
        }

        private class DisposalProxy
        {
            public long Id { get; set; }

            public long InspectionId { get; set; }

            public DateTime? DocumentDate { get; set; }

            public TypeDisposalGji? Type { get; set; }

            public string TypeSurveyPurposes { get; set; }

            public string TypeSurveyTasks { get; set; }

            public string TypeAgreementResult { get; set; }

            public DateTime? StartDate { get; set; }

            public DateTime? EndDate { get; set; }

            public string DocumentNumber { get; set; }

            public string Inspectors { get; set; }

            public string CountDays => this.EndDate.HasValue && this.StartDate.HasValue
                ? ((this.EndDate.Value.Date - this.StartDate.Value.Date).Days + 1).ToString()
                : string.Empty;

            public string CountHours => this.EndDate.HasValue && this.StartDate.HasValue
                ? (((this.EndDate.Value.Date - this.StartDate.Value.Date).Days + 1) * 8).ToString()
                : string.Empty;
        }
    }
}