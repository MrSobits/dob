namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class ProtocolNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_nso_protocol_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера протокола"; } }

        public override string TypeId { get { return "gji_document_prot"; } }

        public override string Description { get { return "Данное правило присваивает номера протокола"; } }
        
    }
}