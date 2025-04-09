import { Component, DestroyRef, inject, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AsyncPipe, NgClass, NgForOf } from "@angular/common";
import { tap } from "rxjs/operators";
import { UserService } from "../../../../core/auth/services/user.service";
import { RxLet } from "@rx-angular/template/let";
import { IfAuthenticatedDirective } from "../../../../core/auth/if-authenticated.directive";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
// Material
import { MatSlideToggleModule } from '@angular/material/slide-toggle';



@Component({
  selector: "app-home-page",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"],
  imports: [
    NgClass,
    AsyncPipe,
    RxLet,
    NgForOf,
    IfAuthenticatedDirective,
    // Material
    MatSlideToggleModule,
  ],
  standalone: true,
})
export default class HomeComponent implements OnInit {
  isAuthenticated = false;
  tagsLoaded = false;
  destroyRef = inject(DestroyRef);

  constructor(
    private readonly router: Router,
    //private readonly userService: UserService,
  ) {}

  ngOnInit(): void {
    
  }

}
