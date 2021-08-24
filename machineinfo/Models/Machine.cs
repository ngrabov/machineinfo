using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace machineinfo.Models
{
    public class Machine
    {
        [Key]
        public int MachineID { get; set; }
        public string Name { get; set; }
        private ICollection<Failure> _failures;
        public ICollection<Failure> Failures { get{ return _failures ?? (_failures = new List<Failure>()); } set{ _failures = value;} }
    }
}