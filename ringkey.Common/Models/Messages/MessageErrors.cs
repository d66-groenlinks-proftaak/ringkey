using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Common.Models.Messages
{
    public enum MessageErrors
    {
        NoError = 0,
        ContentTooShort = 1,
        ContentTooLong = 2,
        TitleTooShort = 3,
        TitleTooLong = 4,
        AuthorTooShort = 5,
        AuthorTooLong = 6,
        InvalidEmail = 7,
        EmailAlreadyOwned = 8
    }
}
