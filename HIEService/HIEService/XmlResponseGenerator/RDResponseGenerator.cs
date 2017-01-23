using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using HIEService.DBHelper;
using HIEService.RequestHandlers;

namespace HIEService.XmlResponseGenerator
{
    public class RDResponseGenerator
    {
        public static Stream GetResponseStream(List<HIEPatientDocument> patientDocs, List<DocumentInfo> requestedDocs)
        {
            String RDResponseXml = _GetResponseXml(patientDocs, requestedDocs);

            String attachments = _GetAttachments(patientDocs);

            String RDFullResponse = File.ReadAllText(HttpContext.Current.Server.MapPath("~/XmlResponseGenerator/XmlResponseTemplates/RetrieveDocumentsTemplates/RDFullResponse.txt"));
            RDFullResponse = RDFullResponse.Replace("##RDResponseSection##", RDResponseXml);
            RDFullResponse = RDFullResponse.Replace("##RDAttachmentSection##", attachments);
            MemoryStream strm = new MemoryStream(Encoding.ASCII.GetBytes(RDFullResponse));
            return strm;
        }

        private static string _GetAttachments(List<HIEPatientDocument> patientDocs)
        {
            String attachments = String.Empty;
            foreach (HIEPatientDocument doc in patientDocs)
            {
                String RDAttachmentSection = File.ReadAllText(HttpContext.Current.Server.MapPath("~/XmlResponseGenerator/XmlResponseTemplates/RetrieveDocumentsTemplates/RDAttachmentSection.txt"));
                RDAttachmentSection = RDAttachmentSection.Replace("##AttachmentFile##", doc.Document.ToString());
                attachments = string.Format("{0}{1}", attachments, RDAttachmentSection);
            }
            return attachments;
        }

        private static string _GetResponseXml(List<HIEPatientDocument> retrievedDocs, List<DocumentInfo> requestedDocs)
        {                       
            XmlDocument RDResponseSection = new XmlDocument();
            RDResponseSection.Load(HttpContext.Current.Server.MapPath("~/XmlResponseGenerator/XmlResponseTemplates/RetrieveDocumentsTemplates/RDResponseSection.xml"));
            XmlNamespaceManager namespaceManager = _GetNamespaceManager(RDResponseSection);

            _InsertDocumentResponseNodes(retrievedDocs, RDResponseSection, namespaceManager);

            if (retrievedDocs.Count < requestedDocs.Count)
            {
                XmlNode registryResponse = RDResponseSection.SelectSingleNode("/env:Envelope/env:Body/xds-b:RetrieveDocumentSetResponse/rs:RegistryResponse", namespaceManager);
                if (retrievedDocs.Count == 0)
                {
                    registryResponse.Attributes["status"].InnerText = "urn:oasis:names:tc:ebxml-regrep:ResponseStatusType:Failure";
                }
                else
                {
                    registryResponse.Attributes["status"].InnerText = "urn:ihe:iti:2007:ResponseStatusType:PartialSuccess";
                }

                XmlElement registryErrorList = _InsertRegistryErrorListNode(RDResponseSection, registryResponse);

                foreach (DocumentInfo requestedDoc in requestedDocs)
                {
                    if (!retrievedDocs.Exists(doc => doc.DocumentUniqueID == requestedDoc.DocumentUniqueId))
                    {
                        _InsertRegistryErrorNode(requestedDoc, RDResponseSection, registryErrorList);
                    }
                }
            }
            return RDResponseSection.InnerXml;
        }

        private static XmlElement _InsertRegistryErrorListNode(XmlDocument RDResponseSection, XmlNode registryResponse)
        {
            XmlElement registryErrorList = RDResponseSection.CreateElement("rs:RegistryErrorList", "urn:oasis:names:tc:ebxml-regrep:xsd:rs:3.0");
            XmlAttribute highestSeverity = RDResponseSection.CreateAttribute("highestSeverity");
            highestSeverity.Value = "urn:oasis:names:tc:ebxml-regrep:ErrorSeverityType:Error";
            registryErrorList.Attributes.Append(highestSeverity);
            registryResponse.AppendChild(registryErrorList);
            return registryErrorList;
        }

        private static void _InsertRegistryErrorNode(DocumentInfo requestedDoc, XmlDocument RDResponseSection, XmlElement registryErrorList)
        {
            XmlElement registryError = RDResponseSection.CreateElement("rs:RegistryError", "urn:oasis:names:tc:ebxml-regrep:xsd:rs:3.0");
            XmlAttribute location = RDResponseSection.CreateAttribute("location");
            XmlAttribute codeContext = RDResponseSection.CreateAttribute("codeContext");
            codeContext.Value = String.Format("The document associated with the DocumentUniqueId '{0}' is not available. This could be because the document is not available to the Document Repository '{1}', the requestor is not authorized to access that document or the document is no longer available.", requestedDoc.DocumentUniqueId, requestedDoc.RepositoryUniqueId);
            XmlAttribute errorCode = RDResponseSection.CreateAttribute("errorCode");
            errorCode.Value = "XDSDocumentUniqueIdError";
            XmlAttribute severity = RDResponseSection.CreateAttribute("severity");
            severity.Value = "urn:oasis:names:tc:ebxml-regrep:ErrorSeverityType:Error";
            registryError.Attributes.Append(codeContext);
            registryError.Attributes.Append(errorCode);
            registryError.Attributes.Append(location);
            registryError.Attributes.Append(severity);
            registryErrorList.AppendChild(registryError);
        }

        private static void _InsertDocumentResponseNodes(List<HIEPatientDocument> retrievedDocs, XmlDocument RDResponseSection, XmlNamespaceManager namespaceManager)
        {
            XmlDocument documentResponseNode = new XmlDocument();
            foreach (HIEPatientDocument doc in retrievedDocs)
            {
                documentResponseNode.Load(HttpContext.Current.Server.MapPath("~/XmlResponseGenerator/XmlResponseTemplates/RetrieveDocumentsTemplates/RDAttachmentDocResponseNode.xml"));
                documentResponseNode.SelectSingleNode("/xds-b:DocumentResponse/xds-b:DocumentUniqueId", namespaceManager).InnerText = doc.DocumentUniqueID;
                RDResponseSection.SelectSingleNode("/env:Envelope/env:Body/xds-b:RetrieveDocumentSetResponse", namespaceManager).AppendChild(RDResponseSection.ImportNode(documentResponseNode.DocumentElement, true));
            }
        }

        private static XmlNamespaceManager _GetNamespaceManager(XmlDocument doc)
        {
            XmlNamespaceManager namespaceMgr = new XmlNamespaceManager(doc.NameTable);
            namespaceMgr.AddNamespace("env", "http://www.w3.org/2003/05/soap-envelope");
            namespaceMgr.AddNamespace("wsa", "http://www.w3.org/2005/08/addressing");
            namespaceMgr.AddNamespace("a", "http://www.w3.org/2005/08/addressing");
            namespaceMgr.AddNamespace("xds-b", "urn:ihe:iti:xds-b:2007");
            namespaceMgr.AddNamespace("rs", "urn:oasis:names:tc:ebxml-regrep:xsd:rs:3.0");
            return namespaceMgr;
        }
    }
}