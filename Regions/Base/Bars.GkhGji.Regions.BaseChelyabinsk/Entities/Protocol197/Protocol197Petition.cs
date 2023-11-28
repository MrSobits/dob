namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using System;

    /// <summary>
    /// Протокол
    /// </summary>
    public class Protocol197Petition : BaseEntity
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public virtual Protocol197 Protocol197 { get; set; }

        /// <summary>
        /// ФИО ходатайствующего
        /// </summary>
        public virtual string PetitionAuthorFIO { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string PetitionAuthorDuty { get; set; }

        /// <summary>
        /// Место работы
        /// </summary>
        public virtual string Workplace { get; set; }

        /// <summary>
        /// Текст ходатайства
        /// </summary>
        public virtual string PetitionText { get; set; }

        /// <summary>
        /// Текст ходатайства
        /// </summary>
        public virtual string PetitionDecisionText { get; set; }

        /// <summary>
        /// Текст ходатайства
        /// </summary>
        public virtual DateTime PetitionDate { get; set; }

        /// <summary>
        /// удоволетворено
        /// </summary>
        public virtual YesNoNotSetPartially Aprooved { get; set; }
    }
}