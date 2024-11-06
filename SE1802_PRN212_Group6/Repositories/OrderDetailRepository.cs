using Microsoft.EntityFrameworkCore;
using SE1802_PRN212_Group6.Data;
using SE1802_PRN212_Group6.Models;

namespace SE1802_PRN212_Group6.Repositories
{
    public class OrderDetailRepository(ApplicationDbContext _dbContext) : GenericRepository<OrderDetail>(_dbContext)
    {
        public OrderDetail? GetByOrderIdAndProductId(int orderId, int productId)
        {
            return _dbContext.OrderDetail
                .Include(x => x.Product)
                .FirstOrDefault(x => x.OrderId == orderId && x.ProductId == productId);
        }
    }
}
