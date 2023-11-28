namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    /// <summary>
    /// Общая информация
    /// </summary>
    public class InfoExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "_INFO";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var period = this.SelectParams.GetAs<ChargePeriod>("Period");
            if (period != null)
            {
                this.SelectParams["Info.Year"] = period.StartDate.Year;
                this.SelectParams["Info.Month"] = period.StartDate.Month;
            }

            var inn = this.Contragent.Inn;
            var kpp = this.Contragent.Kpp;
            var ogrn = this.Contragent.Ogrn;
            var name = this.Contragent.Name;
            var dataId = string.Empty;
            var date = this.SelectParams.GetAs("Info.Date", DateTime.Now);
            var year = this.SelectParams.GetAs("Info.Year", date.Year);
            var month = this.SelectParams.GetAs("Info.Month", date.Month);
            var senderName = this.SelectParams.GetAs<string>("Info.SenderName");
            var phone = this.SelectParams.GetAs<string>("Info.Phone");
            var type = this.SelectParams.GetAs<int>("FormatDataExportProviderType");

            var info = new ExportableRow(1,
                new List<string>
                {
                    this.FormatVersion,
                    inn,
                    kpp,
                    ogrn,
                    name,
                    dataId,
                    year.ToStr(),
                    month.ToStr(),
                    this.GetDateTime(date),
                    senderName,
                    phone,
                    type.ToStr()
                });

            return new List<ExportableRow>(new[] { info });
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => new List<int> { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Версия формата",
                "ИНН",
                "КПП",
                "ОГРН (ОГРНИП)",
                "Наименование организации (ФИО ИП)",
                "Ключ банка данных (Наименование банка данных)",
                "Год",
                "Месяц",
                "Дата и время формирования",
                "ФИО отправителя",
                "Телефон отправителя",
                "Тип поставщика информации"
            };
        }
    }
}