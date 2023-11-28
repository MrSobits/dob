namespace Bars.Gkh.RegOperator.Domain
{
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.Owner;

    /// <summary>
    /// Интерфейс  "Собственник в исковом заявлении"
    /// </summary>
    public interface ILawsuitOwnerInfoService
    {
        /// <summary>
        /// Получение по Лс собственников для искового заявления
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Собственники в исковом заявлении</returns>
        IDataResult GetInfo(BaseParams baseParams);

        /// <summary>
        /// Расчитать задолженность
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        IDataResult DebtCalculate(BaseParams baseParams);



        /// <summary>
        /// Расчитать дату начала задолженности
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        IDataResult DebtStartDateCalculate(BaseParams baseParams);
        
        /// <summary>
        /// Рассчитывает дату по всем Лс дела
        /// </summary>
        /// <param name="documentClwId"></param>
        /// <returns></returns>
        IDataResult CalcLegalWithReferenceCalc(long documentClwId);


        /// <summary>
        /// Расчитать дату начала задолженности
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        IDataResult DebtStartDateCalculateChelyabinsk(BaseParams baseParams);

        /// <summary>
        /// Расчитать дату начала задолженности
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        IDataResult GetDebtStartDateCalculate(BaseParams baseParams);
    }
}