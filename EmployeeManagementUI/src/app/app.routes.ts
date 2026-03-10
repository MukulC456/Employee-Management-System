import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { EmployeeList } from './components/employee-list/employee-list';
import { EmployeeForm } from './components/employee-form/employee-form';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: Login },
  {
    path: 'employees',
    component: EmployeeList,
    canActivate: [AuthGuard]
  },
  {
    path: 'employees/add',
    component: EmployeeForm,
    canActivate: [AuthGuard]
  },
  {
    path: 'employees/edit/:id',
    component: EmployeeForm,
    canActivate: [AuthGuard]
  },
  { path: '**', redirectTo: '/login' }
];