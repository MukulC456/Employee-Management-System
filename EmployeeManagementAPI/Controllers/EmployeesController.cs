using EmployeeManagementAPI.DTOs;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<EmployeeResponseDto>>.Ok(employees));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
                return NotFound(ApiResponse<EmployeeResponseDto>.Fail($"Employee with ID {id} not found"));

            return Ok(ApiResponse<EmployeeResponseDto>.Ok(employee));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _employeeService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                ApiResponse<EmployeeResponseDto>.Ok(created, "Employee created successfully")
            );
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _employeeService.UpdateAsync(id, dto);

            if (updated == null)
                return NotFound(ApiResponse<EmployeeResponseDto>.Fail($"Employee with ID {id} not found"));

            return Ok(ApiResponse<EmployeeResponseDto>.Ok(updated, "Employee updated successfully"));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _employeeService.DeleteAsync(id);

            if (!deleted)
                return NotFound(ApiResponse<object>.Fail($"Employee with ID {id} not found"));

            return NoContent();
        }

        [HttpGet("department/{dept}")]
        public async Task<IActionResult> GetByDepartment(string dept)
        {
            var all = await _employeeService.GetAllAsync();
            var filtered = all.Where(e =>
                e.Department.Equals(dept, StringComparison.OrdinalIgnoreCase));

            return Ok(ApiResponse<IEnumerable<EmployeeResponseDto>>.Ok(filtered));
        }
    }
}