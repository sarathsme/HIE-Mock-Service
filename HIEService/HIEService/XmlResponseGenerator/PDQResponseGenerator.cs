using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using HIEService.DBHelper;

namespace HIEService.XmlResponseGenerator
{
    public class PDQResponseGenerator
    {
        public static XmlElement GetResponseXml(List<HIEPatient> patientList)
        {
            XmlDocument PDQResponseDoc = new XmlDocument();
            PDQResponseDoc.Load(HttpContext.Current.Server.MapPath("~/XmlResponseGenerator/XmlResponseTemplates/PDQResponse.xml"));

            XmlNamespaceManager namespaceMgr = _GetnamespaceManager(PDQResponseDoc);
            XmlNode controlActProcess = PDQResponseDoc.SelectSingleNode("/soap:Envelope/soap:Body/ns1:PRPA_IN201306UV02/ns1:controlActProcess", namespaceMgr);

            foreach (HIEPatient patient in patientList)
            {
                XmlElement subjectXml = _GetSubjectXmlForPatient(patient);
                XmlNode subjectNode = controlActProcess.OwnerDocument.ImportNode(subjectXml, true);
                controlActProcess.InsertBefore(subjectNode, controlActProcess.SelectSingleNode("ns1:queryAck", namespaceMgr));
            }

            return PDQResponseDoc.DocumentElement;
        }

        private static XmlNamespaceManager _GetnamespaceManager(XmlDocument responseXml)
        {
            XmlNamespaceManager namespaceMgr = new XmlNamespaceManager(responseXml.NameTable);
            namespaceMgr.AddNamespace("soap", "http://www.w3.org/2003/05/soap-envelope");
            namespaceMgr.AddNamespace("ns1", "urn:hl7-org:v3");
            return namespaceMgr;
        }

        private static XmlElement _GetSubjectXmlForPatient(HIEPatient patient)
        {
            XmlDocument subjectXml = new XmlDocument();
            subjectXml.Load(HttpContext.Current.Server.MapPath("~/XmlResponseGenerator/XmlResponseTemplates/PDQSubject.xml"));
            XmlNode patientNode = subjectXml.SelectSingleNode("/subject/registrationEvent/subject1/patient");

            if (patient.PatientAddresses.FirstOrDefault() != null)
            {
                XmlNode addressNode = patientNode.OwnerDocument.ImportNode(_GetAddressXml(patient.PatientAddresses.FirstOrDefault()), true);
                patientNode.SelectSingleNode("patientPerson").InsertBefore(addressNode, patientNode.SelectSingleNode("patientPerson/maritalStatusCode"));
            }

            foreach (HIEPatientContactNumber contact in patient.ContactNumbers)
            {
                XmlNode contactNode = patientNode.OwnerDocument.ImportNode(_GetContactNumberXml(contact), true);
                patientNode.SelectSingleNode("patientPerson").InsertBefore(contactNode, patientNode.SelectSingleNode("patientPerson/administrativeGenderCode"));
            }

            patientNode.SelectSingleNode("patientPerson/name[@use='L']/family").InnerText = patient.FamilyName;
            patientNode.SelectSingleNode("patientPerson/name[@use='L']/given[1]").InnerText = patient.FirstName;
            patientNode.SelectSingleNode("patientPerson/name[@use='L']/given[2]").InnerText = patient.MiddleName;

            patientNode.SelectSingleNode("patientPerson/administrativeGenderCode/@code").InnerText = patient.Gender.ToString();
            patientNode.SelectSingleNode("patientPerson/birthTime/@value").InnerText = patient.DateOfBirth.ToString("yyyyMMdd");
            patientNode.SelectSingleNode("patientPerson/asOtherIDs/id[@root='" + ConfigurationManager.AppSettings["SSNRootID"] + "']/@extension").InnerText = patient.SSN;

            XmlNode idNode = patientNode.OwnerDocument.ImportNode(_GetIDXml(patient.EMPID, ConfigurationManager.AppSettings["HIEMPIRootValue"], ConfigurationManager.AppSettings["HIEPrimaryRootAssigningAuthorityName"]), true);
            patientNode.InsertBefore(idNode, patientNode.SelectSingleNode("statusCode"));
            idNode = patientNode.OwnerDocument.ImportNode(_GetIDXml(patient.IPHRID, ConfigurationManager.AppSettings["IPHRRootID"], "IPHR"), true);
            patientNode.InsertBefore(idNode, patientNode.SelectSingleNode("statusCode"));

            return subjectXml.DocumentElement;
        }

        private static XmlElement _GetAddressXml(HIEPatientAddress address)
        {
            XmlDocument addressXmlDoc = new XmlDocument();
            XmlElement rootNode = addressXmlDoc.CreateElement("addr");
            addressXmlDoc.AppendChild(rootNode); 

            XmlAttribute useAttribute = addressXmlDoc.CreateAttribute("use");
            useAttribute.Value = "H";
            rootNode.Attributes.Append(useAttribute);

            XmlElement country = addressXmlDoc.CreateElement("country");
            country.InnerText = address.Country;
            rootNode.AppendChild(country);

            XmlElement city = addressXmlDoc.CreateElement("city");
            city.InnerText = address.City;
            rootNode.AppendChild(city);

            XmlElement state = addressXmlDoc.CreateElement("state");
            state.InnerText = address.State;
            rootNode.AppendChild(state);

            XmlElement streetAddressLine = addressXmlDoc.CreateElement("streetAddressLine");
            streetAddressLine.InnerText = address.StreetAddressLine;
            rootNode.AppendChild(streetAddressLine);

            XmlElement postalCode = addressXmlDoc.CreateElement("postalCode");
            postalCode.InnerText = address.PostalCode;
            rootNode.AppendChild(postalCode);

            return addressXmlDoc.DocumentElement;
        }

        private static XmlElement _GetContactNumberXml(HIEPatientContactNumber contactNumber)
        {
            XmlDocument contactDoc = new XmlDocument();
            XmlElement telecomNode = contactDoc.CreateElement("telecom");
            contactDoc.AppendChild(telecomNode);

            XmlAttribute attrTelecomValue = contactDoc.CreateAttribute("value");
            attrTelecomValue.Value = String.Format("tel:{0}",contactNumber.ContactNumber);
            telecomNode.Attributes.Append(attrTelecomValue);

            XmlAttribute attrTelecomUse = contactDoc.CreateAttribute("use");
            attrTelecomUse.Value = contactNumber.ContactType;
            telecomNode.Attributes.Append(attrTelecomUse);

            return contactDoc.DocumentElement;
        }

        private static XmlElement _GetIDXml(string extension, string root, string assigningAuthorityName)
        {
            XmlDocument idDocument = new XmlDocument();
            XmlElement idNode = idDocument.CreateElement("id");
            idDocument.AppendChild(idNode);

            XmlAttribute attrRoot = idDocument.CreateAttribute("root");
            attrRoot.Value = root;
            idNode.Attributes.Append(attrRoot);

            XmlAttribute attrExtension = idDocument.CreateAttribute("extension");
            attrExtension.Value = extension;
            idNode.Attributes.Append(attrExtension);

            XmlAttribute attrAssigningAuthorityName = idDocument.CreateAttribute("assigningAuthorityName");
            attrAssigningAuthorityName.Value = assigningAuthorityName;
            idNode.Attributes.Append(attrAssigningAuthorityName);

            return idDocument.DocumentElement;
        }
    }
}