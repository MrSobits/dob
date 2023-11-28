namespace Bars.Gkh.SchedulerTasks
{
    using System;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class UpdateRoContractTask : BaseTask, ITask<DynamicDictionary>
    {
        public override void Execute(DynamicDictionary @params)
        {
            var roRepository = Container.Resolve<IRepository<RealityObject>>();

            var roContractManOrgRepository = Container.Resolve<IRepository<ManOrgContractRealityObject>>();

            var nowDate = DateTime.Now.Date;

            ExplicitSessionScope.CallInNewScope(() =>
            {
                var currentContracts = roContractManOrgRepository.GetAll()
                    .Where(x => x.ManOrgContract.StartDate <= nowDate)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= nowDate)
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                        x.ManOrgContract.TypeContractManOrgRealObj
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key,
                        y => new
                        {
                            ManOrgs = y.AggregateWithSeparator(x => x.ManOrgName, ", "),
                            TypesContract = y.AggregateWithSeparator(x => x.TypeContractManOrgRealObj.GetEnumMeta().Display, ", ")
                        });

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        foreach (var ro in roRepository.GetAll())
                        {
                            var contract = currentContracts.Get(ro.Id);

                            if (contract != null)
                            {
                                ro.ManOrgs = contract.ManOrgs;
                                ro.TypesContract = contract.TypesContract;
                            }
                            else
                            {
                                ro.ManOrgs = null;
                                ro.TypesContract = null;
                            }

                            roRepository.Update(ro);
                        }

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            });
        }
    }
}