using Microsoft.EntityFrameworkCore;
using RHmanager.Data;
using RHmanager.Enums;
using RHmanager.Models;
using RHmanager.Services.Interfaces;

namespace RHmanager.Services.Implementation
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly AppDbContext _context;
        private readonly IEmployeeService _employeeService;

        public LeaveRequestService(AppDbContext context, IEmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }

        public async Task<List<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                    .ThenInclude(e => e.Department)
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ValidatedBy)
                .OrderByDescending(lr => lr.RequestDate)
                .ToListAsync();
        }

        public async Task<LeaveRequest?> GetLeaveRequestByIdAsync(int id)
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                    .ThenInclude(e => e.Department)
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ValidatedBy)
                .FirstOrDefaultAsync(lr => lr.Id == id);
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsByEmployeeAsync(int employeeId)
        {
            return await _context.LeaveRequests
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ValidatedBy)
                .Where(lr => lr.EmployeeId == employeeId)
                .OrderByDescending(lr => lr.RequestDate)
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsByManagerAsync(int managerId)
        {
            // Récupère les demandes des employés sous la responsabilité de ce manager
            var teamEmployeeIds = await _context.Employees
                .Where(e => e.ManagerId == managerId)
                .Select(e => e.Id)
                .ToListAsync();

            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.LeaveType)
                .Where(lr => teamEmployeeIds.Contains(lr.EmployeeId))
                .OrderByDescending(lr => lr.RequestDate)
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetPendingLeaveRequestsAsync()
        {
            return await GetLeaveRequestsByStatusAsync(LeaveRequestStatus.Pending);
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsByStatusAsync(LeaveRequestStatus status)
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                    .ThenInclude(e => e.Department)
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ValidatedBy)
                .Where(lr => lr.Status == status)
                .OrderByDescending(lr => lr.RequestDate)
                .ToListAsync();
        }

        public async Task<LeaveRequest> CreateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            // Calculer automatiquement le nombre de jours
            leaveRequest.NumberOfDays = await CalculateBusinessDaysAsync(
                leaveRequest.StartDate,
                leaveRequest.EndDate);

            leaveRequest.RequestDate = DateTime.Now;
            leaveRequest.Status = LeaveRequestStatus.Pending;

            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();
            return leaveRequest;
        }

        public async Task<LeaveRequest> UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
            return leaveRequest;
        }

        public async Task<bool> DeleteLeaveRequestAsync(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
                return false;

            // Seules les demandes en attente peuvent être supprimées
            if (leaveRequest.Status != LeaveRequestStatus.Pending)
                return false;

            _context.LeaveRequests.Remove(leaveRequest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LeaveRequest> ApproveLeaveRequestAsync(int leaveRequestId, int validatorId, string? comment = null)
        {
            var leaveRequest = await GetLeaveRequestByIdAsync(leaveRequestId);
            if (leaveRequest == null)
                throw new Exception("Demande de congé introuvable");

            leaveRequest.Status = LeaveRequestStatus.Approved;
            leaveRequest.ValidatedById = validatorId;
            leaveRequest.ValidationDate = DateTime.Now;
            leaveRequest.ValidationComment = comment;

            await _context.SaveChangesAsync();

            // Déduire du solde si le type de congé le nécessite
            if (leaveRequest.LeaveType.DeductFromBalance)
            {
                await _employeeService.UpdateLeaveBalanceAsync(
                    leaveRequest.EmployeeId,
                    leaveRequest.NumberOfDays);
            }

            return leaveRequest;
        }

        public async Task<LeaveRequest> RejectLeaveRequestAsync(int leaveRequestId, int validatorId, string? comment = null)
        {
            var leaveRequest = await GetLeaveRequestByIdAsync(leaveRequestId);
            if (leaveRequest == null)
                throw new Exception("Demande de congé introuvable");

            leaveRequest.Status = LeaveRequestStatus.Rejected;
            leaveRequest.ValidatedById = validatorId;
            leaveRequest.ValidationDate = DateTime.Now;
            leaveRequest.ValidationComment = comment;

            await _context.SaveChangesAsync();
            return leaveRequest;
        }

        public async Task<bool> HasOverlappingLeaveAsync(int employeeId, DateTime startDate, DateTime endDate, int? excludeRequestId = null)
        {
            var query = _context.LeaveRequests
                .Where(lr => lr.EmployeeId == employeeId &&
                            lr.Status == LeaveRequestStatus.Approved &&
                            ((lr.StartDate <= endDate && lr.EndDate >= startDate)));

            if (excludeRequestId.HasValue)
            {
                query = query.Where(lr => lr.Id != excludeRequestId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<int> CalculateBusinessDaysAsync(DateTime startDate, DateTime endDate)
        {
            // Calcul simple : jours entre les deux dates (incluant le premier et dernier jour)
            int days = (endDate - startDate).Days + 1;

            // Version simple sans exclusion des week-ends
            return await Task.FromResult(days);

            // TODO: Implémenter l'exclusion des week-ends et jours fériés 
        }

        public async Task<List<LeaveType>> GetAllLeaveTypesAsync()
        {
            return await _context.LeaveTypes
                .OrderBy(lt => lt.Name)
                .ToListAsync();
        }
    }
}
