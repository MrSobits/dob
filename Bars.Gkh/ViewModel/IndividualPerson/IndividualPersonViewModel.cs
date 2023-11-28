namespace Bars.Gkh.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class IndividualPersonViewModel : BaseViewModel<IndividualPerson>
    {
        public override IDataResult List(IDomainService<IndividualPerson> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var workId = baseParams.Params.ContainsKey("workId")
                                   ? baseParams.Params["workId"].ToLong()
                                   : 0;

            var data = domain.GetAll()

                .Select(x => new
                {
                    x.Id,
                    x.Fio,
                    PlaceResidence = x.IsPlaceResidenceOutState? x.PlaceResidenceOutState: x.PlaceResidence,
                    ActuallyResidence = x.IsActuallyResidenceOutState ? x.ActuallyResidenceOutState: x.ActuallyResidence,
                    x.BirthPlace,
                    x.Job,
                    x.DateBirth,
                    x.PassportNumber,
                    x.PassportSeries,
                    x.PassportIssued,
                    x.DepartmentCode,
                    x.DateIssue,
                    x.INN,
                    x.FamilyStatus,
                    x.PhoneNumber
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}