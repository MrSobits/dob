namespace Bars.Gkh.Overhaul.Tat.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Castle.Windsor;

    public class ElementWearoutResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public decimal Wearout { get; set; }
        
        public override string DisplayName
        {
            get
            {
                return "Физический износ КЭ";
            }
        }

        public override string Code
        {
            get
            {
                return Id;
            }
        }

        public static string Id
        {
            get
            {
                return "ElementWear";
            }
        }

        public override object GetValue()
        {
            return Wearout;
        }
    }
}