using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EfCore;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
  public class ProductManager : IProductService
  {
    private readonly IUnitOfWork _unitOfWork;

    public ProductManager(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public bool Create(Product entity)
    {
      if (Validation(entity))
      {
        _unitOfWork.Products.Create(entity);
        _unitOfWork.Save();
        return true;
      }

      return false;
    }

    public void Delete(Product entity)
    {
      // iş kuralları

      _unitOfWork.Products.Delete(entity);
      _unitOfWork.Save();

    }

    public List<Product> GetAll()
    {
      return _unitOfWork.Products.GetAll();

    }

    public Product GetById(int id)
    {
      return _unitOfWork.Products.GetById(id);
    }

    public Product GetByIdWithCategories(int id)
    {
      return _unitOfWork.Products.GetByIdWithCategories(id);
    }

    public int GetCountByCategory(string category)
    {
      return _unitOfWork.Products.GetCountByCategory(category);
    }

    public List<Product> GetHomePageProducts()
    {


      return _unitOfWork.Products.GetHomePageProducts();
    }

    public Product GetProductDetails(string url)
    {
      return _unitOfWork.Products.GetProductDetails(url);
    }

    public List<Product> GetProductsByCategory(string name, int page, int pageSize)
    {
      return _unitOfWork.Products.GetProductsByCategory(name, page, pageSize);
    }

    public List<Product> GetSearchResult(string searchString)
    {
      return _unitOfWork.Products.GetSearchResult(searchString);
    }

    public void Update(Product entity)
    {
      _unitOfWork.Products.Update(entity);
      _unitOfWork.Save();
    }

    public bool Update(Product entity, int[] categoryIds)
    {
      if (Validation(entity))
      {
        if(categoryIds.Length == 0)
        {
          ErrorMessage += "you must choose a category";
          return false;
        }

        _unitOfWork.Products.Update(entity, categoryIds);
        _unitOfWork.Save();
        return true;
      }

      return false;
    }

    public string ErrorMessage { get; set; }

    public bool Validation(Product entity)
    {
      var isValid = true;
      if (string.IsNullOrEmpty(entity.Name))
      {
        ErrorMessage += "Product name is required.\n";
        isValid = false;
      }
      
      if (string.IsNullOrEmpty(entity.Url))
      {
        ErrorMessage += "Product Url is required.\n";
        isValid = false;
      }

      if (string.IsNullOrEmpty(entity.Description))
      {
        ErrorMessage += "Description is required.\n";
        isValid = false;
      }

      if (entity.Price <= 0 || entity.Price==null)
      {
        ErrorMessage += "Price cannot be negative or 0 (zero)\n";
        isValid = false;
      }

      return isValid;
    }

    
  }
}
