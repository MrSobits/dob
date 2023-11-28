namespace Bars.B4.Modules.Analytics.Reports.Sti
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Bars.B4.Modules.Analytics.Extensions;
    using Stimulsoft.Report;
    using Stimulsoft.Report.Dictionary;

    public class StiDictBuilder
    {
        public void AddDictionaryDataSource(StiReport stiReport, string name, Type type)
        {
            AddTableSource(stiReport.Dictionary, name, type.Name, type);
        }

        #region Internal realization
        private StiDataSource AddTableSource(StiDictionary stiDictionary, string name, string nameInSource, Type type)
        {
            var tableSource = new StiDataTableSource(nameInSource, name);
            stiDictionary.DataSources.Add(tableSource);
            foreach (var propInfo in type.GetProperties())
            {
                if (propInfo.PropertyType.IsSimpleType())
                {
                    AddSimpleColumn(tableSource, propInfo);
                }
                else if (propInfo.PropertyType.IsCollectionType())
                {
                    var itemType = propInfo.PropertyType.GetItemType();
                    AddComplexColumn(stiDictionary, tableSource, propInfo, type, itemType);
                }
                else if (propInfo.PropertyType.IsClass)
                {
                    AddComplexColumn(stiDictionary, tableSource, propInfo, type, propInfo.PropertyType);
                }
            }
            return tableSource;
        }

        private void AddSimpleColumn(StiDataSource dataSource, PropertyInfo propInfo)
        {
            var column = new StiDataColumn
            {
                Name = propInfo.GetDisplayName(),
                NameInSource = propInfo.Name,
                Type = propInfo.PropertyType
            };
            dataSource.Columns.Add(column);
        }

        private void AddComplexColumn(StiDictionary stiDictionary, StiDataTableSource tableSource, PropertyInfo propInfo, Type parentType, Type colType)
        {
            var childSource = AddTableSource(stiDictionary, propInfo.GetDisplayName(), propInfo.Name, colType);
            if (!parentType.GetProperties().Any(x => "Id".Equals(x.Name))
                && !colType.GetProperties().Any(x => GetRelationColumnName(parentType).Equals(x.Name)))
            {
                return;
            }
            var relation = new StiDataRelation
            {
                Name = tableSource.Name,
                NameInSource = tableSource.NameInSource,
                ParentColumns = new[] { "Id" },
                ChildColumns = new[] { GetRelationColumnName(parentType) },
                ParentSource = tableSource,
                ChildSource = childSource
            };
            stiDictionary.Relations.Add(relation);
        }
        #endregion

        private string GetRelationColumnName(Type type)
        {
            return string.Format("{0}Id", type.Name);
        }
    }

}
