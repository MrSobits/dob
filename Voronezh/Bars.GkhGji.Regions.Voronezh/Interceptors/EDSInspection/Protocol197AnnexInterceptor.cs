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

    public class Protocol197AnnexInterceptor : EmptyDomainInterceptor<Protocol197Annex>
    {
        public IDomainService<EDSInspection> EDSInspectionDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<Protocol197Annex> service, Protocol197Annex entity)
        {
            if (entity.SignedFile != null && entity.TypeAnnex != TypeAnnex.NotSet)
            {
                if (entity.Protocol197.Contragent != null)
                {
                    if (entity.Protocol197.Contragent.IsEDSE)
                    {
                        entity.MessageCheck = MessageCheck.Sent;
                    }
                }
            }
            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<Protocol197Annex> service, Protocol197Annex entity)
        {
            try
            // снимаем задачу
            {
                if (entity.SignedFile != null && entity.TypeAnnex != TypeAnnex.NotSet)
                {
                    if (entity.Protocol197.Contragent != null)
                    {
                        if (entity.Protocol197.Contragent.IsEDSE)
                        {
                            var edsinsp = EDSInspectionDomain.GetAll()
                       .FirstOrDefault(x => x.InspectionGji == entity.Protocol197.Inspection);
                            if (edsinsp == null)
                            {
                                EDSInspection newEDS = new EDSInspection
                                {
                                    Contragent = entity.Protocol197.Contragent,
                                    InspectionGji = entity.Protocol197.Inspection,
                                    InspectionDate = entity.Protocol197.Inspection.ObjectCreateDate,
                                    InspectionNumber = entity.Protocol197.Inspection.InspectionNumber,
                                    NotOpened = true,
                                    ObjectCreateDate = DateTime.Now,
                                    ObjectEditDate = DateTime.Now,
                                    ObjectVersion = 1,
                                    TypeBase = entity.Protocol197.Inspection.TypeBase
                                };
                                EDSInspectionDomain.Save(newEDS);
                            }
                            else
                            {
                                edsinsp.NotOpened = true;
                                EDSInspectionDomain.Update(edsinsp);
                            }

                            string email = entity.Protocol197.Contragent.Email;
                            if (!string.IsNullOrEmpty(email))
                            {
                                try
                                {
                                    EmailSender emailSender = EmailSender.Instance;
                                    emailSender.Send(email, "Уведомление о размещении документа ГЖИ", MakeMessageBody(entity.TypeAnnex), MakeAttachment(entity.SignedFile));
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
                return Failure("При создании документа произошла ошибка");
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

        string MakeMessageBody(TypeAnnex type)
        {
            string state = "";
            switch (type)
            {
                case TypeAnnex.ActCheck:
                    state = "Акт проверки";
                    break;

                case TypeAnnex.Disposal:
                    state = "Материалы правонарушения";
                    break;

                case TypeAnnex.DisposalNotice:
                    state = "Уведомление";
                    break;

                case TypeAnnex.PrescriptionNotice:
                    state = "Уведомление-вызов";
                    break;

                case TypeAnnex.Protocol:
                    state = "Протокол";
                    break;

                case TypeAnnex.Resolution:
                    state = "Постановление";
                    break;
                case TypeAnnex.ProtocolNotification:
                    state = "Уведомление о составлении протокола";
                    break;
                case TypeAnnex.ActDefinition:
                    state = "Определение";
                    break;

            }

            string body = $"Уважаемый(ая) пользователь!\r\n";
            body += $"Государственная Административная комиссия Воронежской области уведомляет Вас о том, что в реестре электронного документооборота размещен новый документ: {state}, файл документа прикреплен к настоящему электронному сообщению.\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }

    }
}
