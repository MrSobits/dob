namespace Bars.B4.Modules.FIAS.AutoUpdater
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader;
    using Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader.Impl;
    using Bars.B4.Modules.FIAS.AutoUpdater.Converter;
    using Bars.B4.Modules.FIAS.AutoUpdater.Converter.Impl;
    using Bars.B4.Modules.FIAS.AutoUpdater.DownloadService;
    using Bars.B4.Modules.FIAS.AutoUpdater.DownloadService.Impl;
    using Bars.B4.Modules.FIAS.AutoUpdater.Impl;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterTransient<IFiasAutoUpdater, FiasAutoUpdater>();
            this.Container.RegisterTransient<IFiasDownloadService, FiasDownloadService>();
            this.Container.RegisterTransient<IFiasArchiveReader, FiasArchiveReader>();
            this.Container.RegisterTransient<IFiasDbConverter, FiasDbConverter>();
        }
    }
}