﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.DocumentGji
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class ChelyabinskDocumentLongText : BaseEntity
    {
        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual DocumentGji DocumentGji { get; set; }

        /// <summary>
        /// Сведения о лицах, допустивших нарушения
        /// </summary>
        public virtual byte[] PersonViolationInfo { get; set; }

        /// <summary>
        /// Сведения о том, что нарушения были допущены в результате
        /// виновных действий (бездействия) должностных лиц и/или
        /// работников проверяемого лица
        /// (о ужос)
        /// </summary>
        public virtual byte[] PersonViolationActionInfo { get; set; }

		/// <summary>
		/// Описание нарушения
		/// </summary>
		public virtual byte[] ViolationDescription { get; set; }
	}
}