namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Entities;

    public class AppealCitsAnswerInterceptor : EmptyDomainInterceptor<AppealCitsAnswer>
    {
        public override IDataResult BeforeCreateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
                        

            var servStateProvider = Container.Resolve<IStateProvider>();
            try
            {
                servStateProvider.SetDefaultState(entity);
            }
            finally
            {
                Container.Release(servStateProvider);
            }

            return this.Success();
            
        }

        public override IDataResult AfterCreateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            var appCitizensContainer = this.Container.Resolve<IDomainService<AppealCits>>();

            if (entity.Addressee != null)
            {
                AppealCits app = appCitizensContainer.Get(entity.AppealCits.Id);
                app.AnswerDate = entity.DocumentDate;
                app.SSTUExportState = Enums.SSTUExportState.NotExported;
                appCitizensContainer.Update(app);
            }

            return this.Success();

        }
        public override IDataResult AfterUpdateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            var appCitizensContainer = this.Container.Resolve<IDomainService<AppealCits>>();

            if (entity.Addressee != null)
            {
                AppealCits app = appCitizensContainer.Get(entity.AppealCits.Id);
                app.AnswerDate = entity.DocumentDate;
                app.SSTUExportState = Enums.SSTUExportState.NotExported;
                appCitizensContainer.Update(app);
            }

            return this.Success();

        }
    }
}
