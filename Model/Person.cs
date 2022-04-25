using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Person
    {
        public Person(string firstName, string lastName) : this(firstName, lastName, Guid.NewGuid()) { }

        [JsonConstructor]
        private Person(string firstName, string lastName, Guid guid)
        {
            FirstName = firstName;
            LastName = lastName;
            Guid = guid;
        }

        public Guid Guid { get; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? City { get; set; }

        public DateTimeOffset? Birthday { get; set; }

        public PersonImageType? ImageType { get; set; }

        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";

        [JsonIgnore]
        public int? Age => (DateTime.Today - Birthday)?.Days / 365;

        [JsonIgnore]
        public int? DaysTillBirthday => CalculateDaysTillBirthday();

        [JsonIgnore]
        public string Initials => GetInitials();

        [JsonIgnore]
        public string? ImageName
        {
            get
            {
                if (ImageType is null)
                {
                    return null;
                }
                
                return $"{Guid}.{ImageType.ToString().ToLower()}";
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Person person &&
                   Guid.Equals(person.Guid) &&
                   FirstName == person.FirstName &&
                   LastName == person.LastName &&
                   City == person.City &&
                   EqualityComparer<DateTimeOffset?>.Default.Equals(Birthday, person.Birthday) &&
                   ImageType == person.ImageType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Guid, FirstName, LastName, City, Birthday, ImageType);
        }

        private string GetInitials()
        {
            char firstInitial = '?';
            char lastInitial = '?';

            if (FirstName.Any())
            {
                firstInitial = FirstName[0];
            }

            if (LastName.Any() && LastName != "N/A")
            {
                lastInitial = LastName[0];
            }

            return $"{firstInitial}{lastInitial}";
        }

        private int? CalculateDaysTillBirthday()
        {
            if (Birthday is null)
            {
                return null;
            }
            DateTimeOffset notNullBirthday = (DateTimeOffset)Birthday;

            DateTimeOffset today = DateTime.Today;
            DateTimeOffset next = notNullBirthday.AddYears(today.Year - notNullBirthday.Year);

            if (next < today)
            {
                next = next.AddYears(1);
            }

            int numDays = (next - today).Days;
            return numDays;
        }
    }
}
