using Microsoft.AspNetCore.Identity;

namespace Event_Management.Models
{
    public class Registration
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public virtual Event? Event { get; set; }

        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
