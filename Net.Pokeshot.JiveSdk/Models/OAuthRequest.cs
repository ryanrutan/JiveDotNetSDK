using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models
{
    public class OAuthRequest
    {
        public string OAuthRequestId { get; set; }
        public User User { get; set; }
        public bool IsCompleted { get; set; }
    }
}
