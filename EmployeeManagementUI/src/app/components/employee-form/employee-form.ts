import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { EmployeeService } from '../../services/employee.service';
import { EmployeeCreateDto } from '../../models/employee.model';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.scss'
})
export class EmployeeForm implements OnInit {
  isEditMode: boolean = false;
  employeeId: number = 0;
  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  formData: EmployeeCreateDto = {
    name: '',
    email: '',
    department: '',
    role: '',
    salary: 0,
    status: 'Active'
  };

  departments: string[] = [
    'Engineering',
    'HR',
    'Finance',
    'Marketing',
    'Operations',
    'Sales'
  ];

  constructor(
    private employeeService: EmployeeService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.employeeId = +id;
      this.loadEmployee(this.employeeId);
    }
  }

  loadEmployee(id: number): void {
    this.loading = true;
    this.employeeService.getById(id).subscribe({
      next: (response) => {
        if (response.success) {
          const emp = response.data;
          this.formData = {
            name: emp.name,
            email: emp.email,
            department: emp.department,
            role: emp.role,
            salary: emp.salary,
            status: emp.status
          };
        }
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load employee!';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    if (this.isEditMode) {
      this.employeeService.update(this.employeeId, this.formData).subscribe({
        next: (response) => {
          if (response.success) {
            this.successMessage = 'Employee updated successfully!';
            setTimeout(() => this.router.navigate(['/employees']), 1500);
          }
          this.loading = false;
        },
        error: () => {
          this.errorMessage = 'Failed to update employee!';
          this.loading = false;
        }
      });
    } else {
      this.employeeService.create(this.formData).subscribe({
        next: (response) => {
          if (response.success) {
            this.successMessage = 'Employee created successfully!';
            setTimeout(() => this.router.navigate(['/employees']), 1500);
          }
          this.loading = false;
        },
        error: () => {
          this.errorMessage = 'Failed to create employee!';
          this.loading = false;
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/employees']);
  }
}