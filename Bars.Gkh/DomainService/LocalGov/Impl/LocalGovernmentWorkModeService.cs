﻿namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public class LocalGovernmentWorkModeService : ILocalGovernmentWorkModeService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddWorkMode(BaseParams baseParams)
        {
            try
            {
                var manorgId = baseParams.Params["manorgId"].ToInt();
                var records = baseParams.Params["records"].As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>().ReadClass<LocalGovernmentWorkModeProxy>())
                    .ToList();

                var service = Container.Resolve<IDomainService<LocalGovernmentWorkMode>>();

                var existingWorkModes = service.GetAll()
                        .Where(x => x.LocalGovernment.Id == manorgId)
                        .ToList();

                foreach (var rec in records)
                {
                    var existingWorkMode = existingWorkModes.FirstOrDefault(x => x.TypeMode == rec.TypeMode && x.TypeDayOfWeek == rec.TypeDayOfWeek);

                    if (existingWorkMode != null)
                    {
                        existingWorkMode.StartDate = rec.StartDate;
                        existingWorkMode.EndDate = rec.EndDate;
                        existingWorkMode.Pause = rec.Pause;
                        existingWorkMode.AroundClock = rec.AroundClock;

                        service.Update(existingWorkMode);
                    }
                    else
                    {
                        var newManagingOrgWorkMode = new LocalGovernmentWorkMode()
                        {
                            LocalGovernment = new LocalGovernment { Id = manorgId },
                            TypeDayOfWeek = rec.TypeDayOfWeek,
                            TypeMode = rec.TypeMode,
                            StartDate = rec.StartDate,
                            EndDate = rec.EndDate,
                            Pause = rec.Pause,
                            AroundClock = rec.AroundClock
                        };

                        service.Save(newManagingOrgWorkMode);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = true, Message = exc.Message };
            }
        }

        protected class LocalGovernmentWorkModeProxy
        {
            public int Id { get; set; }

            public virtual LocalGovernment LocalGovernment { get; set; }

            public virtual TypeMode TypeMode { get; set; }

            public virtual TypeDayOfWeek TypeDayOfWeek { get; set; }

            public virtual DateTime? StartDate { get; set; }

            public virtual DateTime? EndDate { get; set; }

            public virtual string Pause { get; set; }

            public virtual bool AroundClock { get; set; }
        }
    }
}