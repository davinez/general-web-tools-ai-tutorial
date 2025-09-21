import { HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { InjectionToken, inject } from '@angular/core';

export const API_URL = new InjectionToken<string>('API_URL');

export function hasHttpScheme(url: string) {
  return new RegExp('^http(s)?://', 'i').test(url);
}

export function apiUrlInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn) {
  const apiUrl = inject(API_URL, { optional: true });

  const hasScheme = (url: string) => apiUrl && hasHttpScheme(url);

  const prependApiUrl = (url: string) =>
    [apiUrl?.replace(/\/$/g, ''), url.replace(/^\.?\//, '')].filter(val => val).join('/');

  return hasScheme(req.url) === false
    ? next(req.clone({ url: prependApiUrl(req.url) }))
    : next(req);
}
