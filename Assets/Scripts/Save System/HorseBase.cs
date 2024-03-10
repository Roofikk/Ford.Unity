using Ford.WebApi.Data;
using System;
using System.Collections.Generic;

namespace Ford.SaveSystem
{
    public class HorseBase
    {
        public long HorseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public ICollection<HorseUserDto> Users { get; set; }
        public ICollection<SaveData> Saves { get; set; }
    }
}
