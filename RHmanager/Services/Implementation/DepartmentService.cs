using Microsoft.EntityFrameworkCore;
using RHmanager.Data;
using RHmanager.Models;
using RHmanager.Services.Interfaces;

namespace RHmanager.Services.Implementation
{
    public class DepartmentService : IDepartmentService
    {

        private readonly AppDbContext _context;

        public DepartmentService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return false;

            // Vérifier qu'il n'y a pas d'employés dans ce département
            var hasEmployees = await _context.Employees.AnyAsync(e => e.DepartmentId == id);
            if (hasEmployees)
                return false; // Ne pas supprimer si des employés sont rattachés

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<int> GetEmployeeCountByDepartmentAsync(int departmentId)
        {
            return await _context.Employees
                .CountAsync(e => e.DepartmentId == departmentId);
        }

        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return department;
        }
    }
}
