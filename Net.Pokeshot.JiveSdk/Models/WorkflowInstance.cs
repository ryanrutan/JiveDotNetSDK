using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models
{
    public class WorkflowInstance
    {
        public int WorkflowInstanceId { get; set; }
        public JiveContent JiveContent { get; set; }
        public Workflow Workflow { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
