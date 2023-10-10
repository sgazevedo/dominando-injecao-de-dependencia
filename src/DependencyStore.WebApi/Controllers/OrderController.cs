using Dapper;
using DependencyStore.WebApi.Models;
using DependencyStore.WebApi.Repositories.Contracts;
using DependencyStore.WebApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DependencyStore.WebApi.Controllers
{
  public class OrderController : ControllerBase
  {
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeliveryFeeService _deliveryFeeService;
    private readonly IPromoCodeRepository _promoCodeRepository;

    public OrderController(
      ICustomerRepository customerRepository,
      IDeliveryFeeService deliveryFeeService,
      IPromoCodeRepository promoCodeRepository)
    {
      _customerRepository = customerRepository;
      _deliveryFeeService = deliveryFeeService;
      _promoCodeRepository = promoCodeRepository;
    }

    [Route("v1/orders")]
    [HttpPost]
    public async Task<IActionResult> Place(
      int customerId, 
      string zipCode, 
      string promoCode, 
      int[] products)
    {
      // #1 - Recupera o cliente
      var customer = await _customerRepository.GetCustomerAsync(customerId);
      if (customer is null)
        return NotFound();

      // #2 - Calcula o frete
      var deliveryFee = await _deliveryFeeService.GetDeliveryFeeAsync(zipCode);

      // #3 - Calcula o total dos produtos
      decimal subTotal = 0;
      const string getProductQuery = "SELECT [Id], [Name], [Price] FROM PRODUCT WHERE ID=@id";
      for (var p = 0; p < products.Length; p++)
      {
        Product product;
        await using (var conn = new SqlConnection("CONN_STRING"))
          product = await conn.QueryFirstAsync<Product>(getProductQuery, new { Id = p });

        subTotal += product.Price;
      }

      // #4 - Aplica o cupom de desconto
      var cupom = await _promoCodeRepository.GetPromoCodeAsync(promoCode);
      var discount = cupom?.ExpireDate > DateTime.UtcNow 
        ? cupom?.Value ?? 0 
        : 0;

      // #5 - Gera o pedido
      var order = new Order();
      order.Code = Guid.NewGuid().ToString().ToUpper().Substring(0, 8);
      order.Date = DateTime.Now;
      order.DeliveryFee = deliveryFee;
      order.Discount = discount;
      order.Products = products;
      order.SubTotal = subTotal;

      // #6 - Calcula o total
      order.Total = subTotal - discount + deliveryFee;

      // #7 - Retorna
      return Ok(new
      {
        Message = $"Pedido {order.Code} gerado com sucesso!"
      });
    }
  }
}
