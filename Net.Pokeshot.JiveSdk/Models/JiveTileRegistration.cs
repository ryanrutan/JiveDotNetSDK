using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models
{
    public class JiveTileRegistration
    {
        [Key]
        public int JiveTileRegistrationId { get; set; }

        public string Guid { get; set; }

        public string Id { get; set; }

        public string Config { get; set; }

        public string Name { get; set; }

        public string JiveUrl { get; set; }

        public string TenantId { get; set; }

        public string Url { get; set; }
        public string Parent { get; set; }
        public string PlaceUri { get; set; }

        public string Code { get; set; }
    }
}
