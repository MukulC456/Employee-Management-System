import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from '../../services/employee.service';
import { AuthService } from '../../services/auth.service';
import { Employee } from '../../models/employee.model';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.scss'
})
export class EmployeeList implements OnInit {
  employees: Employee[] = [];
  filteredEmployees: Employee[] = [];
  searchText: string = '';
  loading: boolean = false;
  errorMessage: string = '';

  constructor(
    private employeeService: EmployeeService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.loading = true;
    this.employeeService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.employees = response.data;
          this.filteredEmployees = response.data;
        }
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load employees!';
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    const text = this.searchText.toLowerCase();
    this.filteredEmployees = this.employees.filter(e =>
      e.name.toLowerCase().includes(text) ||
      e.email.toLowerCase().includes(text) ||
      e.department.toLowerCase().includes(text)
    );
  }

  onAdd(): void {
    this.router.navigate(['/employees/add']);
  }

  onEdit(id: number): void {
    this.router.navigate(['/employees/edit', id]);
  }

  onDelete(id: number): void {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.delete(id).subscribe({
        next: () => {
          this.loadEmployees();
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete employee!';
        }
      });
    }
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  getEmail(): string {
    return this.authService.getEmail() || '';
  }
}