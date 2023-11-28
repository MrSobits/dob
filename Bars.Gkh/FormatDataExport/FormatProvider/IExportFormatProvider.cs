namespace Bars.Gkh.FormatDataExport.FormatProvider
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Провайдер работы с данными в системе ЖКХ
    /// </summary>
    public interface IExportFormatProvider
    {
        /// <summary>
        /// Событие уведомляет о смене прогесса выполнения
        /// </summary>
        event EventHandler<float> OnProgressChanged;

        /// <summary>
        /// Событие уведомляет о завершении экспорта
        /// </summary>
        event EventHandler<IList<string>> OnAfterExport;

        /// <summary>
        /// Оператор предоставляющий данные
        /// </summary>
        Operator Operator { get; set; }

        /// <summary>
        /// Контрагент предоставляющий данные
        /// </summary>
        Contragent Contragent { get; set; }

        /// <summary>
        /// Параметры фильтрации данных
        /// </summary>
        DynamicDictionary DataSelectorParams { get; }

        /// <summary>
        /// Выгрузить данные
        /// </summary>
        /// <param name="pathToSave">Путь для сохранения</param>
        void Export(string pathToSave);

        /// <summary>
        /// Список служебных секций
        /// </summary>
        IList<string> ServiceEntityCodes { get; }

        /// <summary>
        /// Версия формата
        /// </summary>
        string FormatVersion { get; }

        /// <summary>
        /// Обобщенная информация об ошибках
        /// </summary>
        string SummaryErrors { get; }

        /// <summary>
        /// Коды экспортируемых сущностей
        /// </summary>
        IList<string> EntityCodeList { get; }

            /// <summary>
        /// Токен отмены операции
        /// </summary>
        CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Логи операций
        /// </summary>
        LogOperation LogOperation { get; set; }
    }
}