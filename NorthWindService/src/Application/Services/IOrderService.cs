using NorthWindService.Application.DTOs;

namespace NorthWindService.Application.Services;

public interface IOrderService
{
    Task<List<QueryBasicOrderDTO>> QueryBasicOrder();
    Task UpdateOrderCustomerNameAsync(int orderId, UpdateOrderCustomerNameDTO request);
    Task<OrderDropdownListsResponse> GetOrderDropdownDataAsync();
    Task<int> CreateOrderAsync(NewOrderReqDTO request);
}