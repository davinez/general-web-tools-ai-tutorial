using CoreApp.API.Infrastructure.Hubs.Dto;
using System.Threading.Tasks;

namespace CoreApp.API.Domain.Hubs;

public interface IJobEventStatusHub
{
  Task StatusUpdate(JobEventStatusHubDto jobEventStatus);
}
