namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Entities;
    using System.Linq;

    public class AppealCitsStatSubjectInterceptor : EmptyDomainInterceptor<AppealCitsStatSubject>
    {
        //public override IDataResult AfterCreateAction(IDomainService<AppealCitsStatSubject> service, AppealCitsStatSubject entity)
        //{
        //    var appealCitsContainer = Container.Resolve<IDomainService<AppealCits>>();
        //    var appealCitsStatSubjectContainer = Container.Resolve<IDomainService<AppealCitsStatSubject>>();
        //    var statSubjectGjiContainer = Container.Resolve<IDomainService<StatSubjectGji>>();
        //    try
        //    {
        //        var statSubjectsList = appealCitsStatSubjectContainer.GetAll()
        //            .Where(x=> entity.AppealCits.Id==x.AppealCits.Id)
        //            .Select(x => x.Subject.Name).ToList()
        //            ;
        //        AppealCits appeal = entity.AppealCits;
        //        string statSubjects = statSubjectGjiContainer.Get(entity.Subject.Id).Name;
        //        foreach(string subject in statSubjectsList)
        //        {
        //            if (statSubjects != "")
        //                statSubjects += ", " + subject;
        //            else statSubjects = subject;
        //        }
        //        appeal.StatementSubjects = statSubjects;
        //        appealCitsContainer.Update(appeal);
        //    }
        //    finally
        //    {
           
        //    }

        //    return this.Success();
            
        //}
        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsStatSubject> service, AppealCitsStatSubject entity)
        {
            var appealCitsContainer = Container.Resolve<IDomainService<AppealCits>>();
            var appealCitsStatSubjectContainer = Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            try
            {
                var statSubjectsList = appealCitsStatSubjectContainer.GetAll()
                    .Where(x => entity.AppealCits.Id == x.AppealCits.Id)
                    .Select(x => x.Subject.Name).ToList()
                    ;
                AppealCits appeal = appealCitsContainer.Get(entity.AppealCits.Id);
                string statSubjects = "";
                foreach (string subject in statSubjectsList)
                {
                    if (statSubjects != "")
                        statSubjects += ", " + subject;
                    else statSubjects = subject;
                }
                appeal.StatementSubjects = statSubjects;
                appealCitsContainer.Update(appeal);
            }
            finally
            {

            }

            return this.Success();

        }
    }
}
