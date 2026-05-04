import { Employee } from "./Employee"

export interface User {
    id: number,
    username: string,
    email: string,
    phone: string,
    role: number,
    employeeId: number | null,
    employee: Employee | null
}
