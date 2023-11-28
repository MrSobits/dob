namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class NsoRealObjOverhaulDataService : IRealObjOverhaulDataService
    {
        public IDomainService<PublishedProgramRecord> PublishProgramRecordDomain { get; set; }

        public DateTime? GetPublishDateByRo(RealityObject ro)
        {
            return PublishProgramRecordDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Where(x => x.Stage2.Stage3Version.RealityObject.Id == ro.Id)
                .OrderByDescending(x => x.PublishedProgram.ObjectEditDate)
                .Select(x => x.PublishedProgram.PublishDate)
                .FirstOrDefault();
        }
    }
}