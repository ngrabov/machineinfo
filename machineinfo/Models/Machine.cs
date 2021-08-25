using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace machineinfo.Models
{
    public class Machine
    {
        [Key]
        public int MachineId { get; set; }
        public string Name { get; set; }/* 
        private List<Failure> _failures;
        public List<Failure> Failures { get{ return _failures ?? (_failures = new List<Failure>()); } set{ _failures = value;} } */
        public List<Failure> Failures { get; set; } = new List<Failure>();
    }
}