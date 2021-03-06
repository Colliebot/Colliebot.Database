﻿using Newtonsoft.Json;
using System;

namespace Colliebot
{
    public class DbUser
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relationships
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DbDiscordUser Discord { get; set; }
    }
}
