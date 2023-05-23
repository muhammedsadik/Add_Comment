using Business.Abstract;
using DataAccess.Abstract;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
  public class CategoryManager : ICategoryService
  {
    private readonly IUnitOfWork _unitOfWork;

    public CategoryManager(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }


    public bool Create(Category entity)
    {
      if (Validation(entity))
      {
        _unitOfWork.Categories.Create(entity);
        _unitOfWork.Save();
        return true;
      }

      return false;
    }

    public bool Update(Category entity)
    {

      if (Validation(entity))
      {
        _unitOfWork.Categories.Update(entity);
        _unitOfWork.Save();
        return true;
      }

      return false;
    }

    public void Delete(Category entity)
    {
      _unitOfWork.Categories.Delete(entity);
      _unitOfWork.Save();
    }

    public void DeleteFromCategory(int productId, int categoryId)
    {
      _unitOfWork.Categories.DeleteFromCategory(productId, categoryId);
    }

    public List<Category> GetAll()
    {
      return _unitOfWork.Categories.GetAll();
    }

    public Category GetById(int id)
    {
      return _unitOfWork.Categories.GetById(id);
    }

    public Category GetByIdWithProducts(int CategoryId)
    {
      return _unitOfWork.Categories.GetByIdWithProducts(CategoryId);
    }

    public string ErrorMessage { get; set; }

    public bool Validation(Category entity)
    {
      var isValid = true;
      if (string.IsNullOrEmpty(entity.Name))
      {
        ErrorMessage += "Category name is required.\n";
        isValid = false;
      }

      if (string.IsNullOrEmpty(entity.Url))
      {
        ErrorMessage += "Category Url is required.\n";
        isValid = false;
      }

      return isValid;
    }
  }
}
