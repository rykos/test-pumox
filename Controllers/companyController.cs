using Microsoft.AspNetCore.Mvc;
using pumox.Models;
using System;
using pumox.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace pumox.Controllers
{
    [Authorize]
    [ApiController]
    [Route("company")]
    public class CompanyController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public CompanyController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] Company company)
        {
            this.dbContext.Add(company);
            this.dbContext.SaveChanges();
            return Ok(new { Id = company.Id });
        }

        [HttpPost]
        [Route("search")]
        [AllowAnonymous]
        public IActionResult Search([FromBody] CompanySearch search)
        {
            List<Company> companies = this.dbContext.Companies.Include(c => c.Employees).AsEnumerable()
                .Where(c => (search.Keyword == null) ||
                    c.Name == search.Keyword || c.Employees.Any(e => e.FirstName == search.Keyword || e.LastName == search.Keyword))
                .Where(c => (search.EmployeeDateOfBirthFrom == null) ||
                    c.Employees.Exists(e => search.EmployeeDateOfBirthFrom <= e.DateOfBirth))
                .Where(c => (search.EmployeeDateOfBirthTo == null) ||
                    c.Employees.Exists(e => search.EmployeeDateOfBirthTo >= e.DateOfBirth))
                .Where(c => (search.EmployeeJobTitles == null) ||
                    c.Employees.Any(e => search.EmployeeJobTitles.Contains(e.JobTitle)))
                .ToList();

            return Ok(new
            {
                Results = companies.Select(c => new
                {
                    Name = c.Name,
                    EstablishmentYear = c.EstablishmentYear,
                    Employees = c.Employees.Select(e => new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        DateOfBirth = e.DateOfBirth,
                        JobTitle = e.JobTitle
                    })
                })
            });
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult UpdateCompany([FromBody] CompanyUpdate companyUpdate, Int64 id)
        {
            Company company = dbContext.Companies.Include(c => c.Employees).FirstOrDefault(c => c.Id == id);
            if (company == default)
            {
                return NotFound();
            }
            this.dbContext.Update(company);

            company.Name = companyUpdate.Name;
            company.EstablishmentYear = companyUpdate.EstablishmentYear;
            company.Employees = companyUpdate.Employees;

            this.dbContext.SaveChanges();
            return Ok(
                new
                {
                    Name = company.Name,
                    EstablishmentYear = company.EstablishmentYear,
                    Employees = company.Employees.Select(e => new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        DateOfBirth = e.DateOfBirth,
                        JobTitle = e.JobTitle
                    })
                }
            );
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteCompany(Int64 id)
        {
            Company company = this.dbContext.Companies.Include(c => c.Employees).FirstOrDefault(c => c.Id == id);
            if (company == default)
            {
                return NotFound();
            }
            this.dbContext.Remove(company);
            this.dbContext.SaveChanges();
            return Ok();
        }
    }
}