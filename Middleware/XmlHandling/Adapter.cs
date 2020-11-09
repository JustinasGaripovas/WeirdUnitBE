using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeirdUnitBE.Middleware;
using WeirdUnitBE.Middleware.Observable.ConcreteSubjects;
using System.Xml;



namespace WeirdUnitBE.Middleware.XmlHandling
{

    public class Adapter
    {
        
        public Adapter()
        {
        }

        public bool IsJson(string input)
        {
        input = input.Trim();
        return input.StartsWith("{") && input.EndsWith("}") || input.StartsWith("[") && input.EndsWith("]");
        }

        public string ConvertToJson(string xml)
        {
            // To convert an XML node contained in string xml into a JSON string   
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string jsonText = JsonConvert.SerializeXmlNode(doc);

            return jsonText;
        }

    }

}