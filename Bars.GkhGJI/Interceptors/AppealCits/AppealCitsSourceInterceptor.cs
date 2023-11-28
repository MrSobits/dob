﻿namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Entities;
    using System.Linq;

    public class AppealCitsSourceInterceptor : EmptyDomainInterceptor<AppealCitsSource>
    {
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<AppealCitsSource> service, AppealCitsSource entity)
        {

            if (entity.RevenueSourceNumber != "")
            {
                string answersNums = entity.RevenueSourceNumber;
                string sourceNames = "";
                var appcitCont = this.Container.Resolve<IRepository<AppealCits>>();
                AppealCits thisAppeal = appcitCont.Get(entity.AppealCits.Id);
                var sourcesCont = this.Container.Resolve<IDomainService<AppealCitsSource>>();
                var sources = sourcesCont.GetAll()
                    .Where(x => x.AppealCits.Id == thisAppeal.Id)
                    .Where(x => x.Id != entity.Id)
                    .Select(x => x.RevenueSourceNumber + (x.RevenueDate.HasValue? " от " + x.RevenueDate.Value.ToString("dd.MM.yyyy"):"")).ToList();
                foreach (string thisNum in sources)
                {
                    if (!string.IsNullOrEmpty(thisNum))
                    {
                        answersNums += "; " + thisNum;
                    }
                }
                thisAppeal.IncomingSources = answersNums;
                if (entity.RevenueSource != null)
                {
                    sourceNames = entity.RevenueSource.Name;
                    var names = sourcesCont.GetAll()
                    .Where(x => x.AppealCits.Id == thisAppeal.Id)
                    .Where(x => x.Id != entity.Id)
                    .Where(x=> x.RevenueSource != null)
                    .Select(x => x.RevenueSource.Name).ToList();
                    foreach (string thisName in names)
                    {
                        if (!string.IsNullOrEmpty(thisName))
                        {
                            sourceNames += "; " + thisName;
                        }
                    }
                }
                thisAppeal.IncomingSourcesName = sourceNames;
                appcitCont.Update(thisAppeal);

            }

            return this.Success();

        }

        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsSource> service, AppealCitsSource entity)
        {
            if (entity.RevenueSourceNumber != "")
            {
                string answersNums = entity.RevenueSourceNumber + (entity.RevenueDate.HasValue ? " от " + entity.RevenueDate.Value.ToString("dd.MM.yyyy") : "");
                var appcitCont = this.Container.Resolve<IRepository<AppealCits>>();
                AppealCits thisAppeal = appcitCont.Get(entity.AppealCits.Id);
                var sourcesCont = this.Container.Resolve<IDomainService<AppealCitsSource>>();
                var sources = sourcesCont.GetAll()
                    .Where(x => x.AppealCits.Id == thisAppeal.Id)
                    .Where(x => x.Id != entity.Id)
                    .Select(x => x.RevenueSourceNumber + (x.RevenueDate.HasValue ? " от " + x.RevenueDate.Value.ToString("dd.MM.yyyy") : "")).ToList();
                foreach (string thisNum in sources)
                {
                    if (!string.IsNullOrEmpty(thisNum))
                    {
                        answersNums += "; " + thisNum;
                    }
                }
                thisAppeal.IncomingSources = answersNums;
                appcitCont.Update(thisAppeal);

            }

            return this.Success();

        }


    }
}
