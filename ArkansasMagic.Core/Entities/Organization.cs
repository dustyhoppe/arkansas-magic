using System;
using System.Collections.Generic;

namespace ArkansasMagic.Core.Entities
{
    public class Organization
    {
        public Organization()
        {
            //Events = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumbers { get; set; }
        public string EmailAddress { get; set; }
        public bool ShowEmailInSel { get; set; }
        public string Website { get; set; }
        public string Websites { get; set; }
        public string Brands { get; set; }
        public string PostalAddress { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public bool IsPremium { get; set; }
        public bool IsTestStore { get; set; }
        public DateTime AcceptedTermsAt { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public DateTime UpdatedDateUtc { get; set; }

        //public ICollection<Event> Events { get; set; }
    }
}
