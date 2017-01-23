using System.IO;
using System.ServiceModel;
using System.Xml;

namespace HIEService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IHIEService
    {
        [OperationContract]
        XmlElement PatientDemoQuery(XmlElement request);

        [OperationContract]
        XmlElement iPHRDocumentRegistry(XmlElement request);

        [OperationContract]
        Stream iPHRDocumentRepository(XmlElement request);

        [OperationContract]
        XmlElement PIXManager(XmlElement request);
    }
}
