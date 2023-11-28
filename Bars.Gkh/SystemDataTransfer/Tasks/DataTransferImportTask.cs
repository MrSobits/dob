namespace Bars.Gkh.SystemDataTransfer.Tasks
{
    using System;
    using System.IO;
    using System.ServiceModel;

    using Bars.B4;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.SystemDataTransfer.Domain;

    /// <summary>
    ///  Задача Quartz импорта данных и дальнейшего проброса результата
    /// </summary>
    public class DataTransferImportTask : BaseTask
    {
        /// <inheritdoc />
        public override void Execute(DynamicDictionary dictionary)
        {
            var guid = dictionary.GetAs<Guid>("guid");

            ExplicitSessionScope.CallInNewScope(() =>
            {
                var dataTransferProvider = this.Container.Resolve<IDataTransferProvider>();
                var dataTransferService = this.Container.Resolve<ISystemIntegrationService>();

                Action<string, bool> action = (section, success) =>
                {
                    dataTransferService.HandleSectionImportState(guid, section, success, true);
                };

                try
                {
                    dataTransferProvider.OnSectionImportDone += action;

                    dataTransferService.NotifyStartImport(guid);

                    dataTransferProvider.Import(dictionary.GetAs<Stream>("stream"));
                    dataTransferService.SendImportResult(guid, new BaseDataResult());
                }
                catch (FaultException exception)
                {
                    this.Container.Resolve<ILogManager>().Error("Ошибка во время отправки результата импорта", exception);
                }
                catch (Exception exception)
                {
                    this.Container.Resolve<ILogManager>().Error("Ошибка во время импорта", exception);
                    dataTransferService.SendImportResult(guid, BaseDataResult.Error(exception.Message));
                }
                finally
                {
                    dataTransferProvider.OnSectionImportDone -= action;

                    this.Container.Release(dataTransferProvider);
                    this.Container.Release(dataTransferService);
                }
            });
        }
    }
}