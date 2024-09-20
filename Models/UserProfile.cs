using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace opms_server_core.Models
{
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string CompanyName { get; set; }

        public string Description { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = "User"; // Default value

        public bool IsActive { get; set; } = true; // Default value

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Default value

        public DateTime? UpdatedAt { get; set; }

        // Navigation Property
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
