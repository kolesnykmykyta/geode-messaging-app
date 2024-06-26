import { ResolveFn } from '@angular/router';
import { UserInfo } from '../interfaces/user-info.interface';
import { inject } from '@angular/core';
import { UsersService } from '../services/users.service';

export const usersInitResolver: ResolveFn<UserInfo[]> = (route, state) => {
  let usersService = inject(UsersService);
  return usersService.getAllUsers();
};
