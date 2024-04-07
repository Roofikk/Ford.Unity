﻿using System;

namespace Ford.WebApi.Data
{
    public class User
    {
        public long UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}