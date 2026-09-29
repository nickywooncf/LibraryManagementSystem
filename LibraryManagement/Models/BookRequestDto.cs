using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class BookRequestDto
    {
        [Required(ErrorMessage = "Book Title is mandatory.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author name is mandatory.")]
        public string Author { get; set; }

        [Required]
        public string ISBN { get; set; }
    }
}