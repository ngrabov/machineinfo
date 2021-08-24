using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace machineinfo.Models
{
    public enum Status 
    {
        Active, Inactive
    }
    public class Failure
    {
        [Key]
        public int FailureID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public Status Status { get; set; }
        public DateTime EntryTime { get; set; }
        public string MachineName { get; set; }
        public ICollection<IFormFile> files { get; set; }
    }
}