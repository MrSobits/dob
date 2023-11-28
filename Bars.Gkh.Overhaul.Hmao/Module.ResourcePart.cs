﻿namespace Bars.Gkh.Overhaul.Hmao
{
    using Bars.B4.ResourceBundling;

    public partial class Module
    {
        /// <summary>
        /// Метод регистрации бандлов
        /// </summary>
        private void RegisterBundlers()
        {
            var bundler = Container.Resolve<IResourceBundler>();
            bundler.RegisterCssBundle("b4-all", new[]
                {
                    "~/content/css/b4GkhHmao.css"
                });
        }                                                    
    }                                                        
}                                                            
