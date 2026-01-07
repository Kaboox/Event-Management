using System.ComponentModel.DataAnnotations;

namespace Event_Management.Models
{
    public class Place
    {
        public int Id { get; set; }

        [Display(Name = "Miejsce")]
        public string Name { get; set; }

        public virtual ICollection<Event>? Events { get; set; }
    }
}
