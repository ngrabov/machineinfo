using System.ComponentModel.DataAnnotations;

namespace machineinfo.Models
{
    public class Machine
    {
        [Key]
        public int MachineId { get; set; }
        public string Name { get; set; }
    }
}