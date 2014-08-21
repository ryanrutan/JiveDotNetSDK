using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models
{
    public class WebhookDto
    {
        public bool buggy { get; set; }
        public DateTime buggyDate { get; set; }
        public string callback { get; set;}
        public bool enabled { get; set; }
        public string events;
        public int followerCount { get; set; }
        public string id { get; set; }
        public int likeCount { get; set; }
        public string @object { get; set; }
        public DateTime published { get; set; }
        public JiveResources resources { get; set; }
        public List<string> tags { get; set; }
        public DateTime updated { get; set; }
    }
}
