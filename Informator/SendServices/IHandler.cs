using Informator.Models;

namespace Informator.SendServices
{
    public interface IHandler
    {
        
        void Send(string message, List<string> mailingList);
        void Listener();
    }
}
