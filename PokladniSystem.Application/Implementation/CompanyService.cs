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
    public class CompanyService : ICompanyService
    {
        CRSDbContext _dbContext;
        public CompanyService(CRSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public CompanyViewModel GetCompanyViewModel()
        {
            Company company = _dbContext.Company.FirstOrDefault();
            Contact contact = _dbContext.Contacts.FirstOrDefault(c => c.Id == company.ContactId);
            CompanyViewModel viewModel = new CompanyViewModel() { Company = company, Contact = contact};

            return viewModel;
        }

        public void Edit(Company company)
        {
            Company? companyItem = _dbContext.Company.FirstOrDefault(c => c.Id == company.Id);
            if (companyItem != null)
            {
                companyItem.Name = company.Name;
                companyItem.ICO = company.ICO;
                companyItem.DIC = company.DIC;
                _dbContext.SaveChanges();
            }
        }
    }
}
