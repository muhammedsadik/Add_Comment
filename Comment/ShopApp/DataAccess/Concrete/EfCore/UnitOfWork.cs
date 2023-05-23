using DataAccess.Abstract;


namespace DataAccess.Concrete.EfCore
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly ShopContext _context;

    public UnitOfWork(ShopContext context)
    {
      _context = context;
    }

    private EfCoreCartRepository _cartRepository;
    private EfCoreCategoryRepository _categoryRepository;
    private EfCoreOrderRepository _orderRepository;
    private EfCoreProductRepository _productRepository;
    private EfCoreCommentRepository _commentRepository;

    public ICartRepository Carts => 
      _cartRepository = _cartRepository?? new EfCoreCartRepository(_context);

    public ICategoryRepository Categories => 
      _categoryRepository = _categoryRepository?? new EfCoreCategoryRepository(_context);

    public IOrderRepository Orders => 
      _orderRepository = _orderRepository?? new EfCoreOrderRepository(_context);

    public IProductRepository Products => 
      _productRepository = _productRepository?? new EfCoreProductRepository(_context);

    public ICommentRepository Comments => 
      _commentRepository = _commentRepository ?? new EfCoreCommentRepository(_context);

    public void Dispose()
    {
      _context.Dispose();
    }

    public void Save()
    {
      _context.SaveChanges();
    }
  }
}
