using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Entities
{
    public class Image : BaseItem
    {
        public int PackageId { get; set; }
        public string Base64 { get; set; }

    }
}
