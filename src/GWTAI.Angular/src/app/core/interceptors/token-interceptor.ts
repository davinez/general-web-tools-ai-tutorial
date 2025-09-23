import { HttpErrorResponse, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '@core/authentication';
import { catchError, tap, throwError } from 'rxjs';
import { API_URL, hasHttpScheme } from './api-url-interceptor';

export function tokenInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn) {
  const router = inject(Router);
  const apiUrl = inject(API_URL, { optional: true });
  const tokenService = inject(TokenService);

  const includeApiUrl = (url: string) => {
    if (!apiUrl) {
      return false;
    }
    return new RegExp(`^${apiUrl.replace(/\/$/, '')}`, 'i').test(url);
  };

  const shouldAppendToken = (url: string) => !hasHttpScheme(url) || includeApiUrl(url);

  const handler = () => {
    if (req.url.includes('/auth/logout')) {
      router.navigateByUrl('/auth/login');
    }

    if (router.url.includes('/auth/login')) {
      router.navigateByUrl('/dashboard');
    }
  };

  if (tokenService.valid() && shouldAppendToken(req.url)) {
    return next(
      req.clone({
        headers: req.headers.append('Authorization', tokenService.getBearerToken()),
        withCredentials: true,
      })
    ).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          tokenService.clear();
        }
        return throwError(() => error);
      }),
      tap(() => handler())
    );
  }

  return next(req).pipe(tap(() => handler()));
}
