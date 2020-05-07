using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestMongoDocker.Models;


namespace RestMongoDocker.Repositories
{
  public interface IUserRepository
  {
    // api/[GET]
    public Task<IEnumerable<User>> GetAllUsers();

    // api/where/{id}/equals/{id} /[GET]
    public Task<User> GetUser(long id);

    // api/[POST]
    public Task Create(User User);

    // api/[PUT]
    public Task<bool> Update(User User);

    // api[DELETE]
    public Task<bool> Delete(long id);

    public Task<long> GetNextId();
  }

}