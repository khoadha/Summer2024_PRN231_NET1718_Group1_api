﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class GetRoomCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class AddRoomCategoryDto
    {
        public string Name { get; set; }
    }
}
