using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EdmontonTransit.Database.Entities
{
    public enum TransferConnectionType
    {
        [Description("recommended transfer point between two routes")]
        RECOMMENDED,
        [Description("timed transfer point between two routes")]
        TIMED,
        [Description("minimum amount of time between both routes")]
        MIN_TIME,
        [Description("Transfers not possible")]
        NOT_POSSIBLE
    }
}
