using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models
{
    public class JiveContent
    {
        public string JiveContentId { get; set; }
        public JiveInstance JiveInstance { get; set; }
        public List<WorkflowInstance> WorkflowInstances { get; set; }

    }
}
