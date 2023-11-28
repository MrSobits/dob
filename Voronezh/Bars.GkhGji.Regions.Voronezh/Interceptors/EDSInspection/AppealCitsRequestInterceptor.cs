namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using System.Linq;
    using System.Net.Mail;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class AppealCitsRequestInterceptor : EmptyDomainInterceptor<AppealCitsRequest>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<Contragent> ContragentDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsRequest> service, AppealCitsRequest entity)
        {

            Operator thisOperator = UserManager.GetActiveOperator();
               
            if (thisOperator?.Inspector == null)
            {
                return Failure("Изменение данных запроса доступно только сотрудникам ГЖИ");
            }
            else
            {
                entity.SenderInspector = thisOperator.Inspector;
            }

            return this.Success();

        }

        public override IDataResult AfterUpdateAction(IDomainService<AppealCitsRequest> service, AppealCitsRequest entity)
        {
            try
            // снимаем задачу
            {
                if (entity.SignedFile != null && entity.Signature != null)
                {
                    if (entity.CompetentOrg.Code != null)
                    {

                        var contragent = ContragentDomain.GetAll()
                 .Where(x => x.Inn == entity.CompetentOrg.Code).FirstOrDefault();
                        if (contragent != null && contragent.IsEDSE)
                        {



                            string email = contragent.Email;
                            if (!string.IsNullOrEmpty(email))
                            {
                                try
                                {
                                    EmailSender emailSender = EmailSender.Instance;
                                    emailSender.Send(email, "Уведомление о размещении запроса ГЖИ", MakeMessageBody(), MakeAttachment(entity.SignedFile));
                                }
                                catch
                                { }
                            }
                        }
                    }

                }
            }
            catch
            {
              

            }

            return this.Success();
        }

        Attachment MakeAttachment(FileInfo fileInfo)
        {
            if (fileInfo == null)
                return null;
            var fm = Container.Resolve<IFileManager>();

            return new Attachment(fm.GetFile(fileInfo), fileInfo.FullName);
        }

        string MakeMessageBody()
        {           

            string body = $"Уважаемый(ая) пользователь!\r\n";
            body += $"Государственная Административная комиссия Воронежской области уведомляет Вас о том, что в реестре электронного документооборота размещен новый запрос, файл запроса прикреплен к настоящему электронному сообщению.\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }

    }
}
