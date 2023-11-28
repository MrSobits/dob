namespace Bars.Gkh.Domain.Impl
{
    using System.IO;

    using Bars.B4.Config;
    using Bars.B4.Modules.FileStorage;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class FileService : IFileService
    {
        public IWindsorContainer Container { get; set; }
        public IFileManager FileManager { get; set; }

        public FileInfo ReCreateFile(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return null;
            }

            var pathDir = new DirectoryInfo(Container.Resolve<IConfigProvider>().GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"].GetAs("FileDirectory", string.Empty));
            var path = Path.Combine(pathDir.FullName, string.Format("{0}.{1}", fileInfo.Id, fileInfo.Extention));
            if (File.Exists(path))
            {
                using (var fileInfoStream = new MemoryStream(File.ReadAllBytes(path)))
                {
                    var newFileInfo = FileManager.SaveFile(fileInfoStream, string.Format("{0}.{1}", fileInfo.Name, fileInfo.Extention));
                    return newFileInfo;
                }
            }

            return null;
        }
    }
}