using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models.Dto
{
    public class OAuthRequestDto
    {
        public string oauthToken { get; set; }
        public string instanceUrl { get; set; }
        public string clientId { get; set; }
    }
}
