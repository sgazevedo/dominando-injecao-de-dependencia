using Dapper;
using DependencyStore.WebApi.Models;
using DependencyStore.WebApi.Repositories.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.WebApi.Repositories
{
  public class PromoCodeRepository : IPromoCodeRepository
  {
    private readonly SqlConnection _connection;

    public PromoCodeRepository(SqlConnection connection)
        => _connection = connection;

    public async Task<PromoCode?> GetPromoCodeAsync(string promoCode)
    {
      var query = $"SELECT * FROM PROMO_CODES WHERE CODE={promoCode}";
      return await _connection.QueryFirstOrDefaultAsync<PromoCode>(query);
    }
  }
}
