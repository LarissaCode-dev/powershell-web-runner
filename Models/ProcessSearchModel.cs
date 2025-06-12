using System.Collections.Generic;

namespace WebAppToExecutePowershell.Models
{
    public class ProcessSearchModel
    {
        public string? ProcessName { get; set; }
        public List<PowerShellResult>? Results { get; set; }
    }
}
