namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Gji.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Перехватичик событий для сущности исполнитель обращения
    /// </summary>
    public class AppealCitsExecutantInterceptor : EmptyDomainInterceptor<AppealCitsExecutant>
    {
        /// <summary>
        /// Событие до сохранения исполнителя
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override IDataResult BeforeCreateAction(IDomainService<AppealCitsExecutant> service, AppealCitsExecutant entity)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var stateProvider = Container.Resolve<IStateProvider>();
            var paramService = Container.Resolve<IGjiParamsService>();
            try
            {
                if (!entity.PerformanceDate.HasValue)
                {
                    entity.PerformanceDate = entity.AppealCits.CheckTime.HasValue ? entity.AppealCits.CheckTime.Value.AddDays(-1) : entity.AppealCits.CheckTime;
                }
                if (paramService.GetParamByKey("AutoSetSurety").ToBool())
                {
                    var userOperator = userManager.GetActiveOperator();

                    if (userOperator != null)
                    {
                        if (userOperator.Inspector != null)
                        {
                            entity.Author = userOperator.Inspector;
                        }
                        else
                        {
                            return Failure("Не указан инспектор текущего оператора!");
                        }
                    }
                }

                if (entity.State == null)
                {
                    stateProvider.SetDefaultState(entity);
                    if (entity.State == null)
                    {
                        return Failure("Не задан начальный статус для Исполнителя обращения");
                    }
                }

                return Success();
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(stateProvider);
                Container.Release(paramService);
            }      
        }

        /// <summary>
        /// Событие после сохранения исполнителя
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override IDataResult AfterCreateAction(IDomainService<AppealCitsExecutant> service, AppealCitsExecutant entity)
        {

            var appealCitsContainer = Container.Resolve<IDomainService<GkhGji.Entities.AppealCits>>();
            var AppealCitsExecutantContainer = Container.Resolve<IDomainService<AppealCitsExecutant>>();
            try
            {
                var executantList = AppealCitsExecutantContainer.GetAll()
                  .Where(x => entity.Id == x.AppealCits.Id)
                  .Where(x => x.Executant != null)
                       .Select(x => new
                       {
                           Fio = x.IsResponsible ? "<b>" + x.Executant.Fio + "</b>" : x.Executant.Fio
                       }).ToList();

                var testersList = AppealCitsExecutantContainer.GetAll()
                   .Where(x => entity.AppealCits.Id == x.AppealCits.Id)
                   .Where(x => x.Controller != null)
                   .Select(x => x.Controller.Fio).ToList()
                   ;
                var appeal = appealCitsContainer.Get(entity.AppealCits.Id);
                string executants = "";
                foreach (var subject in executantList)
                {
                    if (executants != "")
                        executants += ", " + subject.Fio;
                    else executants = subject.Fio;
                }
                string controllers = "";
                foreach (string subject in testersList)
                {
                    if (controllers != "")
                        controllers += ", " + subject;
                    else controllers = subject;
                }
                appeal.Executors = executants;
                appeal.Testers = controllers;
                appealCitsContainer.Update(appeal);
                return base.AfterCreateAction(service, entity);
            }
            finally
            {
                Container.Release(appealCitsContainer);
                Container.Release(AppealCitsExecutantContainer);
            }

         
        }

        /// <summary>
        /// Событие до обновления исполнителя
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsExecutant> service, AppealCitsExecutant entity)
        {
            if (entity.IsResponsible 
                && service.GetAll().Where(x => x.AppealCits.Id == entity.AppealCits.Id && x.Id != entity.Id).Any(x => x.IsResponsible))
            {
                return this.Failure("У данного обращения уже имеется ответственный исполнитель");
            }

            var appealCitsContainer = Container.Resolve<IDomainService<GkhGji.Entities.AppealCits>>();
            var AppealCitsExecutantContainer = Container.Resolve<IDomainService<AppealCitsExecutant>>();
            try
            {
                var executantList = AppealCitsExecutantContainer.GetAll()
                    .Where(x => entity.AppealCits.Id == x.AppealCits.Id)
                    .Where(x => x.Executant != null)
                    .Select(x => x.Executant.Fio).ToList()
                    ;
                var testersList = AppealCitsExecutantContainer.GetAll()
                   .Where(x => entity.AppealCits.Id == x.AppealCits.Id)
                   .Where(x => x.Controller != null)
                   .Select(x => x.Controller.Fio).ToList()
                   ;
                var appeal = appealCitsContainer.Get(entity.AppealCits.Id);
                string executants = "";
                foreach (string subject in executantList)
                {
                    if (executants != "")
                        executants += ", " + subject;
                    else executants = subject;
                }
                string controllers = "";
                foreach (string subject in testersList)
                {
                    if (controllers != "")
                        controllers += ", " + subject;
                    else controllers = subject;
                }
                appeal.Executors = executants;
                appeal.Testers = controllers;
                appealCitsContainer.Update(appeal);
            }
            finally
            {
                Container.Release(appealCitsContainer);
                Container.Release(AppealCitsExecutantContainer);
            }
            return this.Success();
        }

        /// <summary>
        /// Событие до удаления исполнителя
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override IDataResult BeforeDeleteAction(IDomainService<AppealCitsExecutant> service, AppealCitsExecutant entity)
        {

            entity.Resolution = null;
            return this.Success();
        }
    }
}
