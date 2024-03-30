using Microsoft.EntityFrameworkCore;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Implementation
{
    public class ContactService : IContactService
    {
        CRSDbContext _dbContext;
        public ContactService(CRSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Create(Contact contact)
        {
            if (_dbContext.Contacts != null)
            {
                _dbContext.Contacts.Add(contact);
                _dbContext.SaveChanges();
                return contact.Id;
            }
            
            return 0;
        }
        public bool Edit(Contact contact)
        {
            Contact? contactItem = _dbContext.Contacts.FirstOrDefault(c => c.Id == contact.Id);
            if (contactItem != null)
            {
                contactItem.Phone = contact.Phone;
                contactItem.Email = contact.Email;
                contactItem.Web = contact.Web;
                contactItem.City = contact.City;
                contactItem.PostalCode = contact.PostalCode;
                contactItem.Street = contact.Street;
                contactItem.BuildingNumber = contact.BuildingNumber;
                _dbContext.SaveChanges();

                return true;
            }

            return false;
        }

    }
}
