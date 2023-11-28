namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using Entities;  
    using System;
    using System.Linq;
    using System.Web;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Castle.Windsor;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    public class AppCitOperationsService : IAppCitOperationsService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }

        public IDomainService<AppealCitsStatSubject> AppealCitsStatSubjectDomain { get; set; }

        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }

        public IDomainService<AppealCitsExecutant> AppealCitsExecutantDomain { get; set; }

        public IDataResult CopyAppeal(BaseParams baseParams)
        {
            var appealId = baseParams.Params.ContainsKey("docId") ? baseParams.Params["docId"].ToLong() : 0;
            if (appealId > 0)
            {
                try
                {
                    var userManager = Container.Resolve<IGkhUserManager>();
                    Operator currentOperator = userManager.GetActiveOperator();
                    Inspector inspector = null;
                    if (currentOperator.Inspector != null)
                        inspector = currentOperator.Inspector;
                    var oldAppeal = AppealCitsDomain.Get(appealId);
                    var newAppeal = new AppealCits
                    {
                       Accepting = oldAppeal.Accepting,
                       AppealRegistrator = inspector != null? inspector:oldAppeal.AppealRegistrator,
                       ApprovalContragent = oldAppeal.ApprovalContragent,
                       CheckTime = oldAppeal.CheckTime,
                       ContragentCorrespondent = oldAppeal.ContragentCorrespondent,
                       Correspondent = oldAppeal.Correspondent,
                       CorrespondentAddress = oldAppeal.CorrespondentAddress,
                       Comment = oldAppeal.Comment,
                       Description = oldAppeal.Description,
                       DateFrom = oldAppeal.DateFrom,
                       Email = oldAppeal.Email,
                       Executant = oldAppeal.Executant,
                       ExtensTime = oldAppeal.ExtensTime,
                       IncomingSources = oldAppeal.IncomingSources,
                       IncomingSourcesName = oldAppeal.IncomingSourcesName,
                       KindStatement = oldAppeal.KindStatement,
                       ManagingOrganization = oldAppeal.ManagingOrganization,
                       Municipality = oldAppeal.Municipality,
                       MunicipalityId = oldAppeal.MunicipalityId,
                       Phone = oldAppeal.Phone,
                       PlannedExecDate = oldAppeal.PlannedExecDate,
                       OrderContragent = oldAppeal.OrderContragent,
                       QuestionStatus = oldAppeal.QuestionStatus,
                       RealityAddresses = oldAppeal.RealityAddresses,
                       SpecialControl = oldAppeal.SpecialControl,
                       SSTUTransferOrg = oldAppeal.SSTUTransferOrg,
                       Surety = oldAppeal.Surety,
                       SuretyDate = oldAppeal.SuretyDate,
                       SuretyResolve = oldAppeal.SuretyResolve,
                       StatementSubjects = oldAppeal.StatementSubjects,
                       TypeCorrespondent = oldAppeal.TypeCorrespondent,
                       Year = oldAppeal.Year,
                       ZonalInspection = oldAppeal.ZonalInspection
                    };
                    AppealCitsDomain.Save(newAppeal);
                    var placeOfOrigin = AppealCitsRealityObjectDomain.GetAll()
                        .Where(x => x.AppealCits.Id == appealId).ToList();
                    foreach (var place in placeOfOrigin)
                    {
                        var newPlace = new AppealCitsRealityObject
                        {
                            AppealCits = newAppeal,
                            RealityObject = place.RealityObject
                        };
                        AppealCitsRealityObjectDomain.Save(newPlace);
                    }
                    var statSubjects = AppealCitsStatSubjectDomain.GetAll()
                       .Where(x => x.AppealCits.Id == appealId).ToList();
                    foreach (var stsub in statSubjects)
                    {
                        var newstsub = new AppealCitsStatSubject
                        {
                            AppealCits = newAppeal,
                            Feature = stsub.Feature,
                            Subject = stsub.Subject,
                            Subsubject = stsub.Subsubject
                        };
                        AppealCitsStatSubjectDomain.Save(newstsub);
                    }

                    var sources = AppealCitsSourceDomain.GetAll()
                  .Where(x => x.AppealCits.Id == appealId).ToList();
                    foreach (var source in sources)
                    {
                        var newsource = new AppealCitsSource
                        {
                            AppealCits = newAppeal,
                           RevenueDate = source.RevenueDate,
                           RevenueForm = source.RevenueForm,
                           RevenueSource = source.RevenueSource,
                           RevenueSourceNumber = source.RevenueSourceNumber
                        };
                        AppealCitsSourceDomain.Save(newsource);
                    }

                    //Проверяющих копировать не требуется
              //      var executants  = AppealCitsExecutantDomain.GetAll()
              //.Where(x => x.AppealCits.Id == appealId).ToList();
              //      foreach (var executant in executants)
              //      {
              //          var newexecutant = new AppealCitsExecutant
              //          {
              //              AppealCits = newAppeal,
              //            Controller = executant.Controller,
              //            Author = executant.Author,
              //            Description = executant.Description,
              //            Executant = executant.Executant,
              //            PerformanceDate = executant.PerformanceDate,
              //            Resolution = executant.Resolution,
              //            State = executant.State,
              //            ZonalInspection = executant.ZonalInspection
              //          };
              //          AppealCitsExecutantDomain.Save(newexecutant);
              //      }
                    return new BaseDataResult { Success = true, Data = newAppeal.Id };
                }
                catch
                {
                    return new BaseDataResult { Success = false, Message = "Не удалось создать копию обращения" };
                }
            }
            else
            {
                return new BaseDataResult { Success = false, Message = "Не найдено обращение для копирования" };
            }
        }



    }
}