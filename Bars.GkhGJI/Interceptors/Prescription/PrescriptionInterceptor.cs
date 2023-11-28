namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Интерцептор предписания
    /// </summary>
    public class PrescriptionInterceptor : PrescriptionInterceptor<Prescription>
    {
        

        private void RemoveReminders(Prescription prescription)
        {
            var reminderDomain = Container.Resolve<IDomainService<Reminder>>();
            using (Container.Using(reminderDomain))
            {
                foreach (var reminder in reminderDomain.GetAll().Where(r => r.DocumentGji.Id == prescription.Id))
                {
                    reminderDomain.Delete(reminder.Id);
                }
            }
        }
    }

    public class PrescriptionInterceptor<T> : DocumentGjiInterceptor<T>
        where T : Prescription
    {
        /// <summary>
        /// Удаление Напоменинаий по действиям ГЖИ
        /// </summary>
        /// <param name="service">Домен-сервич</param>
        /// <param name="entity">Предписание</param>
        /// <returns>IDataResult</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            if (entity.PrescriptionState == PrescriptionState.Active)
            {
                if (entity.CancelledGJI)
                {
                    entity.PrescriptionState = PrescriptionState.CancelledByGJI;
                }
            }
            else if (entity.PrescriptionState == PrescriptionState.CancelledByGJI)
            {
                if (!entity.CancelledGJI)
                {
                    entity.PrescriptionState = PrescriptionState.Active;
                }
            }
            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            if (entity.Executant == null)
            {
                // Пробуем подобрать тип исполнителя
                SetExecutant(entity);
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        /// Удаление зависимых сущностей
        /// </summary>
        /// <param name="service">Домен-сервич</param>
        /// <param name="entity">Предписание</param>
        /// <returns>IDataResult</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {

            var annexService = this.Container.Resolve<IDomainService<PrescriptionAnnex>>();
            var cancelService = this.Container.Resolve<IDomainService<PrescriptionCancel>>();
            var violationRepo = Container.Resolve<IRepository<PrescriptionViol>>(); //Что бы лишний раз не вызывался интерцептор
            var domainPrescriptionLaw = this.Container.Resolve<IDomainService<PrescriptionArticleLaw>>();
            var reminderDomain = Container.Resolve<IDomainService<Reminder>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                // Удаляем все дочерние Приложения
                annexService.GetAll().Where(x => x.Prescription.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => annexService.Delete(x));

                // Удаляем все дочерние Решения об отмене
                cancelService.GetAll().Where(x => x.Prescription.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => cancelService.Delete(x));

                // Удаляем все дочерние Нарушения
                var violationIds = violationRepo.GetAll().Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id).ToList();

                foreach (var violId in violationIds)
                {
                    violationRepo.Delete(violId);
                }

                // Удаляем все статьи закона
                var lawIds = domainPrescriptionLaw.GetAll().Where(x => x.Prescription.Id == entity.Id)
                    .Select(x => x.Id).ToList();

                foreach (var lawId in lawIds)
                {
                    domainPrescriptionLaw.Delete(lawId);
                }


                var reminders = reminderDomain.GetAll().Where(x => x.DocumentGji.Id == entity.Id).Select(x => x.Id).ToList();

                foreach (var id in reminders)
                {
                    reminderDomain.Delete(id);
                }

                return result;
            }
            finally
            {
                Container.Release(annexService);
                Container.Release(cancelService);
                Container.Release(violationRepo);
                Container.Release(domainPrescriptionLaw);
                Container.Release(reminderDomain);
            }
        }

        private void SetExecutant(T prescription)
        {
            //только для проверок, у которых задан контрагент
            if (prescription == null || prescription.Inspection == null || prescription.Inspection.Contragent == null)
            {
                return;
            }

            //только для проверок с основаниями проверки по требованию прокуратуры, проверки по поручению руководителей, проверки по обращениям.
            if (!(prescription.Inspection.TypeBase == TypeBase.ProsecutorsClaim
                  || prescription.Inspection.TypeBase == TypeBase.DisposalHead
                  || prescription.Inspection.TypeBase == TypeBase.CitizenStatement))
            {
                return;
            }

            var executantService = Container.Resolve<IDomainService<ExecutantDocGji>>();
            var manOrgService = Container.Resolve<IDomainService<ManagingOrganization>>();
            try
            {
                var contragentId = prescription.Inspection.Contragent.Id;
                var executantCode = string.Empty;

                // только для проверки, объектом проверки которой является "Организация" или "Должностное лицо"
                if (prescription.Inspection.PersonInspection == PersonInspection.Organization)
                {
                    switch (prescription.Inspection.TypeJurPerson)
                    {
                        case TypeJurPerson.LocalGovernment:
                            executantCode = "15";
                            break;

                        case TypeJurPerson.SupplyResourceOrg:
                        case TypeJurPerson.ServOrg:
                            executantCode = "2";
                            break;

                        case TypeJurPerson.Builder:
                            executantCode = "8";
                            break;

                        case TypeJurPerson.ManagingOrganization:
                            {
                                var typeManagement = manOrgService
                                         .GetAll()
                                         .Where(x => x.Contragent.Id == contragentId)
                                         .Select(x => (TypeManagementManOrg?)x.TypeManagement)
                                         .FirstOrDefault();

                                switch (typeManagement)
                                {
                                    case null:
                                        return;

                                    case TypeManagementManOrg.TSJ:
                                        executantCode = "9";
                                        break;

                                    case TypeManagementManOrg.UK:
                                        executantCode = "0";
                                        break;

                                    case TypeManagementManOrg.JSK:
                                        executantCode = "11";
                                        break;
                                }
                            }
                            break;
                    }
                }
                else if (prescription.Inspection.PersonInspection == PersonInspection.Official)
                {
                    switch (prescription.Inspection.TypeJurPerson)
                    {
                        case TypeJurPerson.LocalGovernment:
                            executantCode = "16";
                            break;

                        case TypeJurPerson.SupplyResourceOrg:
                        case TypeJurPerson.ServOrg:
                            executantCode = "3";
                            break;

                        case TypeJurPerson.ManagingOrganization:
                            {
                                var typeManagement = manOrgService
                                         .GetAll()
                                         .Where(x => x.Contragent.Id == contragentId)
                                         .Select(x => (TypeManagementManOrg?)x.TypeManagement)
                                         .FirstOrDefault();

                                switch (typeManagement)
                                {
                                    case null:
                                        return;

                                    case TypeManagementManOrg.TSJ:
                                        executantCode = "10";
                                        break;

                                    case TypeManagementManOrg.UK:
                                        executantCode = "1";
                                        break;

                                    case TypeManagementManOrg.JSK:
                                        executantCode = "12";
                                        break;
                                }
                            }

                            break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(executantCode))
                {
                    prescription.Executant = executantService.GetAll().FirstOrDefault(x => x.Code == executantCode);
                }
            }
            finally
            {
                Container.Release(executantService);
                Container.Release(manOrgService);
            }
        }

    }
}