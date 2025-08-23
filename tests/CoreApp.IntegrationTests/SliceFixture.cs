//using CoreApp.API;
//using CoreApp.API.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.IO;
//using System.Threading.Tasks;

//namespace CoreApp.IntegrationTests;

//public class SliceFixture : IDisposable
//{
//  private readonly IServiceScopeFactory _scopeFactory;
//  private readonly ServiceProvider _provider;
//  private readonly string _dbName = Guid.NewGuid() + ".db";

//  public SliceFixture()
//  {
//    var services = new ServiceCollection();
//    services.AddCoreAppAPI();

//    var builder = new DbContextOptionsBuilder();
//    //builder.UseInMemoryDatabase(_dbName);
//    services.AddSingleton(new CoreAppContext((DbContextOptions<CoreAppContext>)builder.Options));

//    _provider = services.BuildServiceProvider();

//    GetDbContext().Database.EnsureCreated();
//    _scopeFactory = _provider.GetRequiredService<IServiceScopeFactory>();
//  }

//  public CoreAppContext GetDbContext() => _provider.GetRequiredService<CoreAppContext>();

//  public void Dispose() => File.Delete(_dbName);

//  public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
//  {
//    using var scope = _scopeFactory.CreateScope();
//    await action(scope.ServiceProvider);
//  }

//  public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
//  {
//    using var scope = _scopeFactory.CreateScope();
//    return await action(scope.ServiceProvider);
//  }

//  public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request) =>
//      ExecuteScopeAsync(sp =>
//      {
//        var mediator = sp.GetRequiredService<IMediator>();

//        return mediator.Send(request);
//      });

//  public Task SendAsync(IRequest request) =>
//      ExecuteScopeAsync(sp =>
//      {
//        var mediator = sp.GetRequiredService<IMediator>();

//        return mediator.Send(request);
//      });

//  public Task ExecuteDbContextAsync(Func<CoreAppContext, Task> action) =>
//      ExecuteScopeAsync(sp => action(sp.GetRequiredService<CoreAppContext>()));

//  public Task<T> ExecuteDbContextAsync<T>(Func<CoreAppContext, Task<T>> action) =>
//      ExecuteScopeAsync(sp => action(sp.GetRequiredService<CoreAppContext>()));

//  public Task InsertAsync(params object[] entities) =>
//      ExecuteDbContextAsync(db =>
//      {
//        foreach (var entity in entities)
//        {
//          db.Add(entity);
//        }
//        return db.SaveChangesAsync();
//      });
//}
