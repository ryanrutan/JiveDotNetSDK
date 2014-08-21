using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models.Dto
{
    public class WebhookActivityDto
    {
        public string webhook { get; set; } 
        public Activity activity { get; set; }
    }
}
