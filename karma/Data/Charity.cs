﻿using System;
using System.Collections.Generic;

#nullable disable

namespace karma
{
    public partial class Charity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Added { get; set; }
        public string Website { get; set; }
    }
}
