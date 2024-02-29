using System;
using System.Collections.Generic;
using System.Linq;

namespace Ford.WebApi.Data
{
    public class CreationHorse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public IEnumerable<CreationHorseOwner> HorseOwners { get; set; }
    }

    public class CreationHorseOwner
    {
        public string UserId { get; set; }
        public RoleOwnerAccess RuleAccess { get; set; }
    }

    internal class CreationHorseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public List<CreationHorseOwnerDto> HorseOwners { get; set; }

        public CreationHorseDto(CreationHorse horse)
        {
            Name = horse.Name;
            Description = horse.Description;
            BirthDate = horse.BirthDate;
            Sex = horse.Sex;
            City = horse.City;
            Region = horse.Region;
            Country = horse.Country;
            HorseOwners = new List<CreationHorseOwnerDto>();

            foreach (var owner in horse.HorseOwners)
            {
                HorseOwners.Add(new CreationHorseOwnerDto(owner));
            }
        }
    }

    internal class CreationHorseOwnerDto
    {
        public string UserId { get; set; }
        public string RuleAccess { get; set; }

        public CreationHorseOwnerDto(CreationHorseOwner owner)
        {
            UserId = owner.UserId;
            RuleAccess = owner.RuleAccess.ToString();
        }
    }

    public enum RoleOwnerAccess
    {
        Read,
        Write,
        All,
        Creator
    }
}
