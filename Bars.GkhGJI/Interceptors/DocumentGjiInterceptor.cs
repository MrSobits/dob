﻿namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.NumberValidation;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;


    /// <summary>
    /// Пустышка нужна чтобы регистрировать и в регионах где могли наследвоатся от этого типа чтобы осталось все в рабочем состоянии
    /// </summary>
    public class DocumentGjiInterceptor : DocumentGjiInterceptor<DocumentGji>
    {
        //Внимание!!! не удалять поскольку в регионах могли наследвоатся от этой сущности
    }

    /// <summary>
    /// Делаю Generic чтобы лучше наследоватся и заменять в других модулях
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class DocumentGjiInterceptor<T> : EmptyDomainInterceptor<T>
        where T: DocumentGji
    {
        private long stageId { get; set; }

              /// <summary>
        /// Установить первоночальнрый статус у сущности
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param> 
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var zonalDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            try
            {
                if (entity.State == null)
                {
                    stateProvider.SetDefaultState(entity);
                }
                if (entity.ZonalInspection == null)
                {
                    Operator thisOperator = userManager.GetActiveOperator();
                    if (thisOperator?.Inspector == null)
                    {

                    }
                    else
                    {
                        var zomal = zonalDomain.GetAll().FirstOrDefault(x => x.Inspector == thisOperator.Inspector).ZonalInspection;
                        entity.ZonalInspection = zomal;
                    }

                }

                return this.Success();
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(stateProvider);
                this.Container.Release(zonalDomain);
            }
        }

        /// <summary>
        /// Проверка правильности номера документа ГЖИ
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param> 
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var serviceValidation = this.Container.Resolve<IDomainService<DocNumValidationRule>>();
            var serviceRule = this.Container.ResolveAll<INumberValidationRule>();

            try
            {
                var numValidation = serviceValidation
                                    .GetAll()
                                    .FirstOrDefault(x => x.TypeDocumentGji == entity.TypeDocumentGji);

                if (numValidation != null)
                {
                    var rule = serviceRule.FirstOrDefault(x => x.Id == numValidation.RuleId);
                    if (rule != null)
                    {
                        var result = rule.Validate(entity);
                        if (!result.Success)
                        {
                            return Failure(result.Message);
                        }
                    }
                }

                return base.BeforeUpdateAction(service, entity);
            }
            finally
            {
                this.Container.Release(serviceValidation);
                this.Container.Release(serviceRule);
            }
        }

        /// <summary>
        /// Удаление зависимых сущностей
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param> 
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var domainChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceRef = this.Container.Resolve<IDomainService<DocumentGjiReference>>();
            var serviceInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceReminder = this.Container.Resolve<IDomainService<Reminder>>();

            try
            {
                // Перед удалением проверяем есть ли дочерние документы
                if (domainChildren.GetAll().Count(x => x.Parent.Id == entity.Id) > 0)
                {
                    return this.Failure("Данный документ имеет дочерние документы.");
                }

                // Удаляем все связи на этот документ в таблице DocumentGJIReference
                var refIds = serviceRef.GetAll()
                    .Where(x => x.Document1.Id == entity.Id || x.Document2.Id == entity.Id)
                    .Select(x => x.Id).ToList();

                foreach (var refId in refIds)
                {
                    serviceRef.Delete(refId);
                }

                // Удаляем все дочерние Инспекторы
                var inspectorIds = serviceInspector.GetAll()
                    .Where(x => x.DocumentGji.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var insId in inspectorIds)
                {
                    serviceInspector.Delete(insId);
                }

                // Удаляем записи в таблице DocumentGjiChildren, в которых entity является дочерним документом
                var documentGjiChildrenIds = domainChildren.GetAll()
                    .Where(x => x.Children.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var documentGjiChildrenId in documentGjiChildrenIds)
                {
                    domainChildren.Delete(documentGjiChildrenId);
                }

                // Удаляем Напоминание по действиям ГЖИ
                serviceReminder.GetAll()
                .Where(x => x.DocumentGji.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => serviceReminder.Delete(x));

                // Поскольку удаляем документ то Stage надо будет удалять в методе AfterDelete
                // А поскольку в методе AfterDelete сущности уже небудет в БД то заранее запоминаем StageId чтобы потом его удалить
                if (entity.Stage != null)
                {
                    this.stageId = entity.Stage.Id;
                }
                
                return this.Success();
            }
            finally
            {
                this.Container.Release(domainChildren);
                this.Container.Release(serviceRef);
                this.Container.Release(serviceInspector);
                this.Container.Release(serviceReminder);
            }
        }

        /// <summary>
        /// Удаление этапа проверки
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param> 
        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            // В этом методе мы получаем количество документов для Этапа проверки
            // Если в этом этапе есть еще документы то нельзя удалять этап
            // В противном случае удаляем этап
            var domainServiceStage = this.Container.Resolve<IDomainService<InspectionGjiStage>>();
            var domainServiceDocument = this.Container.Resolve<IDomainService<DocumentGji>>();

            try
            {
                if (this.stageId > 0)
                {

                    var stage = domainServiceStage.Get(this.stageId);

                    if (stage != null)
                    {
                        try
                        {
                            if (!domainServiceDocument.GetAll().Any(x => x.Stage.Id == this.stageId))
                            {
                                // Если в этапе нет документов то удаляем этап
                                domainServiceStage.Delete(this.stageId);
                            }
                        }
                        catch(Exception exc)
                        {
                            return this.Failure(exc.Message);
                        }
                    }
                }
                
                this.CreateReminders(entity);

                return this.Success();
            }
            finally
            {
                this.Container.Release(domainServiceStage);
                this.Container.Release(domainServiceDocument);
            }
        }

        /// <summary>
        /// Создание напоминания
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param> 
        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            this.CreateReminders(entity);

            return this.Success();
        }

        /// <summary>
        /// Создание напоминания
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>  
        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            this.CreateReminders(entity);

            return this.Success();
        }

        /// <summary>
        /// Метод создания напоминания
        /// </summary>
        /// /// <param name="entity">Сущность</param>
        private void CreateReminders(T entity)
        {
            // Получаем правила формирования Напоминаний и запускаем метод создания напоминаний
            var servReminderRule = this.Container.ResolveAll<IReminderRule>();

            try
            {
                var rule = servReminderRule.FirstOrDefault(x => x.Id == "InspectionReminderRule");

                rule?.Create(entity);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                this.Container.Release(servReminderRule);
            }
        }
    }
}
