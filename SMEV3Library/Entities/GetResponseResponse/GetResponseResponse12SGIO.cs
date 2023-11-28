using SMEV3Library.Namespaces;
using System.Xml.Linq;

namespace SMEV3Library.Entities.GetResponseResponse
{
    public class GetResponseResponse12SGIO : GetResponseResponse
    {
        public GetResponseResponse12SGIO(HTTPResponse response) : base(response)
        {
            var GetResponseResponseElement = response.SoapXML.Element(SMEVNamespaces12.SGIOTypesNamespace + "GetResponseResponse");
            if (GetResponseResponseElement == null)
                return;

            var ResponseElement = GetResponseResponseElement.Element(SMEVNamespaces12.SGIOTypesNamespace + "ResponseMessage")?.Element(SMEVNamespaces12.SGIOTypesNamespace + "Response");
            if (ResponseElement == null)
                return;

            isAnswerPresent = true;

            //-----Response-----            
            //---OriginalMessageId---
            OriginalMessageId = ResponseElement.Element(SMEVNamespaces12.SGIOTypesNamespace + "OriginalMessageId")?.Value;
            //---SenderProvidedResponseData---
            XElement SenderProvidedResponseDataElement = ResponseElement.Element(SMEVNamespaces12.SGIOTypesNamespace + "SenderProvidedResponseData");
            if (SenderProvidedResponseDataElement != null)
            {
                //
                MessageId = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.SGIOTypesNamespace + "MessageID")?.Value;
                To = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.SGIOTypesNamespace + "To")?.Value;
                ReplyTo = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.SGIOTypesNamespace + "ReplyTo")?.Value;
                RequestRejected = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.SGIOTypesNamespace + "RequestRejected");
                MessagePrimaryContent = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.SGIOBasicNamespace + "MessagePrimaryContent");
                AsyncProcessingStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.SGIOTypesNamespace + "AsyncProcessingStatus");
                RequestStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.SGIOTypesNamespace + "RequestStatus");
                //--AttachmentHeaderList---
                var AttachmentHeaderListElemnet = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.SGIOBasicNamespace + "AttachmentHeaderList");
                try
                {
                    SetSignatureFileNameSGIO(AttachmentHeaderListElemnet);
                }
                catch
                {
                    SetSignatureFileName(null);
                }
            }

            //-----AttachmentContentList-----
            var AttachmentContentListElement = GetResponseResponseElement.Element(SMEVNamespaces12.SGIOTypesNamespace + "ResponseMessage").Element(SMEVNamespaces12.SGIOBasicNamespace + "AttachmentContentList");
            SetFileNameSGIO(AttachmentContentListElement);
            response.Attachments = Attachments;

            //-----SMEVSignature-----
            //TODO: написать проверку в сервисе
        }
    }
}
