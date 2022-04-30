using Jason.WebApi.Entities;
using Jason.WebApi.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jason.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var result = await _repository.AddAsync(user);
            return Ok(result);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            if (user.Id == 0) return BadRequest("Informe o Id");
            var result = await _repository.Update(user);
            return Ok(result);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var result = await _repository.GetAsync(id);
            return Ok(result);
        }


        [HttpGet("GetPaged")]
        public async Task<IActionResult> GetListPaged(int pageNumber, int rowPerPages, string conditions, string orderby)
        {
            var list = await _repository.GetListPaged(pageNumber, rowPerPages, conditions, orderby);
            return Ok(list);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}
