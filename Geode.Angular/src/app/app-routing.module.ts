import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UsersComponent } from './pages/users/users.component';
import { authorizedGuard } from './shared/services/authorized.guard';
import { notAuthorizedGuard } from './shared/services/not-authorized.guard';
import { MessagesComponent } from './pages/messages/messages.component';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import('./pages/auth/auth.module').then((m) => m.AuthModule),
    canActivate: [notAuthorizedGuard],
  },
  { path: 'users', component: UsersComponent, canActivate: [authorizedGuard] },
  {
    path: 'messages',
    component: MessagesComponent,
    canActivate: [authorizedGuard],
  },
  { path: '**', redirectTo: '/users' },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
