using System.ComponentModel.DataAnnotations;

namespace Event_Management.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Tytuł wydarzenia")]
        public string Title { get; set; } = "";

        [Display(Name = "Opis")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Data wydarzenia")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        // Relacje (Klucze obce)
        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        [Display(Name = "Miejsce")]
        public int PlaceId { get; set; }
        public virtual Place? Place { get; set; }

        public virtual ICollection<Registration>? Registrations { get; set; }
    }
}
