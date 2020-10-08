﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roomie.Models
{
    public class UserViewModelForExperimentation
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Description { get; set; }
        public bool PropertyBool { get; set; }
        public Nullable<int> AddressID { get; set; }
        public Nullable<int> PhotoID { get; set; }
    }
}