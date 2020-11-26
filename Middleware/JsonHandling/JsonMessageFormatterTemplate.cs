using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public abstract class JsonMessageFormatterTemplate
    {
        public object FormatJsonBufferFromParams(params object[] parameters)
        {
            dynamic messageObject = FormatMessageObjectFromParams(parameters);
            object jsonBuffer = FormatJsonBufferFromObject(messageObject);

            return jsonBuffer;
        }

        protected abstract object FormatMessageObjectFromParams(params object[] parameters);

        private object FormatJsonBufferFromObject(dynamic messageObject)
        {
            var messageJson = JsonConvert.SerializeObject(messageObject, Formatting.Indented);
            var jsonBuffer = Encoding.UTF8.GetBytes(messageJson);

            return jsonBuffer;
        }
    }
}