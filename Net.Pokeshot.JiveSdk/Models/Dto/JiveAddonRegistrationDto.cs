using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models.Dto
{
    public class JiveAddonRegistrationDto
    {
        [DataMember]
        public string timestamp { get; set; }
        [DataMember]
        public string jiveSignatureURL { get; set; }
        [DataMember]
        public string tenantId { get; set; }
        [DataMember]
        public string jiveUrl { get; set; }
        [DataMember]
        public string jiveSignature { get; set; }
        [DataMember]
        public string clientSecret { get; set; }
        [DataMember]
        public string clientId { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string scope { get; set; }
        [DataMember]
        public string uninstalled { get; set; }
    }
}
