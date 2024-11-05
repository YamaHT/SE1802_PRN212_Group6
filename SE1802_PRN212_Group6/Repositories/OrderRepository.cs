using SE1802_PRN212_Group6.Data;
using SE1802_PRN212_Group6.Models;

namespace SE1802_PRN212_Group6.Repositories
{
    public class OrderRepository(ApplicationDbContext _dbContext) : GenericRepository<Order>(_dbContext)
    {
        public List<Order> GetAllOrderedByUserId(int userId)
        {
            string[] includes = ["User", "Voucher", "OrderDetails", "OrderDetails.Product", "OrderDetails.Product.Category"];
            return GetAll(includes).Where(x => x.User!.Id == userId && x.OrderDate != null).ToList();
        }
    }
}
