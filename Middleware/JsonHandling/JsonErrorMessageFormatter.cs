using System;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonErrorMessageFormatter : JsonMessageFormatterTemplate
    {
        protected override object FormatMessageObjectFromParams(params object[] parameters)
        {
            Exception exception = (Exception)parameters[0];
            string exceptionMessage = exception.Message;

            var messageObject = new
                {
                    command = ExceptionCommand(),
                    payload = new
                    {
                        message = exceptionMessage
                    }
                };

            return messageObject;
        }

        private string ExceptionCommand()
        {
            return Constants.JsonCommands.ServerCommands.EXCEPTION;
        }
    }
}