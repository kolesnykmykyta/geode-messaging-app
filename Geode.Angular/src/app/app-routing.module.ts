import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UsersComponent } from './pages/users/users.component';
import { authorizedGuard } from './shared/guards/authorized.guard';
import { notAuthorizedGuard } from './shared/guards/not-authorized.guard';
import { MessagesComponent } from './pages/messages/messages.component';
import { usersInitResolver } from './shared/resolvers/users-init.resolver';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import('./pages/auth/auth.module').then((m) => m.AuthModule),
    canActivate: [notAuthorizedGuard],
  },
  {
    path: 'users',
    component: UsersComponent,
    resolve: { initData: usersInitResolver },
    canActivate: [authorizedGuard],
  },
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
