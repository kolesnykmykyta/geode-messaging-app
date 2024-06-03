import { Routes } from '@angular/router';
import { LoginComponent } from './auth/components/login/login.component';
import { RegisterComponent } from './auth/components/register/register.component';

export const routes: Routes = [
    {
        path: 'auth/login',
        component: LoginComponent,
        title: "Login"
    },
    {
        path: 'auth/register',
        component: RegisterComponent,
        title: "Register"
    }
];
