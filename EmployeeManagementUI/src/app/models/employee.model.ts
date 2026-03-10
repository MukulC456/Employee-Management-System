export interface Employee {
  id: number;
  name: string;
  email: string;
  department: string;
  role: string;
  salary: number;
  joinDate: string;
  status: string;
}

export interface EmployeeCreateDto {
  name: string;
  email: string;
  department: string;
  role: string;
  salary: number;
  status: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  role: string;
  expiresAt: string;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  email: string;
  password: string;
  role: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}