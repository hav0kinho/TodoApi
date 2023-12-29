using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Todo
    {
        public int TodoId { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

    }
}
