import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { ACCESS_TOKEN_KEY } from '../constants/storages.constants';
import {
  AUTH_RULE_HEADER_NAME,
  AUTH_RULE_HEADER_VALUES,
} from '../constants/auth-rule-header.constants';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (
      req.headers.get(AUTH_RULE_HEADER_NAME) !== AUTH_RULE_HEADER_VALUES.SKIP
    ) {
      let token = localStorage.getItem(ACCESS_TOKEN_KEY) ?? '';
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return next.handle(req);
  }
}
