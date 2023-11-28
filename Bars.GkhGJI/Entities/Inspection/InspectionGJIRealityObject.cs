namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Проверяемые дома в инспекционной проверки ГЖИ
    /// </summary>
    public class InspectionGjiRealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Мероприятие комиссии
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Номера квартир
        /// </summary>
        public virtual string RoomNums { get; set; }
    }
}