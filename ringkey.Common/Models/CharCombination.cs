using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Common.Models
{
    public class CharCombination
    {
        public char Value { get; set; }
        public char ReplaceValue { get; set; }

        public CharCombination(char value, char replaceValue)
        {
            Value = value;
            ReplaceValue = replaceValue;
        }
    }
}
