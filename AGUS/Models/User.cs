using System.ComponentModel.DataAnnotations;

namespace AGUS.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name {  get; set; }
        public DateTime DateofBirth { get; set; }
        public string? Avt { get; set; }
        public string AccountId { get; set; }
    }
}
