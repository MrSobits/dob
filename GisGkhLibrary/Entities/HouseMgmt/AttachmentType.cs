using System;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Вложение
    /// </summary>
    public class AttachmentType
    {
        /// <summary>
        /// Наименование вложения
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание вложения
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор сохраненного вложения
        /// </summary>
        public Guid Attachment { get; set; }

        /// <summary>
        /// Хэш-тег вложения по алгоритму ГОСТ в binhex
        /// </summary>
        public string AttachmentHASH { get; set; }
    }
}
