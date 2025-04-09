import { Component, signal } from '@angular/core';
import { HeaderComponent } from "./core/layout/header.component";
import { RouterModule, RouterOutlet } from "@angular/router";
import { FooterComponent } from "./core/layout/footer.component";
import { MaterialModule } from './shared/modules/MaterialModule';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SidebarComponent } from './core/layout/sidebar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    RouterOutlet,
    HeaderComponent, 
    FooterComponent,
    SidebarComponent,
    RouterModule, 
    ],
  templateUrl: "./app.component.html",
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  isSmallScreen = signal(window.innerWidth < 768);
  isSidebarOpened = signal(!this.isSmallScreen());

  toggleSidebar() {
    this.isSidebarOpened.set(!this.isSidebarOpened());
  }
  //title = 'homes';
}

