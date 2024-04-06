using Microsoft.AspNetCore.Mvc;
using RubricaTelefonicaAziendale.Dtos;
using RubricaTelefonicaAziendale.Entities;
using RubricaTelefonicaAziendale.Handlers;
using RubricaTelefonicaAziendale.Services;

namespace RubricaTelefonicaAziendale.Controllers
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [Route("web/ws")]
    public class WsController : ControllerBase
    {
        private readonly IPeopleService service;
        public WsController(IPeopleService IService)
        {
            service = IService;
        }

        [HttpPost("send")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ListDto<PeopleDto>>> GetList(WsRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = (from item in ModelState where item.Value.Errors.Any() select item.Value.Errors[0].ErrorMessage).ToList();
                return BadRequest("Problems with received data! " + String.Join(";", [.. errors]));
            }
            try
            {
                PeopleDto? dto = null;
                if (!String.IsNullOrEmpty(request?.PersonId))
                {
                    People? person = await service.GetByID(request.PersonId);
                    if (person != null)
                        dto = PeopleDto.ConvertToDto(person);
                }
                WsMessage msg = new()
                {
                    Message = request?.Message ?? "",
                    Content = dto
                };
                WebSocketHandler.Send(msg);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }

    public class WsRequest
    {
        public String Message { get; set; } = String.Empty;
        public String PersonId { get; set; } = String.Empty;
    }
}