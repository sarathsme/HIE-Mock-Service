using System;
using System.Collections.Generic;
using System.Xml;
using HIEService.DBHelper;
using HIEService.XmlResponseGenerator;

namespace HIEService.RequestHandlers
{
    public class StoredQueryRequest
    {
        public string EMPID
        {
            get;
            set;
        }

        public string MPIRootValue
        {
            get;
            set;
        }

        public StoredQueryRequest(XmlElement request)
        {
            XmlNamespaceManager namespaceMgr = _GetnamespaceManager(request);
            String requestParameterList = request.SelectSingleNode("/soapenv:Body/query:AdhocQueryRequest/rim:AdhocQuery/rim:Slot/rim:ValueList/rim:Value", namespaceMgr).InnerText;

            char seperator = '&';
            EMPID = requestParameterList.Split(seperator)[0].Trim('^','\'');
            MPIRootValue = requestParameterList.Split(seperator)[1].Trim('I', 'S', 'O', '&', '\'');
        }

        public XmlElement ProcessRequestAndGetResponse()
        {
            List<HIEPatientDocument> docSummary = HIEPatientDocument.GetDocumentSummaryForEmpId(EMPID);
            return SQResponseGenerator.GetResponseXml(docSummary);
        }

        private static XmlNamespaceManager _GetnamespaceManager(XmlElement request)
        {
            XmlNamespaceManager namespaceMgr = new XmlNamespaceManager(request.OwnerDocument.NameTable);
            namespaceMgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
            namespaceMgr.AddNamespace("query", "urn:oasis:names:tc:ebxml-regrep:xsd:query:3.0");
            namespaceMgr.AddNamespace("rim", "urn:oasis:names:tc:ebxml-regrep:xsd:rim:3.0");
            return namespaceMgr;
        }


    }
}