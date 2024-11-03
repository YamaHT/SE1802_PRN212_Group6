using SE1802_PRN212_Group6.Data;
using SE1802_PRN212_Group6.Models;

namespace SE1802_PRN212_Group6.Repositories
{
    public class EmployeeRepository(ApplicationDbContext _dbContext) : GenericRepository<Employee>(_dbContext)
    {
    }
}
