namespace Bars.GkhGji.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Данный интерфейс отвечает за формирование документа ГЖИ
    /// в случае, если Инициатор - другой Документ ГЖИ
    /// </summary>
    public interface IDocumentGjiRule
    {
        /// <summary>
        /// Код региона
        /// </summary>
        string CodeRegion { get; }

        /// <summary>
        /// Идентификатр реализации
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Краткое описание
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Наименование документ резултата
        /// </summary>
        string ResultName { get; }

        /// <summary>
        /// Карточка, которую нужно открыть после создания дкоумента
        /// </summary>
        string ActionUrl { get; }

        /// <summary>
        /// Тип документа инициатора, того кто инициирует действие
        /// </summary>
        TypeDocumentGji TypeDocumentInitiator { get; }

        /// <summary>
        /// Тип документа результата, тоесть того который должен получится в резултате формирвоания
        /// </summary>
        TypeDocumentGji TypeDocumentResult { get; }

        /// <summary>
        /// Получение параметров котоыре переданы с клиента
        /// </summary>
        void SetParams(BaseParams baseParams);

        /// <summary>
        /// Метод формирования документа сразу по объекту документа (Если ест ьнеобходимость)
        /// </summary>
        IDataResult CreateDocument(DocumentGji document);

        /// <summary>
        /// проверка валидности правила
        /// Например перед выполнением действия требуется проверить
        /// Можно ли формирвоать какойто дкоумент, например Если уже есть по документу уже созданные 
        /// то можно недават ьсоздавать новые (если требуется по процессу)
        /// </summary>
        IDataResult ValidationRule(DocumentGji document);
    }
}
