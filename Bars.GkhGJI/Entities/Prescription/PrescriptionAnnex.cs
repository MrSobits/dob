namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Приложения предписания ГЖИ
    /// </summary>
    public class PrescriptionAnnex : BaseGkhEntity
    {
        /// <summary>
        /// Предписание
        /// </summary>
        public virtual Prescription Prescription { get; set; }

        /// <summary>
        /// Тип приложения
        /// </summary>
        public virtual TypeAnnex TypeAnnex { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual TypePrescriptionAnnex TypePrescriptionAnnex { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID вложения
        /// </summary>
        public virtual string GisGkhAttachmentGuid { get; set; }

        /// <summary>
        /// Подписанный файл
        /// </summary>
        public virtual FileInfo SignedFile { get; set; }

        /// <summary>
        /// Подпись
        /// </summary>
        public virtual FileInfo Signature { get; set; }

        /// <summary>
        /// Сертификат
        /// </summary>
        public virtual FileInfo Certificate { get; set; }
      
        /// <summary>
        /// Статус файла 
        /// </summary>
        public virtual MessageCheck MessageCheck { get; set; }
    }
}