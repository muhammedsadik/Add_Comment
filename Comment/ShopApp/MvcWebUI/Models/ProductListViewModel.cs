using Entity;

namespace MvcWebUI.Models
{
  public class PageInfo
  {
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public string CurrentCategory { get; set; }

    public int TotalPages()
    {
      return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
  }
  public class ProductListViewModel
  {
    public PageInfo PageInfo { get; set; }
    public List<ProductListModel> Products { get; set; }
   
  }

  public class ProductListModel
  {
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public double? Price { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public bool IsApproved { get; set; }
    public bool IsHome { get; set; }
    public DateTime DateAdded { get; set; }
    public string? CommentCount { get; set; }
    public List<ProductCategory> ProductCategories { get; set; }
  }


}
