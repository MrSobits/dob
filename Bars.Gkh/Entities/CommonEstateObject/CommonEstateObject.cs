namespace Bars.Gkh.Entities.CommonEstateObject
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Объект общего имущества
    /// </summary>
    public class CommonEstateObject : BaseImportableEntity
    {
        /// <summary>
        /// Тип группы
        /// </summary>
        public virtual GroupType GroupType { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Код реформы
        /// </summary>
        public virtual string ReformCode { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        public virtual string ShortName { get; set; }

        /// <summary>
        /// Флаг: Соответствует ЖК РФ
        /// </summary>
        public virtual bool IsMatchHc { get; set; }
        
        /// <summary>
        /// Флаг: Включен в программу субъекта
        /// </summary>
        public virtual bool IncludedInSubjectProgramm { get; set; }

        /// <summary>
        /// Флаг: Является инженерной сетью
        /// </summary>
        public virtual bool IsEngineeringNetwork { get; set; }

        /// <summary>
        /// Множественный объект
        /// </summary>
        public virtual bool MultipleObject { get; set; }

        /// <summary>
        /// Вес
        /// </summary>
        public virtual int Weight { get; set; }

        /// <summary>
        /// Является основным
        /// </summary>
        public virtual bool IsMain { get; set; }
    }
}