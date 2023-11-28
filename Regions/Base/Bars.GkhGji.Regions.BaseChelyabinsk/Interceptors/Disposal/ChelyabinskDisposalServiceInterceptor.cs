namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.Disposal
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    // интерцептор необходимо тоже перекрыть поскольку сущность ChelyabinskDisposal должна выполнить базовый код интерцептора Disposal
    public class ChelyabinskDisposalServiceInterceptor : DisposalServiceInterceptor<ChelyabinskDisposal>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ChelyabinskDisposal> service, ChelyabinskDisposal entity)
        {
            entity.NcObtained = YesNo.No;
            entity.NcSent = YesNo.No;

            return base.BeforeCreateAction(service, entity);
        }
        
        public override IDataResult BeforeDeleteAction(IDomainService<ChelyabinskDisposal> service, ChelyabinskDisposal entity)
        {
            // Получаем предметы проверки и удаляем их
            var serviceSubj = this.Container.Resolve<IDomainService<DisposalVerificationSubject>>();
            var serviceDocs = this.Container.Resolve<IDomainService<DisposalDocConfirm>>();
            var serviceLongText = this.Container.Resolve<IDomainService<DisposalLongText>>();
            var factViols = this.Container.ResolveDomain<DisposalFactViolation>();
            var surveyObjectivesDomain = this.Container.ResolveDomain<DisposalSurveyObjective>();
            var surveyPurposeDomain = this.Container.ResolveDomain<DisposalSurveyPurpose>();

            try
            {
                // удаляем субтаблицы добавленные в регионе НСО

                serviceSubj.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => serviceSubj.Delete(x));

                serviceDocs.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => serviceDocs.Delete(x));

                serviceLongText.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => serviceLongText.Delete(x));

                factViols.GetAll()
                    .Where(x => x.Disposal.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => factViols.Delete(x));

                surveyObjectivesDomain.GetAll()
                    .Where(x => x.Disposal.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => surveyObjectivesDomain.Delete(x));

                surveyPurposeDomain.GetAll()
                    .Where(x => x.Disposal.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => surveyPurposeDomain.Delete(x));

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                this.Container.Release(serviceSubj);
                this.Container.Release(serviceDocs);
                this.Container.Release(serviceLongText);
                this.Container.Release(factViols);
                this.Container.Release(surveyObjectivesDomain);
                this.Container.Release(surveyPurposeDomain);
            }
            
        }

    }
}

