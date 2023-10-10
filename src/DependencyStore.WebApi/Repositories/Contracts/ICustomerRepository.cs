using DependencyStore.WebApi.Models;

namespace DependencyStore.WebApi.Repositories.Contracts
{
  public interface ICustomerRepository
  {
    Task<Customer?> GetCustomerAsync(int id);
  }
}
