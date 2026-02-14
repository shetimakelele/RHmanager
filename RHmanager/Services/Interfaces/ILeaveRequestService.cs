using RHmanager.Enums;
using RHmanager.Models;

namespace RHmanager.Services.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequest>> GetAllLeaveRequestsAsync();
        Task<LeaveRequest?> GetLeaveRequestByIdAsync(int id);
        Task<List<LeaveRequest>> GetLeaveRequestsByEmployeeAsync(int employeeId);
        Task<List<LeaveRequest>> GetLeaveRequestsByManagerAsync(int managerId);
        Task<List<LeaveRequest>> GetPendingLeaveRequestsAsync();
        Task<List<LeaveRequest>> GetLeaveRequestsByStatusAsync(LeaveRequestStatus status);
        Task<LeaveRequest> CreateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<LeaveRequest> UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<bool> DeleteLeaveRequestAsync(int id);
        Task<LeaveRequest> ApproveLeaveRequestAsync(int leaveRequestId, int validatorId, string? comment = null);
        Task<LeaveRequest> RejectLeaveRequestAsync(int leaveRequestId, int validatorId, string? comment = null);
        Task<bool> HasOverlappingLeaveAsync(int employeeId, DateTime startDate, DateTime endDate, int? excludeRequestId = null);
        Task<int> CalculateBusinessDaysAsync(DateTime startDate, DateTime endDate);
        Task<List<LeaveType>> GetAllLeaveTypesAsync();
    }
}
