﻿namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class ResolutionAnnexController : GkhGji.Controllers.ResolutionAnnexController<ResolutionAnnex>
    {
        public ResolutionAnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
            stamp = Properties.Resources.stamp_Voronezh;
        }
    }
}