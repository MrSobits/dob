namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class IndividualPersonService : IIndividualPersonService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var zonalInspId = baseParams.Params.GetAs<long>("inpectorId");

                var zonalInspectionNames = new StringBuilder();
                var zonalInspectionIds = new StringBuilder();

                // Пробегаемся по зон. инспекциям и формируем итоговую строку наименований и строку идентификаторов
                var serviceZonalInspectionInspector = Container.Resolve<IDomainService<ZonalInspectionInspector>>();

                var dataZonalInspection = serviceZonalInspectionInspector.GetAll()
                    .Where(x => x.Inspector.Id == zonalInspId)
                    .Select(x => new
                    {
                        x.ZonalInspection.Id,
                        x.ZonalInspection.ZoneName
                    })
                    .ToArray();

                foreach (var item in dataZonalInspection)
                {
                    if (!string.IsNullOrEmpty(item.ZoneName))
                    {
                        if (zonalInspectionNames.Length > 0)
                            zonalInspectionNames.Append(", ");

                        zonalInspectionNames.Append(item.ZoneName);
                    }

                    if (item.Id > 0)
                    {
                        if (zonalInspectionIds.Length > 0)
                            zonalInspectionIds.Append(", ");

                        zonalInspectionIds.Append(item.Id);
                    }
                }

                Container.Release(serviceZonalInspectionInspector);


                return new BaseDataResult(new
                {
                    zonalInspectionNames = zonalInspectionNames.ToString(),
                    zonalInspectionIds = zonalInspectionIds.ToString(),

                });
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}