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
    public enum Priority
    {
        High, Medium, Low
    }
    public class Failure
    {
        [Key]
        public int FailureId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public DateTime EntryTime { get; set; }
        public int MachineId { get; set; }
        public byte[] Stream { get; set; }
        public string fileURLs { get; set;}
        public List<IFormFile> files { get; set; }
    }
}