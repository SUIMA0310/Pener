using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pener.Services.User;
using Raven.Client.Documents.Session;

namespace Pener.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAsyncDocumentSession _dbSession;

        public ValuesController(
            UserManager<User> userManager,
            IAsyncDocumentSession dbSession)
        {
            _userManager = userManager;
            _dbSession = dbSession;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var user = new User()
            {
                UserName = "Admin"
            };
            await _userManager.CreateAsync(user);
            await _userManager.AddPasswordAsync(user, "!123456abcDEF");
            await _dbSession.SaveChangesAsync();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
