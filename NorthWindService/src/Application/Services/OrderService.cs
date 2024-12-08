using NorthWindService.Application.DTOs;
using NorthWindService.Infrastructure.Repositories;

namespace NorthWindService.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<QueryBasicOrderDTO>> QueryBasicOrder()
    {
        var orders = await _repository.QueryBasicOrderAsync();


        return orders.Select(order => new QueryBasicOrderDTO
        {
            OrderId = order.OrderId,
            OrderDate = order.OrderDate,
            CustomerName = order.CustomerName,
            Freight = order.Freight,
            ShippedDate = order.ShippedDate,
            // 使用Domain的業務邏輯方法
            FormattedFreight = order.GetFormattedFreight(),
            FormattedOrderDate = order.GetFormattedOrderDate(),
            ShipStatus = order.GetShipStatus()
        }).ToList();
    }

    public async Task UpdateOrderCustomerNameAsync(int orderId, UpdateOrderCustomerNameDTO request)
    {
        await _repository.UpdateCustomerNameAsync(orderId, request);
    }

    public async Task<OrderDropdownListsResponse> GetOrderDropdownDataAsync()
    {
        var dropdownLists = await _repository.GetOrderDropdownDataAsync();

        var oCustomers = dropdownLists.customers.Select(p => new CustomerDto
        {
            Id = p.Id,
            CompanyName = p.CompanyName,
        }).ToList();

        var oEmployee = dropdownLists.employees.Select(p => new EmployeeDto
        {
            Id = p.Id.ToString(),
            FullName = p.FullName,
        }).ToList();

        var oShipper = dropdownLists.shippers.Select(p => new ShipperDto
        {
            Id = p.Id.ToString(),
            CompanyName = p.CompanyName,
        }).ToList();

        return new OrderDropdownListsResponse
        {
            Customers = oCustomers,
            Employees = oEmployee,
            Shippers = oShipper,
        };
    }

    public async Task<int> CreateOrderAsync(NewOrderReqDTO request)
    {
        return await _repository.CreateOrderAsync(request);
    }
}