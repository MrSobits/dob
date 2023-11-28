namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class OwnerViewModel : BaseViewModel<Owner>
    {
        public override IDataResult List(IDomainService<Owner> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var protocolId = baseParams.Params.ContainsKey("protocolId")
                                   ? baseParams.Params["protocolId"].ToLong()
                                   : 0;
            var personId = baseParams.Params.ContainsKey("personId")
                                   ? baseParams.Params["personId"].ToLong()
                                   : 0;

            var fio = baseParams.Params.ContainsKey("personFIO")
                                   ? baseParams.Params["personFIO"].ToString()
                                   : "";

            if (personId > 0)
            {
                var data = domainService.GetAll()
                    .Where(x => x.IndividualPerson != null && x.IndividualPerson.Id == personId)
                .Select(x => new
                {
                    x.Id,
                    x.Transport.NamberTransport,
                    x.Transport.NameTransport,
                    x.DataOwnerStart,
                    x.DataOwnerEdit
                })
                .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
            }
            else if (!string.IsNullOrEmpty(fio))
            {
                var data = domainService.GetAll()
                .Where(x => x.IndividualPerson != null && x.IndividualPerson.Fio == fio)
                .Select(x => new
                {
                    x.Id,
                    x.Transport.NamberTransport,
                    x.Transport.NameTransport,
                    x.DataOwnerStart,
                    x.DataOwnerEdit
                })
                .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
            }
            else
            {
                var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    IndividualPerson = x.IndividualPerson != null ? x.IndividualPerson.Fio : "",
                    Contragent = x.Contragent != null ? x.Contragent.Name : "",
                    ContragentContact = x.ContragentContact != null ? x.ContragentContact.FullName : "",
                    x.Transport.NamberTransport,
                    x.Transport.NameTransport,
                    x.TypeViolator,
                    x.DataOwnerStart,
                    x.DataOwnerEdit
                })
                .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
            }
            
        }
   
    }
}