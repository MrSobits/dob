namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class SubjectVerificationInterceptor : EmptyDomainInterceptor<SubjectVerification>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<SubjectVerification> service, SubjectVerification entity)
        {
            var disposalSubjectVerifications = Container.Resolve<IDomainService<DisposalSubjectVerification>>()
                         .GetAll()
                         .Count(x => x.SubjectVerification.Id == entity.Id) ;
            
            if (disposalSubjectVerifications > 0)
            {
                return Failure("Существуют связанные записи в разделе \"Предмет проверки\"");
            }

            return Success();
        }
    }
}