using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ToDoList.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Determines whether the password combination is valid for the user

        [HttpGet("{id}/{password}")]
        public async Task<ActionResult<IdentityUser>> Get(string id, string password)
        {
            var user = await _userManager.FindByNameAsync(id);
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
