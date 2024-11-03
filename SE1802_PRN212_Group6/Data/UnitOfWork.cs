using SE1802_PRN212_Group6.Repositories;
#pragma warning disable

namespace SE1802_PRN212_Group6.Data
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingRepository BookingRepository { get; }
        public CategoryRepository CategoryRepository { get; }
        public EmployeeRepository EmployeeRepository { get; }
        public OrderDetailRepository OrderDetailRepository { get; }
        public OrderRepository OrderRepository { get; }
        public ProductRepository ProductRepository { get; }
        public TableRepository TableRepository { get; }
        public UserRepository UserRepository { get; }
        public VoucherRepository VoucherRepository { get; }

        public UnitOfWork()
        {
            _dbContext = new ApplicationDbContext();

            BookingRepository = new(_dbContext);
            CategoryRepository = new(_dbContext);
            EmployeeRepository = new(_dbContext);
            OrderDetailRepository = new(_dbContext);
            OrderRepository = new(_dbContext);
            ProductRepository = new(_dbContext);
            TableRepository = new(_dbContext);
            UserRepository = new(_dbContext);
            VoucherRepository = new(_dbContext);
        }

        public void SaveChanges() => _dbContext.SaveChanges();
    }
}
