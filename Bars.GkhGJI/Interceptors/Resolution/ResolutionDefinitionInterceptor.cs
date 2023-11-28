namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;

    public class ResolutionDefinitionInterceptor : ResolutionDefinitionInterceptor<ResolutionDefinition>
    {
        //Все методы переопределеять и добавлять в Generic

        public IGkhUserManager UserManager { get; set; }
        public override IDataResult BeforeUpdateAction(IDomainService<ResolutionDefinition> service, ResolutionDefinition entity)
        {
            var servStateProvider = this.Container.Resolve<IStateProvider>();
            var ComissionMeetingDomain = Container.Resolve<IDomainService<ComissionMeeting>>();
            var ZonalInspectionInspectorDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var ResolutionDomain = Container.Resolve<IDomainService<Resolution>>();

            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
                return Failure("Создание документов комиссии доступно только членам комиссии");

            var inspector = thisOperator.Inspector;
            var zonal = ZonalInspectionInspectorDomain.GetAll()
                .FirstOrDefault(x => x.Inspector == inspector)?.ZonalInspection;      

            if (zonal == null)
            {
                return Failure("Создание документов комиссии доступно только членам комиссии");
            }

            if (entity.ComissionMeeting == null && entity.ConciderationDate.HasValue)
            {
                var comission = ComissionMeetingDomain.GetAll()
                    .FirstOrDefault(x => x.CommissionDate == entity.ConciderationDate && x.ZonalInspection == zonal);
                if (comission != null)
                {
                    entity.ComissionMeeting = comission;
                }
                else
                {
                    comission = new ComissionMeeting
                    {
                        CommissionDate = entity.ConciderationDate.Value,
                        ComissionName = $"Комиссия от {entity.ConciderationDate.Value.ToString("dd.MM.yyyy")}",
                        Description = "Создано из определения",
                        ZonalInspection = zonal,
                        CommissionNumber = "б/н"
                    };
                    servStateProvider.SetDefaultState(comission);
                    ComissionMeetingDomain.Save(comission);
                    entity.ComissionMeeting = comission;
                }
            }
            else if (entity.ComissionMeeting != null)
            {
                entity.ConciderationDate = entity.ComissionMeeting.CommissionDate;
            }

            return Success();
        }
    }

    // Generic класс для определения постановления чтобы было лучше расширять сущности без дублирования кода через subclass
    public class ResolutionDefinitionInterceptor<T> : EmptyDomainInterceptor<T>
        where T : ResolutionDefinition
    {
        public IGkhUserManager UserManager { get; set; }
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var servStateProvider = this.Container.Resolve<IStateProvider>();
            var ComissionMeetingDomain = Container.Resolve<IDomainService<ComissionMeeting>>();
            var ZonalInspectionInspectorDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var ResolutionDomain = Container.Resolve<IDomainService<Resolution>>();

            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
                return Failure("Создание документов комиссии доступно только членам комиссии");

            var inspector = thisOperator.Inspector;
            var zonal = ZonalInspectionInspectorDomain.GetAll()
                .FirstOrDefault(x => x.Inspector == inspector)?.ZonalInspection;

            var resolutionZonal = ResolutionDomain.GetAll()
                .FirstOrDefault(x => x.Id == entity.Resolution.Id)?.ZonalInspection;

            resolutionZonal = zonal;

            if (zonal == null)
            {
                return Failure("Создание документов комиссии доступно только членам комиссии");
            }

            if (entity.ComissionMeeting == null && entity.ExecutionDate.HasValue)
            {
                var comission = ComissionMeetingDomain.GetAll()
                    .FirstOrDefault(x => x.CommissionDate == entity.ExecutionDate && x.ZonalInspection == zonal);
                if (comission != null)
                {
                    entity.ComissionMeeting = comission;
                }
                else
                {
                    comission = new ComissionMeeting
                    {
                        CommissionDate = entity.ExecutionDate.Value,
                        ComissionName = $"Комиссия от {entity.ExecutionDate.Value.ToString("dd.MM.yyyy")}",
                        Description = "Создано из определения",
                        ZonalInspection = zonal,
                        CommissionNumber = "б/н"
                    };
                    servStateProvider.SetDefaultState(comission);
                    ComissionMeetingDomain.Save(comission);
                    entity.ComissionMeeting = comission;
                }
            }
            else if (entity.ComissionMeeting != null)
            {
                entity.ExecutionDate = entity.ComissionMeeting.CommissionDate;
            }

            return Success();
        }
    }
}
