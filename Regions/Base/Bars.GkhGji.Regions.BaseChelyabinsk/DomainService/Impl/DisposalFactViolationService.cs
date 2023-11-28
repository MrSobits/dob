namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    using Castle.Windsor;

    public class DisposalFactViolationService : IDisposalFactViolationService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddFactViolation(BaseParams baseParams)
        {
            var dispFactViolDomain = this.Container.ResolveDomain<DisposalViolation>();
            var factViolDomain = this.Container.ResolveDomain<ViolationGji>();
            var inspViolDomain = this.Container.ResolveDomain<InspectionGjiViol>();
            var inspViolStageDomain = this.Container.ResolveDomain<InspectionGjiViolStage>();
            var disposalDomain = this.Container.ResolveDomain<Disposal>();

            try
            {
                var disposalId = baseParams.Params.GetAsId("disposalId");
                var factViolIds = baseParams.Params.GetAs<string>("factViolIds").ToLongArray();

                var existRecs = dispFactViolDomain.GetAll()
                    .Where(x => x.Document.Id == disposalId)
                    .ToList();

                var disposal = disposalDomain.Load(disposalId);

                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        foreach (var disposalFactViolation in existRecs.Where(x => !factViolIds.Contains(x.InspectionViolation.Violation.Id)))
                        {
                            dispFactViolDomain.Delete(disposalFactViolation.Id);
                        }

                        foreach (var factViol in factViolIds.Where(x => existRecs.All(y => y.InspectionViolation.Violation.Id != x)))
                        {
                            var newInspectionViolation = new InspectionGjiViol
                            {
                                Violation = factViolDomain.Load(factViol),
                                Inspection = disposal.Inspection,
                            };
                            inspViolDomain.Save(newInspectionViolation);
                            dispFactViolDomain.Save(new DisposalViolation
                            {
                                Document = new DocumentGji {Id = disposal.Id },
                                InspectionViolation = newInspectionViolation,                               
                                TypeViolationStage = GkhGji.Enums.TypeViolationStage.Detection
                            });
                        }

                        tr.Commit();
                    }
                    catch(Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                this.Container.Release(dispFactViolDomain);
                this.Container.Release(factViolDomain);
                this.Container.Release(disposalDomain);
            }

            return new BaseDataResult();
        }
    }
}