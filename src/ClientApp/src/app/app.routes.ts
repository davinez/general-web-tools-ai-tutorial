import { Routes } from "@angular/router";
import { inject } from "@angular/core";
import { UserService } from "./core/auth/services/user.service";
import { map } from "rxjs/operators";
import { PublicLayoutComponent } from "./core/layout/public-layout.component";
import { PublicGuard } from "./shared/guards/public.guard";
import { AuthGuard } from "./shared/guards/auth.guard";
import { AppLayoutComponent } from "./core/layout/app-layout.component";

export const routes: Routes = [
  // Unauthorized
  {
    path: "",
    component: PublicLayoutComponent,
    canActivate: [PublicGuard],
    children: [
      {
        path: "",
        loadComponent: () =>
          import("./public-home.component"),
      },
      {
        path: "login",
        loadComponent: () => import("./core/auth/auth.component"),
      },
      {
        path: "register",
        loadComponent: () => import("./core/auth/auth.component"),
      },
    ]
  },
  // Authorized
  {
    path: "app",
    component: AppLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      // Tutorial
      {
        path: "tutorial",
        loadChildren: () => import("./features/tutorial/tutorial.routes"),
      },
      //
      {
        path: "",
        loadComponent: () => import("./features/processor/pages/home/home.component"),
      },
      {
        path: "settings",
        loadComponent: () => import("./features/settings/settings.component"),
      },
      {
        path: "profile",
        loadChildren: () => import("./features/profile/profile.routes"),
      },
      {
        path: "editor",
        children: [
          {
            path: "",
            loadComponent: () =>
              import("./features/article/pages/editor/editor.component"),
          },
          {
            path: ":slug",
            loadComponent: () =>
              import("./features/article/pages/editor/editor.component"),
          },
        ],
      },
      {
        path: "articles",
        loadComponent: () => import("./features/article/pages/home/home.component"),
      },
      {
        path: "article/:slug",
        loadComponent: () =>
          import("./features/article/pages/article/article.component"),
      },
    ],
  },
  // Catch-all
  {
    path: '**',
    redirectTo: '',
  }
];
