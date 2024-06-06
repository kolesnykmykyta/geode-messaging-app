import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './auth/components/login/login.component';
import { RegisterComponent } from './auth/components/register/register.component';
import { UsersComponent } from './users/users.component';

const routes: Routes = [
    {
        path: 'auth/login',
        component: LoginComponent,
        title: "Login"
    },
    {
        path: 'auth/register',
        component: RegisterComponent,
        title: "Register"
    },
    {
      path: 'users',
      component: UsersComponent,
      title: "Users"
    }
];

@NgModule({
    declarations: [],
    imports: [
      RouterModule.forRoot(routes)
    ],
    exports: [RouterModule]
  })
export class AppRoutingModule { }
