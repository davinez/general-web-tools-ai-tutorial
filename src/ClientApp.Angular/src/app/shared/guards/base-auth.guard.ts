import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserService } from 'src/app/core/auth/services/user.service';


@Injectable()
export abstract class BaseAuthGuard implements CanActivate {
  constructor(
    protected userService: UserService,
    protected router: Router
  ) {}

  protected abstract requiresAuth: boolean;
  protected abstract redirectTo: string;

  canActivate(): Observable<boolean | UrlTree> {
    return this.userService.isAuthenticated.pipe(
      map((isAuth) => {
        if (this.requiresAuth && !isAuth) {
          return this.router.parseUrl(this.redirectTo);
        }
        if (!this.requiresAuth && isAuth) {
          return this.router.parseUrl(this.redirectTo);
        }
        return true;
      })
    );
  }
}
