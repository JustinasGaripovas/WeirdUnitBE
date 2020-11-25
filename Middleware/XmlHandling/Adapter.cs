using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace WeirdUnitBE.Middleware.XmlHandling
{
    public class Adapter
    {
        public bool IsJson(string input)
        {
            input = input.Trim();
            return InputWrappedInCurlyBrackets(input) || InputWrappedInSquareBrackets(input);
        }

        public bool InputWrappedInCurlyBrackets(string input)
        {
            return input.StartsWith("{") && input.EndsWith("}");
        }

        public bool InputWrappedInSquareBrackets(string input)
        {
            return input.StartsWith("[") && input.EndsWith("]");
        }

        public string ConvertToJson(string xmlString)
        {   
            XmlDocument xmldocument = new XmlDocument();
            xmldocument.LoadXml(xmlString);

            string jsonString = JsonConvert.SerializeXmlNode(xmldocument);

            return jsonString;
        }

    }

}