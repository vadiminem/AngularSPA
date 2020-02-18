using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularSPA.Models
{
    public class FileInfo
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Hash { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string Location { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Size { get; set; }

        public FileInfo() { }

        public FileInfo(string hash, string location, string size)
        {
            Hash = hash;
            Location = location;
            Size = size;
        }

    }
}
