using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using HIEService.DBHelper;
using HIEService.XmlResponseGenerator;

namespace HIEService.RequestHandlers
{
    public class PDQRequest
    {
        String SSNExtension;
        String MRNRootID;
        String MRNassigningAuthority;
        String MRNExtension;
        String familyName;
        String firstName;
        PatientAddress address;
        DateTime? dateOfBirth;

        public PDQRequest(XmlElement request)
        {
            XmlNamespaceManager namespaceMgr = _GetNamespaceManager();
            XmlNode parameterListNode = request.SelectSingleNode("/soapenv:Body/ns3:PRPA_IN201305UV02/ns3:controlActProcess/ns3:queryByParameter/ns3:parameterList", namespaceMgr);

            XmlNodeList livingSubjectIDList = parameterListNode.SelectNodes("ns3:livingSubjectId", namespaceMgr);
            foreach (XmlNode livingSubID in livingSubjectIDList)
            {
                if (livingSubID.SelectSingleNode("ns3:value", namespaceMgr).Attributes["root"].InnerText == ConfigurationManager.AppSettings["SSNRootID"].ToString())
                {
                    SSNExtension = livingSubID.SelectSingleNode("ns3:value", namespaceMgr).Attributes["extension"].InnerText;
                }
                else
                {
                    MRNRootID = livingSubID.SelectSingleNode("ns3:value", namespaceMgr).Attributes["root"].InnerText;
                    MRNExtension = livingSubID.SelectSingleNode("ns3:value", namespaceMgr).Attributes["extension"].InnerText;
                    MRNassigningAuthority = livingSubID.SelectSingleNode("ns3:value", namespaceMgr).Attributes["assigningAuthorityName"].InnerText;
                }
            }
            if (parameterListNode.SelectSingleNode("ns3:livingSubjectName/ns3:value/ns3:family", namespaceMgr) != null)
            {
                familyName = parameterListNode.SelectSingleNode("ns3:livingSubjectName/ns3:value/ns3:family", namespaceMgr).InnerText;                
            }
            if (parameterListNode.SelectSingleNode("ns3:livingSubjectName/ns3:value/ns3:given", namespaceMgr) != null)
            {
                firstName = parameterListNode.SelectSingleNode("ns3:livingSubjectName/ns3:value/ns3:given", namespaceMgr).InnerText;
            }
            if (parameterListNode.SelectSingleNode("ns3:patientAddress", namespaceMgr) != null)
            {
                address = new PatientAddress(parameterListNode.SelectSingleNode("ns3:patientAddress", namespaceMgr), namespaceMgr);
            }
            if (parameterListNode.SelectSingleNode("ns3:livingSubjectBirthTime/ns3:value", namespaceMgr).Attributes["value"] != null && !String.IsNullOrEmpty(parameterListNode.SelectSingleNode("ns3:livingSubjectBirthTime/ns3:value", namespaceMgr).Attributes["value"].Value))
            {
                dateOfBirth = DateTime.ParseExact(parameterListNode.SelectSingleNode("ns3:livingSubjectBirthTime/ns3:value", namespaceMgr).Attributes["value"].Value, "yyyyMMdd", null);
            }
        }

        public XmlElement ProccessRequestAndGetResponse()
        {
            List<HIEPatient> patientList = HIEPatient.GetPatientList(SSNExtension, firstName, familyName);
            if (patientList.Count > 1)
            {
                patientList.RemoveAll(patient => !patient.CheckPatientMatchWithFilter(address, dateOfBirth));   
            }
            return PDQResponseGenerator.GetResponseXml(patientList);
        }

        private static XmlNamespaceManager _GetNamespaceManager()
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(new XmlDocument().NameTable);
            mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
            mgr.AddNamespace("ns2", "http://schemas.xmlsoap.org/ws/2004/08/addressing");
            mgr.AddNamespace("ns3", "urn:hl7-org:v3");
            mgr.AddNamespace("ns4", "urn:gov:hhs:fha:nhinc:common:patientcorrelationfacade");
            return mgr;
        }
    }

    public class PatientAddress
    {
        public string streetAddressLine
        {
            get;
            set;
        }
        public String city
        {
            get;
            set;
        }
        public String state
        {
            get;
            set;
        }
        public String PostalCode
        {
            get;
            set;
        }

        public PatientAddress(XmlNode addressNode, XmlNamespaceManager namespaceMgr)
        {
            streetAddressLine = addressNode.SelectSingleNode("ns3:value/ns3:streetAddressLine", namespaceMgr).InnerText;
            city = addressNode.SelectSingleNode("ns3:value/ns3:city", namespaceMgr).InnerText;
            state = addressNode.SelectSingleNode("ns3:value/ns3:state", namespaceMgr).InnerText;
            PostalCode = addressNode.SelectSingleNode("ns3:value/ns3:postalCode", namespaceMgr).InnerText;

        }
    }
}