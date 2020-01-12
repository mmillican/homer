using System;

namespace Homer.Shared.Entities.Contacts
{
    public class Address 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string FormalNames { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public DateTime LastUpdated { get; set; }
        public bool NeedsUpdate { get; set; }
    }
}