﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionTech_Anbar_Project.ViewModel.Categories
{
    public class SubCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<MainType> mainTypes { get; set; }
    }
}
