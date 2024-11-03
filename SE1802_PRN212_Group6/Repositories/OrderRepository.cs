using SE1802_PRN212_Group6.Data;
using SE1802_PRN212_Group6.Models;

namespace SE1802_PRN212_Group6.Repositories
{
    public class OrderRepository(ApplicationDbContext _dbContext) : GenericRepository<Order>(_dbContext)
    {
    }
}
