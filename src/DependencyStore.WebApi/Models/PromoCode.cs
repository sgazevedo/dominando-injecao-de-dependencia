namespace DependencyStore.WebApi.Models
{
  public class PromoCode
  {
    public DateTime ExpireDate { get; set; }
    public decimal Value { get; set; }
    public string Code { get; set; }
  }
}
