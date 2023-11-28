namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.DomainService;

    /// <summary>
    /// Операция "Перераспределение оплаты"
    /// </summary>
    public class Create2025Operation : PersonalResolutionOperationBase
    {
        /// <summary>
        /// Ключ регистрации
        /// </summary>
        public static string Key => nameof(Create2025Operation);

        /// <inheritdoc />
        public override string Code => Key;

        /// <inheritdoc />
        public override string Name => "Создать протоколы 20.25";

        /// <inheritdoc />
        public override string PermissionKey => "GkhGji.Resolution.Registry.Action.Create2025Operation";


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