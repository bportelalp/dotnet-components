using System;
using System.Collections.Generic;

namespace BP.Samples.EFTests.Infrastructure.DataAccess.Entities
{
    public partial class Address
    {
        public Address()
        {
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string Direction { get; set; } = null!;
        public string? DirectionExtra { get; set; }
        public string ZipCode { get; set; } = null!;
        public string Town { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string Country { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
