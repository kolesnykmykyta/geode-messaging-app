import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UsersComponent } from './pages/users/users.component';
import { authGuard } from './shared/services/auth.guard';

const routes: Routes = [
  { path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)},
  { path: 'users', component: UsersComponent, canActivate: [authGuard] },
  { path: "**", redirectTo: "/users"}
];

@NgModule({
    declarations: [],
    imports: [
      RouterModule.forRoot(routes)
    ],
    exports: [RouterModule]
  })
export class AppRoutingModule { }
