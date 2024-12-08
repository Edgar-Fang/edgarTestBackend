namespace NorthWindService.Application.DTOs;

public class QueryBasicOrderDTO
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? CustomerName { get; set; }
    public float Freight { get; set; }
    public DateTime? ShippedDate { get; set; }
    
    public string FormattedFreight { get; set; } 
    public string FormattedOrderDate{ get; set; } 
    public string ShipStatus { get; set; } 
}