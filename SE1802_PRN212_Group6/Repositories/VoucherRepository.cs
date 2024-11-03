using SE1802_PRN212_Group6.Data;
using SE1802_PRN212_Group6.Models;

namespace SE1802_PRN212_Group6.Repositories
{
    public class VoucherRepository(ApplicationDbContext _dbContext) : GenericRepository<Voucher>(_dbContext)
    {
    }
}
