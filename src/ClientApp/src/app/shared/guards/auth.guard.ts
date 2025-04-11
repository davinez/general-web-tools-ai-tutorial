import { Injectable } from '@angular/core';
import { BaseAuthGuard } from './base-auth.guard';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard extends BaseAuthGuard {
  protected override requiresAuth = true;
  protected override redirectTo = '/login';
}
