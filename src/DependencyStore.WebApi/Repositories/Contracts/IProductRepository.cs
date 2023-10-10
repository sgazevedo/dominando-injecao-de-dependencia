using DependencyStore.WebApi.Models;

namespace DependencyStore.WebApi.Repositories.Contracts
{
  public interface IProductRepository
  {
    Task<IEnumerable<Product>> GetByIds(int[] idsProducts);
  }
}
