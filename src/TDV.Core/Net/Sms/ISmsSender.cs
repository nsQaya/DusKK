using System.Threading.Tasks;

namespace TDV.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}