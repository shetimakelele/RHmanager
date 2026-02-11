using System.ComponentModel.DataAnnotations;

namespace RHmanager.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Description { get; set; }

        // Relation : un département a plusieurs employés
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
