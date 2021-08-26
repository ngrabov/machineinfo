using System.Collections.Generic;
using machineinfo.Models;

namespace machineinfo.ViewModels
{
    #nullable enable
    public class FailureVM
    {
        public IEnumerable<Failure>? failures { get; set; }
    }
}