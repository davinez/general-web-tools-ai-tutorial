using CoreApp.API.Domain.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CoreApp.API.Infrastructure.Hubs;

public class JobEventStatusHub : Hub<IJobEventStatusHub>
{
}
