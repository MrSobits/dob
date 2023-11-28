namespace Bars.GkhRf.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип оплаты подрядчику
    /// </summary>
    public enum TypePaymentRfCtr
    {
        [Display("Аванс")]
        Prepayment = 10,

        [Display("Оплата за капремонт")]
        CrPayment = 20,
    }
}
