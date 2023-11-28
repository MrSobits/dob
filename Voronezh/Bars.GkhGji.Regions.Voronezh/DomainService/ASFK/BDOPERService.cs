using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;
using Castle.Windsor;
using NHibernate.Util;
using System;
using System.Linq;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class BDOPERService : IBDOPERService
    {
        #region Properties              

        public IWindsorContainer Container { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<VTOPER> VTOPERDomain { get; set; }
        public IDomainService<BDOPER> BDOPERDomain { get; set; }
        public IDomainService<Protocol197> Protocol197Domain { get; set; }
        public IDomainService<Resolution> ResolutionDomain { get; set; }
        public IRepository<Resolution> ResolutionRepo { get; set; }
        public IDomainService<ResolutionPayFine> ResolutionPayFineDomain { get; set; }
        public IDomainService<IndividualPerson> IndividualPersonDomain { get; set; }
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Public methods

        public IDataResult AddPayFines(BaseParams baseParams)
        {
            var bdoperId = baseParams.Params.GetAs<long>("bdoperId");
            var resolId = baseParams.Params.GetAs<long>("resolutionId");

            var bdoper = BDOPERDomain.Get(bdoperId);
            var vtoper = VTOPERDomain.GetAll()
                .Where(x => x.GUID == bdoper.GUID)
                .FirstOrDefault();

            if (bdoper.IsPayFineAdded == true)
            {
                var relatedPayFine = ResolutionPayFineDomain.GetAll()
                .Where(x => x.Resolution.Id == bdoper.Resolution.Id)
                .Where(x => x.DocumentDate == vtoper.DateDoc)
                .Where(x => x.DocumentNum == vtoper.NomDoc)
                .Where(x => x.Amount == bdoper.Sum)
                .Where(x => x.TypeDocumentPaid == TypeDocumentPaidGji.PaymentASFK)
                .FirstOrDefault();

                ResolutionPayFineDomain.Delete(relatedPayFine.Id);
                bdoper.Resolution = null;
                bdoper.IsPayFineAdded = false;
                BDOPERDomain.Update(bdoper);
            }

            ResolutionPayFineDomain.Save(new ResolutionPayFine
            {
                Resolution = ResolutionDomain.Get(resolId),
                DocumentDate = vtoper.DateDoc,
                DocumentNum = vtoper.NomDoc,
                Amount = bdoper.Sum,
                TypeDocumentPaid = TypeDocumentPaidGji.PaymentASFK
            });

            bdoper.Resolution = ResolutionDomain.Get(resolId);
            bdoper.IsPayFineAdded = true;
            BDOPERDomain.Update(bdoper);

            CalcBalance(ResolutionDomain.Get(resolId));

            return new BaseDataResult { Success = true, Message = "Успешно" };
        }

        public IDataResult GetResolution(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var bdoperId = loadParam.Filter.GetAs("bdoperId", 0L);

            var bdoper = BDOPERDomain.Get(bdoperId);
            var relatedResolId = bdoper.Resolution != null ? bdoper.Resolution.Id : 0;

            var data = ResolutionDomain.GetAll()
                .Where(x => x.Id == relatedResolId)
                .Select(x => new
                  {
                      x.Id,
                      x.DocumentDate,
                      x.DocumentNumber,
                      ComissionName = GetComissionName(x.Id),
                      x.InLawDate,
                      x.IndividualPerson.Fio,
                      ContragentName = x.Contragent.Name,
                      x.PenaltyAmount
                  })
                  .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public IDataResult GetListResolutionsForSelect(BaseParams baseParams)
        {
            //Если надо будет фильтровать еще и по комиссии
            //var zonaInspId = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
            //    .Where(x => x.Inspector.Id == UserManager.GetActiveOperator().Inspector.Id)
            //    .Select(x => x.ZonalInspection.Id)
            //    .FirstOrDefault();
            //var protocols197 = Container.Resolve<IDomainService<Protocol197>>().GetAll()
            //    .Where(x => x.ZonalInspection.Id == zonaInspId)
            //    .Select(x => x.Id)
            //    .ToList();
            //var resolIdsList = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
            //    .Where(x => protocols197.Contains(x.Parent.Id))
            //    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
            //    .Select(x => x.Children.Id)
            //    .ToList();

            var loadParam = baseParams.GetLoadParam();
            var bdoperId = baseParams.Params.GetAs("bdoperId", 0L);
            var showAll = baseParams.Params.GetAs("showAll", false);

            var bdoperPurpose = BDOPERDomain.Get(bdoperId).Purpose;

            var relatedViolatorId = IndividualPersonDomain.GetAll()
                .Where(x => bdoperPurpose.ToLower().Contains(x.Fio.ToLower()))
                .Select(x => x.Id)
                .FirstOrDefault();

            var data = ResolutionDomain.GetAll()
                .WhereIf(!showAll, x => x.IndividualPerson.Id == relatedViolatorId)
                .Where(x => x.Paided != Gkh.Enums.YesNoNotSet.Yes)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNumber,
                    ComissionName = GetComissionName(x.Id),
                    x.InLawDate,
                    x.IndividualPerson.Fio,
                    ContragentName = x.Contragent.Name,
                    x.PenaltyAmount
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        #endregion

        #region Private methods

        private string GetComissionName(long resolId)
        {
            var prot197Id = DocumentGjiChildrenDomain.GetAll()
                .Where(x => x.Children.Id == resolId)
                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol197)
                .Select(x => x.Parent.Id)
                .FirstOrDefault();
            var comissionName = Protocol197Domain.GetAll()
                .Where(x => x.Id == prot197Id)
                .Select(x => x.ComissionMeeting.ComissionName)
                .FirstOrDefault();

            return comissionName;
        }

        private void CalcBalance(Resolution resolution)
        {
            try
            {
                // Заполняем поле "Штраф оплачен (подробно)"
                var resolutionPayFineSum = ResolutionPayFineDomain
                         .GetAll()
                         .Where(x => x.Resolution.Id == resolution.Id)
                         .Sum(x => x.Amount)
                         .ToDecimal();
                var maxpaymentdate = ResolutionPayFineDomain
                         .GetAll()
                         .Where(x => x.Resolution.Id == resolution.Id)
                         .Max(x => x.DocumentDate);
                if (maxpaymentdate.HasValue)
                {
                    resolution.PaymentDate = maxpaymentdate;
                }

                if (resolution.PenaltyAmount.HasValue && resolutionPayFineSum > resolution.PenaltyAmount)
                {
                    resolution.PayStatus = ResolutionPaymentStatus.OverPaid;
                }
                else if (resolution.PenaltyAmount.HasValue && resolutionPayFineSum == resolution.PenaltyAmount)
                {
                    resolution.PayStatus = ResolutionPaymentStatus.Paid;
                }
                else if (resolution.PenaltyAmount.HasValue && resolutionPayFineSum > 0)
                {
                    resolution.PayStatus = ResolutionPaymentStatus.PartialPaid;
                }
                else
                {
                    resolution.PayStatus = ResolutionPaymentStatus.NotPaid;
                }
                ResolutionRepo.Update(resolution);
            }
            catch (Exception e)
            {

            }
        }

        #endregion
    }
}
