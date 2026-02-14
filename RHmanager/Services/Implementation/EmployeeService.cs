using Microsoft.EntityFrameworkCore;
using RHmanager.Data;
using RHmanager.Models;
using RHmanager.Services.Interfaces;

namespace RHmanager.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {

        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return false;

            // Vérifier qu'il n'a pas de demandes de congés
            var hasLeaveRequests = await _context.LeaveRequests.AnyAsync(lr => lr.EmployeeId == id);
            if (hasLeaveRequests)
                return false;

            // Vérifier qu'il n'est pas manager d'autres employés
            var hasManagedEmployees = await _context.Employees.AnyAsync(e => e.ManagerId == id);
            if (hasManagedEmployees)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await _context.Employees
                .AnyAsync(e => e.Email == email &&
                              (excludeId == null || e.Id != excludeId));
        }

        public async Task<bool> EmployeeNumberExistsAsync(string employeeNumber, int? excludeId = null)
        {
            return await _context.Employees
                .AnyAsync(e => e.EmployeeNumber == employeeNumber &&
                              (excludeId == null || e.Id != excludeId));
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();
        }

        public async Task<List<Employee>> GetAvailableManagersAsync(int? excludeEmployeeId = null)
        {
            // Retourne les employés qui peuvent être managers (exclut l'employé lui-même)
            var query = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (excludeEmployeeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeEmployeeId.Value);
            }

            return await query
                .OrderBy(e => e.LastName)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .Include(e => e.LeaveRequests)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            return await _context.Employees
                .Include(e => e.Manager)
                .Where(e => e.DepartmentId == departmentId)
                .OrderBy(e => e.LastName)
                .ToListAsync();
        }

        public async Task<List<Employee>> GetEmployeesByManagerAsync(int managerId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.ManagerId == managerId)
                .OrderBy(e => e.LastName)
                .ToListAsync();
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task UpdateLeaveBalanceAsync(int employeeId, decimal daysToDeduct)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                employee.RemainingLeaveDays -= daysToDeduct;
                await _context.SaveChangesAsync();
            }
        }
    }
}
