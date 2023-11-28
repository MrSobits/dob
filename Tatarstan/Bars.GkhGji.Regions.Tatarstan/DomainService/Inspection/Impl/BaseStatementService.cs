namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Gkh.Entities;
    using System.Text;
    using GkhGji.Entities;

    public class BaseStatementService : Bars.GkhGji.DomainService.BaseStatementService
    {
        private bool CheckAppealCits(AppealCits appealCits, out string errStr)
        {
            errStr = "";
            var msg = new StringBuilder();

            if (!CheckThematics(appealCits.Id)) msg.AppendLine("не заполнены тематики;");

            if (!CheckAppealCitsRealityObjects(appealCits.Id)) msg.AppendLine("не заполнено место возникновения проблемы;");

            if (!CheckAppealCitsConsideration(appealCits)) msg.AppendLine("не заполнено одно или несколько полей вкладки \'Рассмотрение\'");

            if (msg.Length != 0)
            {
                msg.Insert(0, string.Format("Ошибки заполнения обращения {0}: ", appealCits.DocumentNumber)); 
                errStr = msg.ToString();
            }

            return errStr.IsEmpty();
        }

        private bool CheckAppealCitsConsideration(AppealCits appealCits)
        {
            if (appealCits.ExecuteDate == null ||
                appealCits.SuretyDate == null ||
                appealCits.Surety == null ||
                appealCits.Executant == null ||
                appealCits.SuretyResolve == null)
            {
                return false;
            }

            return true;
        }

        private bool CheckThematics(long id)
        {
            return AppealStatSubjectDomain.GetAll().Any(x => x.AppealCits.Id == id);
        }

        private bool CheckAppealCitsRealityObjects(long id)
        {
            var servAppealCitRealObj = Container.Resolve<IDomainService<AppealCitsRealityObject>>();

            return servAppealCitRealObj.GetAll().Any(x => x.AppealCits.Id == id);
        }

        public override IDataResult CheckAppealCits(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("appealCitizensId");
            string msg;
            var res = CheckAppealCits(Container.Resolve<IDomainService<AppealCits>>().Load(id), out msg);

            return new BaseDataResult(res, !res ? msg : string.Empty);
        }

        public override IDataResult AddAppealCitizens(BaseParams baseParams)
        {
            var inspectionId = baseParams.Params.GetAs<long>("inspectionId");
            var objectIds = baseParams.Params.GetAs("objectIds", string.Empty);

            var servBaseStatementAppealCitizens = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var servAppealCitRealObj = Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var servInspectionRealityObj = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var servBaseStatement = Container.Resolve<IDomainService<BaseStatement>>();
            var servAppealCits = Container.Resolve<IDomainService<AppealCits>>();
            var serInspectionGjis = Container.Resolve<IDomainService<InspectionGji>>();

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var listObjects = ArrayToDeleteBaseStatementAppealCits(inspectionId);

                    foreach (var id in listObjects)
                    {
                        servBaseStatementAppealCitizens.Delete(id);
                    }

                    if (!string.IsNullOrEmpty(objectIds))
                    {
                        var appealsIds = objectIds.Split(',').Select(x => x.ToLong()).ToList();
                        var appealsRemainIds = appealsIds;

                        foreach (var newId in objectIds.Split(',').Select(x => x.ToLong()).ToList())
                        {
                            var appealCits = servAppealCits.Load(newId);

                            string errStr;
                            if (!CheckAppealCits(appealCits, out errStr))
                            {
                                throw (new ValidationException(errStr));
                            }

                            servBaseStatementAppealCitizens.Save(new InspectionAppealCits
                            {
                                Inspection = servBaseStatement.Load(inspectionId),
                                AppealCits = appealCits
                            });
                        }

                        // Проверяемые дома обращения которых нет в проверки
                        var realObjs = servAppealCitRealObj.GetAll()
                            .Where(y => appealsRemainIds.Contains(y.AppealCits.Id) &&
                                !servInspectionRealityObj.GetAll().Any(x => x.Inspection.Id == inspectionId && x.RealityObject.Id == y.RealityObject.Id))
                            .ToArray();

                        // Добавляем  дома в проверку
                        foreach (var realObj in realObjs.Distinct(x => x.RealityObject.Id))
                        {
                            servInspectionRealityObj.Save(new InspectionGjiRealityObject
                                             {
                                                 Inspection = serInspectionGjis.Load(inspectionId),
                                                 RealityObject = realObj.RealityObject
                                             });
                        }
                    }

                    transaction.Commit();
                    return new BaseDataResult { Success = true };
                }
                catch (ValidationException exc)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = exc.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(servBaseStatementAppealCitizens);
                    Container.Release(servAppealCitRealObj);
                    Container.Release(servInspectionRealityObj);
                    Container.Release(servBaseStatement);
                    Container.Release(servAppealCits);
                    Container.Release(serInspectionGjis);
                }
            }
        }

        public override IDataResult CreateWithAppealCits(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var service = Container.Resolve<IDomainService<BaseStatement>>();
                var serviceAppeal = Container.Resolve<IDomainService<AppealCits>>();
                var serviceBaseStatementAppeal = Container.Resolve<IDomainService<InspectionAppealCits>>();
                var serviceInspectionRobject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

                var serviceContragent = Container.Resolve<IDomainService<Contragent>>();
                var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();

                try
                {
                    var contragentId = baseParams.Params.GetAs<long>("contragentId");
                    var baseStatement = baseParams.Params.GetAs<BaseStatement>("baseStatement");
                    var appealCitsIds = baseParams.Params.GetAs<long[]>("appealCits") ?? new long[0];
                    var realityObjId = baseParams.Params.GetAs<long>("realtyObjId");

                    if (contragentId > 0)
                    {
                        baseStatement.Contragent = serviceContragent.Load(contragentId);
                    }

                    service.Save(baseStatement);

                    //add citizens appeals
                    foreach (var appealCitId in appealCitsIds)
                    {
                        var appealCits = serviceAppeal.Load(appealCitId);
                        
                        string errStr;
                        if (!CheckAppealCits(appealCits, out errStr))
                        {
                            throw (new ValidationException(errStr));
                        }

                        var newRec = new InspectionAppealCits
                        {
                            AppealCits = appealCits,
                            Inspection = baseStatement
                        };

                        serviceBaseStatementAppeal.Save(newRec);

                        if (realityObjId > 0)
                        {
                            var newInspRo = new InspectionGjiRealityObject
                            {
                                Inspection = baseStatement,
                                RealityObject = serviceRobject.Load(realityObjId)
                            };

                            serviceInspectionRobject.Save(newInspRo);
                        }
                    }

                    transaction.Commit();

                    return new BaseDataResult{ Success = true, Data = baseStatement };
                }
                catch (ValidationException exc)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = exc.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(service);
                    Container.Release(serviceAppeal);
                    Container.Release(serviceRobject);
                    Container.Release(serviceContragent);
                    Container.Release(serviceInspectionRobject);
                    Container.Release(serviceBaseStatementAppeal);
                }
            }
        }
    }
}