using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestMongoDocker.Models;


namespace RestMongoDocker.Repositories
{
  public interface ITodoRepository
  {
    // api/[GET]
    public Task<IEnumerable<Todo>> GetAllTodos();

    // api/where/{id}/equals/{id} /[GET]
    public Task<Todo> GetTodo(long id);

    // api/[POST]
    public Task Create(Todo todo);

    // api/[PUT]
    public Task<bool> Update(Todo todo);

    // api[DELETE]
    public Task<bool> Delete(long id);

    public Task<long> GetNextId();
  }

}