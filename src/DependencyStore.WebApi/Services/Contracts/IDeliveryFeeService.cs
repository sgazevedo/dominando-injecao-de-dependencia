namespace DependencyStore.WebApi.Services.Contracts
{
  public interface IDeliveryFeeService
  {
    Task<decimal> GetDeliveryFeeAsync(string zipCode);
  }
}
