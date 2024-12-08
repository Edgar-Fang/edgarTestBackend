namespace NorthWindService.Application.DTOs;

public class NewOrderReqDTO
{
   public string CustomerID { get; set; }
   public string EmployeeID { get; set; }
   public float Freight { get; set; }
}