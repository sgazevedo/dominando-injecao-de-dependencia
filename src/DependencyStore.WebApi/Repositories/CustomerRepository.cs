using Dapper;
using DependencyStore.WebApi.Models;
using DependencyStore.WebApi.Repositories.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.WebApi.Repositories
{
  public class CustomerRepository : ICustomerRepository
  {
    private readonly SqlConnection _connection;

    public CustomerRepository(SqlConnection connection)
    {
      _connection = connection;
    }

    public async Task<Customer?> GetCustomerAsync(int id)
    {
      var query = $"SELECT * FROM CUSTOMER WHERE ID={id}";
      return await _connection.QueryFirstAsync<Customer>(query);
    }
  }
}
