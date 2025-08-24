using System.Threading.Tasks;

namespace CoreApp.API.Domain.Hubs;

public interface IJobEventStatusHub
{
  Task StatusUpdate(string eventId, string status);
}
