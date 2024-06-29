using System.Threading.Tasks;

namespace ViewModel
{
    public interface IMessenger
    {
        public Task Message(string message);
    }
}
