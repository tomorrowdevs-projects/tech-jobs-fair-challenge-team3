using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RubricaTelefonicaAziendale.Dtos;
using RubricaTelefonicaAziendale.Entities;
using RubricaTelefonicaAziendale.Services;

namespace RubricaTelefonicaAziendale.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Route("web/people")]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService service;
        public PeopleController(IPeopleService IService)
        {
            service = IService;
        }

        [HttpPost("list")]
        [ProducesResponseType(typeof(ListDto<PeopleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ListDto<PeopleDto>>> GetList(PeopleListRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = (from item in ModelState where item.Value.Errors.Any() select item.Value.Errors[0].ErrorMessage).ToList();
                return BadRequest("Problems with received data! " + String.Join(";", [.. errors]));
            }
            ListDto<PeopleDto>? response = await service.GetListAsync();
            if (response != null) return Ok(response);
            else return Problem("Error retrieving data!");
        }


        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(PeopleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PeopleDto>> GetById(String id)
        {
            if (!ModelState.IsValid)
            {
                var errors = (from item in ModelState where item.Value.Errors.Any() select item.Value.Errors[0].ErrorMessage).ToList();
                return BadRequest("Problems with received data! " + String.Join(";", errors.ToArray()));
            }
            People? dbobj = await service.GetByID(id);
            if (dbobj != null)
            {
                PersonDto response = PersonDto.ConvertToDto(dbobj);
                return Ok(response);
            }
            else return Problem("Error retrieving data!");
        }


        [HttpPost("insert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Insert(PeopleDto obj)
        {
            if (!ModelState.IsValid)
            {
                var errors = (from item in ModelState where item.Value.Errors.Any() select item.Value.Errors[0].ErrorMessage).ToList();
                return BadRequest("Problems with received data! " + String.Join(";", errors.ToArray()));
            }
            if (String.IsNullOrEmpty(obj.Firstname) && String.IsNullOrEmpty(obj.Lastname)) return BadRequest("Invalid person name");
            People dbobj = PeopleDto.ConvertToEntity(obj, new People());
            bool insert = await service.Insert(dbobj);
            if (insert) return Ok("Person added successfully");
            else return Problem("Error inserting new person!");
        }

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update(PeopleDto obj)
        {
            if (!ModelState.IsValid)
            {
                var errors = (from item in ModelState where item.Value.Errors.Any() select item.Value.Errors[0].ErrorMessage).ToList();
                return BadRequest("Problems with received data! " + String.Join(";", errors.ToArray()));
            }
            People? dbobj = await service.GetByID(obj.Id);
            dbobj = PeopleDto.ConvertToEntity(obj, dbobj ?? new People());
            if (dbobj != null)
            {
                bool update = await service.Update(dbobj);
                if (update) return Ok("Person updated successfully");
                else return Problem("Error updating selected item!");
            }
            else return Problem("Error retrieving data!");
        }

        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> SafeDelete([FromForm] String id)
        {
            if (!ModelState.IsValid)
            {
                var errors = (from item in ModelState where item.Value.Errors.Any() select item.Value.Errors[0].ErrorMessage).ToList();
                return BadRequest("Problems with received data! " + String.Join(";", errors.ToArray()));
            }
            bool delete = await service.Delete(id);
            if (delete)
                return Ok("Assistance contract deleted successfully");
            else
                return Problem("Error deleting selected item!");
        }

    }
}