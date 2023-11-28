namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ActRemovalServiceInterceptor: ActRemovalServiceInterceptor<ActRemoval>
    {
    }

    public class ActRemovalServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T: ActRemoval
    {

        public IDomainService<DocumentGjiChildren> ParentChildrenDomain { get; set; }
        public IDomainService<Prescription> PrescriptionDomain { get; set; }
        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {

            if (entity.DocumentDate.HasValue)
            {
                //Cghjcnfdkztv xnj yt ecnhfytyj
                var parentDisp = ParentChildrenDomain.GetAll()
                    .FirstOrDefault(x => x.Children.Id == entity.Id && x.Parent.TypeDocumentGji == Enums.TypeDocumentGji.Disposal).Parent;
                if (parentDisp != null)
                {
                    var parentPrescrDoc = ParentChildrenDomain.GetAll()
                        .FirstOrDefault(x => x.Children.Id == parentDisp.Id && x.Parent.TypeDocumentGji == Enums.TypeDocumentGji.Prescription).Parent;
                    if (parentPrescrDoc != null)
                    {
                        var prescr = PrescriptionDomain.Get(parentPrescrDoc.Id);
                        var maxDateRemovalEx = PrescriptionViolDomain.GetAll()
                            .Where(x => x.Document.Id == parentPrescrDoc.Id)
                            .Max(x => x.DatePlanExtension);
                        var maxDateRemoval = PrescriptionViolDomain.GetAll()
                          .Where(x => x.Document.Id == parentPrescrDoc.Id)
                          .Max(x => x.DatePlanRemoval);
                        var dateRemoval = maxDateRemovalEx > maxDateRemoval ? maxDateRemovalEx : maxDateRemoval;
                        if (dateRemoval < entity.DocumentDate)
                        {
                            if (entity.TypeRemoval == Gkh.Enums.YesNoNotSet.Yes)
                            {
                                prescr.TypePrescriptionExecution = Enums.TypePrescriptionExecution.Violated;
                                PrescriptionDomain.Update(prescr);
                            }
                            else
                            {
                                prescr.TypePrescriptionExecution = Enums.TypePrescriptionExecution.NotExecuted;
                                PrescriptionDomain.Update(prescr);
                            }

                        }
                        else
                        {
                            if (entity.TypeRemoval == Gkh.Enums.YesNoNotSet.Yes)
                            {
                                prescr.TypePrescriptionExecution = Enums.TypePrescriptionExecution.Executed;
                                PrescriptionDomain.Update(prescr);
                            }
                            else
                            {
                                prescr.TypePrescriptionExecution = Enums.TypePrescriptionExecution.NotExecuted;
                                PrescriptionDomain.Update(prescr);
                            }
                        }
                    }
                }
            }

            return base.AfterUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var domainServiceViol = Container.Resolve<IDomainService<ActRemovalViolation>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                // Удаляем всех дочерних Нарушения
                var violIds = domainServiceViol.GetAll().Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id).ToList();

                foreach (var violId in violIds)
                {
                    domainServiceViol.Delete(violId);
                }

                return result;
            }
            finally
            {
                Container.Release(domainServiceViol);
            }
            
        }
    }
}