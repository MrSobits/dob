﻿namespace Bars.GkhGji.DomainService
{
    using B4;

    /// <summary>
    /// Интерфейс для осуществления различных операций с ЛС
    /// </summary>
    public interface IPersonalResolutionOperation
    {
        string Code { get; }

        string Name { get; }

        /// <summary>
        /// Ключ прав доступа
        /// </summary>
        string PermissionKey { get; }

        IDataResult Execute(BaseParams baseParams);

        IDataResult GetDataForUI(BaseParams baseParams);
    }
}