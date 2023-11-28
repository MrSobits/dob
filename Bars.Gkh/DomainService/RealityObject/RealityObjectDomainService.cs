namespace Bars.Gkh.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    /// <summary>
    /// Домен сервис для фильтрация по оператору и его МО
    /// </summary>
    public class RealityObjectDomainService : FileStorageDomainService<RealityObject>
    {
        /// <summary>
        /// DomainService Органы местного самоуправления
        /// </summary>
        public IDomainService<LocalGovernment> LocalGovernmentDomain { get; set; }

        /// <summary>
        /// DomainService Региональный оператор
        /// </summary>
        public IDomainService<RegOperator> RegOperatorDomain { get; set; }

        /// <summary>
        /// DomainService Жилой дом договора управляющей организации
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRoDomain { get; set; }

        /// <summary>
        /// DomainService Фильтрация по оператору и его МО
        /// </summary>
        /// <returns></returns>
        public override IQueryable<RealityObject> GetAll()
        {

            /* Фильтрация по оператору такая:
             * у оператора есть список МО и список Контрагентов
             * Если указан список контаргентов, то мы получаем только
             * те дома которые находятся в управлении у данных контарегнтов в текущий момент
             * Но! Если в поле "Организации" содержится запись из реестра ("Органы местного самоуправления" или Участники процесса / Роли контрагента/ Региональные операторы), фильтровать только по МО.
             * Если указан список МО то находим те дома которые принадлежат к этому МО
             */
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var municipalityList = userManager.GetMunicipalityIds();
            var contragentList = userManager.GetContragentIds();

            //Если в поле "Организации" содержится запись из реестра "Органы местного самоуправления", фильтровать только по МО.
            var isLocalGovernment = false;

            //Если контрагент "Региональные операторы" (Участники процесса / Роли контрагента), фильтровать только по МО.
            var isRegOperator = false;

            if (contragentList.Any())
            {
                isLocalGovernment = this.LocalGovernmentDomain.GetAll().Count(x => contragentList.Contains(x.Contragent.Id)) > 0;
                isRegOperator = this.RegOperatorDomain.GetAll().Count(x => contragentList.Contains(x.Contragent.Id)) > 0;
            }

            var noServiceFilter =
                userManager.GetActiveUser()
                    .With(y => y.Roles.Select(x => x.Role))
                    .Return(y => y.Any(x => userManager.GetNoServiceFilterRoles().Contains(x)));

            return base.GetAll()
                .WhereIf(
                    municipalityList.Count > 0,
                    x => municipalityList.Contains(x.Municipality.Id) ||
                        (x.Municipality.ParentMo != null && municipalityList.Contains(x.Municipality.ParentMo.Id)) ||
                        municipalityList.Contains(x.MoSettlement.Id))
                .WhereIf(
                    (!noServiceFilter && !isLocalGovernment && !isRegOperator) && contragentList.Count > 0,
                    y => this.ManOrgContractRoDomain.GetAll()
                        .Any(x => x.RealityObject.Id == y.Id
                            && contragentList.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id)
                            && x.ManOrgContract.StartDate <= DateTime.Today
                            && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Today)));

        }


    }
}