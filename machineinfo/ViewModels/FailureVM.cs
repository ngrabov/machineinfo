using System.Collections.Generic;
using machineinfo.Models;

namespace machineinfo.ViewModels
{
    #nullable enable
    public class FailureVM
    {
        public int? FailureId { get; set; }
        public string? Name { get; set; }
        public Priority? Priority { get; set; }
        public string? Description {get; set;}
        public Status? Status { get; set;}
        public string? fileURLs { get; set; }
        public string? MachineName { get; set; }
    }
}