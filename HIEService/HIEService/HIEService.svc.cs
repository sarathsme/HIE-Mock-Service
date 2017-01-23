using System.IO;
using System.ServiceModel.Web;
using System.Xml;
using HIEService.RequestHandlers;
using HIEService.XmlResponseGenerator;

namespace HIEService
{
    public class HIEService : IHIEService
    {
        public XmlElement PatientDemoQuery(XmlElement request)
        {
            PDQRequest searchRequest = new PDQRequest(request);
            return searchRequest.ProccessRequestAndGetResponse();
        }

        public XmlElement iPHRDocumentRegistry(XmlElement request)
        {
            StoredQueryRequest storedQueryRequest = new StoredQueryRequest(request);
            return storedQueryRequest.ProcessRequestAndGetResponse();
        }

        public Stream iPHRDocumentRepository(XmlElement request)
        {
            RDRequest documentRequest = new RDRequest(request);
            WebOperationContext.Current.OutgoingResponse.ContentType = "multipart/related; boundary=MIMEBoundaryurn_uuid_48A421395349697E1E1463990281525; type=\"application/xop+xml\"; start=\"<0.urn:uuid:48A421395349697E1E1463990281526@apache.org>\"; start-info=\"application/soap+xml\"; action=\"urn:ihe:iti:2007:RetrieveDocumentSetResponse\"";
            return documentRequest.ProcessRequestAndGetResponse();          
        }

        public XmlElement PIXManager(XmlElement request)
        {
            PIXRequest pixRequest = new PIXRequest(request);
            return PIXResponseGenerator.GetResponse();
        }

    }
}
