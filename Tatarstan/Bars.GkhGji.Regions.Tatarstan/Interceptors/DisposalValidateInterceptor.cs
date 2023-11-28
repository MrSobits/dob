namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    // интерцептор для распоряжения создаваемого от проверки обращения граждан: "Есть ли у всех обращений у проверки  "Место возникновения" " 
    // Отдельно т.к. в других регионах возможно было заменить данную проверку
    public class DisposalValidateInterceptor : EmptyDomainInterceptor<Disposal>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Disposal> service, Disposal entity)
        {
            if (entity.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                var serviceAppCitsRobject = Container.Resolve<IDomainService<AppealCitsRealityObject>>();
                var serviceAppCitsStatSubject = Container.Resolve<IDomainService<AppealCitsStatSubject>>();

                var appCits = Container.Resolve<IDomainService<InspectionAppealCits>>()
                                    .GetAll()
                                    .Where(x => x.Inspection.Id == entity.Inspection.Id)
                                    .Select(x => new
                                    {
                                        HasRo = serviceAppCitsRobject.GetAll().Any(y => y.AppealCits.Id == x.AppealCits.Id),
                                        HasStatSubject = serviceAppCitsStatSubject.GetAll().Where(y => y.Subject != null).Any(y => y.AppealCits.Id == x.AppealCits.Id),
                                        x.AppealCits.DocumentNumber,
                                        HasRequiredField = !string.IsNullOrEmpty(x.AppealCits.NumberGji) && x.AppealCits.ZonalInspection != null,
                                    })
                                   .ToList();
                if (appCits.Any(x => !x.HasRo || !x.HasStatSubject || !x.HasRequiredField))
                {
                    var failureMessage = new StringBuilder();
                    var tmpMsg = string.Empty;
                    failureMessage.AppendLine("Невозможно сформировать проверку, так как у обращений не заполнены поля:");

                    if (appCits.Any(x => !x.HasRequiredField))
                    {
                        failureMessage.Append("Обязательные поля: ");
                        failureMessage.AppendLine(appCits.Where(x => !x.HasRequiredField).Select(x => x.DocumentNumber).Aggregate(tmpMsg, (current, str) => current + string.Format(" {0}; ", str)));
                    }

                    if (appCits.Any(x => !x.HasRo))
                    {
                        failureMessage.Append("Адрес дома: ");
                        failureMessage.AppendLine(appCits.Where(x => !x.HasRo).Select(x => x.DocumentNumber).Aggregate(tmpMsg, (current, str) => current + string.Format(" {0}; ", str)));
                    }

                    if (appCits.Any(x => !x.HasStatSubject))
                    {
                        failureMessage.Append("Тематики: ");
                        failureMessage.AppendLine(appCits.Where(x => !x.HasStatSubject).Select(x => x.DocumentNumber).Aggregate(tmpMsg, (current, str) => current + string.Format(" {0}; ", str)));
                    }

                    return Failure(failureMessage.ToString());
                }
            }

            return this.Success();
        }
    }
}