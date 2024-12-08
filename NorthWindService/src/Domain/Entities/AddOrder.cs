using NorthWindService.src.Infrastructure.Persistence.Entities;

namespace NorthWindService.Domain.Entities;

public class AddOrder
{
    public static Order Create(
        int orderId,
        string customerId,
        string employeeId,
        float freight)
    {
        return new Order
        {
            OrderId = (short)orderId,
            CustomerId = customerId,
            EmployeeId = Convert.ToInt16(employeeId),
            Freight = freight
        };
    }

    public int OrderId { get; private set; }
    public string CustomerId { get; private set; }
    public string EmployeeId { get; private set; }
    public decimal? Freight { get; private set; }
}