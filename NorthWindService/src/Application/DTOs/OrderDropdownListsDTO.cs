using NorthWindService.Domain.Entities;

namespace NorthWindService.Application.DTOs;

public class OrderDropdownListsResponse // 改用 Response 後綴
{
    public List<CustomerDto> Customers { get; set; }
    public List<EmployeeDto> Employees { get; set; }
    public List<ShipperDto> Shippers { get; set; }
}

public class CustomerDto
{
    public string Id { get; set; }
    public string CompanyName { get; set; }
}

public class EmployeeDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
}

public class ShipperDto
{
    public string Id { get; set; }
    public string CompanyName { get; set; }
}