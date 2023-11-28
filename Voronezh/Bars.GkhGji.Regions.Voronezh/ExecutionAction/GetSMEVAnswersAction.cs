using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
using System;

namespace Bars.GkhGji.Regions.Voronezh.ExecutionAction
{
    /// <summary>
    /// Периодическая проверка СМЭВа на результаты
    /// </summary>
    public class GetSMEVAnswersAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        public override string Description => "Запрашивает из СМЭВа все ответы и обрабатывает их (пока только те, что добавлены после 2018)";

        public override string Name => "Проверить ответы в СМЭВ";

        public override Func<IDataResult> Action => GetSMEVResponses;

       // public bool IsNeedAction() => true;

        private IDataResult GetSMEVResponses()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), new BaseParams());
                return new BaseDataResult(true, "Задача успешно поставлена");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(taskManager);
            }
        }
    }
}
