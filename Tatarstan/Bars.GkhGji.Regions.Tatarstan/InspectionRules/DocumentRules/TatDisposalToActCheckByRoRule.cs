namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using Bars.GkhGji.InspectionRules;

    /// <summary>
    /// Поскольку в РТ данный акт должен называтся просто АктПроверки, то переопределяем свойство и заменяем реализацию
    /// </summary>
    public class TatDisposalToActCheckByRoRule : DisposalToActCheckByRoRule
    {
 
        public override string ResultName
        {
            get { return "Акт проверки"; }
        }
    }
}
