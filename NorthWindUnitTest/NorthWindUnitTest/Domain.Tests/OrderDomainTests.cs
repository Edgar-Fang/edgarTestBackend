namespace NorthWindUnitTest.Domain
{
    public class OrderDomainTests
    {
        /// <summary>
        /// 基本建構測試
        /// </summary>
        [Fact]
        public void Create_Should_Return_Valid_OrderDomain()
        {
            // Arrange
            var orderId = 1;
            var orderDate = new DateTime(2024, 1, 1);
            var customerName = "Test Customer";
            var freight = 100.00f;
            var shippedDate = new DateTime(2024, 1, 5);

            // Act
            var order = OrderDomain.Create(
                orderId,
                orderDate,
                customerName,
                freight,
                shippedDate
            );

            // Assert
            Assert.NotNull(order);
            Assert.Equal(orderId, order.OrderId);
            Assert.Equal(orderDate, order.OrderDate);
            Assert.Equal(customerName, order.CustomerName);
            Assert.Equal(freight, order.Freight);
            Assert.Equal(shippedDate, order.ShippedDate);
        }

        /// <summary>
        /// 金額格式化測試
        /// </summary>
        [Fact]
        public void GetFormattedFreight_Should_Return_Correct_Format()
        {
            // Arrange
            var order = OrderDomain.Create(1, null, "Test", 1234.56f, null);

            // Act
            var result = order.GetFormattedFreight();

            // Assert
            Assert.Equal("$1,234.56", result);
        }

        /// <summary>
        /// 日期格式化測試
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData(2024, 1, 1, "2024/01/01")] // 使用年、月、日作為參數
        [InlineData(null, null, null, "未設定日期")]
        public void GetFormattedOrderDate_Should_Return_Correct_Format(int? year, int? month, int? day, string expected)
        {
            // Arrange
            DateTime? orderDate = null;
            if (year.HasValue && month.HasValue && day.HasValue)
            {
                orderDate = new DateTime(year.Value, month.Value, day.Value);
            }

            var order = OrderDomain.Create(1, orderDate, "Test", 100f, null);

            // Act
            var result = order.GetFormattedOrderDate();

            // Assert
            Assert.Equal(expected, result);
        }
        
        
        /// <summary>
        /// 出貨狀態測試
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData(2024, 1, 1, "已出貨")] // 有出貨日期
        [InlineData(null, null, null, "處理中")] // 無出貨日期
        public void GetShipStatus_Should_Return_Correct_Status(int? year, int? month, int? day, string expected)
        {
            // Arrange
            DateTime? shippedDate = null;
            if (year.HasValue && month.HasValue && day.HasValue)
            {
                shippedDate = new DateTime(year.Value, month.Value, day.Value);
            }

            var order = OrderDomain.Create(
                orderId: 1,
                orderDate: null,
                customerName: "Test",
                freight: 100f,
                shippedDate: shippedDate
            );

            // Act
            var result = order.GetShipStatus();

            // Assert
            Assert.Equal(expected, result);
        }

        
        
        /// <summary>
        /// 空值處理測試
        /// </summary>
        [Fact]
        public void Create_Should_Handle_All_Null_Values()
        {
            // Arrange & Act
            var order = OrderDomain.Create(1, null, "Test", 100f, null);

            // Assert
            Assert.Null(order.OrderDate);
            Assert.Null(order.ShippedDate);
            Assert.Equal("未設定日期", order.GetFormattedOrderDate());
            Assert.Equal("處理中", order.GetShipStatus());
        }
    }
}