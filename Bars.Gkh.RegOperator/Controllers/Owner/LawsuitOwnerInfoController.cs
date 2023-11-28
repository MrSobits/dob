namespace Bars.Gkh.RegOperator.Controllers.Owner
{
    using B4;
    using Domain;
    using Entities.Owner;
    using System.Web.Mvc;

    using Bars.Gkh.Domain;

    /// <summary>
    /// Контролер "Собственник в исковом заявлении"
    /// </summary>
    public class LawsuitOwnerInfoController : B4.Alt.DataController<LawsuitOwnerInfo>
    {
        public ILawsuitOwnerInfoService Service { get; set; }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.GetInfo(baseParams));
        }

        /// <summary>
        /// Расчитать задолженность
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        [ActionPermission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate")]
        public ActionResult DebtCalculate(BaseParams baseParams)
        {
            return this.Service.DebtCalculate(baseParams).ToJsonResult();
        }       

        /// <summary>
        /// Расчитать дату начала задолженности
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        [ActionPermission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate")]
        public ActionResult DebtStartDateCalculate(BaseParams baseParams)
        {
            return this.Service.DebtStartDateCalculate(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Расчитать дату начала задолженности
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        [ActionPermission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate")]
        public ActionResult GetDebtStartDateCalculate(BaseParams baseParams)
        {
            return this.Service.GetDebtStartDateCalculate(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Расчитать дату начала задолженности
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        [ActionPermission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate")]
        public ActionResult DebtStartDateCalculateChelyabinsk(BaseParams baseParams)
        {
            return this.Service.DebtStartDateCalculateChelyabinsk(baseParams).ToJsonResult();
        }
    }
}