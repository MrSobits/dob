namespace Bars.Gkh.FormatDataExport.FormatProvider.Converter
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    using Bars.B4.DataModels;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Конвертер данных приведения к формату экспорта
    /// </summary>
    public class ExportFormatConverter : IExportFormatConverter
    {
        private const int DefaultDecimalPlaces = 2;
        private const string DefaultDocumentNumber = "б/н";
        private readonly Regex dateRegEx = new Regex(@"\D(\d{1,2}\.\d{1,2}\.\d{2,4})|\D(\d{1,2}\s\w+?\s\d{2,4})\D", RegexOptions.Compiled);

        /// <inheritdoc />
        public string Yes => "1";

        /// <inheritdoc />
        public string No => "2";

        /// <inheritdoc />
        public string NotSet => "0";

        /// <inheritdoc />
        public string GetDate(DateTime? date)
        {
            return date?.ToString("dd.MM.yyyy") ?? string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDate(string date)
        {
            DateTime dt;
            return DateTime.TryParse(date, out dt) ? dt.ToString("dd.MM.yyyy") : string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDateTime(DateTime? date)
        {
            return date?.ToString("dd.MM.yyyy hh:mm:ss") ?? string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetTime(DateTime? date)
        {
            return date?.ToString("hh:mm:ss") ?? string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetFirstDateYear(int? year)
        {
            return year != null && year >= 1 && year <= 9999 ? this.GetDate(new DateTime(year.Value, 1, 1)) : string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetFirstDateYear(string year)
        {
            if (string.IsNullOrEmpty(year) || year == "0")
            {
                return string.Empty;
            }

            return year.Length <= 4 ? this.GetDate(new DateTime(year.ToInt(), 1, 1)) : string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDecimal(string decimalValue)
        {
            decimal result;
            return decimal.TryParse(decimalValue, out result)
                ? result.ToString($"F{ExportFormatConverter.DefaultDecimalPlaces}", System.Globalization.CultureInfo.InvariantCulture)
                : string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDecimal(string decimalValue, int decimalPlaces)
        {
            decimal result;
            return decimal.TryParse(decimalValue, out result)
                ? result.ToString($"F{decimalPlaces}", System.Globalization.CultureInfo.InvariantCulture)
                : string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDecimal(decimal? decimalValue)
        {
            return decimalValue.HasValue
                ? decimalValue.Value.ToString($"F{ExportFormatConverter.DefaultDecimalPlaces}", System.Globalization.CultureInfo.InvariantCulture)
                : string.Empty;

        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDecimal(decimal? decimalValue, int decimalPlaces)
        {
            return decimalValue.HasValue
                ? decimalValue.Value.ToString($"F{decimalPlaces}", System.Globalization.CultureInfo.InvariantCulture)
                : string.Empty;

        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDocumentNumber(string number)
        {
            return string.IsNullOrEmpty(number) ? ExportFormatConverter.DefaultDocumentNumber : number;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDocumentNumber(string number, string defaultValue)
        {
            return string.IsNullOrEmpty(number) ? defaultValue : number;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDocumentNumber(int? number)
        {
            return number?.ToString() ?? ExportFormatConverter.DefaultDocumentNumber;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDocumentNumber(int? number, string defaultValue)
        {
            return number?.ToString() ?? defaultValue;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetPaymentFoundation(ManOrgSetPaymentsOwnersFoundation paymentsOwnersFoundation)
        {
            switch (paymentsOwnersFoundation)
            {
                case ManOrgSetPaymentsOwnersFoundation.OwnersMeetingProtocol:
                    return this.Yes;

                case ManOrgSetPaymentsOwnersFoundation.OpenTenderResult:
                    return this.No;

                default:
                    return string.Empty;
            }
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetStrId(IHaveId entity)
        {
            // ReSharper disable once MergeConditionalExpression
            return entity != null ? entity.Id.ToString() : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetStrId(IHaveExportId entity)
        {
            // ReSharper disable once MergeConditionalExpression
            return entity != null ? entity.ExportId.ToString() : null;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetValueOrDefault(string value, string defaultValue = null)
        {
            return value.IsNotEmpty() ? value : defaultValue;
        }

        /// <inheritdoc />
        public string FindDate(string stringWithDate)
        {
            if (!string.IsNullOrWhiteSpace(stringWithDate))
            {
                var match = this.dateRegEx.Match(stringWithDate);

                if (match.Success)
                {
                    for (var i = 1; i < match.Groups.Count; i++)
                    {
                        if (match.Groups[i].Success)
                        {
                            return this.GetDate(match.Groups[i].Value);
                        }
                    }
                }
            }

            return string.Empty;
        }

        /// <inheritdoc />
        public string GetNotZeroValue(int? value)
        {
            return value > 0 ? value.ToStr() : null;
        }

        /// <inheritdoc />
        public string GetNotZeroValue(decimal? value)
        {
            return value > 0 ? this.GetDecimal(value) : null;
        }

        /// <inheritdoc />
        public string GetNotZeroValue(decimal? value, int decimalPlaces)
        {
            return value > 0 ? this.GetDecimal(value, decimalPlaces) : null;
        }
    }
}