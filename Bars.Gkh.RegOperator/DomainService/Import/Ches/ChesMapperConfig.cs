namespace Bars.Gkh.RegOperator.DomainService.Import.Ches
{
    using AutoMapper;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    internal static class ChesMapperConfig
    {
        /// <summary>
        /// Инициализировать <see cref="Mapper"/>
        /// </summary>
        internal static void ConfigureAutoMapper()
        {
            Mapper.CreateMap<ChesMatchAccountOwner, ChesNotMatchAccountOwner>().ConvertUsing<ChesMatchAccountOwnerConverter>();
            Mapper.CreateMap<ChesNotMatchAccountOwner, ChesMatchAccountOwner>().ConvertUsing<ChesMatchAccountOwnerConverter>();
        }
    }

    /// <summary>
    /// Конвертер из Сопоставленнного абонента в сопоставленного и наоборот
    /// </summary>
    internal class ChesMatchAccountOwnerConverter : 
        ITypeConverter<ChesMatchAccountOwner, ChesNotMatchAccountOwner>,
        ITypeConverter<ChesNotMatchAccountOwner, ChesMatchAccountOwner>
    {
        /// <inheritdoc />
        ChesNotMatchAccountOwner ITypeConverter<ChesMatchAccountOwner, ChesNotMatchAccountOwner>.Convert(ResolutionContext context)
        {
            var owner = context.SourceValue as ChesMatchAccountOwner;
            var legalOwner = owner as ChesMatchLegalAccountOwner;
            if (legalOwner.IsNotNull())
            {
                return new ChesNotMatchLegalAccountOwner
                {
                    Inn = legalOwner.Inn,
                    Kpp = legalOwner.Kpp,
                    Name = legalOwner.Name,
                    OwnerType = legalOwner.OwnerType,
                    PersonalAccountNumber = legalOwner.PersonalAccountNumber
                };
            }

            var individualOwner = owner as ChesMatchIndAccountOwner;
            if (individualOwner.IsNotNull())
            {
                return new ChesNotMatchIndAccountOwner
                {
                    BirthDate = individualOwner.BirthDate,
                    Firstname = individualOwner.Firstname,
                    Surname = individualOwner.Surname,
                    Lastname = individualOwner.Lastname,
                    OwnerType = individualOwner.OwnerType,
                    PersonalAccountNumber = individualOwner.PersonalAccountNumber
                };
            }

            return null;
        }

        /// <inheritdoc />
        ChesMatchAccountOwner ITypeConverter<ChesNotMatchAccountOwner, ChesMatchAccountOwner>.Convert(ResolutionContext context)
        {
            var owner = context.SourceValue as ChesNotMatchAccountOwner;
            var legalOwner = owner as ChesNotMatchLegalAccountOwner;
            if (legalOwner.IsNotNull())
            {
                return new ChesMatchLegalAccountOwner
                {
                    Inn = legalOwner.Inn,
                    Kpp = legalOwner.Kpp,
                    Name = legalOwner.Name,
                    OwnerType = legalOwner.OwnerType,
                    PersonalAccountNumber = legalOwner.PersonalAccountNumber
                };
            }

            var individualOwner = owner as ChesNotMatchIndAccountOwner;
            if (individualOwner.IsNotNull())
            {
                return new ChesMatchIndAccountOwner
                {
                    BirthDate = individualOwner.BirthDate,
                    Firstname = individualOwner.Firstname,
                    Surname = individualOwner.Surname,
                    Lastname = individualOwner.Lastname,
                    OwnerType = individualOwner.OwnerType,
                    PersonalAccountNumber = individualOwner.PersonalAccountNumber
                };
            }

            return null;
        }
    }

}