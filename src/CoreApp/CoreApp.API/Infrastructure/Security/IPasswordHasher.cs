using System;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure.Security;

public interface IPasswordHasher : IDisposable
{
    Task<byte[]> Hash(string password, byte[] salt);
}
