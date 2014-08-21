using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models
{
    public class JivePlace
    {
        public string JivePlaceId { get; set; }
        public JiveInstance JiveInstance { get; set; }

        public List<Workflow> Workflows { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
