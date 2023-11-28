namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Gkh.Domain;

    public class InspectionGjiRealityObjectViewModel : BaseViewModel<InspectionGjiRealityObject>
    {
        public override IDataResult List(IDomainService<InspectionGjiRealityObject> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var inspectionId = baseParams.Params.ContainsKey("inspectionId")
                                   ? baseParams.Params["inspectionId"].ToLong()
                                   : 0;

            var listObjects = new List<long>();
            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            if (documentId > 0)
            {
                var document = Container.Resolve<IDomainService<DocumentGji>>().Load(documentId);
                if (document != null && document.TypeDocumentGji == TypeDocumentGji.Disposal)
                {
                    // Если переданный id документа является идентификатором распоряжения 
                    // то тогдазначит требуется поулчить список всех домов по нарушениям 
                    // предписаний по которому создано данное распоряжение

                    // Получаем все идентификаторы Предписаний по которым создано данное РАспоряжение
                    var prescriptionsIds = 
                        Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                            .Where(x => x.Children.Id == documentId && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                            .Select(x => x.Parent.Id)
                            .ToList();

                    // Теперь получаем все идентификаторы домовчерез Нарушения Предписания
                    listObjects.AddRange(
                        Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                            .Where(x => prescriptionsIds.Contains(x.Document.Id))
                            .Select(x => x.InspectionViolation.RealityObject.Id)
                            .Distinct()
                            .ToList());
                }
            }

            #region Формируем словарь Актов для таблицы проверяемых домов
            //Поскольку необходимо показывать Акт проверки по дому, то формируем словарь по
            //RealityObjectId в котором выводим Акт проверки по дому в виде №123 dd.mm.yyyy
            var dictActCheck = Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                .Where(x => x.ActCheck.Inspection.Id == inspectionId)
                .Where(x => x.RealityObject != null)
                .Select(x => new
                    {
                        roId = x.RealityObject.Id,
                        actId = x.ActCheck.Id,
                        x.ActCheck.DocumentNumber,
                        x.ActCheck.DocumentDate
                    })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, y => y.Select(x => new
                {
                    x.actId,
                    x.DocumentNumber,
                    x.DocumentDate
                })
                .FirstOrDefault());
            #endregion

            var service = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            var data = service.GetAll()
                .Where(x => x.Inspection.Id == inspectionId)
                .WhereIf(documentId > 0, x => listObjects.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    x.Id,
                    MunicipalityName = x.RealityObject.Municipality.Name,
                    RealityObjectId = x.RealityObject.Id,
                    x.RealityObject.Address,
                    Area = x.RealityObject.AreaMkd,
                    x.RoomNums
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.MunicipalityName,
                    x.RealityObjectId,
                    x.Address,
                    x.Area,
                    x.RoomNums,
                    ActCheckGJIName =
                                 dictActCheck.ContainsKey(x.RealityObjectId)
                                      ? string.Format(
                                          "№{0} от {1} г",
                                          dictActCheck[x.RealityObjectId].DocumentNumber,
                                          dictActCheck[x.RealityObjectId].DocumentDate.ToDateTime().ToShortDateString())
                                                                : null
                })
                .AsQueryable()
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<InspectionGjiRealityObject> domainService, BaseParams baseParams)
        {
            var entity = domainService.Get(baseParams.Params.GetAsId());
            if (entity == null)
            {
                return new BaseDataResult(null);
            }

            return new BaseDataResult(new
            {
                entity.Id,
                entity.RoomNums,
                entity.RealityObject.Address
            });
        }
    }

}