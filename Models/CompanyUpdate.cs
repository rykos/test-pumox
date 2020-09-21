using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pumox.Models
{
    public class CompanyUpdate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int EstablishmentYear { get; set; }
        [Required]
        public List<Employee> Employees { get; set; }
    }
}