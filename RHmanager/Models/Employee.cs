using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RHmanager.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string EmployeeNumber { get; set; } = string.Empty; // Matricule

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Position { get; set; } = string.Empty; // Poste

        [Required]
        public DateTime HireDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal AnnualLeaveDays { get; set; } = 25; // Solde annuel

        [Column(TypeName = "decimal(5,2)")]
        public decimal RemainingLeaveDays { get; set; } = 25; // Jours restants

        // Clé étrangère vers Department
        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;

        // Manager (auto-référence)
        public int? ManagerId { get; set; }

        [ForeignKey(nameof(ManagerId))]
        public Employee? Manager { get; set; }

        // Relation : un employé peut avoir plusieurs demandes
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
