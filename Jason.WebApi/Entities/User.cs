using Jason.WebApi.Infra;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jason.WebApi.Entities
{
    [Table("User")]
    public class User : BaseEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
