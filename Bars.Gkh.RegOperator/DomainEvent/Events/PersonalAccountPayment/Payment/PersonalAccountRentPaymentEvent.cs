﻿namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.Payment
{
    using Domain.ValueObjects;
    using Entities;

    public class PersonalAccountRentPaymentEvent : PersonalAccountPaymentEvent
    {
        public PersonalAccountRentPaymentEvent(MoneyStream money, BasePersonalAccount account)
            : base(account, money)
        {
        }
    }
}