using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestMongoDocker.Repositories;
using RestMongoDocker.Models;
using RestMongoDocker.Kafka;
using Newtonsoft.Json;
using Confluent.Kafka;

namespace UserApp.Controllers
{
  [Controller]
  [Route("LoyaltyService/[controller]")]
  public class UserController : Controller
  {
    private readonly IUserRepository _userrepo;
    private readonly ProducerConfig _producerConfig;

    public UserController(IUserRepository repo, ProducerConfig producerConfig)
    {
      this._userrepo = repo;
      this._producerConfig = producerConfig;
    }

    // GET /User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
      var producer = new ProducerWrapper(this._producerConfig, "testtopic");
      await producer.WriteMessage("Returned all Users");

      return new ObjectResult(await _userrepo.GetAllUsers());
    }

    // GET /User/1
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(long id)
    {
      var User = await this._userrepo.GetUser(id);
      if (User == null)
        return new NotFoundResult();

      return new ObjectResult(User);
    }

    // POST /User
    [HttpPost]
    public async Task<ActionResult<User>> Post([FromBody] User User)
    {
      User.Id = await this._userrepo.GetNextId();

      // string serializedOrder = JsonConvert.SerializeObject(User);
      // await producer.WriteMessage(serializedOrder);

      await _userrepo.Create(User);
      return new OkObjectResult(User);
    }

    // PUT /User/1
    [HttpPut("{id}")]
    public async Task<ActionResult<User>> Put(long id, [FromBody] User User)
    {
      var UserFromDb = await _userrepo.GetUser(id);

      if (UserFromDb == null)
        return new NotFoundResult();

      User.Id = UserFromDb.Id;
      User.InternalId = UserFromDb.InternalId;

      await _userrepo.Update(User);

      return new OkObjectResult(User);
    }

    // DELETE /Users/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
      var post = await _userrepo.GetUser(id);

      if (post == null)
        return new NotFoundResult();

      await _userrepo.Delete(id);

      return new OkResult();
    }

    // User Create GET
    // The get needs to return a users calculated
    // milage

  }
}
