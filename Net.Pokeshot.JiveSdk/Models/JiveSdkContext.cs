using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Pokeshot.JiveSdk.Models
{
    public class JiveSdkContext : DbContext
    {
        public JiveSdkContext()
            : base("name=JiveSdkContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<JiveInstance> JiveInstances { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JiveAddonRegistration> JiveRegistrations { get; set; }
        public DbSet<JiveTileRegistration> JiveTileRegistrations { get; set; }
        public DbSet<JiveAddon> JiveAddons { get; set; }
    }
}
