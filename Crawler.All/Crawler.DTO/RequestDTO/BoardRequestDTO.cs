﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.DTO.RequestDTO
{
    public class BoardGetbyIdRequestDTO
    {
        public int? BoardId { get; set; }
    }

    public class BoardCreateRequestDTO
    {
        public int? For_WebsiteId { get; set; }

        public string Label{ get; set; }
    }

    public class BoardGetListRequestDTO
    {

    }
}