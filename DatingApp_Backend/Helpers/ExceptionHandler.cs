using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_Backend.Helpers
{
    public class ExceptionHandler : Exception
    {
        public ExceptionHandlerEnumType type { get; set; }
        public ExceptionHandler(ExceptionHandlerEnumType type, string msg) : base(message : msg)
        {
            this.type = type;
        }
    }

    public enum ExceptionHandlerEnumType
    {
        NotFound = 0,
        NotUniqueValue = 1,
        Unauthorized = 2
    }
}
