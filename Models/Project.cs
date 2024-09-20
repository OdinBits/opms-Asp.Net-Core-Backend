using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace opms_server_core.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(600)]
        public string? ProjectTheme { get; set; }

        [MaxLength(100)]
        public string? Reason { get; set; }

        [MaxLength(100)]
        public string? Type { get; set; }

        [MaxLength(100)]
        public string? Division { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(50)]
        public string? Priority { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(255)]
        public string? Location { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile? UserProfile { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Add the Status column
        [Required]
        [MaxLength(50)]
        public string? Status { get; set; } = "Registered"; // Default value as 'Registered'
    }
}
