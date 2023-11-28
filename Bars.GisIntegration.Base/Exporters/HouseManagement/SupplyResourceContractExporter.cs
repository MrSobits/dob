﻿namespace Bars.GisIntegration.Base.Exporters.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement;
    using Bars.GisIntegration.Base.Tasks.SendData.HouseManagement;

    /// <summary>
    /// Класс экспортер данных по договорам
    /// </summary>
    public class SupplyResourceContractExporter : BaseDataExporter
    {
        /// <summary>
        /// Наименование экспортера
        /// </summary>
        public override string Name => "Экспорт договора ресурсоснабжения";

        /// <summary>
        /// Порядок импорта в списке
        /// </summary>
        public override int Order => 100;

        /// <summary>
        /// Получить список методов, которые должны быть выполнены перед текущим
        /// </summary>
        /// <returns>Список методов, которые должны быть выполнены перед текущим</returns>
        public override List<string> GetDependencies()
        {
            var orgRegistryExporter = this.Container.Resolve<IDataExporter>("OrgRegistryExporter");
      //      var dataProviderExporter = this.Container.Resolve<IDataExporter>("DataProviderExporter");

            try
            {
                return new List<string>
                {
                    orgRegistryExporter.Name,
            //        dataProviderExporter.Name,
                    "Экспортировать список справочников",
                    "Экспортировать данные справочника"
                };
            }
            finally
            {
                this.Container.Release(orgRegistryExporter);
          //      this.Container.Release(dataProviderExporter);
            }
        }

        /// <summary>
        /// Тип задачи получения результатов экспорта
        /// </summary>
        public override Type SendDataTaskType => typeof(ExportSupplyResourceContractTask);

        /// <summary>
        /// Тип задачи подготовки данных
        /// </summary>
        public override Type PrepareDataTaskType => typeof(SupplyResourceContractPrepareDataTask);

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public override FileStorageName? FileStorage => FileStorageName.HomeManagement;
    }
}
