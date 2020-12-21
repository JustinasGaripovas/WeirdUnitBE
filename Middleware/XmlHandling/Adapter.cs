using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace WeirdUnitBE.Middleware.XmlHandling
{
    public class Adapter : ISubject
    {
        public string ConvertToJson(string stringMessage)
        {   
            string stringJson;
            if(!IsJson(stringMessage))
            {
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(stringMessage);

                stringJson = JsonConvert.SerializeXmlNode(xmldocument);
            }
            else
            {
                stringJson = stringMessage;
            }

            return stringJson;
        }

        private bool IsJson(string input)
        {
            input = input.Trim();
            return InputWrappedInCurlyBrackets(input) || InputWrappedInSquareBrackets(input);
        }

        private bool InputWrappedInCurlyBrackets(string input)
        {
            return input.StartsWith("{") && input.EndsWith("}");
        }

        private bool InputWrappedInSquareBrackets(string input)
        {
            return input.StartsWith("[") && input.EndsWith("]");
        }


    public void Request()
    {
       Console.WriteLine("RealSubject: Handling Request.");
    }

    }
}