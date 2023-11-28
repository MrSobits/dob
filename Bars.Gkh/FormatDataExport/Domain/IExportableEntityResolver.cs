namespace Bars.Gkh.FormatDataExport.Domain
{
    using System.Collections.Generic;
    using System.Management.Instrumentation;

    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Сервис получения экспортируемых секций
    /// </summary>
    public interface IExportableEntityResolver
    {
        /// <summary>
        /// Получить экспортируемую сущность по коду
        /// </summary>
        /// <param name="entityCode">Код сущности</param>
        /// <param name="providerType">Тип поставщика информации</param>
        /// <exception cref="InstanceNotFoundException">Сущность не найдена</exception>
        IExportableEntity GetEntity(string entityCode, FormatDataExportProviderType providerType);

        /// <summary>
        /// Получить все сущности доступные поставщику
        /// </summary>
        /// <param name="providerType">Тип поставщика информации</param>
        ICollection<IExportableEntity> GetEntityList(FormatDataExportProviderType providerType);

        /// <summary>
        /// Получить все зависимые сущности доступные поставщику
        /// </summary>
        /// <param name="entityGroupCodes">Коды группы сущностей</param>
        /// <param name="providerType">Тип поставщика информации</param>
        ICollection<IExportableEntity> GetInheritedEntityList(IList<string> entityGroupCodes, FormatDataExportProviderType providerType);
    }
}