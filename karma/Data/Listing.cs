﻿using System;
using System.Collections.Generic;

#nullable disable

namespace karma
{
    public partial class Listing
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Img { get; set; }
        public DateTime Added { get; set; }
    }
}
