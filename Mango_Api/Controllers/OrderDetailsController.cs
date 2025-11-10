using Mango_Api.Data;
using Mango_Api.Models;
using Mango_Api.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderDetailsController : ControllerBase
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ApiResponse _response;

    public OrderDetailsController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _response = new ApiResponse();
    }

    [HttpPut("{orderDetailsId:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateOrderDetails(int orderDetailsId, [FromBody] OrderDetailsUpdateDto orderDetailsUpdateDto)
    {
        try
        {
            OrderDetail? orderDetails = await _applicationDbContext.OrderDetails
                .FirstOrDefaultAsync(u => u.OrderDetailId == orderDetailsId);

            if (orderDetails == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "Order Details not found." };
                return NotFound(_response);
            }

            // Update Rating if provided and valid
            if (orderDetailsUpdateDto.Rating > 0)
            {
                if (orderDetailsUpdateDto.Rating < 1 || orderDetailsUpdateDto.Rating > 5)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Rating must be between 1 and 5." };
                    return BadRequest(_response);
                }
                orderDetails.Rating = orderDetailsUpdateDto.Rating;
            }

            await _applicationDbContext.SaveChangesAsync();

            _response.Result = orderDetails;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            return BadRequest(_response);
        }
    }

}
