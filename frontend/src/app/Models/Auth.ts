import { User } from "./User"

export interface LoginForm {
    username: string,
    password: string
}

export interface RegistrationForm {
    username: string,
    password: string,
    email?: string,
    phone?: string
}

export interface LoginResponse {
    token: string,
    user: User
}