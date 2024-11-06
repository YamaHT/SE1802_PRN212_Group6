using Microsoft.EntityFrameworkCore;
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

        public Order? GetInCartByUserId(int userId)
        {
            return _dbContext.Order
                .Include(x => x.User)
                .Include(x => x.OrderDetails).ThenInclude(x => x.Product).ThenInclude(x => x.Category)
                .FirstOrDefault(x => x.User!.Id == userId && x.OrderDate == null);
        }
    }
}