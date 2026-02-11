using RHmanager.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RHmanager.Models
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        // Clé étrangère vers Employee (demandeur)
        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        // Clé étrangère vers LeaveType
        [Required]
        public int LeaveTypeId { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public LeaveType LeaveType { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(1, 365)]
        public int NumberOfDays { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        // Statut de la demande
        [Required]
        public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        // Validation
        public int? ValidatedById { get; set; }

        [ForeignKey(nameof(ValidatedById))]
        public Employee? ValidatedBy { get; set; }

        public DateTime? ValidationDate { get; set; }

        [MaxLength(500)]
        public string? ValidationComment { get; set; }
    }
}
