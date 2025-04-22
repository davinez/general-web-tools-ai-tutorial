using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Features.Profiles;

public interface IProfileReader
{
    Task<ProfileEnvelope> ReadProfile(string username, CancellationToken cancellationToken);
}
