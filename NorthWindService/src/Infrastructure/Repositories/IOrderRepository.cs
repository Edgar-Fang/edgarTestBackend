using NorthWindService.Application.DTOs;
using NorthWindService.Domain.Entities;
using NorthWindService.src.Infrastructure.Persistence.Entities;

namespace NorthWindService.Infrastructure.Repositories;

public interface IOrderRepository
{
    Task<List<OrderDomain>> QueryBasicOrderAsync();

    Task UpdateCustomerNameAsync(int orderId, UpdateOrderCustomerNameDTO request);

    Task<(IEnumerable<CustomerDropDownDomain> customers, IEnumerable<EmployeeDropDownDomain> employees,
        IEnumerable<ShipperDropDownDomain> shippers)> GetOrderDropdownDataAsync();
    Task<int> CreateOrderAsync(NewOrderReqDTO request);
}