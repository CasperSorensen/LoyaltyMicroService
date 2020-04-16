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

namespace TodoApp.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class TodoController : Controller
  {
    private readonly ITodoRepository _repo;
    private readonly ProducerConfig _producerconfig;

    public TodoController(ITodoRepository repo, ProducerConfig producerconfig)
    {
      this._repo = repo;
      this._producerconfig = producerconfig;
    }

    // GET api/todos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> Get()
    {

      try
      {
        var producer = new ProducerWrapper(this._producerconfig, "testtopic");
        await producer.WriteMessage("Returned all todos");
      }
      catch (OperationCanceledException)
      {
        Console.WriteLine("ffs");
      }

      return new ObjectResult(await _repo.GetAllTodos());
    }

    // GET api/todos/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> Get(long id)
    {
      var todo = await _repo.GetTodo(id);

      if (todo == null)
        return new NotFoundResult();

      return new ObjectResult(todo);
    }

    // POST api/todos
    [HttpPost]
    public async Task<ActionResult<Todo>> Post([FromBody] Todo todo)
    {
      todo.Id = await _repo.GetNextId();

      // string serializedOrder = JsonConvert.SerializeObject(todo);
      // await producer.WriteMessage(serializedOrder);

      await _repo.Create(todo);
      return new OkObjectResult(todo);
    }

    // PUT api/todos/1
    [HttpPut("{id}")]
    public async Task<ActionResult<Todo>> Put(long id, [FromBody] Todo todo)
    {
      var todoFromDb = await _repo.GetTodo(id);

      if (todoFromDb == null)
        return new NotFoundResult();

      todo.Id = todoFromDb.Id;
      todo.InternalId = todoFromDb.InternalId;

      await _repo.Update(todo);

      return new OkObjectResult(todo);
    }

    // DELETE api/todos/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
      var post = await _repo.GetTodo(id);

      if (post == null)
        return new NotFoundResult();

      await _repo.Delete(id);

      return new OkResult();
    }
  }
}
