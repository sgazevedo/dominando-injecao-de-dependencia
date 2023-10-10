using DependencyStore.WebApi.Models;

namespace DependencyStore.WebApi.Repositories.Contracts
{
  public interface IPromoCodeRepository
  {
    Task<PromoCode?> GetPromoCodeAsync(string promoCode);
  }
}
