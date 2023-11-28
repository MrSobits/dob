﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.Navigation
{
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.GkhGji.Contracts;

    public class DocumentsGjiRegisterMenuProvider : INavigationProvider
    {
        public string Key => "DocumentsGjiRegister";

        public string Description => "Меню для документов ГЖИ";

        public void Init(MenuItem root)
        {

            var disposalsText = ApplicationContext.Current.Container.Resolve<IDisposalText>().SubjectiveManyCase;

            root.Add("Материалы правонарушений", "B4.controller.documentsgjiregister.Disposal").AddRequiredPermission("GkhGji.DocumentsGji.Disposal.View");
            //root.Add("Акты проверок", "B4.controller.documentsgjiregister.ActCheck").AddRequiredPermission("GkhGji.DocumentsGji.ActCheck.View");
            //root.Add("Акты обследований", "B4.controller.documentsgjiregister.ActSurvey").AddRequiredPermission("GkhGji.DocumentsGji.ActSurvey.View");
            //root.Add("Акты проверки предписаний", "B4.controller.documentsgjiregister.ActRemoval").AddRequiredPermission("GkhGji.DocumentsGji.ActRemoval.View");
            //root.Add("Предписания", "B4.controller.documentsgjiregister.Prescription").AddRequiredPermission("GkhGji.DocumentsGji.Prescription.View");
            root.Add("Протоколы", "B4.controller.documentsgjiregister.ProtocolGji").AddRequiredPermission("GkhGji.DocumentsGji.Protocol.View");
            root.Add("Протоколы по ст.19.7 КоАП РФ", "B4.controller.documentsgjiregister.Protocol197").AddRequiredPermission("GkhGji.DocumentsGji.Protocol197.View");
            root.Add("Постановления", "B4.controller.documentsgjiregister.Resolution").AddRequiredPermission("GkhGji.DocumentsGji.Resolution.View");
            //root.Add("Представления", "B4.controller.documentsgjiregister.Presentation").AddRequiredPermission("GkhGji.DocumentsGji.Presentation.View");
            //root.Add($"{disposalsText} вне инспекторской деятельности", "B4.controller.documentsgjiregister.DisposalNullInspection").AddRequiredPermission("GkhGji.DocumentsGji.DisposalNullInspection.View");
        }
    }
}