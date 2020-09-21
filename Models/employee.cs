using System;
using System.ComponentModel.DataAnnotations;

namespace pumox.Models
{
    public class Employee
    {
        public Int64 Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [EnumDataType(typeof(JobTitle), ErrorMessage = "Job title does not exist")]
        public JobTitle JobTitle { get; set; }
        public Company Company { get; set; }
    }
}