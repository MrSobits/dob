using GisGkhLibrary.Entities.HouseMgmt;
using GisGkhLibrary.Entities.HouseMgmt.Account;
using GisGkhLibrary.Entities.HouseMgmt.Person;
using GisGkhLibrary.Entities.HouseMgmt.Person.Identifiers;
using GisGkhLibrary.Enums;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.HouseManagement;
using GisGkhLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GisGkhLibrary.Managers
{
    /// <summary>
    /// Человекоюзабельная обертка для методов импорта\экспорта ЛС
    /// </summary>
    public static class AccountManager
    {
        /// <summary>
        /// Импорт данныx о лицевых счетах
        /// </summary>
        public static void ImportAccountData(IEnumerable<Account> account)
        {
            HouseManagementService.ImportAccountData(account.Select(x => ConvertAccount(x)).ToArray());
        }

        private static importAccountRequestAccount ConvertAccount(Account account)
        {
            return new importAccountRequestAccount
            {
                TransportGUID = Guid.NewGuid().ToString(),
                AccountGUID = account.AccountGUID.ToString(),
                AccountNumber = account.AccountNumber,
                AccountReasons = ConvertAccountReasons(account.Contracts),
                Item = account.AccountType == Enums.HouseMgmt.AccountType.NotDefined,
                ItemElementName = ConvertAccountType(account.AccountType),
                CreationDateSpecified = account.CreationDate.HasValue,
                CreationDate = account.CreationDate.GetValueOrDefault(),
                LivingPersonsNumber = account.LivingPersonsNumber.ToString(),
                TotalSquareSpecified = account.TotalSquare.HasValue,
                TotalSquare = account.TotalSquare.GetValueOrDefault(),
                HeatedAreaSpecified = account.HeatedArea.HasValue,
                HeatedArea = account.HeatedArea.GetValueOrDefault(),
                Closed = ConvertClosed(account.CloseInfo),
                Accommodation = account.Accommodations.Select(x => ConvertAccomodation(x)).ToArray(),
                PayerInfo = ConvertPayerInfo(account.PayerInfo)
            };
        }

        private static ClosedAccountAttributesType ConvertClosed(CloseInfo closeInfo)
        {
            if (closeInfo == null)
                return null;

            return new ClosedAccountAttributesType
            {
                CloseReason = closeInfo.CloseReason.GetHouseManagementNsiRef(),
                CloseDate = closeInfo.CloseDate,
                Description = closeInfo.Description
            };
        }

        private static ItemChoiceType7 ConvertAccountType(Enums.HouseMgmt.AccountType accountType)
        {
            switch(accountType)
            {
                case Enums.HouseMgmt.AccountType.CRAccount:
                    return ItemChoiceType7.isCRAccount;
                case Enums.HouseMgmt.AccountType.OGVorOMSAccount:
                    return ItemChoiceType7.isOGVorOMSAccount;
                case Enums.HouseMgmt.AccountType.RCAccount:
                    return ItemChoiceType7.isRCAccount;
                case Enums.HouseMgmt.AccountType.RSOAccount:
                    return ItemChoiceType7.isRSOAccount;
                case Enums.HouseMgmt.AccountType.TKOAccount:
                    return ItemChoiceType7.isTKOAccount;
                case Enums.HouseMgmt.AccountType.UOAccount:
                    return ItemChoiceType7.isUOAccount;
                default:
                    return ItemChoiceType7.isCRAccount;
            }
        }

        private static AccountTypeAccommodation ConvertAccomodation(Accommodation accommodation)
        {
            if (accommodation == null)
                return null;

            return new AccountTypeAccommodation
            {
                Item = accommodation.Identifier.ToString(),
                ItemElementName = ConvertIdentifierType(accommodation.IdentifierType),
                SharePercentSpecified = accommodation.SharePercent.HasValue,
                SharePercent = accommodation.SharePercent.GetValueOrDefault()
            };
        }

        private static ItemChoiceType8 ConvertIdentifierType(Enums.HouseMgmt.AccommodationIdentifierType identifierType)
        {
            switch(identifierType)
            {
                case Enums.HouseMgmt.AccommodationIdentifierType.FIASHouseGuid:
                    return ItemChoiceType8.FIASHouseGuid;
                case Enums.HouseMgmt.AccommodationIdentifierType.PremisesGUID:
                    return ItemChoiceType8.PremisesGUID;
                case Enums.HouseMgmt.AccommodationIdentifierType.LivingRoomGUID:
                    return ItemChoiceType8.LivingRoomGUID;
                default:
                    return ItemChoiceType8.FIASHouseGuid;
            }
        }

        private static AccountTypePayerInfo ConvertPayerInfo(PayerInfo payerInfo)
        {
            if (payerInfo == null)
                return null;

            return new AccountTypePayerInfo
            {
                IsRenterSpecified = payerInfo.IsRenter.HasValue,
                IsRenter = payerInfo.IsRenter.GetValueOrDefault(),
                isAccountsDividedSpecified = payerInfo.IsAccountsDivided.HasValue,
                isAccountsDivided = payerInfo.IsAccountsDivided.GetValueOrDefault(),
                Item = payerInfo.Person is PhysicalPerson ? (object)(ConvertIndividualPayerInfo(payerInfo.Person as PhysicalPerson)) : (object)(ConvertRegOrgPayerInfo(payerInfo.Person as JuridicalPerson))
            };
        }

        private static AccountIndType ConvertIndividualPayerInfo(PhysicalPerson payerInfo)
        {
            return new AccountIndType
            {
                SexSpecified = payerInfo.Gender.HasValue,
                Sex = ConvertToAccountIndTypeSex(payerInfo.Gender.GetValueOrDefault()),
                DateOfBirthSpecified = payerInfo.DateOfBirth.HasValue,
                DateOfBirth = payerInfo.DateOfBirth.GetValueOrDefault(),
                Item = ConvertIdentifier(payerInfo.Identifier)
            };
        }

        private static object ConvertIdentifier(IdentifierBase identifier)
        {
            if (identifier is Identifier id)
            {
                return new ID
                {
                    Type = id.IdentifierType.GetHouseManagementNsiRef(),
                    Series = id.Series,
                    Number = id.Number,
                    IssueDate = id.IssueDate
                };
            }
            else if (identifier is SNILSIdentifier snils)
            {
                return snils.Number;
            }
            else throw new GISGKHRequestException($"Не найдено преобразование из типа {identifier.GetType().Name} в формат ГИС ГМП");
        }

        private static RegOrgVersionType ConvertRegOrgPayerInfo(JuridicalPerson payerInfo)
        {
            return new RegOrgVersionType
            {
                orgVersionGUID = payerInfo.OrgRootEntityGUID.ToString()
            };
        }

        private static AccountIndTypeSex ConvertToAccountIndTypeSex(Gender gender)
        {
            switch(gender)
            {
                case Gender.Man:
                    return AccountIndTypeSex.M;
                case Gender.Women:
                    return AccountIndTypeSex.F;
               default:
                    return AccountIndTypeSex.M;
            }
        }

        private static AccountReasonsImportType ConvertAccountReasons(IEnumerable<SupplyResourceContract> contracts)
        {
            return new AccountReasonsImportType
            {
                SupplyResourceContract = contracts.Select(x => ConvertSupplyResourceContract(x)).ToArray()
                //SocialHireContract
                //TKOContract
            };
        }

        private static AccountReasonsImportTypeSupplyResourceContract ConvertSupplyResourceContract(SupplyResourceContract contract)
        {
            var dict = GetContractDictionary(contract);
            return new AccountReasonsImportTypeSupplyResourceContract
            {
                ItemsElementName = dict.Select(x => x.Key).ToArray(),
                Items = dict.Select(x => x.Value).ToArray(),
            };
        }

        private static Dictionary<ItemsChoiceType7, object> GetContractDictionary(SupplyResourceContract contract)
        {
            var dict = new Dictionary<ItemsChoiceType7, object>();

            dict.Add(ItemsChoiceType7.ContractGUID, contract.ContractGuid.ToString());
            dict.Add(ItemsChoiceType7.ContractNumber, contract.ContractNumber);
            dict.Add(ItemsChoiceType7.IsContract, contract.IsContract);
            dict.Add(ItemsChoiceType7.SigningDate, contract.SigningDate.GetValueOrDefault());

            return dict;
        }
    }
}
