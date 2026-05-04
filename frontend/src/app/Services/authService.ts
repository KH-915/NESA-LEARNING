import { inject, Injectable, computed, signal, PLATFORM_ID } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, tap, catchError, throwError } from "rxjs";
import { LoginForm, LoginResponse, RegistrationForm } from "../Models/Auth"
import { User } from "../Models/User";
import { stringify } from "querystring";
import { isPlatformBrowser } from "@angular/common";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private readonly LOGIN_API_URL = 'http://localhost:5245/api/Auth/login';
    private readonly LOGOUT_API_URL = 'http://localhost:5245/api/Auth/logout';
    private readonly REGISTER_API_URL = 'http://localhost:5245/api/Auth/register';

    private platformId = inject(PLATFORM_ID);

    private readonly http = inject(HttpClient);

    private _isLoggedIn = signal<boolean>(this.hasToken());
    readonly isLoggedIn = this._isLoggedIn.asReadonly();

    private _currentUser = signal <User | null>(this.getUser());

    readonly statusLabel = computed(() => this.isLoggedIn()?"Online":"Offline");

    login (loginForm: LoginForm): Observable<LoginResponse> {
        console.log("Auth Service Logging In...")
        return this.http.post<LoginResponse>(this.LOGIN_API_URL, loginForm).pipe(
            // Login Succesfully
            tap((response) =>{
                localStorage.setItem('auth_token', response.token)
                localStorage.setItem('user_info', JSON.stringify(response.user))
                this._isLoggedIn.set(true)
                console.log("Logged In Successfully!")
            }),
            // Login Failed
            catchError((err) => {
                console.error('Login Attempt Failed!', err);
                if(err.statusText == 'Unauthorized'){
                    return throwError(() => new Error('Wrong username or password!'))
                }
                return throwError(() => new Error(err.statusText || 'Server Error'))
            })
        );
    }

    logout (): void {
        const token = localStorage.getItem('auth_token');

        if(token){
            this.http.post(this.LOGOUT_API_URL, { token: token }).subscribe({
                next: () => console.log('Logout Succesfully!'),
                error: (err) => console.error('Error While Attempting Logout:', err)
            });
        }

        localStorage.removeItem('auth_token')
        localStorage.removeItem('user_info')

        this._isLoggedIn.set(false)
        this._currentUser.set(null)
    }

    register (registrationForm: RegistrationForm): void {
        this.http.post(this.REGISTER_API_URL, registrationForm).subscribe({
            next: () => console.log('Registration Successfully!'),
            error: (err) => console.error('Error While Attempting Registeration:', err)
        })
    }

    hasToken(): boolean {
        if(isPlatformBrowser(this.platformId)) return !!localStorage.getItem('auth_token');
        return false;
    }

    getUser(): User | null {
        if(isPlatformBrowser(this.platformId)) {
            const user = localStorage.getItem('user_info');
            return !!user ? JSON.parse(user) : null;
        }
        return null;
    }
}