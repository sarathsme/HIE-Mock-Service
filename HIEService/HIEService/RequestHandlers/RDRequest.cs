using System.Collections.Generic;
using System.IO;
using System.Xml;
using HIEService.DBHelper;
using HIEService.XmlResponseGenerator;

namespace HIEService.RequestHandlers
{
    public class RDRequest
    {
        List<DocumentInfo> DocumentRequestList
        {
            get;
            set;
        }

        public RDRequest(XmlElement request)
        {
            XmlNamespaceManager namespaceMgr = _GetNamespaceManager(request);
            DocumentRequestList = new List<DocumentInfo>();

            XmlNodeList docRequestNodes = request.SelectNodes("/soap:Body/urn:RetrieveDocumentSetRequest/urn:DocumentRequest", namespaceMgr);
            foreach (XmlNode docRequest in docRequestNodes)
            {
                DocumentRequestList.Add(new DocumentInfo(docRequest, namespaceMgr));
            }
        }

        public Stream ProcessRequestAndGetResponse()
        {
            List<HIEPatientDocument> patientDocuments = HIEPatientDocument.GetDocumentForUniqueID(DocumentRequestList);
            return RDResponseGenerator.GetResponseStream(patientDocuments, DocumentRequestList);
        }

        private static XmlNamespaceManager _GetNamespaceManager(XmlElement request)
        {
            XmlNamespaceManager namespaceMgr = new XmlNamespaceManager(request.OwnerDocument.NameTable);
            namespaceMgr.AddNamespace("soap", "http://www.w3.org/2003/05/soap-envelope");
            namespaceMgr.AddNamespace("urn", "urn:ihe:iti:xds-b:2007");
            return namespaceMgr;
        }
    }

    public class DocumentInfo
    {
        public string RepositoryUniqueId
        {
            get;
            set;
        }

        public string DocumentUniqueId
        {
            get;
            set;
        }

        public DocumentInfo(XmlNode docRequest, XmlNamespaceManager namespaceMgr)
        {
            RepositoryUniqueId = docRequest.SelectSingleNode("urn:RepositoryUniqueId", namespaceMgr).InnerText;
            DocumentUniqueId = docRequest.SelectSingleNode("urn:DocumentUniqueId", namespaceMgr).InnerText;
        }
    }
}