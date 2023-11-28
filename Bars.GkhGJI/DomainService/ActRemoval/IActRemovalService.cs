﻿namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public interface IActRemovalService
    {
        IDataResult GetInfo(long? documentId);

        IDataResult ListView(BaseParams baseParams);

        IQueryable<ViewActRemoval> GetViewList();
    }
}