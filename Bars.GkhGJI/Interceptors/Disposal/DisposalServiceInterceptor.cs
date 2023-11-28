namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.NumberValidation;

    /// <summary>
    /// Такую пустышку навсякий смлучай нужно чтобы в регионах (Там где уже заменили или отнаследовались от этого класса) непопадало и можно было бы изменять методы как сущности Disposal
    /// </summary>
    public class DisposalServiceInterceptor : DisposalServiceInterceptor<Disposal>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в DisposalServiceInterceptor<T>
    }

    /// <summary>
    /// Короче такой поворот событий делается для того чтобы в Модулях регионов  спомошью 
    /// SubClass расширять сущность Disposal + не переписывать код который регистрируется по сущности
    /// то есть в Disposal добавляеться поля, но интерцептор поскольку Generic просто наследуется  
    /// </summary>
    public class DisposalServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : Disposal
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            if (entity.IndividualPerson == null)
            {
                var PersonService = Container.Resolve<IDomainService<IndividualPerson>>();
                try
                {


                    //var existsPerson = PersonService.GetAll()
                    //    .Where(x=> x.Fio == )

                    IndividualPerson person = new IndividualPerson();
                    person.Fio = entity.Fio;
                    person.PlaceResidence = entity.PlaceResidence;
                    person.ActuallyResidence = entity.ActuallyResidence;
                    person.BirthPlace = entity.BirthPlace;
                    person.Job = entity.Job;
                    person.DateBirth = entity.DateBirth;
                    person.PassportNumber = entity.PassportNumber.HasValue ? entity.PassportNumber.Value.ToString():"";
                    person.PassportSeries = entity.PassportSeries.HasValue ? entity.PassportNumber.Value.ToString() : "";
                    person.DepartmentCode = entity.DepartmentCode.HasValue ? entity.PassportNumber.Value.ToString() : "";
                    person.DateIssue = entity.DateIssue;
                    person.PassportIssued = entity.PassportIssued;
                    person.INN = entity.INN;


                    PersonService.Save(person);
                }
                catch
                { }
                finally
                {
                    Container.Release(PersonService);
                }
             
            }
            //доделать добавление строки в транспорт


            if (!ValidateContragentState(entity))
            {
                return Failure("Организация, указанная в основании проверки, не действует");
            }
            //проставляем вид контроля/надзора в проверку
            try
            {
                var inspectionService = Container.Resolve<IRepository<InspectionGji>>();
                var inspection = inspectionService.Get(entity.Inspection.Id);
                switch (entity.KindKNDGJI)
                {
                    case KindKNDGJI.HousingSupervision:
                        {
                            inspection.ControlType = ControlType.HousingSupervision;
                            inspectionService.Update(inspection);
                        }
                        break;
                    case KindKNDGJI.LicenseControl:
                        {
                            inspection.ControlType = ControlType.LicensedControl;
                            inspectionService.Update(inspection);
                        }
                        break;
                    case KindKNDGJI.NotSet:
                        {
                            inspection.ControlType = ControlType.NotSet;
                            inspectionService.Update(inspection);
                        }
                        break;
                    case KindKNDGJI.Both:
                        {
                            inspection.ControlType = ControlType.Both;
                            inspectionService.Update(inspection);
                        }
                        break;
                }
                

            }
            catch
            { }

            // проверяем дату проверки
            if (entity.DateEnd < entity.DateStart)
            {
                return Failure("Дата окончания проверки должна быть больше или равна дате начала проверки");
            }

            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            if (!ValidateContragentState(entity))
            {
                return Failure("Организация, указанная в основании проверки, не действует");
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {

            var annexService = Container.Resolve<IDomainService<DisposalAnnex>>();
            var expertService = Container.Resolve<IDomainService<DisposalExpert>>();
            var provDocsService = Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var typeServiceService = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var domainServiceViolation = Container.Resolve<IDomainService<DisposalViolation>>();
            
            try
            {
                var refFuncs = new List<Func<long, string>>
                               {
                                  id => annexService.GetAll().Any(x => x.Disposal.Id == id) ? "Приложения" : null,
                                  id => expertService.GetAll().Any(x => x.Disposal.Id == id) ? "Эксперты" : null,
                                  id => provDocsService.GetAll().Any(x => x.Disposal.Id == id) ? "Предоставляемые документы" : null,
                                  id => typeServiceService.GetAll().Any(x => x.Disposal.Id == id) ? "Типы обследования" : null
                               };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }

                // Удаляем все дочерние Нарушения
                var violationIds = domainServiceViolation.GetAll().Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id).ToList();

                foreach (var violId in violationIds)
                {
                    domainServiceViolation.Delete(violId);
                }
            }
            finally
            {
                Container.Release(annexService);
                Container.Release(expertService);
                Container.Release(provDocsService);
                Container.Release(typeServiceService);
                Container.Release(domainServiceViolation);
            }
            
            if (entity.TypeDisposal != TypeDisposalGji.NullInspection)
            {
                
                var servReminders = Container.Resolve<IDomainService<Reminder>>();
                var reminders = servReminders.GetAll().Where(x => x.DocumentGji.Id == entity.Id).Select(x => x.Id).ToList();

                foreach (var id in reminders)
                {
                    servReminders.Delete(id);
                }
            }

            return base.BeforeDeleteAction(service, entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            
                // После удаления распоряжения нужно удалить все пустыепроверки по Отопительному сезону или по Административной деятельности
                // Пустые - это значит в которых нет ниодного документа
                var domainServiceBaseHeatSeason = Container.Resolve<IDomainService<BaseHeatSeason>>();
                var domainServiceActivityTsj = Container.Resolve<IDomainService<BaseActivityTsj>>();
                var domainServiceDocument = Container.Resolve<IDomainService<DocumentGji>>();

                try
                {
                    // если тип распоряжения nullInspection, то дополнительных очисток не требуется
                    if (entity.TypeDisposal != TypeDisposalGji.NullInspection)
                    {
                        var baseHeatSeasonIds =
                        domainServiceBaseHeatSeason.GetAll()
                                                   .Where(
                                                       x =>
                                                       !domainServiceDocument.GetAll().Any(y => y.Inspection.Id == x.Id))
                                                   .Select(x => x.Id)
                                                   .ToList();

                        baseHeatSeasonIds.ForEach(x => domainServiceBaseHeatSeason.Delete(x));

                        var baseActivityIds =
                            domainServiceActivityTsj.GetAll()
                                                       .Where(
                                                           x =>
                                                           !domainServiceDocument.GetAll().Any(y => y.Inspection.Id == x.Id))
                                                       .Select(x => x.Id)
                                                       .ToList();

                        baseActivityIds.ForEach(x => domainServiceActivityTsj.Delete(x));
                    }
                    
                    return base.AfterDeleteAction(service, entity);
                }
                finally
                {
                    Container.Release(domainServiceBaseHeatSeason);
                    Container.Release(domainServiceActivityTsj);
                    Container.Release(domainServiceDocument);
                }
            
        }

        protected bool ValidateContragentState(T document)
        {
            var contragent = document.ReturnSafe(x => x.Inspection.Contragent);

            if (contragent != null)
            {
                if (contragent.ContragentState == ContragentState.Bankrupt
                    || contragent.ContragentState == ContragentState.Liquidated)
                {
                    return (document.DocumentDate ?? DateTime.Now.Date) <= contragent.DateTermination;
                }
            }

            return true;
        }
    }
}