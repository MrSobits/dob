namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Linq;
    using System.Text;
    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class OwnerInterceptor : EmptyDomainInterceptor<Owner>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Owner> service, Owner entity)
        {
            if (entity.IndividualPerson != null)
            {
                entity.TypeViolator = Enums.TypeViolator.PhisicalPerson;
            }
            else if (entity.ContragentContact != null)
            {
                entity.TypeViolator = Enums.TypeViolator.OfficialPerson;
            }
            else
            {
                entity.TypeViolator = Enums.TypeViolator.UridicalPerson;
            }
            if (!string.IsNullOrEmpty(entity.NamberTransport))
            {
                var transportDomain = Container.Resolve<IDomainService<Transport>>();
                var existsTransport = transportDomain.GetAll().FirstOrDefault(x => x.NamberTransport == entity.NamberTransport);
                if (existsTransport != null)
                {
                    entity.Transport = existsTransport;
                }
                else
                {
                    var newTransport = new Transport
                    {
                        NamberTransport = entity.NamberTransport,
                        NameTransport = entity.NameTransport
                    };
                    transportDomain.Save(newTransport);
                    entity.Transport = newTransport;
                }
            }
            else
            {
                return Failure("Не указан госномер транспортного средства");
            }

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Owner> service, Owner entity)
        {
            if (entity.DataOwnerStart == null)
            {
                entity.DataOwnerStart = DateTime.Now.AddMonths(-1);
            }
           

            return this.Success();
        }
    }
}