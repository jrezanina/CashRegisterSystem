using Microsoft.EntityFrameworkCore;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace PokladniSystem.Application.Implementation
{
    public class CategoryService : ICategoryService
    {
        CRSDbContext _dbContext;
        public CategoryService(CRSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IList<Category> GetCategories()
        {
            return _dbContext.Categories.ToList();
        }

        public void Create(Category category)
        {
            if (_dbContext.Categories != null)
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
            }
        }

        public bool Delete(int id)
        {
            bool deleted = false;
            Category? categoryItem = _dbContext.Categories.FirstOrDefault(c => c.Id == id);

            if (categoryItem != null)
            {
                var productCategories = _dbContext.ProductCategories.Where(pc => pc.CategoryId == id);
                if (productCategories != null)
                {
                    _dbContext.ProductCategories.RemoveRange(productCategories);
                    _dbContext.SaveChanges();
                }
                _dbContext.Categories.Remove(categoryItem);
                _dbContext.SaveChanges();
                deleted = true;
            }
            return deleted;
        }

        public void Edit(Category category)
        {
            Category? categoryItem = _dbContext.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (categoryItem != null)
            {
                categoryItem.Name = category.Name;
                _dbContext.SaveChanges();
            }
        }
    }
}
