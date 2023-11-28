namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.DomainService;

    /// <summary>
    /// Операция "Изменение "Направлено в ССП""
    /// </summary>
    public class ChangeSentToOSPOperation : PersonalResolutionOperationBase
    {
        /// <summary>
        /// Ключ регистрации
        /// </summary>
        public static string Key => nameof(ChangeSentToOSPOperation);

        /// <inheritdoc />
        public override string Code => Key;

        /// <inheritdoc />
        public override string Name => "Направлено в ССП (да/нет)";

        /// <inheritdoc />
        public override string PermissionKey => "GkhGji.Resolution.Registry.Action.ChangeSentToOSPOperation";
    }
}