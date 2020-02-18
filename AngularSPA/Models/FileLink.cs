using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularSPA.Models
{
    public class FileLink
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string Name { get; set; }
        [Required]
        public int HashId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string Username { get; set; }
        [Required]
        public DateTime Date { get; set; }


        public FileLink() { }
        public FileLink(string name, int hashId, string username, DateTime date)
        {
            Name = name;
            HashId = hashId;
            Username = username;
            Date = date;
        }
    }
}
