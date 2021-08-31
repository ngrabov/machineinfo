using System.Collections.Generic;
using machineinfo.Models;

namespace machineinfo.ViewModels
{
    #nullable enable
    public class MachineVM
    {
        public string? Name { get; set; }
        public IEnumerable<Failure>? Failures {get; set;}
        public IEnumerable<Machine>? Machines {get; set;}
    }
}