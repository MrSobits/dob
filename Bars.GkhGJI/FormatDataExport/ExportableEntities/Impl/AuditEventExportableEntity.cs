namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Мероприятия проверки
    /// </summary>
    public class AuditEventExportableEntity : BaseExportableEntity<InspectionGji>
    {
        /// <inheritdoc />
        public override string Code => "AUDITEVENT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        private const string EventMessage =
            "Мероприятие по контролю, необходимые для достижения целей и задач проведения проверки: " +
            "Провести комплексное обследование жилищного фонда. " +
            "В случае выявления нарушений принять предусмотренные меры по их устранению.";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.EntityRepository.GetAll()
                .Select(x => x.Id)
                .AsEnumerable()
                .Select(x => new ExportableRow(x,
                        new List<string>
                        {
                            x.ToStr(), // 1. Проверка
                            this.Yes, // 2. Номер (передаем 1)
                            AuditEventExportableEntity.EventMessage, // 3. Мероприятие
                            string.Empty // 4. Дополнительная информация
                        }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Проверка",
                "Номер",
                "Мероприятие",
                "Дополнительная информация"
           };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "AUDIT"
            };
        }
    }
}