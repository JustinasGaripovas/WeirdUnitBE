using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace WeirdUnitBE.Middleware.XmlHandling
{
    public interface ISubject
    {
        void Request();
    }

    // class RealSubject : ISubject
    // {
    //     public void Request()
    //     {
    //         Console.WriteLine("RealSubject: Handling Request.");
    //     }
    // }

    class Proxy : ISubject
    {
        private Adapter _realSubject;
        
        public Proxy(Adapter realSubject)
        {
            this._realSubject = realSubject;
        }

        public string stringJson(string stringMessage)
        {
            Request();
            return _realSubject.ConvertToJson(stringMessage);
        }

        public void Request()
        {
                Console.WriteLine("Proxy: Logging the time of request. {0}", DateTime.Now.ToString("h:mm:ss t") );
        }
        
    }


}