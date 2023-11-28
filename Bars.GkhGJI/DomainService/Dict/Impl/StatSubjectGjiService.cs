using System.Linq;
using Castle.Windsor;

namespace Bars.GkhGji.DomainService
{
    using B4;
    using B4.Utils;

    using Entities;

    public class StatSubjectGjiService : IStatSubjectGjiService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddSubsubject(BaseParams baseParams)
        {
            try
            {
                var subjectId = baseParams.Params.ContainsKey("subjectId") ? baseParams.Params["subjectId"].ToLong() : 0;

                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                    ? baseParams.Params["objectIds"].ToString()
                                    : string.Empty;

                if (string.IsNullOrEmpty(objectIds) || subjectId == 0)
                {
                    return new BaseDataResult
                        {
                            Success = false,
                            Message = "Не удалось получить тематику и/или подтематики"
                        };
                }
                var service = Container.Resolve<IDomainService<StatSubjectSubsubjectGji>>();

                var existRecs =
                    service.GetAll()
                           .Where(x => x.Subject.Id == subjectId)
                           .Select(x => x.Subsubject.Id)
                           .Distinct()
                           .ToList();

                foreach (var id in objectIds.Split(',').Select(x => x.ToLong()))
                {
                    if (!existRecs.Contains(id))
                    {
                        var newRec = new StatSubjectSubsubjectGji
                            {
                                Subject = new StatSubjectGji {Id = subjectId},
                                Subsubject = new StatSubsubjectGji {Id = id}
                            };

                        service.Save(newRec);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }
    }
}