using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pumox.Models
{
    public class CompanySearch
    {
        public string Keyword { get; set; } = null;
        public DateTime? EmployeeDateOfBirthFrom { get; set; } = null;
        public DateTime? EmployeeDateOfBirthTo { get; set; } = null;
        public List<JobTitle> EmployeeJobTitles { get; set; } = null;
    }
}

// {
//  "Keyword": "<string>",
//  "EmployeeDateOfBirthFrom": "<DateTime?>",
//  "EmployeeDateOfBirthTo": "<DateTime?>",
//  "EmployeeJobTitles": [“<string(enum)>”, …]
// }