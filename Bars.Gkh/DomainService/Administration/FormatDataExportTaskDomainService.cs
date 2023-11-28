namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Entities.Administration.FormatDataExport;

    public class FormatDataExportTaskDomainService : BaseDomainService<FormatDataExportTask>
    {
        public IEnumerable<IDomainServiceInterceptor<FormatDataExportTask>> DomainServiceInterceptors { get; set; }

        /// <inheritdoc />
        public override IDataResult Delete(BaseParams baseParams)
        {
            var idList = baseParams.Params.GetAs("records", new List<long>(), true);
            foreach (var id in idList)
            {
                var task = this.Repository.Get(id);

                if (task == null)
                {
                    throw new ArgumentNullException(nameof(baseParams),
                        $@"Не найдена запись с идентификатором {id}");
                }

                task.IsDelete = true;
                this.Repository.Update(task);
                IDataResult result =new BaseDataResult();

                this.CallAfterDeleteInterceptors(task, ref result, this.DomainServiceInterceptors);
            }

            return new BaseDataResult(idList);
        }
    }
}