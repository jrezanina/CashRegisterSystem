using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Abstraction
{
    public interface ICategoryService
    {
        IList<Category> GetCategories();
        void Create(Category category);
        bool Delete(int id);
        void Edit(Category category);
    }
}
