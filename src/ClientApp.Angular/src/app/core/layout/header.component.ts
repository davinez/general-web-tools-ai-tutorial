import { ChangeDetectionStrategy, Component, EventEmitter, inject, Output } from "@angular/core";
import { UserService } from "../auth/services/user.service";
import { RouterLink, RouterLinkActive } from "@angular/router";
import { AsyncPipe, NgIf } from "@angular/common";
import { IfAuthenticatedDirective } from "../auth/if-authenticated.directive";
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: "app-layout-header",
  templateUrl: "./header.component.html",
  imports: [
    NgIf,
    AsyncPipe,
    RouterLink,
    IfAuthenticatedDirective,
    MatToolbarModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
  ],
  standalone: true,
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent {
  @Output() toggleSidebar = new EventEmitter<void>();

  currentUser$ = inject(UserService).currentUser;

  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

  onLogout() {
    // implement your logout logic here
    console.log('Logout clicked');
  }
}
