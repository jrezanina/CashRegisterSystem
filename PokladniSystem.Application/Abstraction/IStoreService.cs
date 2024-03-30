using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Abstraction
{
    public interface IStoreService
    {
        public IList<StoreViewModel> GetStoreViewModels();
        void Create(Store store);
        void Edit(Store store);

    }
}
