﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMO.CJR.Framework
{
    [Flags]
    public enum MessageType
    {
        Request = 0x1,
        Response = 0x2,
        Event = 0x3
    }
}
