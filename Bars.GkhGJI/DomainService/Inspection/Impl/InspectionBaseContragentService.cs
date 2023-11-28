namespace Bars.GkhGji.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с Органами совместной проверки
    /// </summary>
    public class InspectionBaseContragentService : IInspectionBaseContragentService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionBaseContragent> InspectionBaseContragentDomain { get; set; }

        public IDomainService<InspectionGji> InspectionGjiDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        /// <inheritdoc />
        public IDataResult AddContragents(BaseParams baseParams)
        {
            var inspectionId = baseParams.Params.GetAsId("inspectionId");
            var contragentIds = baseParams.Params.GetAs<long[]>("contragentIds");

            this.Container.InTransaction(
                () =>
                {
                    var inspection = this.InspectionGjiDomain.Get(inspectionId);

                    var existsContragentQuery = this.InspectionBaseContragentDomain.GetAll()
                        .Where(x => x.InspectionGji.Id == inspectionId);

                    var recordsForSave = this.ContragentDomain.GetAll()
                        .WhereContains(x => x.Id, contragentIds)
                        .Where(x => !existsContragentQuery.Any(y => y.Contragent == x))
                        .Select(
                            x => new InspectionBaseContragent
                            {
                                Contragent = x,
                                InspectionGji = inspection
                            })
                        .ToList();

                    recordsForSave.ForEach(this.InspectionBaseContragentDomain.Save);
                });

            return new BaseDataResult();
        }
    }
}