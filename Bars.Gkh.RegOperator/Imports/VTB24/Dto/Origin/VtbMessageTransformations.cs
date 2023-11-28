namespace Bars.Gkh.RegOperator.Imports.VTB24.Dto.Origin
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;
    using AutoMapper;
    using Bars.Gkh.RegOperator.Domain.ImportExport;
    using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;

    public class VtbMessageTransformations
    {
        private static readonly Regex FileNameValidatorRegex = new Regex(@"\d{1,12}");
        public const string VtbMessageId = "VtbMessageId";
        public const string VtbImportedSum = "VtbImportedSum";
        public const string VtbSender = "VtbSender";

        public static void ConfigureAutoMapper()
        {
            Mapper.CreateMap<VtbOperation, PersonalAccountPaymentInfoIn>()
                .ForMember(dst => dst.OwnerType, opt => opt.UseValue(PersonalAccountPaymentInfoIn.AccountType.Personal))
                // сумма пени
                .ForMember(dst => dst.PenaltyPaid,
                    opt => opt.MapFrom(v => v.PaymentType == VtbPaymentType.Penalty ? v.Amount : Decimal.Zero))
                // сумма плaтежа
                .ForMember(dst => dst.TargetPaid,
                    opt => opt.MapFrom(v => v.PaymentType == VtbPaymentType.Payment ? v.Amount : Decimal.Zero))
                .ForMember(dst => dst.PaymentDate, opt => opt.MapFrom(v => v.DateOperation))
                .ForMember(dst => dst.AccountNumber, opt => opt.MapFrom(v => v.Phone))
                .ForMember(dst => dst.DatePeriod, opt => opt.MapFrom(v => v.PaymentPeriod))
                .ForMember(dst => dst.ExternalSystemTransactionId, opt => opt.MapFrom(v => v.Uni))
                .ForMember(dst => dst.ReceiptId, opt => opt.MapFrom(v => v.ReceiptNumber.ToString(CultureInfo.InvariantCulture)));
        }

        private static IList<PersonalAccountPaymentInfoIn> Tranform(IEnumerable<VtbOperation> operations)
        {
            return Enumerable.ToList(operations.Select(Mapper.Map<PersonalAccountPaymentInfoIn>));
        }

        public static ImportResult<PersonalAccountPaymentInfoIn> Transform(Stream source, string fileName)
        {
            var s = new XmlSerializer(typeof(VtbMessage));
            var vtbMessage = (VtbMessage) s.Deserialize(source);

            if (!vtbMessage.IsValid())
            {
                throw new ArgumentException(
                    "Схема документа BTБ24 корректна, но содержимое не проходит внутренние проверки");
            }

            var result = new ImportResult<PersonalAccountPaymentInfoIn>
            {
                FileNumber = fileName,
                FileDate = vtbMessage.Date
            };

            // трансформируем и дополняем платежи
            var infoIns = Tranform(vtbMessage.Operations);

            result.Rows = (from v in infoIns select new ImportRow<PersonalAccountPaymentInfoIn>() {Value = v}).ToList();

            result.GeneralData[VtbMessageId] = vtbMessage.Id;
            result.GeneralData[VtbImportedSum] = vtbMessage.Total.Amount;
            result.GeneralData[VtbSender] = vtbMessage.Sender;

            return result;
        }


    }
}
