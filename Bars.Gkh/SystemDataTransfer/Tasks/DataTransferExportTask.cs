namespace Bars.Gkh.SystemDataTransfer.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.ServiceModel;

    using Bars.B4;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.SystemDataTransfer.Domain;

    /// <summary>
    /// Задача Quartz экспорта данных и дальнейшего проброса результата
    /// </summary>
    public class DataTransferExportTask : BaseTask
    {
        /// <inheritdoc />
        public override void Execute(DynamicDictionary dynamicDictionary)
        {
            var guid = dynamicDictionary.GetAs<Guid>("guid");

            ExplicitSessionScope.CallInNewScope(() =>
            {
                var dataTransferProvider = this.Container.Resolve<IDataTransferProvider>();
                var dataTransferService = this.Container.Resolve<ISystemIntegrationService>();
                try
                {
                    // сообщаем о взятии задачи в работу
                    dataTransferService.NotifyStartExport(guid);

                    var resultStream = dataTransferProvider.Export(
                        dynamicDictionary.GetAs("typeNames", new List<string>()),
                        dynamicDictionary.GetAs("exportDependencies", false));

                    dataTransferService.SendExportResult(guid, new GenericDataResult<Stream>(resultStream));
                }
                catch (FaultException exception)
                {
                    this.Container.Resolve<ILogManager>().Error("Ошибка во время отправки результата экспорта", exception);
                }
                catch (Exception exception)
                {
                    this.Container.Resolve<ILogManager>().Error("Ошибка во время отправки результата импорта", exception);
                    dataTransferService.SendExportResult(guid, new GenericDataResult<Stream>(null, exception.Message) { Success = false });
                }
                finally
                {
                    this.Container.Release(dataTransferProvider);
                    this.Container.Release(dataTransferService);
                }
            });
        }
    }
}