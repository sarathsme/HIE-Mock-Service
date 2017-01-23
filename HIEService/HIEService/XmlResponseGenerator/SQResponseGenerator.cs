using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using HIEService.DBHelper;

namespace HIEService.XmlResponseGenerator
{
    public class SQResponseGenerator
    {
        public static XmlElement GetResponseXml(List<HIEPatientDocument> docdetails)
        {
            XmlDocument SQResponseDoc = new XmlDocument();
            SQResponseDoc.Load(HttpContext.Current.Server.MapPath("~/XmlResponseGenerator/XmlResponseTemplates/SQResponse.xml"));

            foreach (HIEPatientDocument docData in docdetails)
            {
                XmlNode extrinsicObjectNode = SQResponseDoc.ImportNode(_GetExtrinsicObjectForUniqueId(docData), true);
                XmlNamespaceManager namespaceManager = _GetNamespaceManager(SQResponseDoc);
                SQResponseDoc.SelectSingleNode("/env:Envelope/env:Body/query:AdhocQueryResponse/rim:RegistryObjectList", namespaceManager).InsertBefore(extrinsicObjectNode, SQResponseDoc.SelectSingleNode("/env:Envelope/env:Body/query:AdhocQueryResponse/rim:RegistryObjectList/rim:ObjectRef", namespaceManager));
            }

            return SQResponseDoc.DocumentElement;
        }

        private static XmlElement _GetExtrinsicObjectForUniqueId(HIEPatientDocument docSummary)
        {
            XmlDocument SQExtrinsicObjectDoc = new XmlDocument();
            SQExtrinsicObjectDoc.Load(HttpContext.Current.Server.MapPath("~/XmlResponseGenerator/XmlResponseTemplates/SQExtrinsicObject.xml"));

            XmlNamespaceManager namespaceManager = _GetNamespaceManager(SQExtrinsicObjectDoc);
            SQExtrinsicObjectDoc.SelectSingleNode("/rim:ExtrinsicObject/rim:ExternalIdentifier[2]/@value", namespaceManager).InnerText = docSummary.DocumentUniqueID;
            SQExtrinsicObjectDoc.SelectSingleNode("/rim:ExtrinsicObject/@status", namespaceManager).InnerText = String.Format("urn:oasis:names:tc:ebxml-regrep:StatusType:{0}", docSummary.IsDeprecated? "Deprecated" : "Approved");
            return SQExtrinsicObjectDoc.DocumentElement;
        }

        private static XmlNamespaceManager _GetNamespaceManager(XmlDocument SQResponseDoc)
        {
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(SQResponseDoc.NameTable);
            namespaceManager.AddNamespace("env", "http://www.w3.org/2003/05/soap-envelope");
            namespaceManager.AddNamespace("query", "urn:oasis:names:tc:ebxml-regrep:xsd:query:3.0");
            namespaceManager.AddNamespace("rim", "urn:oasis:names:tc:ebxml-regrep:xsd:rim:3.0");
            return namespaceManager;
        }
    }
}