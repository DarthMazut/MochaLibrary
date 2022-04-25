using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum PersonImageType
    {
        JPG,
        PNG
    }

    public static class PersonImageTypeExtensions
    {
        public static PersonImageType? ResolvePathExtension(string path)
        {
            string? extension = Path.GetExtension(path)[1..];
            if (extension is not null)
            {
                if (Enum.TryParse<PersonImageType>(extension.ToUpper(), out PersonImageType parsedImageType))
                {
                    return parsedImageType;
                }
            }

            return null;
        }
    }
}
