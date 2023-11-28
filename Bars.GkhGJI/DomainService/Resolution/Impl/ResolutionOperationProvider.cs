namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Castle.Windsor;

    public class ResolutionOperationProvider : IResolutionOperationProvider
    {
        public IWindsorContainer Container { get; set; }

        private static List<ResolutionOperationProxy> _operations;

        public List<ResolutionOperationProxy> GetAllOperations()
        {
            if (_operations != null)
            {
                return _operations;
            }

            return
                _operations = Container.ResolveAll<IPersonalResolutionOperation>()
                    .Select(x => new ResolutionOperationProxy
                    {
                        Code = x.Code,
                        Name = x.Name,
                        PermissionKey = x.PermissionKey
                    })
                    .ToList();
        }

        public IDataResult ChangeSumAmount(BaseParams baseParams)
        {
            try 
            {
                var loadParam = baseParams.GetLoadParam();
                var amount = baseParams.Params.GetAs<decimal?>("PenaltyAmount");
                var resolutionIds = baseParams.Params.GetAs<long[]>("resolutionIds");
                var domainService = Container.ResolveDomain<Resolution>();

                if (amount != null)
                {
                    var data = domainService.GetAll()
                    .Where(x => resolutionIds.Contains(x.Id))
                    .Select(x => x);

                    var totalCount = data.Count();

                    foreach (var res in data)
                    {
                        res.PenaltyAmount = amount;
                        domainService.Update(res);
                    }
                    return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
                }
                return new ListDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
        }

        public IDataResult Execute(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("operationCode");

            if (code.IsEmpty() || !Container.Kernel.HasComponent(code))
            {
                return new BaseDataResult(false, "Не удалось получить исполнителя операции");
            }

            return Container.Resolve<IPersonalResolutionOperation>(code).Execute(baseParams);
        }
        public IDataResult CreateProtocols(BaseParams baseParams)
        {
            try
            {
                var loadParam = baseParams.GetLoadParam();
                var protocolDate = baseParams.Params.GetAs<DateTime?>("NextCommissionDate");
                var resolutionIds = baseParams.Params.GetAs<long[]>("resolutionIds");
                var domainService = Container.ResolveDomain<DocumentGji>();

                var data = domainService.GetAll()
                .Where(x => resolutionIds.Contains(x.Id))
                .Select(x => x);

                var totalCount = data.Count();
                var documentRulesService = Container.ResolveAll<IDocumentGjiRule>();
                var rule = documentRulesService.FirstOrDefault(x => x.Id == "ResolutionToProtocolRule");
                foreach (var res in resolutionIds.ToList())
                {
                    if (protocolDate.HasValue)
                    {
                        var result = rule.CreateDocument(new DocumentGji { Id = res, ObjectCreateDate = protocolDate.Value });
                    }
                    else
                    {
                        var result = rule.CreateDocument(new DocumentGji { Id = res });
                    }
                }
                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
        }

        public IDataResult ChangeSentToOSP(BaseParams baseParams)
        {
            try
            {
                var loadParam = baseParams.GetLoadParam();
                var sentToOSP = CheckboxToBool(baseParams.Params.GetAs<string>("sentToOSP"));
                var resolutionIds = baseParams.Params.GetAs<long[]>("resolutionIds");
                var domainService = Container.ResolveDomain<Resolution>();

                if (sentToOSP != null)
                {
                    var data = domainService.GetAll()
                    .Where(x => resolutionIds.Contains(x.Id))
                    .Select(x => x);

                    var totalCount = data.Count();

                    foreach (var res in data)
                    {
                        res.SentToOSP = (bool)sentToOSP;
                        domainService.Update(res);
                    }
                    return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
                }
                return new ListDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
        }

        public IDataResult ChangeOSP(BaseParams baseParams)
        {
            var jurInstDomainService = Container.Resolve<IDomainService<JurInstitution>>();
            try
            {
                var loadParam = baseParams.GetLoadParam();
                var ospId = baseParams.Params.GetAs<long?>("ospId");
                var resolutionIds = baseParams.Params.GetAs<long[]>("resolutionIds");
                var domainService = Container.ResolveDomain<Resolution>();
                
                if (ospId != null)
                {
                    var data = domainService.GetAll()
                    .Where(x => resolutionIds.Contains(x.Id))
                    .Select(x => x);

                    var totalCount = data.Count();

                    foreach (var res in data)
                    {
                        res.OSP = jurInstDomainService.GetAll().Where(x => x.Id == ospId).Select(x => x).FirstOrDefault();
                        domainService.Update(res);
                    }
                    return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
                }
                return new ListDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
        }

        public bool? CheckboxToBool(string entry)
        {
            if (entry.Contains("false"))
            {
                return false;
            }
            else if (entry.Contains("true"))
            {
                return true;
            }
            else return null;
        }
    }
}
