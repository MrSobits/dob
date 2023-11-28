using GisGkhLibrary.Enums;
using GisGkhLibrary.NsiCommon;
using System;
using System.Linq;

namespace GisGkhLibrary.Helpers
{
    internal static class DictionaryHelper
    {       
        /// <summary>
        /// Выдернуть строковое значение
        /// </summary>
        internal static string GetStringValue(NsiElementType element, string parameter)
        {
            return ((NsiElementStringFieldType)(element.NsiElementField.Where(y => y.Name == parameter).First())).Value;
        }

        /// <summary>
        /// Выдернуть ref значение
        /// </summary>
        /// <returns></returns>
        internal static (DictionaryType type, string name, string code, Guid guid) GetRefValue(NsiElementType element, string parameter)
        {
            var nsiRef = ((NsiElementNsiRefFieldType)(element.NsiElementField.Where(y => y.Name == parameter).First())).NsiRef;
            return ((DictionaryType)Enum.Parse(typeof(DictionaryType), nsiRef.NsiItemRegistryNumber),
                    nsiRef.Ref.Name,
                    nsiRef.Ref.Code,
                    Guid.Parse(nsiRef.Ref.GUID));
        }

        /// <summary>
        /// Выдернуть ОКЕИ значение
        /// </summary>
        internal static OKEI? GetOKEIValue(NsiElementType element, string parameter)
        {
            return OKEIHelper.GetOKEI(((NsiElementOkeiRefFieldType)(element.NsiElementField.Where(y => y.Name == parameter).First())).Code);
        }

        /// <summary>
        /// Выдернуть булевое значение
        /// </summary>
        internal static bool? GetBoolValue(NsiElementType element, string parameter)
        {
            var item = ((NsiElementBooleanFieldType)(element.NsiElementField.Where(y => y.Name == parameter).First()));

            return !item.ValueSpecified ? (bool?)null : item.Value;
        }
    }
}
