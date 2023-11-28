namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Проверки
    /// </summary>
    public class AuditExportableEntity : BaseExportableEntity<InspectionGji>
    {
        /// <inheritdoc />
        public override string Code => "AUDIT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;
        public IRepository<DocumentGji> DocumentGjiRepository { get; set; }
        public IRepository<Disposal> DisposalRepository { get; set; }
        public IRepository<BaseJurPerson> BaseJurPersonRepository { get; set; }
        public IRepository<BaseLicenseApplicants> BaseLicenseApplicantsRepository { get; set; }
        public IRepository<BaseStatement> BaseStatementRepository { get; set; }
        public IRepository<BaseDispHead> BaseDispHeadRepository { get; set; }
        public IRepository<BaseProsClaim> BaseProsClaimRepository { get; set; }
        public IRepository<DisposalTypeSurvey> DisposalTypeSurveyRepository { get; set; }
        public IRepository<TypeSurveyGoalInspGji> TypeSurveyGoalInspGjiRepository { get; set; }
        public IRepository<TypeSurveyTaskInspGji> TypeSurveyTaskInspGjiRepository { get; set; }
        public IRepository<DocumentGjiInspector> DocumentGjiInspectorRepository { get; set; }

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var disposalDict = this.GetDisposalDict();
            var inspectionDict = this.GetInspectionDict();

            var endCheckDict = this.BaseJurPersonRepository.GetAll()
                .Where(x => x.State.Name == "Завершено")
                .Select(x => new
                    {
                        x.Contragent.Id,
                        x.DateStart
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id, x => x.DateStart)
                .ToDictionary(x => x.Key, x => x.OrderBy(d => d).LastOrDefault());

            var planInspectionDict = inspectionDict.Values
                .Where(x => x.PlanId.HasValue)
                .GroupBy(x => x.PlanId)
                .SelectMany(
                    x => x.OrderBy(p => p.Id)
                        .Select(
                            (p, i) => new
                            {
                                p.Id,
                                Number = (i + 1).ToString()
                            }))
                .ToDictionary(x => x.Id, x => x.Number);

            var gjiContragent = this.ProxySelectorFactory.GetSelector<GjiProxy>()
                .ProxyListCache.Values
                .Single();
            
            var documents = this.DocumentGjiRepository.GetAll()
                .WhereNotNull(x => x.Inspection)
                .Where(x => x.TypeDocumentGji != TypeDocumentGji.Protocol && x.TypeDocumentGji != TypeDocumentGji.Prescription)
                .Select(x => x.Inspection.Id);

            return this.EntityRepository.GetAll()
                .WhereNotNull(x => x.Contragent)
                .Where(x => documents.Any(y => y == x.Id))
                .AsEnumerable()
                .Select(x =>
                        {
                        var jurInspection = inspectionDict.Get(x.Id);
                        var disposal = disposalDict.Get(x.Id);
                        var baseCheck = 4; // поставить заглушку, чтобы выгружалась всегда значение = 4

                        return new ExportableRow(
                            x.Id,
                            new List<string>
                            {
                                x.Id.ToStr(), // 1. Уникальный код проверки
                                this.GetStrId(gjiContragent), // 2. Контролирующий орган
                                this.GetStrId(gjiContragent), // 3. Уникальный идентификатор функции контролирующего органа
                                this.GetType(x.TypeBase), // 4. Вид проверки
                                this.GetCheckStatus(x.State),  // 5. Статус проверки
                                this.GetPlanId(jurInspection), // 6. План проверок
                                planInspectionDict.Get(jurInspection?.Id ?? 0), // 7. Порядковый номер в плане
                                this.GetUriRegistrationNumber(jurInspection),   // 8. Учётный номер проверки в едином реестре проверок
                                this.GetUriRegistrationDate(jurInspection),     // 9. Дата присвоения учётного номера проверки
                                this.GetDate(endCheckDict.Get(jurInspection?.ContragentId ?? 0)), // 10. Дата окончания последней проверки
                                this.IsRegistred(x.State), // 11. Проверка должна быть зарегистрирована в едином реестре проверок
                                baseCheck.ToStr(), // 12. Основание регистрации проверки в едином реестре проверок
                                this.IsNotLicenseControl(x.TypeBase), // 13. Вид осуществления контрольной деятельности
                                this.GetCheckForm(jurInspection),     // 14. Форма проведения проверки
                                disposal?.DocumentNumber.Cut(255),    // 15. Номер распоряжения (приказа)
                                this.GetDate(disposal?.DocumentDate), // 16. Дата утверждения распоряжения (приказа)
                                disposal?.Inspectors, // 17. Фио и должностных лиц, уполномоченных на проведение проверки
                                string.Empty, // 18. Фио и должность экспертов, привлекаемых к проведению проверки
                                string.Empty, // 19. Тип внеплановой проверки
                                x.Contragent.Id.ToStr(), // 20. Субъект проверки – ЮЛ,ИП
                                string.Empty, // 21. Субъект проверки - Гражданин
                                x.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Small ? this.Yes : this.No, // 22. Субъект проверки является субъектом малого предпринимательства
                                x.Contragent.FactAddress.ToStr() , // 23. Место фактического осуществления деятельности
                                string.Empty, // 24. Другие физ. лица, в отношении которых проводится проверка

                                // Информация об уведомлении
                                this.Yes, // 25. Статус уведомления
                                string.Empty, // 26. Дата уведомления
                                string.Empty, // 27. Способ уведомления о проведении проверки

                                // Сведения о проверке
                                this.GetCheckReason(x, disposalDict, inspectionDict), // 28. Основание проведения проверки
                                string.Empty, // 29. Предписание, на основании которого проводится проверка
                                string.Empty, // 30. Связанная проверка
                                this.GetValueOrDefault(disposal?.TypeSurveyPurposes.Cut(2000),x.TypeBase.GetDisplayName()), // 31. Цель проведения проверки с реквизитами документов основания
                                string.Empty, // 32. Дополнительная информация об основаниях проведения проверки
                                this.GetValueOrDefault(disposal?.TypeSurveyTasks.Cut(2000), x.TypeBase.GetDisplayName()), // 33. Задачи проведения проверки
                                this.GetDate(disposal?.StartDate), // 34. Дата начала проведения проверки
                                this.GetDate(disposal?.EndDate),   // 35. Дата окончания проведения проверки
                                disposal?.CountDays,  // 36. Срок проведения проверки-Рабочих дней
                                disposal?.CountHours, // 37. Срок проведения проверки-Рабочих часов
                                string.Empty, // 38. Орган государственного надзора (контроля) и/или орган муниципального контроля
                                disposal?.TypeAgreement == TypeAgreementProsecutor.NotSet ? this.No : this.Yes, // 39. Наличие информации о согласовании проверки с органами прокуратуры
                                
                                // Информация о согласовании проведении проверки с органами прокуратуры
                                disposal?.TypeAgreementResult == TypeAgreementResult.Agreed ? this.Yes : this.No, // 40. Проверка согласована
                                disposal?.DocumentNumber.ToStr(), // 41. Номер приказа о согласовании(отказе) в проведении проверки
                                this.GetDate(disposal?.DocumentDate), // 42. Дата приказа о согласовании(отказе) в проведении проверки
                                string.Empty, // 43. Дата вынесения решения о согласовании (отказе) в проведении проверки
                                string.Empty, // 44. Место вынесения решения о согласовании (отказе) в проведении проверки
                                string.Empty, // 45. Должность подписанта
                                string.Empty, // 46. ФИО подписанта
                                string.Empty, // 47. Дополнительная информация о проверке
                                string.Empty, // 48. Причины невозможности проведения проверки

                                // Информация об изменении проверки
                                string.Empty, // 49. Причина изменения проверки
                                string.Empty, // 50. Дата изменения проверки
                                string.Empty, // 51. Номер решения об изменении проверки
                                string.Empty, // 52. Дополнительная информация об изменения проверки
                                string.Empty, // 53. Организация, принявшая решение об изменении проверки

                                // Информация об отмене проверки
                                string.Empty, // 54. Причина отмены проверки
                                string.Empty, // 55. Дата отмены проверки
                                string.Empty, // 56. Номер решения об отмене проверки
                                string.Empty, // 57. Дополнительная информация об отмене проверки
                                string.Empty  // 58. Организация, принявшая решение об отмене проверки
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
                case 1:
                case 2:
                case 3:
                case 4:
                case 6:
                case 10:
                case 12:
                case 13:
                case 21:
                case 22:
                case 24:
                case 30:
                case 32:
                case 33:
                case 38:
                case 39:
                case 40:
                case 41:
                case 48:
                case 49:
                case 53:
                case 54:
                case 57:
                    return row.Cells[cell.Key].IsEmpty();
                case 5:
                case 8:
                    if (row.Cells[3] == "1") // если проверка плановая
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 19:
                case 20:
                    return row.Cells[19].IsEmpty() && row.Cells[20].IsEmpty();
                case 25:
                case 26:
                    if (row.Cells[24] == "3") // если уведомление отправлено
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 35:
                case 36:
                    return row.Cells[35].IsEmpty() && row.Cells[36].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код проверки",
                "Контролирующий орган",
                "Уникальный идентификатор функции контролирующего органа ",
                "Вид проверки ",
                "Статус проверки",
                "План проверок",
                "Порядковый номер в плане",
                "Учётный номер проверки в едином реестре проверок",
                "Дата присвоения учётного номера проверки",
                "Дата окончания последней проверки",
                "Проверка должна быть зарегистрирована в едином реестре проверок",
                "Основание регистрации проверки в едином реестре проверок",
                "Вид осуществления контрольной деятельности",
                "Форма проведения проверки",
                "Номер распоряжения (приказа)",
                "Дата утверждения распоряжения (приказа)",
                "ФИО и должностных лиц, уполномоченных на проведение проверки",
                "ФИО и должность экспертов, привлекаемых к проведению проверки",
                "Тип внеплановой проверки",
                "Субъект проверки – ЮЛ,ИП",
                "Субъект проверки - Гражданин",
                "Субъект проверки является субъектом малого предпринимательства",
                "Место фактического осуществления деятельности",
                "Другие физ. лица, в отношении которых проводится проверка",

                // Информация об уведомлении
                "Статус уведомления",
                "Дата уведомления",
                "Способ уведомления о проведении проверки",

                // Сведения о проверке
                "Основание проведения проверки",
                "Предписание, на основании которого проводится проверка",
                "Связанная проверка",
                "Цель проведения проверки с реквизитами документов основания",
                "Дополнительная информация об основаниях проведения проверки",
                "Задачи проведения проверки",
                "Дата начала проведения проверки",
                "Дата окончания проведения проверки",
                "Срок проведения проверки-Рабочих дней",
                "Срок проведения проверки-Рабочих часов",
                "Орган государственного надзора (контроля) и/или орган муниципального контроля, с которым проверка проводится совместно",
                "Наличие информации о согласовании проверки с органами прокуратуры",

                // Информация о согласовании проведении проверки с органами прокуратуры
                "Проверка согласована",
                "Номер приказа о согласовании(отказе) в проведении проверки",
                "Дата приказа о согласовании(отказе) в проведении проверки",
                "Дата вынесения решения о согласовании (отказе) в проведении проверки",
                "Место вынесения решения о согласовании (отказе) в проведении проверки",
                "Должность подписанта",
                "ФИО подписанта",
                "Дополнительная информация о проверке",
                "Причины невозможности проведения проверки",

                // Информация об изменении проверки
                "Причина изменения проверки",
                "Дата изменения проверки",
                "Номер решения об изменении проверки",
                "Дополнительная информация об изменения проверки",
                "Организация, принявшая решение об изменении проверки",

                // Информация об отмене проверки
                "Причина отмены проверки",
                "Дата отмены проверки",
                "Номер решения об отмене проверки",
                "Дополнительная информация об отмене проверки",
                "Организация, принявшая решение об отмене проверки" 
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "GJI",
                "FRGUFUNC",
                "AUDITPLAN",
                "CONTRAGENT",
                "IND",
                "PROTOCOLAUDIT",
                "PRECEPTAUDIT"
            };
        }

        private IDictionary<long, AuditExportableEntity.DisposalProxy> GetDisposalDict()
        {
            // Цели проверок
            var purposeDict = this.TypeSurveyGoalInspGjiRepository.GetAll()
                .Select(
                    x => new
                    {
                        x.TypeSurvey.Id,
                        x.SurveyPurpose.Name
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id, x => x.Name)
                .ToDictionary(x => x.Key, x => x.Distinct().AggregateWithSeparator(";"));

            // Задачи проверки
            var taskDict = this.TypeSurveyTaskInspGjiRepository.GetAll()
                .Select(
                    x => new
                    {
                        x.TypeSurvey.Id,
                        x.SurveyObjective.Name
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id, x => x.Name)
                .ToDictionary(x => x.Key, x => x.Distinct().AggregateWithSeparator(";"));

            var typeSurveyGjiDict = this.DisposalTypeSurveyRepository.GetAll()
                .Select(
                    x => new
                    {
                        x.Disposal.Id,
                        TypeSurveyId = x.TypeSurvey.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        Purposes = x.Select(y => purposeDict.Get(y.TypeSurveyId)).Distinct().AggregateWithSeparator(";"),
                        Tasks = x.Select(y => taskDict.Get(y.TypeSurveyId)).Distinct().AggregateWithSeparator(";"),
                    });
            
            var inspectorDict = this.DocumentGjiInspectorRepository.GetAll()
                .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal)
                .Where(x => x.Inspector != null)
                .Select(
                    x => new
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
                .Select(
                    x => new
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
                .Select(
                    x => new AuditExportableEntity.DisposalProxy
                    {
                        Id = x.Id,
                        InspectionId = x.InspectionId,
                        DocumentDate = x.DocumentDate,
                        Type = x.TypeDisposal,
                        TypeAgreement = x.TypeAgreementProsecutor,
                        TypeSurveyPurposes = typeSurveyGjiDict.Get(x.Id)?.Purposes,
                        TypeSurveyTasks = typeSurveyGjiDict.Get(x.Id)?.Tasks,
                        StartDate = x.DateStart,
                        EndDate = x.DateEnd,
                        DocumentNumber = x.DocumentNumber,
                        Inspectors = inspectorDict.Get(x.Id),
                        TypeAgreementResult = x.TypeAgreementResult
                    })
                .GroupBy(x => x.InspectionId)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Id).FirstOrDefault());
        }

        private IDictionary<long, AuditExportableEntity.InspectionProxy> GetInspectionDict()
        {
            var jurPersonList = this.BaseJurPersonRepository.GetAll()
                .Select(
                    x => new AuditExportableEntity.InspectionProxy
                    {
                        Id = x.Id,
                        TypeForm = x.TypeForm,
                        UriRegistrationDate = x.UriRegistrationDate,
                        UriRegistrationNumber = x.UriRegistrationNumber,
                        ContragentId = x.Contragent.Id,
                        PlanId = x.Plan.Id,
                        Reason = x.Reason,
                        TypeBaseJurPerson = x.TypeBaseJuralPerson
                    })
                .ToList();

            var dispHeadList = this.BaseDispHeadRepository.GetAll()
                .Select(
                    x => new AuditExportableEntity.InspectionProxy
                    {
                        Id = x.Id,
                        TypeForm = x.TypeForm
                    })
                .ToList();

            var licenseAppList = this.BaseLicenseApplicantsRepository.GetAll()
                .Select(
                    x => new AuditExportableEntity.InspectionProxy
                    {
                        Id = x.Id,
                        FormCheck = x.FormCheck,
                    })
                .ToList();

            var statementList = this.BaseStatementRepository.GetAll()
                .Select(
                    x => new AuditExportableEntity.InspectionProxy
                    {
                        Id = x.Id,
                        FormCheck = x.FormCheck,
                    })
                .ToList();

            return this.BaseProsClaimRepository.GetAll()
                .Select(
                    x => new AuditExportableEntity.InspectionProxy
                    {
                        Id = x.Id,
                        TypeForm = x.TypeForm
                    })
                .ToList()
                .Union(jurPersonList)
                .Union(dispHeadList)
                .Union(licenseAppList)
                .Union(statementList)
                .ToDictionary(x => x.Id);
        }

        private string GetType(TypeBase typeBase)
        {
            return typeBase == TypeBase.PlanJuridicalPerson ? this.Yes : this.No;
        }

        private string GetCheckStatus(State state)
        {
            return state?.Name == "В работе" ? this.Yes : this.No;
        }

        private string IsRegistred(State state)
        {
            return state?.Name == "Передано в сторонние системы" ? this.Yes : this.No;
        }

        private string GetPlanId(AuditExportableEntity.InspectionProxy jurPerson)
        {
            return (jurPerson?.PlanId).ToStr();
        }

        private string GetUriRegistrationNumber(AuditExportableEntity.InspectionProxy jurPerson)
        {
            return (jurPerson?.UriRegistrationNumber).ToStr();
        }

        private string GetUriRegistrationDate(AuditExportableEntity.InspectionProxy jurPerson)
        {
            return this.GetDate(jurPerson?.UriRegistrationDate);
        }

        private string IsNotLicenseControl(TypeBase typeBase)
        {
            return typeBase != TypeBase.LicenseApplicants ? this.Yes : this.No;
        }

        private string GetCheckForm(AuditExportableEntity.InspectionProxy inspection)
        {
            if (inspection == null)
            {
                return string.Empty;
            }

            switch (inspection.TypeForm)
            {
                case TypeFormInspection.Documentary:
                    return "1";
                case TypeFormInspection.Exit:
                    return "2";
                case TypeFormInspection.ExitAndDocumentary:
                    return "3";
            }

            switch (inspection.FormCheck)
            {
                case FormCheck.Documentation:
                    return "1";
                case FormCheck.Exit:
                    return "2";
                case FormCheck.ExitAndDocumentation:
                    return "3";
            }

            return string.Empty;
        }

        private string GetCheckReason(
            InspectionGji inspection,
            IDictionary<long, AuditExportableEntity.DisposalProxy> disposalDict,
            IDictionary<long, AuditExportableEntity.InspectionProxy> planInspectionDict)
        {
            var jurPerson = planInspectionDict.Get(inspection.Id);

            switch (jurPerson?.TypeBaseJurPerson)
            {
                case TypeBaseJurPerson.StartBusinessAfter3Years:
                    return "1";
                case TypeBaseJurPerson.StateRegistrationAfter3Years:
                    return "2";
                case TypeBaseJurPerson.LastWorkAfter3Years:
                    return "3";
            }

            // Проверка исполнения предписаний
            if (disposalDict.Get(inspection.Id)?.Type == TypeDisposalGji.DocumentGji)
            {
                return "4";
            }

            switch (inspection.TypeBase)
            {
                case TypeBase.CitizenStatement:
                    return "5";
                case TypeBase.DisposalHead:
                    return "6";
                case TypeBase.LicenseApplicants:
                    return "15";
            }

            return string.Empty;
        }
        
        private class InspectionProxy
        {
            public long Id { get; set; }

            public TypeFormInspection? TypeForm { get; set; }

            public FormCheck? FormCheck { get; set; }

            public int? UriRegistrationNumber { get; set; }

            public DateTime? UriRegistrationDate { get; set; }

            public long? ContragentId { get; set; }

            public long? PlanId { get; set; }

            public string Reason { get; set; }

            public TypeBaseJurPerson? TypeBaseJurPerson { get; set; }
        }

        private class DisposalProxy
        {
            public long Id { get; set; }

            public long InspectionId { get; set; }

            public DateTime? DocumentDate { get; set; }

            public TypeDisposalGji? Type { get; set; }

            public TypeAgreementProsecutor? TypeAgreement { get; set; }

            public string TypeSurveyPurposes { get; set; }

            public string TypeSurveyTasks { get; set; }

            public TypeAgreementResult? TypeAgreementResult { get; set; }

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