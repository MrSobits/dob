namespace Bars.GkhGji.Regions.Tatarstan.Quartz.Impl
{
    using System;
    using B4.IoC.Lifestyles.SessionLifestyle;
    using B4.Logging;
    using B4.Modules.Quartz;
    using B4.Utils;
    using Integration;

    public class GisChargeTask : BaseTask
    {
        public override void Execute(DynamicDictionary @params)
        {
            ExplicitSessionScope.CallInNewScope(() =>
            {
                var logger = Container.Resolve<ILogManager>();

                try
                {
                    var result = Container.Resolve<IGisGmpIntegration>().UploadCharges();

                    if (result.Success)
                    {
                        logger.Info("Отправка начислений в ГИС ГМП успешно выполнена");
                    }
                    else
                    {
                        logger.Error("При отправке начислений в ГИС ГМП произошла ошибка: " + result.Message);
                    }
                }
                catch (Exception e)
                {
                    logger.Error("При отправке начислений в ГИС ГМП произошла ошибка: " + e.Message);
                }
            });
        }
    }
}