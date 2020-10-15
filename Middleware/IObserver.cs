using System.Threading.Tasks;

namespace WeirdUnitBE.Middleware
{
    public interface IObserver
    {
        Task SendData(ISubject subject, object data);
    }
}