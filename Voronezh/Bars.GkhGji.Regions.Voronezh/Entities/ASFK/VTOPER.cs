using Bars.GkhGji.Regions.Voronezh.Enums;
using Bars.B4.DataAccess;
using System;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Regions.Voronezh.Entities.ASFK
{
    /// <summary>
    /// Информация по исполненным операциям (раздел 2) и реквизиты ТОФК, по которым учтена операция
    /// </summary>
    public class VTOPER : BaseEntity
    {
        /// <summary>
        /// Файл в формате .VTH из Федерального казначейства в котором находился данный VTOPER
        /// </summary>
        public virtual ASFK ASFK { get; set; }

        /// <summary>
        /// ГУИД
        /// </summary>
        public virtual string GUID { get; set; }

        /// <summary>
        /// Код документа, подтверждающего проведение операции
        /// </summary>
        public virtual ASFKConfirmingDocCode KodDoc { get; set; }

        /// <summary>
        /// Номер документа, подтверждающего проведение операции
        /// </summary>
        public virtual string NomDoc { get; set; }

        /// <summary>
        /// Дата документа, подтверждающего проведение операции
        /// </summary>
        public virtual DateTime DateDoc { get; set; }

        /// <summary>
        /// Код документа АДБ
        /// </summary>
        public virtual ASFKADBDocCode KodDocAdb { get; set; }

        /// <summary>
        /// Номер документа АДБ
        /// </summary>
        public virtual string NomDocAdb { get; set; }

        /// <summary>
        /// Дата документа АДБ
        /// </summary>
        public virtual DateTime? DateDocAdb { get; set; }

        /// <summary>
        /// Сумма поступлений
        /// </summary>
        public virtual decimal SumIn { get; set; }

        /// <summary>
        /// Сумма возвратов
        /// </summary>
        public virtual decimal SumOut { get; set; }

        /// <summary>
        /// Сумма зачетов
        /// </summary>
        public virtual decimal SumZach { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// Тип КБК
        /// </summary>
        public virtual ASFKKBKType TypeKbk { get; set; }

        /// <summary>
        /// КБК
        /// </summary>
        public virtual string Kbk { get; set; }

        /// <summary>
        /// Код цели субсидии/субвенции
        /// </summary>
        public virtual string AddKlass { get; set; }

        /// <summary>
        /// Код по ОКТМО (не опечатка, в документации поле называется OKATO, а описывается как ОКТМО)
        /// </summary>
        public virtual string Okato { get; set; }

        /// <summary>
        /// ИНН АДБ
        /// </summary>
        public virtual string InnAdb { get; set; }

        /// <summary>
        /// КПП АДБ
        /// </summary>
        public virtual string KppAdb { get; set; }
    }
}
