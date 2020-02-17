using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularSPA.Models
{
    public class FileDbContext : DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
        {

        }

        public DbSet<FileInfo> Files { get; set; }
        public DbSet<FileLink> Links { get; set; }
    }
}
