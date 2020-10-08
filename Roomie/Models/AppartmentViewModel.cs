using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roomie.Models
{
    public class AppartmentViewModel
    {
        public int ID { get; set; }
        public decimal RentCost { get; set; }
        public string Description { get; set; }
        public Nullable<int> PhotoID { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public virtual Photo Photo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileLinker> ProfileLinkers { get; set; }
    }
}