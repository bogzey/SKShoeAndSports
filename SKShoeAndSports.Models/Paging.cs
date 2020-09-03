﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class Paging
    {
        public int TotalItem { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string urlParam { get; set; }

        public int TotalPage => (int)Math.Ceiling((decimal)TotalItem / ItemsPerPage);
    }
}
