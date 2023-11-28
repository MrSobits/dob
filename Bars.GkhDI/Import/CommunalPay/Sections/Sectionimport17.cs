namespace Bars.GkhDi.Import.Sections
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Gkh.PassportProvider;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class SectionImport17 : ISectionImport
    {
        public IPassportProvider PassportProvider { get; set; }

        public string Name
        {
            get { return "Импорт из комплат секция #17"; }
        }

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section17.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;
            var realityObjectDict = importParams.RealObjsImportInfo;

            foreach (var section17Record in sectionsData.Section17)
            {
                var realityObject = realityObjectDict.ContainsKey(section17Record.CodeErc) ? realityObjectDict[section17Record.CodeErc] : null;
                if (realityObject == null)
                {
                    logImport.Warn(this.Name, string.Format("Не удалось получить дом с кодом ЕРЦ {0}", section17Record.CodeErc));
                    continue;
                }

                if (section17Record.ServiceDevice.IsEmpty())
                {
                    logImport.Warn(this.Name, string.Format("Не найден вид коммунального ресурса с кодом {0}", section17Record.ServiceDeviceCode));
                    continue;
                }

                if (section17Record.UnitsDevice.IsEmpty())
                {
                    logImport.Warn(this.Name, string.Format("Не найдена единица измерения с кодом {0}", section17Record.UnitsDeviceCode));
                }

                List<SerializePassportValue> values = new List<SerializePassportValue>
                {
                    new SerializePassportValue
                    {
                        ComponentCode = "Form_6_6_2",
                        CellCode = section17Record.ServiceDevice + ":2",
                        Value = "3"
                    }
                };

                if (section17Record.NumDevice.IsNotEmpty())
                {
                    values.Add(new SerializePassportValue
                    {
                        ComponentCode = "Form_6_6_2",
                        CellCode = section17Record.ServiceDevice + ":7",
                        Value = section17Record.NumDevice
                    });
                }

                if (section17Record.UnitsDevice.IsNotEmpty())
                {
                    values.Add(new SerializePassportValue
                    {
                        ComponentCode = "Form_6_6_2",
                        CellCode = section17Record.ServiceDevice + ":4",
                        Value = section17Record.UnitsDevice
                    });
                }

                if (section17Record.InstallDateDevice != null)
                {
                    values.Add(new SerializePassportValue
                    {
                        ComponentCode = "Form_6_6_2",
                        CellCode = section17Record.ServiceDevice + ":5",
                        Value = section17Record.InstallDateDevice.ToDateString()
                    });
                }

                if (section17Record.CheckDateDevice != null)
                {
                    values.Add(new SerializePassportValue
                    {
                        ComponentCode = "Form_6_6_2",
                        CellCode = section17Record.ServiceDevice + ":6",
                        Value = section17Record.CheckDateDevice.ToDateString()
                    });
                }

                this.PassportProvider.UpdateForm(realityObject.Id, "Form_6_6", values);
            }
        }
    }
}