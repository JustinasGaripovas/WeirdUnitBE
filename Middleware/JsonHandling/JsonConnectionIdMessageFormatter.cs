using System;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonConnectionIdMessageFormatter : JsonMessageFormatterTemplate
    {
        protected override object FormatMessageObjectFromParams(params object[] parameters)
        {
            string connectionId = (string)parameters[0];

            var messageObject = new
                {
                    command = ConnIdCommand(),
                    payload = connectionId
                };

            return messageObject;
        }

        private string ConnIdCommand()
        {
            return Constants.JsonCommands.ServerCommands.CONN_ID;
        }
    }
}