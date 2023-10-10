using Dapper;
using DependencyStore.WebApi.Models;
using DependencyStore.WebApi.Repositories.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.WebApi.Repositories
{
  public class ProductRepository : IProductRepository
  {
    private readonly SqlConnection _connection;

    public ProductRepository(SqlConnection connection)
    {
      _connection = connection;
    }

    public async Task<IEnumerable<Product>> GetByIds(int[] idsProducts)
    {
      var products = new List<Product>();
      foreach (var id in idsProducts) 
      {
        var query = $"SELECT * FROM PRODUCT WHERE ID={id}";
        var product = await _connection.QueryFirstAsync<Product>(query);
        products.Add(product);
      }

      return products;
    }
  }
}
