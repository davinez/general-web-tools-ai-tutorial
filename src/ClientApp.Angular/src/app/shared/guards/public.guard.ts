import { Injectable } from '@angular/core';
import { BaseAuthGuard } from './base-auth.guard';

@Injectable({
  providedIn: 'root',
})
export class PublicGuard extends BaseAuthGuard {
  protected override requiresAuth = false;
  protected override redirectTo = '/app';
}
