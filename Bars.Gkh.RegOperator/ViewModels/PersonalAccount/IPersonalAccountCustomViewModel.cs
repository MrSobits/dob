namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using B4;
    using DataResult;

    internal interface IPersonalAccountCustomViewModel
    {
        ListDataResult<AccountByRealityObjectDto> ListAccountByRealityObject(BaseParams baseParams);
    }

    internal class AccountByRealityObjectDto
    {
        public long Id { get; set; }

        public string AccountOwnerName { get; set; }
    }
}