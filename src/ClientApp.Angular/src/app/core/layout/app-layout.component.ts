import { Component, signal } from '@angular/core';
import { RouterModule, RouterOutlet } from "@angular/router";
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { HeaderComponent } from './header.component';
import { FooterComponent } from './footer.component';
import { SidebarComponent } from './sidebar.component';

// import { MaterialModule } from './shared/modules/MaterialModule';


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
  templateUrl: "./app-layout.component.html",
  styleUrls: ['./app-layout.component.scss'],
})
export class AppLayoutComponent {
  isSmallScreen = signal(window.innerWidth < 768);
  isSidebarOpened = signal(!this.isSmallScreen());

  toggleSidebar() {
    this.isSidebarOpened.set(!this.isSidebarOpened());
  }
  
  //title = 'homes';
}

