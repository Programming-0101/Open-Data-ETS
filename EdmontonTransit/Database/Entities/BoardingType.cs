using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EdmontonTransit.Database.Entities
{
    public enum BoardingType
    {
        [Description("Regular {0}")]
        REGULAR,
        [Description("No {0} available")]
        NONE,
        [Description("Phone agency to arrange {0}")]
        PHONE,
        [Description("Make arrangements with driver")]
        ARRANGE
    }
}
