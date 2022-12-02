using System;
using System.Collections.Generic;

namespace BP.Samples.EFTests.Infrastructure.DataAccess.Entities
{
    public partial class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string SurName { get; set; } = null!;
        public string Idcard { get; set; } = null!;
        public DateTime DateBirth { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
