namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.DomainService;

    /// <summary>
    /// Операция "Изменение отдела судебных приставов"
    /// </summary>
    public class ChangeOSPOperation : PersonalResolutionOperationBase
    {
        /// <summary>
        /// Ключ регистрации
        /// </summary>
        public static string Key => nameof(ChangeOSPOperation);

        /// <inheritdoc />
        public override string Code => Key;

        /// <inheritdoc />
        public override string Name => "Отдел судебных приставов";

        /// <inheritdoc />
        public override string PermissionKey => "GkhGji.Resolution.Registry.Action.ChangeOSPOperation";
    }
}