using System.Reflection;
using NHibernate.Mapping;

namespace Bars.B4.Modules.Analytics.Reports.Sti
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Bars.B4.Modules.Analytics.Extensions;
    using Stimulsoft.Report;

    /// <summary>
    /// 
    /// </summary>
    public class StiReportDataRegistrator
    {
        private readonly IDictionary<string, IList> collectionProps;

        /// <summary>
        /// 
        /// </summary>
        public StiReportDataRegistrator()
        {
            this.collectionProps = new Dictionary<string, IList>();
        }

        public void RegData(StiReport stiReport, string name, Type type, object data)
        {
            stiReport.RegData(name, data);

            var dataType = data.GetType();

            if (dataType.IsCollectionType())
            {
                var collection = (IEnumerable)data;
                foreach (var item in collection)
                {
                    this.FillData(type, item);
                }
            }
            else
            {
                this.FillData(type, data);
            }

            foreach (var collKey in this.collectionProps.Keys)
            {
                stiReport.RegData(collKey, this.collectionProps[collKey]);
            }
        }

        private void FillData(Type type, object obj)
        {
            foreach (var propInfo in type.GetProperties())
            {
                if (propInfo.PropertyType.IsCollectionType())
                {
                    IList collection;
                    if (!this.collectionProps.TryGetValue(propInfo.Name, out collection))
                    {
                        collection = new List<object>();
                        this.collectionProps[propInfo.Name] = collection;
                    }
                    
                    var items = (IEnumerable)propInfo.GetValue(obj, new object[] { });
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            collection.Add(item);
                        }
                    }
                }
            }
        }
    }
}
