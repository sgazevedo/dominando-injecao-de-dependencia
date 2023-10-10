using DependencyStore.WebApi.Models;
using DependencyStore.WebApi.Repositories.Contracts;
using DependencyStore.WebApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DependencyStore.WebApi.Controllers
{
  public class OrderController : ControllerBase
  {
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeliveryFeeService _deliveryFeeService;
    private readonly IPromoCodeRepository _promoCodeRepository;
    private readonly IProductRepository _productRepository;

    public OrderController(
      ICustomerRepository customerRepository,
      IDeliveryFeeService deliveryFeeService,
      IPromoCodeRepository promoCodeRepository,
      IProductRepository productRepository)
    {
      _customerRepository = customerRepository;
      _deliveryFeeService = deliveryFeeService;
      _promoCodeRepository = promoCodeRepository;
      _productRepository = productRepository;
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

      // #3 - Obtém os produtos
      var orderProducts = await _productRepository.GetByIds(products);

      // #4 - Aplica o cupom de desconto
      var cupom = await _promoCodeRepository.GetPromoCodeAsync(promoCode);
      var discount = cupom?.ExpireDate > DateTime.UtcNow 
        ? cupom?.Value ?? 0M 
        : 0M;

      // #5 - Gera o pedido
      var order = new Order(deliveryFee, discount, orderProducts.ToList());
      
      // #7 - Retorna
      return Ok(new
      {
        Message = $"Pedido {order.Code} gerado com sucesso!"
      });
    }
  }
}
