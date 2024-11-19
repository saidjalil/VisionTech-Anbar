using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionTech_Anbar_Project.DAL
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 4. Configure Connection String
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=VisionTechAnbar;Trusted_Connection=True;");
        }


    }
}
