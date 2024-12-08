using AutoFixture;
using Moq;
using NorthWindService.Application.Services;
using NorthWindService.Infrastructure.Repositories;
using NorthWindService.src.Infrastructure.Persistence.Entities;

namespace NorthwindServices.tests.Application.Tests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockRepository;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockRepository = new Mock<IOrderRepository>();
        _orderService = new OrderService(_mockRepository.Object);  
    }
    
    /// <summary>
    /// 正常流程測試
    /// </summary>
    [Fact]
    public async Task QueryBasicOrder_Should_Return_Correct_DTOs()
    {
        // Arrange
        var testOrders = new List<OrderDomain>
        {
            OrderDomain.Create(
                1,
                new DateTime(2024, 1, 1),
                "Test Customer",
                100.00f,
                new DateTime(2024, 1, 5)
            )
        };

        _mockRepository
            .Setup(repo => repo.QueryBasicOrderAsync())
            .ReturnsAsync(testOrders);

        // Act
        var result = await _orderService.QueryBasicOrder();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var dto = result.First();
        Assert.Equal(1, dto.OrderId);
        Assert.Equal("Test Customer", dto.CustomerName);
        Assert.Equal("NT$100.00", dto.FormattedFreight);
        Assert.Equal("已出貨", dto.ShipStatus);
    }

    /// <summary>
    /// 空資料測試
    /// </summary>
    [Fact]
    public async Task QueryBasicOrder_When_Empty_Should_Return_Empty_List()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.QueryBasicOrderAsync())
            .ReturnsAsync(new List<OrderDomain>());

        // Act
        var result = await _orderService.QueryBasicOrder();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    /// <summary>
    /// Null 日期處理測試
    /// </summary>
    [Fact]
    public async Task QueryBasicOrder_Should_Map_Null_Dates_Correctly()
    {
        // Arrange
        var testOrders = new List<OrderDomain>
        {
            OrderDomain.Create(
                1,
                null, // 測試 null 日期
                "Test Customer",
                100.00f,
                null
            )
        };

        _mockRepository
            .Setup(repo => repo.QueryBasicOrderAsync())
            .ReturnsAsync(testOrders);

        // Act
        var result = await _orderService.QueryBasicOrder();

        // Assert
        Assert.NotNull(result);
        var dto = result.First();
        Assert.Equal("未設定日期", dto.FormattedOrderDate);
        Assert.Equal("處理中", dto.ShipStatus);
    }

    /// <summary>
    /// 異常處理測試
    /// </summary>
    [Fact]
    public async Task QueryBasicOrder_Should_Handle_Repository_Exception()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.QueryBasicOrderAsync())
            .ThrowsAsync(new Exception("DB Error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            _orderService.QueryBasicOrder());
    }
}