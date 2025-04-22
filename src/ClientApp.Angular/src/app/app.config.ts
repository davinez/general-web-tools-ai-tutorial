import { provideAppInitializer, ApplicationConfig, inject, provideZoneChangeDetection } from "@angular/core";
import { provideRouter } from "@angular/router";

import { routes } from "./app.routes";
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { JwtService } from "./core/auth/services/jwt.service";
import { UserService } from "./core/auth/services/user.service";
import { apiInterceptor } from "./core/interceptors/api.interceptor";
import { tokenInterceptor } from "./core/interceptors/token.interceptor";
import { errorInterceptor } from "./core/interceptors/error.interceptor";
import { EMPTY } from "rxjs";


// If userService.getCurrentUser() is an Observable, 
// Angular will wait for it to complete before bootstrapping the app, 
// which is exactly what you want during app initialization.
export function initAuth(jwtService: JwtService, userService: UserService) {
  return jwtService.getToken() ? userService.getCurrentUser() : EMPTY;
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([apiInterceptor, tokenInterceptor, errorInterceptor])
    ),
    provideAppInitializer(() => {
      const jwtService = inject(JwtService);
      const userService = inject(UserService);
      return initAuth(jwtService, userService); // returns Observable directly
    })
  ]
};

