namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Короче это типа меню для проверок , его можно в регионах переделывать и елать свои меню если требуется
    /// </summary>
    public class InspectionMenuService : IInspectionMenuService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGji> InspectionDomain { get; set; }

        public IDomainService<DocumentGji> DocumentDomain { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public virtual IDataResult GetMenu(long inspectionId, long? documentId)
        {
            var list = new List<MenuItem>();

            if (inspectionId > 0)
            {

                var inspection = InspectionDomain.GetAll().FirstOrDefault(x => x.Id == inspectionId);

                if (inspection != null)
                {
                    list.AddRange(GetMenuItems(inspection));
                }
                if (documentId.HasValue && documentId.Value > 0)
                {
                    int i = 0;
                    foreach (var item in list)
                    {
                        if (item.Options.ContainsKey("documentId"))
                        {
                            var docId = item.Options.GetAs<long?>("documentId");
                            if (docId == documentId)
                            {
                                list[i].Options.Add("selected",true);
                            }
                            int k = 0;
                            foreach (var subItem in item.Items)
                            {
                                var subdocId = subItem.Options.GetAs<long?>("documentId");
                                if (subdocId == documentId)
                                {
                                    list[i].Items[k].Options.Add("selected", true);
                                }
                                k++;
                            }
                        }
                        i++;
                    }
                }

                if (!list.Any())
                {
                    //Если вообще нет никаких пунктов то ставим прост очтобы пользователь видел не пустой список
                    var item = new MenuItem { Caption = "Разделы отсутствуют" };
                    list.Add(item);
                }
            }

            return new BaseDataResult(list);
        }

        public virtual IList<MenuItem> GetMenuItems(InspectionGji inspection)
        {
            var menu = new List<MenuItem>();

            if (inspection.TypeBase != TypeBase.ProsecutorsResolution 
                && inspection.TypeBase != TypeBase.ActivityTsj
                && inspection.TypeBase != TypeBase.HeatingSeason 
                && inspection.TypeBase != TypeBase.ProtocolMvd
                && inspection.TypeBase != TypeBase.AdministrativeCase
                && inspection.TypeBase != TypeBase.ProtocolMhc
                && inspection.TypeBase != TypeBase.ProtocolRSO
                && inspection.TypeBase != TypeBase.Protocol197)
            {
                // Если это не Постановление прокуратыры, Не Деятельность ТСЖ и не Отопительный сезон и не Протокол МВД
                // То у проверки есть основание, которое будет идти первым пунктом в дереве
                menu.Add(ItemBaseItem(inspection));
            }

            // Получаем количество документов по каждому этапу 
            var dictCounts =
                DocumentDomain.GetAll()
                              .Where(x => x.Stage.Inspection.Id == inspection.Id)
                              .Select(x => new { x.Id, StageId = x.Stage.Id, x.DocumentNumber })
                              .AsEnumerable()
                              .GroupBy(x => x.StageId)
                              .ToDictionary(
                                  x => x.Key,
                                  y =>
                                  y.Select(x => new DocumentProxy { Id = x.Id, DocumentNumber = x.DocumentNumber })
                                   .ToList());

            // Получаем все этапы проверки
            // За исключением этапа Акты проверки предписаний (они же акты устранения нарушений)
            // Потому что Акты проверки предписаний показываются через карточку Акта проверки во вкладке Акты проверки предписаний
            var dictStages =
                InspectionStageDomain.GetAll()
                                     .Where(
                                         x => x.Inspection.Id == inspection.Id && x.TypeStage != TypeStage.ActRemoval)
                                     .Select(
                                         x =>
                                         new
                                             {
                                                 x.Id,
                                                 x.TypeStage,
                                                 Parent = x.Parent == null ? 0 : x.Parent.Id,
                                                 x.Position
                                             })
                                     .OrderBy(x => x.Parent)
                                     .ThenBy(x => (int)x.TypeStage)
                                     .AsEnumerable()
                                     .GroupBy(x => x.Parent)
                                     .ToDictionary(
                                         x => x.Key,
                                         y =>
                                         y.Select(
                                             z =>
                                             new InspectionGjiStageProxyRow
                                                 {
                                                     Id = z.Id,
                                                     TypeStage = z.TypeStage,
                                                     Parent = z.Parent,
                                                     Position = z.Position
                                                 })
                                                .OrderBy(x => x.Position)
                                                .ThenBy(x => (int)x.TypeStage)
                                                .ToList());

            foreach (var item in dictStages.Where(item => item.Key == 0))
            {
                // Если есть Этап но нет ниодного документа то недобавляем этап в меню
                menu.AddRange(item.Value
                    .Where(stage => dictCounts.ContainsKey(stage.Id))
                    .OrderBy(stage => stage.Position)
                    .Select(stage => InitItem(inspection, stage, dictStages, dictCounts)));
            }

            return menu;
        }

        /// <summary>
        /// Метод получения узла для Основания проверки
        /// </summary>
        public virtual MenuItem ItemBaseItem(InspectionGji inspection)
        {
            var baseInspection = new MenuItem { Caption = "Основание проверки" };
            baseInspection.AddOption("inspectionId", inspection.Id);
            baseInspection.AddOption("title", "Основание проверки");

            switch (inspection.TypeBase)
            {
                case TypeBase.PlanJuridicalPerson:
                    {
                        baseInspection.Caption = "Плановая проверка юр. лица";
                        baseInspection.Href = "B4.controller.basejurperson.Edit";
                    }

                    break;

                case TypeBase.DisposalHead:
                    {
                        baseInspection.Caption = "Административное дело";
                        baseInspection.Href = "B4.controller.basedisphead.Edit";
                    }

                    break;

                case TypeBase.Inspection:
                    {
                        baseInspection.Caption = "Инспекционная проверка";
                        baseInspection.Href = "B4.controller.baseinscheck.Edit";
                    }

                    break;

                case TypeBase.ProsecutorsClaim:
                    {
                        baseInspection.Caption = "Требование прокуратуры";
                        baseInspection.Href = "B4.controller.baseprosclaim.Edit";
                    }

                    break;

                case TypeBase.CitizenStatement:
                    {
                        baseInspection.Caption = "Проверка по обращению граждан";
                        baseInspection.Href = "B4.controller.basestatement.Edit";
                    }

                    break;

                case TypeBase.LicenseApplicants:
                    {
                        baseInspection.Caption = "Проверка соискателей лицензии";
                        baseInspection.Href = "B4.controller.baselicenseapplicants.Edit";
                    }

                    break;

                case TypeBase.PlanAction:
                    {
                        baseInspection.Caption = "Проверка по плану мероприятий";
                        baseInspection.Href = "B4.controller.baseplanaction.Edit";
                    }

                    break;

                case TypeBase.Default:
                    {
                        baseInspection.Caption = "Основание проверки";
                        baseInspection.Href = "B4.controller.basedefault.Edit";
                    }

                    break;

                case TypeBase.LicenseReissuance:
                    {
                        baseInspection.Caption = "Проверка лицензиата";
                        baseInspection.Href = "B4.controller.baselicensereissuance.Edit";
                    }

                    break;
            }

            baseInspection.WithIcon("icon-page-world");
            return baseInspection;
        }

        /// <summary>
        /// Метод получения списка узлов-этапов проверки
        /// </summary>
        public virtual MenuItem InitItem(   InspectionGji inspection, 
                                            InspectionGjiStageProxyRow curentStage, 
                                            Dictionary<long, List<InspectionGjiStageProxyRow>> dictStages, 
                                            Dictionary<long, List<DocumentProxy>> dictCounts)
        {
            var item = GetItem(curentStage, dictCounts);
            item.AddOption("inspectionId", inspection.Id);
            item.AddOption("title", item.Caption);

            if (dictStages.ContainsKey(curentStage.Id))
            {
                foreach (var childItem in dictStages[curentStage.Id])
                {
                    if (dictCounts.ContainsKey(childItem.Id))
                    {
                        item.Items.Add(InitItem(inspection, childItem, dictStages, dictCounts));
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// Метод получения узела этапа проверки
        /// </summary>
        public virtual MenuItem GetItem(InspectionGjiStageProxyRow curentStage, Dictionary<long, List<DocumentProxy>> dictCounts)
        {
            var item = new MenuItem
            {
                Caption = "Этап проверки",
                Href = "B4.controller.dict.PlanJurPersonGji"
            };

            int cnt = 0;

            var number = "";

            if (dictCounts.ContainsKey(curentStage.Id))
            {
                cnt = dictCounts[curentStage.Id].Count;
                if (cnt == 1)
                {
                    item.AddOption("documentId", dictCounts[curentStage.Id][0].Id);
                    number = dictCounts[curentStage.Id][0].DocumentNumber;
                }
                else
                {
                    item.AddOption("stageId", curentStage.Id);
                }
            }

            switch (curentStage.TypeStage)
            {
                case TypeStage.Disposal:
                    {
                        item.Caption = "Дело АП";
                        item.WithIcon("icon-rosette");

                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.Disposal";
                        }
                    }

                    break;

                case TypeStage.DisposalPrescription:
                    {
                        item.Caption = DisposalTextService.SubjectiveCase;
                        item.WithIcon("icon-rosette");

                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.Disposal";
                        }
                    }

                    break;

                case TypeStage.ActCheck:
                    {
                        item.Caption = "Aкт проверки";
                        item.Href = "B4.controller.ActCheck";
                        item.WithIcon("icon-page-white-edit");

                        if (cnt > 1)
                        {
                            item.Caption = "Aкты проверок";
                            item.Href = "B4.controller.actcheck.ListPanel";
                        }
                    }

                    break;

                case TypeStage.ActCheckGeneral:
                    {
                        item.Caption = "Aкт проверки (общий)";
                        item.Href = "B4.controller.ActCheck";
                        item.WithIcon("icon-page-white-edit");

                        if (cnt > 1)
                        {
                            item.Caption = "Aкты проверок (общие)";
                            item.Href = "B4.controller.actcheck.ListPanel";
                        }
                    }

                    break;


                case TypeStage.ActView:
                    {
                        item.Caption = "Aкт осмотра";
                        item.Href = "B4.controller.ActCheck";
                        item.WithIcon("icon-page-white-edit");

                        if (cnt > 1)
                        {
                            item.Caption = "Aкты осмотра";
                            item.Href = "B4.controller.actcheck.ListPanel";
                        }
                    }

                    break;


                case TypeStage.ActSurvey:
                    {
                        item.Caption = "Aкт обследования";
                        item.Href = "B4.controller.ActSurvey";
                        item.WithIcon("icon-page-white-magnify");

                        if (cnt > 1)
                        {
                            item.Caption = "Aкты обследования";
                            item.Href = "B4.controller.actsurvey.ListPanel";
                        }
                    }

                    break;

                case TypeStage.ActVisual:
                    {
                        item.Caption = "Акт визуального осмотра";
                        item.Href = "B4.controller.ActVisual";
                        item.WithIcon("icon-page-white-magnify");
                    }

                    break;

                case TypeStage.Prescription:
                    {
                        item.Caption = "Предписание";
                        item.Href = "B4.controller.Prescription";
                        item.WithIcon("icon-page-white-error");

                        if (cnt > 1)
                        {
                            item.Caption = "Предписания";
                            item.Href = "B4.controller.prescription.ListPanel";
                        }

                    }

                    break;

                case TypeStage.Protocol:
                    {
                        item.Caption = "Протокол";
                        item.Href = "B4.controller.ProtocolGji";
                        item.WithIcon("icon-page-white-medal");

                        if (cnt > 1)
                        {
                            item.Caption = "Протоколы";
                            item.Href = "B4.controller.protocolgji.ListPanel";
                        }
                    }

                    break;

                case TypeStage.Resolution:
                    {
                        item.Caption = "Постановление";
                        item.Href = "B4.controller.Resolution";
                        item.WithIcon("icon-page-white-star");

                        if (cnt > 1)
                        {
                            item.Caption = "Постановления";
                            item.Href = "B4.controller.resolution.ListPanel";
                        }
                    }

                    break;

                case TypeStage.ActRemoval:
                    {
                        item.Caption = "Aкт проверки предписания";
                        item.Href = "B4.controller.ActRemoval";

                        if (cnt > 1)
                        {
                            item.Caption = "Aкты проверок предписаний";
                            item.Href = "B4.controller.actremoval.ListPanel";
                        }
                    }

                    break;

                case TypeStage.AdministrativeCase:
                    {
                        item.Caption = "Административное дело";
                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.admincase.Edit";
                        }
                    }

                    break;

                case TypeStage.ResolutionProsecutor:
                    {
                        item.Caption = "Постановление прокуратуры";
                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.resolpros.Edit";
                        }
                    }

                    break;

                case TypeStage.Presentation:
                    {
                        item.Caption = "Представление";
                        item.Href = "B4.controller.Presentation";
                        item.WithIcon("icon-page-white-text");
                        if (cnt > 1)
                        {
                            item.Caption = "Представления";
                            item.Href = "B4.controller.presentation.ListPanel";
                        }
                    }

                    break;

                case TypeStage.ProtocolMvd:
                    {
                        item.Caption = "Протокол МВД";
                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.protocolmvd.Edit";
                        }
                    }

                    break;

                case TypeStage.ProtocolRSO:
                    {
                        item.Caption = "Протокол РСО";
                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.protocolrso.Edit";
                        }
                    }

                    break;

                case TypeStage.ProtocolMhc:
                    {
                        item.Caption = "Протокол МЖК";
                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.protocol197.Edit";
                        }
                    }

                    break;

                case TypeStage.Protocol197:
                    {
                        item.Caption = "Протокол по ст.19.7";
                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.protocol197.Edit";
                        }
                    }

                    break;

                case TypeStage.ResolutionRospotrebnadzor:
                    {
                        item.Caption = "Постановление Роспотребнадзора";
                        item.Href = "B4.controller.ResolutionRospotrebnadzor";
                        item.WithIcon("icon-page-white-star");
                        if (cnt > 1)
                        {
                            item.Caption = "Постановления Роспотребнадзора";
                            item.Href = "B4.controller.resolutionrospotrebnadzor.ListPanel";
                        }
                    }

                    break;
            }

            if (cnt == 1 && !string.IsNullOrWhiteSpace(number))
            {
                item.Caption = string.Format("{0} ({1})", item.Caption, number);
            }

            return item;
        }

        public class InspectionGjiStageProxyRow
        {
            public long Id { get; set; }

            public long Parent { get; set; }

            public long Position { get; set; }

            public TypeStage TypeStage { get; set; }
        }

        public class DocumentProxy
        {
            public long Id { get; set; }
            public string DocumentNumber { get; set; }
        }
    }
}