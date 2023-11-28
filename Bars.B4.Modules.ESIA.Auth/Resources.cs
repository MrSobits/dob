using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bars.B4.Modules.ESIA.Auth
{
    public class Resources : IResourceManifest
    {
        public void InitManifests(IResourceManifestContainer container)
        {
            AddResource(container, "Views/Login/ESIA.cshtml");
        }


        private void AddResource(IResourceManifestContainer container, string path)
        {
            container.Add(path, string.Format("Bars.B4.Modules.ESIA.Auth.dll/Bars.B4.Modules.ESIA.Auth.{0}", path.Replace("/", ".")));
        }

    }
}
