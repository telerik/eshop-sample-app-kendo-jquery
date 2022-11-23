using System;
using System.Collections.Generic;

namespace Entities
{
    public partial class Contact
    {
        public Contact()
        {
            SalesOrders = new HashSet<SalesOrder>();
        }

        public int ContactId { get; set; }
        public string? EmailAddress { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;

        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}
