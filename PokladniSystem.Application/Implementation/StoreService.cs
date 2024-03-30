using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.ViewModels;
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
    public class StoreService : IStoreService
    {
        CRSDbContext _dbContext;
        public StoreService(CRSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IList<StoreViewModel> GetStoreViewModels()
        {
            IList<StoreViewModel> viewModels = new List<StoreViewModel>();
            IList<Store> stores = _dbContext.Stores.ToList();
            
            foreach(Store store in stores)
            {
                viewModels.Add(new StoreViewModel() 
                {
                    Store = store, 
                    Contact = _dbContext.Contacts.FirstOrDefault(c => c.Id == store.ContactId) 
                });
            }

            return viewModels;
        }

        public void Create(Store store)
        {
            if (_dbContext.Stores != null)
            {
                _dbContext.Stores.Add(store);
                _dbContext.SaveChanges();
            }
        }

        public void Edit(Store store)
        {
            Store? storeItem = _dbContext.Stores.FirstOrDefault(s => s.Id == store.Id);
            if (storeItem != null)
            {
                storeItem.Name = store.Name;
                _dbContext.SaveChanges();
            }
        }
    }
}
