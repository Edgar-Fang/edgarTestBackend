using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NorthWindService.Application.Services;
using NorthWindService.Infrastructure.Repositories;
using NorthWindService.src.Infrastructure.Persistence.Context;
using NorthWindService.src.Infrastructure.Persistence.Entities;
using Xunit;

namespace NorthWindUnitTest.Infrastructure.Tests
{
    public class OrderRepositoryTests
    {
        private readonly NorthwindContext _context;
        private readonly IOrderRepository _orderRepository;


        public OrderRepositoryTests()
        {
            // 設置 in-memory database
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .UseInternalServiceProvider(new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider())
                .Options;
            _context = new NorthwindContext(options);
            _orderRepository = new OrderRepository(_context);
        }

        /// <summary>
        /// 空資料測試
        /// </summary>
        [Fact]
        public async Task QueryBasicOrderAsync_WithNoData_ShouldReturnEmptyList()
        {
            // Arrange
            // 不加入任何資料

            // Act
            var result = await _orderRepository.QueryBasicOrderAsync();

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// 大量資料效能測試 
        /// </summary>
        [Fact]
        public async Task QueryBasicOrderAsync_ShouldExecuteEfficientlyWithLargeDataSet()
        {
            // Arrange
            var testOrders = Enumerable.Range(1, 10000)
                .Select(i => new Order
                {
                    OrderId = (short)i,
                    CustomerId = $"CUST{i}",
                    OrderDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-i)),
                    Freight = i * 10,
                    ShippedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-i + 5)),
                })
                .ToList();

            await _context.Orders.AddRangeAsync(testOrders);
            await _context.SaveChangesAsync();

            // Act
            var sw = Stopwatch.StartNew();
            var result = await _orderRepository.QueryBasicOrderAsync();
            sw.Stop();

            // Assert
            Assert.True(sw.ElapsedMilliseconds < 1000); // 執行時間應在1秒內
            Assert.Equal(10000, result.Count);
        }

        /// <summary>
        /// 異常處理測試 
        /// </summary>
        [Fact]
        public async Task QueryBasicOrderAsync_Should_Handle_DbException()
        {
            // Arrange
            var mockContext = new Mock<NorthwindContext>();
            mockContext.Setup(c => c.Orders)
                .Throws(new DbUpdateException("Database error"));

            var repository = new OrderRepository(mockContext.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() =>
                repository.QueryBasicOrderAsync());
        }
    }
}