using Bars.GkhGji.Regions.Voronezh.Enums;
using Bars.B4.DataAccess;
using System;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Voronezh.Entities.ASFK
{
    /// <summary>
    /// Информация из расчетных документов, может быть BDPD или BDPL
    /// </summary>
    public class BDOPER : BaseEntity
    {
        /// <summary>
        /// Файл в формате .BD из Федерального казначейства в котором находился данный BDOPER
        /// </summary>
        public virtual ASFK ASFK { get; set; }

        /// <summary>
        /// Привязанное постановление
        /// </summary>
        public virtual Resolution Resolution { get; set; }

        /// <summary>
        /// ГУИД, по нему связан с VTOPER
        /// </summary>
        public virtual string GUID { get; set; }

        /// <summary>
        /// Имеется ли оплата в постановлении
        /// </summary>
        public virtual bool IsPayFineAdded { get; set; }

        /// <summary>
        /// Сумма платежа
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// ИНН плательщика
        /// </summary>
        public virtual string InnPay { get; set; }

        /// <summary>
        /// КПП плательщика
        /// </summary>
        public virtual string KppPay { get; set; }

        /// <summary>
        /// Наименование плательщика
        /// </summary>
        public virtual string NamePay { get; set; }

        /// <summary>
        /// Назначение платежа
        /// </summary>
        public virtual string Purpose { get; set; }

        /// <summary>
        /// Код документа АДБ
        /// </summary>
        public virtual ASFKADBDocCode KodDocAdb { get; set; }

        /// <summary>
        /// КБК
        /// </summary>
        public virtual string Kbk { get; set; }

        /// <summary>
        /// ID выписки (ASFK) на случай смены
        /// </summary>
        public virtual long RelatedASFKId { get; set; }
    }
}
