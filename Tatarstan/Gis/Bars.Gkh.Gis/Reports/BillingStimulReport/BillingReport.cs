namespace Bars.Gkh.Gis.Reports.BillingStimulReport
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using B4;
    using B4.Modules.Reports;

    using Bars.Gkh.Gis.Utils;

    using Castle.Windsor;
    using Gkh.Gis.DomainService.Report;
    using Stimulsoft.Report;

    public class BillingReport : IBillingReport
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>Отчет</summary>
        protected StiReport Report { get; set; }

        /// <summary>Формат печатной формы</summary>
        protected StiExportFormat ExportFormat { get; set; }

        public BillingReport()
        {
            Report = new StiReport();
        }

        public virtual IList<PrintFormExportFormat> GetExportFormats()
        {
            return new[]
            {
                new PrintFormExportFormat { Id = (int)StiExportFormat.Excel2007, Name = "MS Excel 2007"       },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Word2007,  Name = "MS Word 2007"        },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Pdf,       Name = "Adobe Acrobat"       },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Ppt2007,   Name = "MS Power Point"      },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Odt,       Name = "OpenDocument Writer" },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Text,      Name = "Текст (TXT)"         },
                new PrintFormExportFormat { Id = (int)StiExportFormat.ImagePng,  Name = "Изображение (PNG)"   },
                new PrintFormExportFormat { Id = (int)StiExportFormat.ImageSvg,  Name = "Изображение (SVG)"   },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Html,      Name = "Веб страница (Html)" }
            };
        }

        public virtual void PrepareReport(ReportParams reportParams)
        {
            throw new NotImplementedException();
        }

        public virtual string Name { get; set; }

        public virtual string Desciption { get; set; }

        public virtual string GroupName { get; set; }

        public virtual string ParamsController { get; set; }

        public virtual string RequiredPermission { get; set; }

        protected virtual byte[] BynaryReportTemplate { get; set; }

        public virtual void SetUserParams(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        public virtual void SetExportFormat(int formatId)
        {
            ExportFormat = (StiExportFormat)formatId;
        }

        public virtual void Open(byte[] reportTemplate)
        {
            Report.Load(reportTemplate);
        }

        public virtual void Generate(Stream result)
        {
            Report.Compile();

            if (!Report.IsRendered)
            {
                Report.Render();
            }

            result.Seek(0, SeekOrigin.Begin);

            Report.ExportDocument(ExportFormat, result);
        }

        public virtual MemoryStream GetGeneratedReport()
        {
            var reportParams = new ReportParams();
            Open(BynaryReportTemplate);
            PrepareReport(reportParams);

            var ms = new MemoryStream();
            Generate(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public virtual byte[] GetTemplate()
        {
            return BynaryReportTemplate;
        }

        /// <summary>
        /// Пребразование списка объектов в таблицу
        /// Источник: http://stackoverflow.com/questions/18100783/how-to-convert-a-list-into-data-table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        protected static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);

            //Получение всех свойств
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var t in props)
            {
                var prop = t;
                // типы B4 переделаны в строковые, т.к. не понимаются стимулрепортом
                if (prop.PropertyType.FullName.Contains("Bars.B4"))
                {
                    prop = new DummyPropertyInfo(prop.Name, typeof(string));
                }
                //Добавление колонки
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    //добавление значения
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
