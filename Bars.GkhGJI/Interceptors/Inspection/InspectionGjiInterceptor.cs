namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    // Пустышка нужна чтобы регистрировать и в регионах где могли наследвоатся от этого типа чтобы осталось все в рабочем состоянии
    public class InspectionGjiInterceptor : InspectionGjiInterceptor<InspectionGji>
    {
        //Внимание!!! не удалять поскольку в регионах могли наследвоатся от этой сущности
    }

    // Делаю Generic чтобы лучше наследоватся и заменят ьв других модулях
    public class InspectionGjiInterceptor<T> : EmptyDomainInterceptor<T>
        where T: InspectionGji
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            try
            {
                stateProvider.SetDefaultState(entity);

                return Success();
            }
            finally
            {
                Container.Release(stateProvider);
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var domainServiceDocument = Container.Resolve<IDomainService<DocumentGji>>();
            var domainServiceViolation = Container.Resolve<IDomainService<InspectionGjiViol>>();
            var domainServiceStage = Container.Resolve<IDomainService<InspectionGjiStage>>();
            var domainServiceObject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var domainServiceInspector = Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var domainServiceDocRef = Container.Resolve<IDomainService<InspectionDocGjiReference>>();
            var servReminder = Container.Resolve<IDomainService<Reminder>>();
            var appealCitsServ = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var reminderServ = Container.Resolve<IDomainService<Reminder>>();

            try
            {
                // Удаляем все дочерние документы у постановления
                domainServiceDocument.GetAll().Where(x => x.Inspection.Id == entity.Id).ForEach(
                    x =>
                        {
                            var type = x.GetType();
                            var domainServiceType = typeof(IDomainService<>).MakeGenericType(type);
                            var domainServiceImpl = (IDomainService)this.Container.Resolve(domainServiceType);
                            try
                            {
                                domainServiceImpl.Delete(x.Id);
                            }
                            finally
                            {
                                this.Container.Release(domainServiceImpl);
                            }
                        });

                // Удаляем все дочерние Нарушения
                domainServiceViolation.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceViolation.Delete(x));

                // Удаляем все дочерние Этапы проверки
                domainServiceStage.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceStage.Delete(x));

                // Удаляем все дочерние Проверяемые дома
                domainServiceObject.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceObject.Delete(x));

                // Удаляем всех дочерних инспекторов
                domainServiceInspector.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceInspector.Delete(x));

                // Удаляем напоминания по проверке
                servReminder.GetAll().Where(x => x.InspectionGji.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => servReminder.Delete(x));

                // удаляем Неявные ссылки (Если такие имеются)
                domainServiceDocRef.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceDocRef.Delete(x));

                appealCitsServ.GetAll().Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => appealCitsServ.Delete(x));

                reminderServ.GetAll().Where(x => x.InspectionGji.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => reminderServ.Delete(x));

                return Success();
            }
            finally
            {
                Container.Release(domainServiceDocument);
                Container.Release(domainServiceViolation);
                Container.Release(domainServiceStage);
                Container.Release(domainServiceObject);
                Container.Release(domainServiceInspector);
                Container.Release(servReminder);
                Container.Release(appealCitsServ);
                Container.Release(reminderServ);
            }
        }

        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            this.CreateReminders(entity);

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var zonalInspInspectorService = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var zonalInspInspectionsGjiService = Container.Resolve<IDomainService<InspectionGjiZonalInspection>>();
            try
            {
                this.CreateReminders(entity);
                //this.SendRequests(entity);

                var thisInspector = this.Container.Resolve<IGkhUserManager>().GetActiveOperator().Inspector;
                var thisZonalInsp = zonalInspInspectorService.GetAll().Where(x => x.Inspector == thisInspector).FirstOrDefault().ZonalInspection;

                var newZonalInspInspection = new InspectionGjiZonalInspection
                {
                    Inspection = entity,
                    ZonalInspection = thisZonalInsp
                };

                zonalInspInspectionsGjiService.Save(newZonalInspInspection);

                return this.Success();
            }
            finally
            {
                this.Container.Release(zonalInspInspectorService);
                this.Container.Release(zonalInspInspectionsGjiService);
            }
        }

        private void CreateReminders(T entity)
        {
            // Получаем правила формирования Напоминаний и запускаем метод создания напоминаний
            var servReminderRule = Container.ResolveAll<IReminderRule>();

            try
            {
                var rule = servReminderRule.FirstOrDefault(x => x.Id == "InspectionReminderRule");
                if (rule != null)
                {
                    rule.Create(entity);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(servReminderRule);
            }
        }

        private void SendRequests(T entity)
        {
            // Получаем правила формирования Напоминаний и запускаем метод создания напоминаний
            var smevRule = Container.ResolveAll<ISMEVRule>();

            try
            {
                var rule = smevRule.FirstOrDefault(x => x.Id == "BaseSMEVInspectionRule");
                if (rule != null)
                {
                    rule.SendRequests(entity);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(smevRule);
            }
        }
    }
}
