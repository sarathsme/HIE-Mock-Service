using System.Web;
using System.Xml;

namespace HIEService.XmlResponseGenerator
{
    public class PIXResponseGenerator
    {
        public static XmlElement GetResponse()
        {
            XmlDocument pixResponse = new XmlDocument();
            pixResponse.Load(HttpContext.Current.Server.MapPath("~/XmlResponseGenerator/XmlResponseTemplates/PIXReponse.xml"));
            return pixResponse.DocumentElement;
        }
    }
}