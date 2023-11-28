namespace Bars.Gkh.Gis.Entities.Register.LoadedFileRegister
{
    using System;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Modules.Security;
    using Enum;
    using SupplierRegister;

    /// <summary>
    /// Реестр загруженных файлов
    /// </summary>
    public class LoadedFileRegister : BaseEntity
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User B4User { get; set; }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public virtual long Size { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual TypeStatus TypeStatus { get; set; }

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Идентификатор лога
        /// </summary>
        public virtual FileInfo Log { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public virtual SupplierRegister Supplier { get; set; }

        /// <summary>
        /// Формат загрузки
        /// </summary>
        public virtual TypeImportFormat Format { get; set; }

        /// <summary>
        /// Результат импорта
        /// </summary>
        public virtual ImportResult ImportResult { get; set; }

        /// <summary>
        /// Название импорта
        /// </summary>
        public virtual string ImportName { get; set; }


        /// <summary>
        /// Расчетный месяц
        /// </summary>
        public virtual DateTime? CalculationDate { get; set; }

        
    }
}