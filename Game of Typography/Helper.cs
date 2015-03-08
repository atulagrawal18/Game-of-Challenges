using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_of_Typography
{
    public static class Helper
    {
        public static List<string> GetNamesInEnum(Type enumType)
        {
            if (enumType == null)
                return new List<string>();

            return Enum.GetNames(enumType).ToList();
        }
    }
}
