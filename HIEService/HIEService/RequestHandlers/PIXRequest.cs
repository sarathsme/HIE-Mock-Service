using System;
using System.Xml;
using HIEService.XmlResponseGenerator;

namespace HIEService.RequestHandlers
{
    public class PIXRequest
    {
        String FacilityCode
        {
            get;
            set;
        }

        String FacilityOid
        {
            get;
            set;
        }

        String IPHROID
        {
            get;
            set;
        }

        String IPHRID
        {
            get;
            set;
        }

        public PIXRequest(XmlElement request)
        {
            XmlNamespaceManager namespaceMgr = _GetNamespaceManager();
            XmlNode paramterList = request.SelectSingleNode("/soap:Body/ns:PRPA_IN201309UV02/ns:controlActProcess/ns:queryByParameter/ns:parameterList", namespaceMgr);
            FacilityCode = paramterList.SelectSingleNode("ns:dataSource/ns:value/@assigningAuthorityName", namespaceMgr).InnerText;
            FacilityOid = paramterList.SelectSingleNode("ns:dataSource/ns:value/@root", namespaceMgr).InnerText;
            IPHROID = paramterList.SelectSingleNode("ns:patientIdentifier/ns:value/@root", namespaceMgr).InnerText;
            IPHRID = paramterList.SelectSingleNode("ns:patientIdentifier/ns:value/@extension", namespaceMgr).InnerText;
        }

        public XmlElement ProcessRequestAndGetResponse()
        {
            return PIXResponseGenerator.GetResponse();
        }

        private static XmlNamespaceManager _GetNamespaceManager()
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(new XmlDocument().NameTable);
            mgr.AddNamespace("soap", "http://www.w3.org/2003/05/soap-envelope");
            mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            mgr.AddNamespace("soap", "http://www.w3.org/2003/05/soap-envelope");
            mgr.AddNamespace("ns", "urn:hl7-org:v3");
            return mgr;
        }
    }
}