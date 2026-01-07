using System.ComponentModel.DataAnnotations;

namespace Event_Management.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa Kategorii")]
        public string Name { get; set; }

        public virtual ICollection<Event>? Events { get; set; }
    }
}
