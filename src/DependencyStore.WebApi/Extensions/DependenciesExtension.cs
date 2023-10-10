using DependencyStore.WebApi.Repositories;
using DependencyStore.WebApi.Repositories.Contracts;
using DependencyStore.WebApi.Services;
using DependencyStore.WebApi.Services.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.WebApi.Extensions
{
  public static class DependenciesExtension
  {
    public static void AddSqlConnection(this IServiceCollection services, string connectionString)
    {
      services.AddScoped(x => new SqlConnection(connectionString));
    }

    public static void AddRepositories(this IServiceCollection services)
    {
      services.AddTransient<ICustomerRepository, CustomerRepository>();
      services.AddTransient<IProductRepository, ProductRepository>();
      services.AddTransient<IPromoCodeRepository, PromoCodeRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
      services.AddTransient<IDeliveryFeeService, DeliveryFeeService>();
    }
  }
}
