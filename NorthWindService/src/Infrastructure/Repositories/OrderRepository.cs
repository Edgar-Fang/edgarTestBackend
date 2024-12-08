using Microsoft.EntityFrameworkCore;
using NorthWindService.Application.DTOs;
using NorthWindService.Domain.Entities;
using NorthWindService.src.Infrastructure.Persistence.Context;
using NorthWindService.src.Infrastructure.Persistence.Entities;

namespace NorthWindService.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly NorthwindContext _context;

    public OrderRepository(NorthwindContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDomain>> QueryBasicOrderAsync()
    {
        var orders = await _context.Orders.ToListAsync();

        return orders
            .Select(o => OrderDomain.Create(
                o.OrderId,
                o.OrderDate?.ToDateTime(TimeOnly.MinValue),
                o.CustomerId,
                o.Freight ?? 0,
                o.ShippedDate?.ToDateTime(TimeOnly.MinValue)
            )).ToList();
    }

    public async Task UpdateCustomerNameAsync(int orderId, UpdateOrderCustomerNameDTO request)
    {
        var order = await _context.Orders.FindAsync((short)orderId);

        if (order == null)
            throw new KeyNotFoundException($"Order with ID {orderId} not found");

        order.CustomerId = request.CustomerName;
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<CustomerDropDownDomain> customers, IEnumerable<EmployeeDropDownDomain> employees,
        IEnumerable<ShipperDropDownDomain> shippers)> GetOrderDropdownDataAsync()
    {
        var customers = await _context.Customers
            .Select(c => new CustomerDropDownDomain(c.CustomerId, c.CompanyName))
            .ToListAsync();

        var employees = await _context.Employees
            .Select(e => new EmployeeDropDownDomain(e.EmployeeId, e.FirstName, e.LastName))
            .ToListAsync();

        var shippers = await _context.Shippers
            .Select(s => new ShipperDropDownDomain(s.ShipperId, s.CompanyName))
            .ToListAsync();

        return (customers, employees, shippers);
    }

    public async Task<int> CreateOrderAsync(NewOrderReqDTO request)
    {
        var maxOrderId = await _context.Orders
            .MaxAsync(o => (int?)o.OrderId) ?? 0;

        var order = AddOrder.Create(
            maxOrderId + 1,
            request.CustomerID,
            request.EmployeeID,
            request.Freight
        );

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order.OrderId;
    }
}