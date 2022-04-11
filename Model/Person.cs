using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Person
    {
        public Person(string firstName)
        {
            FirstName = firstName;
        }

        public Person(string firstName, string lastName, string city, DateTime birthday, string imageId)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            Birthday = birthday;
            ImageID = imageId;
        }

        public string FullName => $"{FirstName} {LastName}";

        public string FirstName { get; set; }

        public string LastName { get; set; } = "N/A";

        public string City { get; set; } = "Unknown";

        public DateTime Birthday { get; set; } = new(1980, 1, 1);

        public int Age => (DateTime.Today - Birthday).Days / 365;

        public string? ImageID { get; set; }

        public string GetInitials()
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

        // Thank you StackOverflow! <3
        public int CalculateDaysTillBirthday()
        {
            DateTime today = DateTime.Today;
            DateTime next = Birthday.AddYears(today.Year - Birthday.Year);

            if (next < today)
            {
                next = next.AddYears(1);
            }

            int numDays = (next - today).Days;
            return numDays;
        }
    }
}
