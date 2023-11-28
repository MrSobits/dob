namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Dicts
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Префикс зональной жилищной инспекции
    /// </summary>
    public class ZonalInspectionPrefix : BaseImportableEntity
    {
        /// <summary>
        /// Зональная нспекция
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Префикс акта проверки
        /// </summary>
        public virtual string ActCheckPrefix { get; set; }

        /// <summary>
        /// Префикс протокола
        /// </summary>
        public virtual string ProtocolPrefix { get; set; }

        /// <summary>
        /// Префикс акта предписания
        /// </summary>
        public virtual string PrescriptionPrefix { get; set; }

        /// <summary>
        /// Префикс УИН
        /// </summary>
        public virtual string UINPrefix { get; set; }
    }
}