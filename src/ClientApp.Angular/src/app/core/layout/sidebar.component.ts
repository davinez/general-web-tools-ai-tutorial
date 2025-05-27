import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { RouterLink } from '@angular/router';
import { MaterialModule } from 'src/app/shared/modules/MaterialModule';

@Component({
  selector: 'app-layout-sidebar',
  imports: [MatListModule, RouterLink],
  template: `
    <mat-nav-list>
      <a mat-list-item routerLink="/home">Home</a>
      <a mat-list-item routerLink="/about">About</a>
      <a mat-list-item routerLink="/contact">Contact</a>
    </mat-nav-list>
  `,
  styleUrls: ['./sidebar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SidebarComponent { }
