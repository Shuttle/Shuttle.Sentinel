﻿using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class RegisterDataStore
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
    }
}