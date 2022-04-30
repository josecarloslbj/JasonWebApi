using Jason.WebApi.Entities;
using Jason.WebApi.Infra.Interfaces;

namespace Jason.WebApi.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(string connectionString)
       : base(connectionString)
        {

        }
    }
}
