using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EdmontonTransit.Database.Entities
{
    public enum ServiceExceptionType
    {
        [Description("A value of 1 indicates that service has been added for the specified date")]
        ADDED = 1,
        [Description("A value of 2 indicates that service has been removed for the specified date")]
        REMOVED
    }
}
