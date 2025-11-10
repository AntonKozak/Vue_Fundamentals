using System.Net;
using Mango_Api.Data;
using Mango_Api.Models;
using Mango_Api.Models.Dto;
using Mango_Api.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ApiResponse _response;

    public OrderController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _response = new ApiResponse();
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetOrders(string userId = "")
    {
        try
        {
            IEnumerable<OrderHeader> orderHeadersList = await _applicationDbContext.OrderHeaders
            .Include(u => u.OrderDetails)
            .ThenInclude(u => u.MenuItem)
            .OrderByDescending(u => u.OrderHeaderId)
            .ToListAsync();

            if (!string.IsNullOrEmpty(userId))
            {
                orderHeadersList = orderHeadersList.Where(u => u.ApplicationUserId == userId);
            }

            _response.Result = orderHeadersList;
            return Ok(_response);

        }
        catch (System.Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages
                = new List<string>() { ex.ToString() };
            return BadRequest(_response);
        }
    }

    [HttpGet("{orderId:int}")]
    public async Task<ActionResult<ApiResponse>> GetOrder(int orderId)
    {
        try
        {
            OrderHeader? orderHeader = await _applicationDbContext.OrderHeaders
            .Include(u => u.OrderDetails)
            .ThenInclude(u => u.MenuItem)
            .FirstOrDefaultAsync(u => u.OrderHeaderId == orderId);

            if (orderHeader == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { "Order not found." };
                return NotFound(_response);
            }

            _response.Result = orderHeader;
            return Ok(_response);
        }
        catch (System.Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages
                = new List<string>() { ex.ToString() };
            return BadRequest(_response);
        }
    }


    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDto orderHeaderCreateDto)
    {
        try
        {
            if (ModelState.IsValid)
            {

                OrderHeader orderHeader = new()
                {
                    PickUpName = orderHeaderCreateDto.PickUpName,
                    PickUpEmail = orderHeaderCreateDto.PickUpEmail,
                    PickUpPhoneNumber = orderHeaderCreateDto.PickUpPhoneNumber,
                    OrderDate = DateTime.UtcNow,
                    OrderTotal = orderHeaderCreateDto.OrderTotal,
                    Status = StaticDetails.status_confirmed,
                    TotalItem = orderHeaderCreateDto.TotalItem,
                    ApplicationUserId = orderHeaderCreateDto.ApplicationUserId,
                };

                _applicationDbContext.OrderHeaders.Add(orderHeader);
                await _applicationDbContext.SaveChangesAsync();

                foreach (var orderDetailsDto in orderHeaderCreateDto.OrderDetails)
                {
                    OrderDetail orderDetail = new()
                    {
                        OrderHeaderId = orderHeader.OrderHeaderId,
                        MenuItemId = orderDetailsDto.MenuItemId,
                        Quantity = orderDetailsDto.Quantity,
                        ItemName = orderDetailsDto.ItemName,
                        Price = orderDetailsDto.Price,
                    };
                    _applicationDbContext.OrderDetails.Add(orderDetail);
                }
                await _applicationDbContext.SaveChangesAsync();
                _response.Result = orderHeader;
                orderHeader.OrderDetails = [];
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtAction("GetOrder", new
                {
                    orderId = orderHeader.OrderHeaderId
                }
                , _response); ;
            }
            else
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();
                return BadRequest(_response);
            }


        }
        catch (System.Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages
                = new List<string>() { ex.ToString() };
            return BadRequest(_response);
        }
    }

    [HttpPut("{orderId:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateOrder(int orderId, [FromBody] OrderHeaderUpdateDto orderHeaderUpdateDto)
    {
        try
        {
            OrderHeader? orderHeader = await _applicationDbContext.OrderHeaders
                .FirstOrDefaultAsync(u => u.OrderHeaderId == orderId);

            if (orderHeader == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages = new List<string> { "Order not found." };
                return NotFound(_response);
            }

            if (!string.IsNullOrWhiteSpace(orderHeaderUpdateDto.PickUpName))
            {
                orderHeader.PickUpName = orderHeaderUpdateDto.PickUpName;
            }

            if (!string.IsNullOrWhiteSpace(orderHeaderUpdateDto.PickUpEmail))
            {
                if (!IsValidEmail(orderHeaderUpdateDto.PickUpEmail))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Invalid email format." };
                    return BadRequest(_response);
                }
                orderHeader.PickUpEmail = orderHeaderUpdateDto.PickUpEmail;
            }

            if (!string.IsNullOrWhiteSpace(orderHeaderUpdateDto.PickUpPhoneNumber))
            {
                orderHeader.PickUpPhoneNumber = orderHeaderUpdateDto.PickUpPhoneNumber;
            }

            if (!string.IsNullOrEmpty(orderHeaderUpdateDto.Status))
            {
                var statusUpdateResult = UpdateOrderStatus(orderHeader, orderHeaderUpdateDto.Status);
                if (!statusUpdateResult.IsValid)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { statusUpdateResult.ErrorMessage };
                    return BadRequest(_response);
                }
            }

            await _applicationDbContext.SaveChangesAsync();

            _response.Result = orderHeader;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages = new List<string> { ex.Message };
            return StatusCode(500, _response);
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private (bool IsValid, string ErrorMessage) UpdateOrderStatus(OrderHeader orderHeader, string newStatus)
    {
        var currentStatus = orderHeader.Status.ToLower();
        var requestedStatus = newStatus.ToLower();

        var allowedTransitions = new Dictionary<string, List<string>>
        {
            { StaticDetails.status_confirmed.ToLower(), new List<string>
                { StaticDetails.status_readyForPickUp.ToLower(), StaticDetails.status_cancelled.ToLower() }
            },
            { StaticDetails.status_readyForPickUp.ToLower(), new List<string>
                { StaticDetails.status_completed.ToLower(), StaticDetails.status_cancelled.ToLower() }
            }
        };

        if (allowedTransitions.ContainsKey(currentStatus))
        {
            if (allowedTransitions[currentStatus].Contains(requestedStatus))
            {
                orderHeader.Status = newStatus;
                return (true, string.Empty);
            }
        }

        if (requestedStatus == StaticDetails.status_cancelled.ToLower()
            && currentStatus != StaticDetails.status_completed.ToLower())
        {
            orderHeader.Status = StaticDetails.status_cancelled;
            return (true, string.Empty);
        }

        if (currentStatus == requestedStatus)
        {
            return (true, string.Empty);
        }

        return (false, $"Cannot change status from '{orderHeader.Status}' to '{newStatus}'");
    }
}
