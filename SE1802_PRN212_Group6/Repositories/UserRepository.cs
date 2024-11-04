using SE1802_PRN212_Group6.Data;
using SE1802_PRN212_Group6.Models;

namespace SE1802_PRN212_Group6.Repositories
{
    public class UserRepository(ApplicationDbContext _dbContext) : GenericRepository<User>(_dbContext)
    {
        public User? GetByEmailAndPassword(string email, string password)
        {
            return _dbContext.User.FirstOrDefault(x => x.Email == email && x.Password == password);
        }

        public bool CheckEmailExisted(string email)
        {
            return _dbContext.User.Any(x => x.Email == email);
        }
    }
}
