namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using System;
    using System.Collections.Generic;
    using Bars.GkhGji.Enums;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Повестка на Комиссию
    /// </summary>
    public class Subpoena : BaseGkhEntity
    {
        /// <summary>
        /// Имя повестки
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Связь с протоколом 
        /// </summary>
        public virtual Protocol Protocol { get; set; }


        /// <summary>
        /// Дата рассмотрения дела
        /// </summary>
        public virtual DateTime? DateOfProceedings { get; set; }

        /// <summary>
        /// Комиссия
        /// </summary>
        public virtual ComissionMeeting ComissionMeeting { get; set; }

        /// <summary>
        /// Время рассмотрения дела(час)
        /// </summary>
        public virtual int HourOfProceedings { get; set; }

        /// <summary>
        /// Время рассмотрения дела(мин)
        /// </summary>
        public virtual int MinuteOfProceedings { get; set; }

        /// <summary>
        /// Количество экземпляров
        /// </summary>
        public virtual int ProceedingCopyNum { get; set; }

        /// <summary>
        /// Место рассмотрения дела
        /// </summary>
        public virtual string ProceedingsPlace { get; set; }



    }
}