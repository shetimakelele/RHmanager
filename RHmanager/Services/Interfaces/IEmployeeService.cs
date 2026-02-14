using RHmanager.Models;

namespace RHmanager.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<List<Employee>> GetEmployeesByDepartmentAsync(int departmentId);
        Task<List<Employee>> GetEmployeesByManagerAsync(int managerId);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<bool> EmployeeNumberExistsAsync(string employeeNumber, int? excludeId = null);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task UpdateLeaveBalanceAsync(int employeeId, decimal daysToDeduct);
        Task<List<Employee>> GetAvailableManagersAsync(int? excludeEmployeeId = null);
    }
}
