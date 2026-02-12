using RHmanager.Models;

namespace RHmanager.Services.Interfaces
{
    public interface IDepartmentService
    {
            Task<List<Department>> GetAllDepartmentsAsync();
            Task<Department?> GetDepartmentByIdAsync(int id);
            Task<Department> CreateDepartmentAsync(Department department);
            Task<Department> UpdateDepartmentAsync(Department department);
            Task<bool> DeleteDepartmentAsync(int id);
            Task<int> GetEmployeeCountByDepartmentAsync(int departmentId);
    }
}
