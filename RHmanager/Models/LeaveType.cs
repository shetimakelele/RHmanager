using System.ComponentModel.DataAnnotations;

namespace RHmanager.Models
{
    public class LeaveType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // CP, RTT, Maladie, etc.

        public bool DeductFromBalance { get; set; } = true; // Décompte du solde ?

        // Relation
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
