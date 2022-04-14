using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PersonFilter
    {
        public bool Contains { get; set; }

        public bool MatchWholeWords { get; set; }

        public string? Expression { get; set; }

        public PersonFilterValue FilterValue { get; set; }

        public bool CheckPerson(Person person)
        {
            _ = Expression ?? throw new ArgumentException();
            _ = person ?? throw new ArgumentNullException(nameof(person));

            bool returnValue = true;
            if (FilterValue == PersonFilterValue.FullName)
            {
                returnValue = person.FullName.Contains(Expression, StringComparison.InvariantCultureIgnoreCase);
            }

            return Contains ? 
                returnValue : 
                !returnValue;
        }

        public override string ToString()
        {
            return $"[{FilterValue}={Expression}(MatchWholeWords={MatchWholeWords})(Contains={Contains})]";
        }
    }
}
