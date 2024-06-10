import { HttpInterceptorFn } from "@angular/common/http";

export const authInterceptor: HttpInterceptorFn = (request, next) => {
    let token = localStorage.getItem("accessToken") ?? '';
    request = request.clone({
        setHeaders: {
            Authorization: token
        }
    })

    return next(request)
}