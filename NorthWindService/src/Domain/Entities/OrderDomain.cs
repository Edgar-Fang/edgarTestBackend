using System.Globalization;

public class OrderDomain
{
    // 基本屬性
    public int OrderId { get; private set; }  // 考慮使用private set
    public DateTime? OrderDate { get; private set; }
    public string CustomerName { get; private set; }
    public float Freight { get; private set; }
    public DateTime? ShippedDate { get; private set; }

    // 建構函式做映射
    public static OrderDomain Create(
        int orderId, 
        DateTime? orderDate, 
        string customerName,
        float freight,
        DateTime? shippedDate)
    {
        return new OrderDomain
        {
            OrderId = orderId,
            OrderDate = orderDate,
            CustomerName = customerName,
            Freight = freight,
            ShippedDate = shippedDate
        };
    }

    public string GetFormattedFreight() => Freight.ToString("C", new CultureInfo("zh-TW"));
    public string GetFormattedOrderDate() => OrderDate?.ToString("yyyy/MM/dd") ?? "未設定日期";
    public string GetShipStatus() => ShippedDate.HasValue ? "已出貨" : "處理中";
}
