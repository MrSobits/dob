namespace Bars.Gkh1468.DomainService.Impl
{
    using System.IO;
    using B4;
    using Castle.Windsor;
    using Gkh1468.Domain.ProviderPassport.Serialize;
    using Gkh1468.Entities;
    using Gkh1468.Report;

    using Stimulsoft.Report;

    public class OkiPassportProvSignature : ISignature<OkiProviderPassport>
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<OkiProviderPassport> DomainOkiProviderPassport { get; set; }

        public MemoryStream GetXmlStream(long id)
        {
            var prov = DomainOkiProviderPassport.Get(id);

            return new ProviderPassportSerializeProvider(Container).GetStreamXml(prov) as MemoryStream;
        }

        public MemoryStream GetPdfStream(long id)
        {
            var prov = DomainOkiProviderPassport.Get(id);

            var p = new OkiPassportReport { Container = Container, OkiProviderPassport = prov, ExportFormat = StiExportFormat.Pdf };

            return p.GetGeneratedReport();
        }
    }
}