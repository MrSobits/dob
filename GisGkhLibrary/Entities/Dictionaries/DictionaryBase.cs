using GisGkhLibrary.Enums;
using GisGkhLibrary.NsiCommon;
using System;

namespace GisGkhLibrary.Entities.Dictionaries
{
    public abstract class DictionaryBase
    {
        public DictionaryBase(){}

        public DictionaryBase(NsiElementType element)
        {
            Code = element.Code;
            GUID = Guid.Parse(element.GUID);
            IsActual = element.IsActual;
        }

        /// <summary>
        /// Код справочника
        /// </summary>
        public abstract DictionaryType DictionaryType { get;}

        /// <summary>
        /// Справочник имеет постраничный вывод?
        /// </summary>
        public abstract bool Paging { get;}

        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public Guid GUID { get; set; }

        /// <summary>
        /// Код элемента справочника, уникальный в пределах справочника (но может быть несколько версий записи с одним кодом).
        /// </summary>
        public string Code { get; internal set; }

        /// <summary>
        /// Признак актуальности
        /// </summary>
        public bool IsActual { get; internal set; }

        internal RegOrgCommon.nsiRef GetRegOrgNsiRef()
        {
            return new RegOrgCommon.nsiRef
            {
                Code = Code,
                GUID = GUID.ToString()
            };
        }

        internal HouseManagement.nsiRef GetHouseManagementNsiRef()
        {
            return new HouseManagement.nsiRef
            {
                Code = Code,
                GUID = GUID.ToString()
            };
        }

        internal nsiRef GetNsiCommonNsiRef()
        {
            return new nsiRef
            {
                Code = Code,
                GUID = GUID.ToString()
            };
        }
    }
}
