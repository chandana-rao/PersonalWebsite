using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<FinalProject.Models.Education> academics { get; set; }
        public DbSet<FinalProject.Models.Project> projects { get; set; }
        public DbSet<FinalProject.Models.PExperience> experiences { get; set; }
        public DbSet<FinalProject.Models.Recruiter> recruiters { get; set; }
        public DbSet<FinalProject.Models.Files> documents { get; set; }
        
    }
}
