using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NorthWindService.Api.Controllers;
using NorthWindService.Application.DTOs;
using NorthWindService.Application.Services;

namespace NorthWindUnitTest.APIContoller.Tests;

public class OrdersControllerTests
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly OrderController _controller;
    private readonly IFixture _fixture;

    public OrdersControllerTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _controller = new OrderController(_orderServiceMock.Object);
        _fixture = new Fixture();
    }

    /// <summary>
    /// 查詢成功測試
    /// </summary>
    [Fact]
    public async Task GetOrderList_ShouldReturnOkResult()
    {
        // Arrange
        var orderDtos = _fixture.CreateMany<QueryBasicOrderDTO>(3).ToList();
        _orderServiceMock.Setup(x => x.QueryBasicOrder())
            .ReturnsAsync(orderDtos);

        // Act
        var result = await _controller.QueryBasicOrder();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var returnValue = okResult.Value as IEnumerable<QueryBasicOrderDTO>;
        returnValue.Should().HaveCount(3);
    }

    /// <summary>
    /// 查詢異常測試
    /// </summary>
    [Fact]
    public async Task GetOrderList_WhenExceptionOccurs_ShouldReturn500()
    {
        // Arrange
        _orderServiceMock.Setup(x => x.QueryBasicOrder())
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.QueryBasicOrder();

        // Assert
        var statusCodeResult = result.Result as ObjectResult;
        statusCodeResult.StatusCode.Should().Be(500);
    }

    /// <summary>
    /// 更新成功測試
    /// </summary>
    [Fact]
    public async Task UpdateOrderCustomerName_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var orderId = 1;
        var request = new UpdateOrderCustomerNameDTO
        {
            CustomerName = "NEWID"
        };

        _orderServiceMock.Setup(x => x.UpdateOrderCustomerNameAsync(orderId, request))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateOrderCustomerName(orderId, request);

        // Assert
        Assert.IsType<OkResult>(result);
        _orderServiceMock.Verify(x =>
                x.UpdateOrderCustomerNameAsync(orderId, request),
            Times.Once);
    }

    /// <summary>
    /// 更新失敗測試
    /// </summary>
    [Fact]
    public async Task UpdateOrderCustomerName_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var orderId = 999222;
        var request = new UpdateOrderCustomerNameDTO
        {
            CustomerName = "NEWID"
        };

        _orderServiceMock
            .Setup(x => x.UpdateOrderCustomerNameAsync(orderId, request))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.UpdateOrderCustomerName(orderId, request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}