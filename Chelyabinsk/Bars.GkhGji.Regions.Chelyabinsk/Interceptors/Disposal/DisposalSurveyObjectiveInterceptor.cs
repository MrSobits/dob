﻿namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Entities;
    using Enums;
    using System;
    using System.Linq;

    class DisposalSurveyObjectiveInterceptor : EmptyDomainInterceptor<DisposalSurveyObjective>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<SurveyObjective> SurveyObjectiveDomain { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<DisposalSurveyObjective> service, DisposalSurveyObjective entity)
        {

            try//пробуем проставить вид контроля в распоряжение по задаче
            {
                if (entity.Disposal != null)
                {
                    var dispRepo = this.Container.Resolve<IRepository<Disposal>>();
                    Disposal disposal = dispRepo.Get(entity.Disposal.Id);
                    if (entity.SurveyObjective != null)
                    {
                        var surveyObj = SurveyObjectiveDomain.Get(entity.SurveyObjective.Id);
                        if (surveyObj.Code == "10")
                        {
                            disposal.KindKNDGJI = GkhGji.Enums.KindKNDGJI.LicenseControl;
                            dispRepo.Update(disposal);
                        }
                        if (surveyObj.Code == "20")
                        {
                            disposal.KindKNDGJI = GkhGji.Enums.KindKNDGJI.HousingSupervision;
                            dispRepo.Update(disposal);
                        }
                    }

                }


            }
            catch (Exception e)
            {


            }
            return base.AfterCreateAction(service, entity);
        }       

    }
}
