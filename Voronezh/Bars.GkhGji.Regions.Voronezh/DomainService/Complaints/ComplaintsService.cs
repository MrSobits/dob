using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Castle.Windsor;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Helpers;
using SMEV3Library.Namespaces;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class ComplaintsService : IComplaintsService
    { 

        #region Properties              

        public IDomainService<SMEVComplaintsRequestFile> SMEVComplaintsRequestFileDomain { get; set; }

        public IDomainService<SMEVComplaintsRequest> SMEVComplaintsRequestDomain { get; set; }
        public IDomainService<SMEVComplaintsExecutant> SMEVComplaintsExecutantDomain { get; set; }
        public IDomainService<SMEVComplaints> SMEVComplaintsDomain { get; set; }
        public IRepository<Contragent> ContragentDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;

        #endregion

        #region Constructors

        public ComplaintsService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods
               
        /// <summary>
        /// Запрос информации о платежах
        /// </summary>
        public bool SendRequest(SMEVComplaintsRequest requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVComplaintsRequestFileDomain.GetAll().Where(x => x.SMEVComplaintsRequest == requestData).ToList().ForEach(x => SMEVComplaintsRequestFileDomain.Delete(x.Id));

                //PayRegDomain.GetAll().Where(x => x.PayReg == requestData).ToList().ForEach(x => PayRegDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = XElement.Parse(requestData.TextReq);
                
                ChangeState(requestData, BaseChelyabinsk.Enums.RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                //requestData.BillFor = "Запрос оплат";
                requestData.MessageId = requestResult.MessageId;
                SMEVComplaintsRequestDomain.Update(requestData);

                //
                indicator?.Report(null, 80, "Сохранение данных для отладки");
                SaveFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                SaveFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

                indicator?.Report(null, 90, "Обработка результата");
                if (requestResult.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                    SaveException(requestData, requestResult.Error.Exception);
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveFile(requestData, requestResult.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в файле Fault.xml");
                }
                else if (requestResult.Status != "requestIsQueued")
                {
                    SetErrorState(requestData, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
                }
                else
                {
                    //изменяем статус
                    //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани
                    requestData.RequestState = BaseChelyabinsk.Enums.RequestState.Queued;
                    requestData.Answer = "Поставлено в очередь";
                    SMEVComplaintsRequestDomain.Update(requestData);
                    return true;
                }
            }
            catch (HttpRequestException)
            {
                //ошибки связи прокидываем в контроллер
                throw;
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendPaymentRequest exception: " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData">Запрос</param>
        /// <param name="response">Ответ</param>
        /// <param name="indicator">Индикатор прогресса для таски</param>
        public bool TryProcessResponse(SMEVComplaintsRequest requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //сохранение данных
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponceRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponceResponse.dat");
                response.Attachments.ForEach(x => SaveFile(requestData, x.FileData, x.FileName));

                indicator?.Report(null, 70, "Обработка результата");

                //ошибки наши
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво - чтобы можнор было считать повторно ,если это наш косяк
                _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                //Ошибки присланные
                if (response.FaultXML != null)
                {
                    SaveFile(requestData, response.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения с оплатами из ГИС ГМП, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения с оплатами из ГИС ГМП, подробности в приаттаченом файле AsyncProcessingStatus.xml");
                }
                //сервер отклонил запрос?
                //else if (response.RequestRejected != null)
                //{
                //    requestData.RequestState = BaseChelyabinsk.Enums.RequestState.ResponseReceived;
                //    requestData.Answer = response.RequestRejected.Element(SMEVNamespaces12.TypesNamespace + "RejectionReasonDescription")?.Value.Cut(500);
                //    SMEVComplaintsRequestDomain.Update(requestData);
                //    return true;
                //}
                else
                {
                    //перехватываем и получаем нужный контент для тестирования
                    response.MessagePrimaryContent = GetMPC();
                    //ответ пустой?
                    if (response.MessagePrimaryContent == null)
                    {
                        SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
                        return false;
                    }

                }
                //Парсим ответ если это запрос жалоб
                if (requestData.TypeComplainsRequest == BaseChelyabinsk.Enums.TypeComplainsRequest.ComplaintsRequest)
                {
                    KndResponse newResponce = Deserialize<KndResponse>(XElement.Parse(response.MessagePrimaryContent.FirstNode.ToString()));
                    if (newResponce != null)
                    {
                        if (newResponce.Item is getComplaintsResultType)
                        {
                            getComplaintsResultType resType = newResponce.Item as getComplaintsResultType;
                            foreach (var complaint in resType.complaint.ToList())
                            {
                                SMEVComplaints newComplaint = new SMEVComplaints
                                {
                                    ComplaintId = complaint.id,
                                    CommentInfo = $"Наименование жалобы: {complaint.name}. Пояснительный комментарий: {complaint.commentInfo}",
                                    DocNumber = complaint.number,
                                    ComplaintDate = complaint.epguData != null? complaint.epguData.complaintDate.ToString("dd.MM.yyyy"):DateTime.Now.ToString("dd.MM.yyyy"),
                                    TypeAppealDecision = GetStringFromcadeNameTypeArray(complaint.typeAppealDecision),
                                    LifeEvent = GetStringFromcadeNameTypeArray(complaint.lifeEvent),
                                    AppealNumber = complaint.epguData != null ? complaint.epguData.appealNumber:"",
                                    Okato = complaint.epguData != null ? complaint.epguData.okato : "",
                                    OrderId = complaint.epguData != null ? complaint.epguData.orderId : "",
                                    RequesterRole = GetReqesterRole(complaint.applicantData != null? complaint.applicantData.requesterRole:requesterRoleType.PERSON),
                                    EsiaOid = complaint.applicantData != null ? complaint.applicantData.esiaOid:""
                                };
                                if (complaint.applicantData != null)
                                {
                                    GetApplicantInfo(complaint.applicantData.Item, ref newComplaint);
                                }
                                SMEVComplaintsDomain.Save(newComplaint);
                            }
                          

                        }
                    }
                    requestData.RequestState = BaseChelyabinsk.Enums.RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVComplaintsRequestDomain.Update(requestData);

                }
                else
                {
                    requestData.RequestState = BaseChelyabinsk.Enums.RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVComplaintsRequestDomain.Update(requestData);
                }

                return false;
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "TryProcessResponse exception: " + e.Message);
                return false;
            }
        }
        #endregion

        #region Private methods

        private XNamespace Schema = "http://schemas.xmlsoap.org/soap/envelope/";

        private void GetApplicantInfo(object item, ref SMEVComplaints newComplaint)
        {
            if (item != null)
            {
                if (item is applicantBusinessmanType)
                {
                    applicantBusinessmanType buisnessman = item as applicantBusinessmanType;
                    newComplaint.BirthAddress = buisnessman.applicantPerson.birthAddress;
                    newComplaint.RequesterFIO = $"{buisnessman.applicantPerson.lastName} {buisnessman.applicantPerson.firstName} {buisnessman.applicantPerson.middleName}";
                    newComplaint.Gender = buisnessman.applicantPerson.gender == genderType.Male ? Gkh.Enums.Gender.Male : Gkh.Enums.Gender.Female;
                    if (buisnessman.applicantPerson.identityDocument.Item is passportRfType)
                    {
                        var passportRF = buisnessman.applicantPerson.identityDocument.Item as passportRfType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.passportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    else if (buisnessman.applicantPerson.identityDocument.Item is internationalPassportRfType)
                    {
                        var passportRF = buisnessman.applicantPerson.identityDocument.Item as internationalPassportRfType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.internationalPassportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    else if (buisnessman.applicantPerson.identityDocument.Item is notRestrictedDocumentType)
                    {
                        var passportRF = buisnessman.applicantPerson.identityDocument.Item as notRestrictedDocumentType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.internationalPassportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    if (buisnessman.applicantPerson.registrationAddress != null)
                    {
                        newComplaint.RegAddess = $"{buisnessman.applicantPerson.registrationAddress.index} {buisnessman.applicantPerson.registrationAddress.fullAddress}";
                    }
                    newComplaint.INNFiz = buisnessman.applicantPerson.inn;
                    newComplaint.Nationality = buisnessman.applicantPerson.nationality;
                    newComplaint.BirthDate = buisnessman.applicantPerson.birthDate;
                    newComplaint.Email = buisnessman.applicantPerson.email;
                    newComplaint.MobilePhone = buisnessman.applicantPerson.mobilePhone;
                    newComplaint.SNILS = buisnessman.applicantPerson.snils;
                }
                else if (item is applicantLegalType)
                {                  
                    applicantLegalType buisnessman = item as applicantLegalType;
                    Contragent contragent = ContragentDomain.GetAll()
                      .Where(x => x.Ogrn == buisnessman.ogrn && x.Inn == buisnessman.inn).FirstOrDefault();
                    if (contragent == null)
                    {
                        contragent = new Contragent
                        {
                            Ogrn = buisnessman.ogrn,
                            Inn = buisnessman.inn,
                            Kpp = buisnessman.kpp,
                            Name = buisnessman.legalFullName,
                            ShortName = buisnessman.legalShortName,
                            Email = buisnessman.email,
                            ContragentState = Gkh.Enums.ContragentState.Active,
                            Description = "Создан из запроса по досудебному обжалованию",
                            ActivityGroundsTermination = Gkh.Enums.GroundsTermination.NotSet,
                            JuridicalAddress = buisnessman.legalAddress.fullAddress,
                            Phone = buisnessman.mobilePhone,
                            ObjectCreateDate = DateTime.Now,
                            ObjectEditDate = DateTime.Now,
                            ObjectVersion = 1
                        };
                        ContragentDomain.Save(contragent);
                    }
                    newComplaint.RequesterFIO = $"{buisnessman.lastName} {buisnessman.firstName} {buisnessman.middleName}";
                    newComplaint.Gender = Gkh.Enums.Gender.NotSet;
                    newComplaint.WorkingPosition = buisnessman.workingPosition;
                    newComplaint.Email = buisnessman.email;
                    newComplaint.MobilePhone = buisnessman.mobilePhone;
                }
                else if (item is applicantPersonType)
                {
                    applicantPersonType buisnessman = item as applicantPersonType;
                    newComplaint.BirthAddress = buisnessman.birthAddress;
                    newComplaint.RequesterFIO = $"{buisnessman.lastName} {buisnessman.firstName} {buisnessman.middleName}";
                    newComplaint.Gender = buisnessman.gender == genderType.Male ? Gkh.Enums.Gender.Male : Gkh.Enums.Gender.Female;
                    if (buisnessman.identityDocument.Item is passportRfType)
                    {
                        var passportRF = buisnessman.identityDocument.Item as passportRfType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.passportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    else if (buisnessman.identityDocument.Item is internationalPassportRfType)
                    {
                        var passportRF = buisnessman.identityDocument.Item as internationalPassportRfType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.internationalPassportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    else if (buisnessman.identityDocument.Item is notRestrictedDocumentType)
                    {
                        var passportRF = buisnessman.identityDocument.Item as notRestrictedDocumentType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.internationalPassportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    if (buisnessman.registrationAddress != null)
                    {
                        newComplaint.RegAddess = $"{buisnessman.registrationAddress.index} {buisnessman.registrationAddress.fullAddress}";
                    }
                    newComplaint.INNFiz = buisnessman.inn;
                    newComplaint.Nationality = buisnessman.nationality;
                    newComplaint.BirthDate = buisnessman.birthDate;
                    newComplaint.Email = buisnessman.email;
                    newComplaint.MobilePhone = buisnessman.mobilePhone;
                    newComplaint.SNILS = buisnessman.snils;
                }
            }
        }

        private RequesterRole GetReqesterRole(requesterRoleType role)
        {
            switch (role)
            {
                case requesterRoleType.PERSON: return RequesterRole.PERSON;
                case requesterRoleType.BUSINESSMAN: return RequesterRole.BUSINESSMAN;
                case requesterRoleType.EMPLOYEE: return RequesterRole.EMPLOYEE;
                default:
                    return RequesterRole.PERSON;

            }
        }

        private string GetStringFromcadeNameTypeArray(codeNameType[] typeArray)
        {
            string str = string.Empty;
            foreach (codeNameType type in typeArray.ToList())
            {
                str += $"{type.code} {type.Value}; ";
            }
            return str;
        }

        private XElement GetMPC()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\or\\newfilesRequest.xml");
            XElement mpc = XElement.Parse(doc.InnerXml);
            var soapXML = GetBody(mpc);
            var GetResponseResponseElement = soapXML.Element(SMEVNamespaces12.TypesNamespace + "GetResponseResponse");
            var ResponseElement = GetResponseResponseElement.Element(SMEVNamespaces12.TypesNamespace + "ResponseMessage")?.Element(SMEVNamespaces12.TypesNamespace + "Response");
            XElement SenderProvidedResponseDataElement = ResponseElement.Element(SMEVNamespaces12.TypesNamespace + "SenderProvidedResponseData");
            return SenderProvidedResponseDataElement.Element(SMEVNamespaces12.BasicNamespace + "MessagePrimaryContent");
        }

        private XElement GetBody(XElement SoapXml)
        {
            var body = SoapXml.Element(Schema + "Body");
            return body;
        }

        private T Deserialize<T>(XElement element)
        where T : class
        {
            XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(element.ToString()))
                return (T)ser.Deserialize(sr);
        }


        private DateTime? NullableDateParse(string value)
        {
            if (value == null)
                return null;

            DateTime result;

            return (DateTime.TryParse(value, out result) ? result : (DateTime?)null);
        }  

      
               
        private void SaveFile(SMEVComplaintsRequest request, byte[] data, string fileName)
        {
            if (data == null)
                return;

            //сохраняем отправленный пакет
            SMEVComplaintsRequestFileDomain.Save(new SMEVComplaintsRequestFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVComplaintsRequest = request,
                SMEVFileType = BaseChelyabinsk.Enums.SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVComplaintsRequest request, XElement data, string fileName)
        {
            if (data == null)
                return;

            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVComplaintsRequestFile faultRec = new SMEVComplaintsRequestFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVComplaintsRequest = request,
                SMEVFileType = BaseChelyabinsk.Enums.SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            };

            SMEVComplaintsRequestFileDomain.Save(faultRec);
        }

        private void SaveException(SMEVComplaintsRequest request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVComplaintsRequestFileDomain.Save(new SMEVComplaintsRequestFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVComplaintsRequest = request,
                SMEVFileType = BaseChelyabinsk.Enums.SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }

        private void ChangeState(SMEVComplaintsRequest requestData, BaseChelyabinsk.Enums.RequestState state)
        {
            requestData.RequestState = state;
            SMEVComplaintsRequestDomain.Update(requestData);
        }

        private void SetErrorState(SMEVComplaintsRequest requestData, string error)
        {
            requestData.RequestState = BaseChelyabinsk.Enums.RequestState.Error;
            requestData.Answer = error;
            SMEVComplaintsRequestDomain.Update(requestData);
        }

        #endregion

        #region Nested classes
        internal class Identifiers
        {
            internal string SenderIdentifier;
            internal string SenderRole;
            internal string OriginatorID;
        }

        #endregion

    }
}
