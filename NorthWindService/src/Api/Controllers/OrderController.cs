using Microsoft.AspNetCore.Mvc;
using NorthWindService.Application.DTOs;
using NorthWindService.Application.Services;

namespace NorthWindService.Api.Controllers;

public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("QueryBasicOrder")]
    public async Task<ActionResult<List<QueryBasicOrderDTO>>> QueryBasicOrder()
    {
        try
        {
            var result = await _orderService.QueryBasicOrder();
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("UpdateCustomerName/{orderId}")]
    public async Task<ActionResult> UpdateOrderCustomerName(int orderId, [FromBody] UpdateOrderCustomerNameDTO request)
    {
        try
        {
            await _orderService.UpdateOrderCustomerNameAsync(orderId, request);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            // 回傳 400 Bad Request，並帶上錯誤訊息
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// 取得新增訂單所需的下拉選單資料
    /// </summary>
    /// <returns>顧客、員工、貨運商的下拉選單資料</returns>
    /// <response code="200">成功取得下拉選單資料</response>
    [HttpGet("QueryOrderAddDropDownLists")] // 改成跟 Service 方法名稱一致
    [ProducesResponseType(typeof(OrderDropdownListsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrderDropdownListsResponse>> GetOrderDropdownData()
    {
        var response = await _orderService.GetOrderDropdownDataAsync();
        return Ok(response);
    }


    /// <summary>
    /// 新增訂單
    /// </summary>
    /// <param name="request">訂單資料</param>
    /// <returns>新建立的訂單編號</returns>
    /// <response code="201">訂單建立成功</response>
    /// <response code="400">驗證失敗</response>
    /// <response code="500">系統錯誤</response>
    [HttpPost("CreateOrder")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateOrder([FromBody] NewOrderReqDTO request)
    {
        try
        {
            // 基本驗證
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderId = await _orderService.CreateOrderAsync(request);

            return StatusCode(StatusCodes.Status201Created, orderId);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "內部伺服器錯誤" });
        }
    }
}