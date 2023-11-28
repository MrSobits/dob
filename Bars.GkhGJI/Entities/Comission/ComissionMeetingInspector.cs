using Bars.Gkh.Enums;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Voronezh.Enums;

    /// <summary>
    /// Обращение граждан
    /// </summary>
    public partial class ComissionMeetingInspector : BaseEntity
    {

        /// <summary>
        /// Заседание
        /// </summary>
        public virtual ComissionMeeting ComissionMeeting { get; set; }

        /// <summary>
        /// Члекн комиссии
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual YesNoNotSet YesNoNotSet { get; set; }
        /// <summary>
        /// Заменяющий должность
        /// </summary>
        public virtual TypeCommissionMember TypeCommissionMember { get; set; }

    }
}