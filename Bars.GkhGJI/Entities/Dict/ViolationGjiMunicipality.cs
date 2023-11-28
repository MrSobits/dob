namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    ///    Связь между муницапальным образованием и нарушением для территориального признака нарушения
    /// </summary>
    public class ViolationGjiMunicipality : BaseGkhEntity
    {

        public virtual ViolationGji ViolationGji { get; set; }


        public virtual Municipality Municipality { get; set; } 
    }
}