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
    public partial class ComissionMeetingDocument : BaseEntity
    {

        /// <summary>
        /// Заседание
        /// </summary>
        public virtual ComissionMeeting ComissionMeeting { get; set; }

        /// <summary>
        /// Члекн комиссии
        /// </summary>
        public virtual DocumentGji DocumentGji { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Решение комиссии
        /// </summary>
        public virtual ComissionDocumentDecision ComissionDocumentDecision { get; set; }


    }
}