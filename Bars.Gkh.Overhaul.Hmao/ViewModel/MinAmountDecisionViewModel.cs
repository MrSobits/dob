﻿namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class MinAmountDecisionViewModel : BaseViewModel<MinAmountDecision>
    {
        public override IDataResult Get(IDomainService<MinAmountDecision> domainService, BaseParams baseParams)
        {
            var value = domainService.Get(baseParams.Params["id"].To<long>());

            if (value == null)
            {
                return new BaseDataResult(null);
            }

            var sizeOfPaymentSubject =
                Container.Resolve<IDomainService<PaymentSizeMuRecord>>().GetAll()
                    .Where(x => x.Municipality.Id == value.RealityObject.Municipality.Id)
                    .Where(x => x.PaymentSizeCr.DateStartPeriod < value.ObjectCreateDate)
                    .Where(x => !x.PaymentSizeCr.DateEndPeriod.HasValue
                                || x.PaymentSizeCr.DateEndPeriod > value.ObjectCreateDate)
                    .Select(x => x.PaymentSizeCr.PaymentSize)
                    .FirstOrDefault();


            var res = new
            {
                value.Id,
                value.PropertyOwnerProtocol,
                value.PropertyOwnerDecisionType,
                value.MoOrganizationForm,
                value.MethodFormFund,
                value.SizeOfPaymentOwners,
                value.PaymentDateStart,
                value.PaymentDateEnd,
                SizeOfPaymentSubject = sizeOfPaymentSubject
            };

            return new BaseDataResult(res);
        }
    }
}