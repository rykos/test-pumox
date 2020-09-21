using System;
using System.Collections.Generic;

namespace pumox.Models
{
    public class Company
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }
        public List<Employee> Employees { get; set; }
    }
}