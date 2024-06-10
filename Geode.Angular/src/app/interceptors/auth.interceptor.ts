import { HttpInterceptorFn } from "@angular/common/http";

import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let token = localStorage.getItem("accessToken") ?? '';
    req = req.clone({
        setHeaders: {
            Authorization: token
        }
    })

    return next.handle(req)
  }
}