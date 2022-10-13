﻿using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterMessageTypesHandled : EndpointMessage
    {
        public List<string> MessageTypesHandled { get; set; } = new List<string>();
    }
}
