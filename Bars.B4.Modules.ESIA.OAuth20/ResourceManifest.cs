namespace Bars.B4.Modules.ESIA.OAuth20
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

        protected override void BaseInit(IResourceManifestContainer container)
        {  
            AddResource(container, "esia/logout.ashx");
            AddResource(container, "esia/oauthlogin.ashx");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
        {

            container.Add(path, string.Format("Bars.B4.Modules.ESIA.OAuth20.dll/Bars.B4.Modules.ESIA.OAuth20.{0}", path.Replace("/", ".")));
        }
    }
}
