using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.DTOs;
using EmployeeManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetAllAsync()
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .OrderBy(e => e.Name)
                .ToListAsync();

            return employees.Select(MapToDto);
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            return employee == null ? null : MapToDto(employee);
        }

        public async Task<EmployeeResponseDto> CreateAsync(EmployeeCreateDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim().ToLower(),
                Department = dto.Department,
                Role = dto.Role.Trim(),
                Salary = dto.Salary,
                Status = dto.Status,
                JoinDate = DateTime.UtcNow
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return MapToDto(employee);
        }

        public async Task<EmployeeResponseDto?> UpdateAsync(int id, EmployeeUpdateDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return null;

            employee.Name = dto.Name.Trim();
            employee.Email = dto.Email.Trim().ToLower();
            employee.Department = dto.Department;
            employee.Role = dto.Role.Trim();
            employee.Salary = dto.Salary;
            employee.Status = dto.Status;

            await _context.SaveChangesAsync();

            return MapToDto(employee);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }

        private static EmployeeResponseDto MapToDto(Employee e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Email = e.Email,
            Department = e.Department,
            Role = e.Role,
            Salary = e.Salary,
            JoinDate = e.JoinDate,
            Status = e.Status
        };
    }
}