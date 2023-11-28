namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.DomainService;

    /// <summary>
    /// Операция "Перераспределение оплаты"
    /// </summary>
    public class SumAmountOperation : PersonalResolutionOperationBase
    {
        /// <summary>
        /// Ключ регистрации
        /// </summary>
        public static string Key => nameof(SumAmountOperation);

        /// <inheritdoc />
        public override string Code => Key;

        /// <inheritdoc />
        public override string Name => "Сумма штрафа";

        /// <inheritdoc />
        public override string PermissionKey => "GkhGji.Resolution.Registry.Action.SumAmountOperation";


        ///// <summary>
        ///// Интерфейс сервиса перераспределения оплат
        ///// </summary>
        //public IPersonalResolutionSumAmountService PersonalResolutionSumAmountService { get; set; }

        ///// <inheritdoc />
        //public override IDataResult Execute(BaseParams baseParams)
        //{
        //    return this.PersonalResolutionSumAmountService.Execute(baseParams);
        //}

        ///// <inheritdoc />
        //public override IDataResult GetDataForUI(BaseParams baseParams)
        //{
        //    return this.PersonalResolutionSumAmountService.GetDataForUI(baseParams);
        //}
    }
}