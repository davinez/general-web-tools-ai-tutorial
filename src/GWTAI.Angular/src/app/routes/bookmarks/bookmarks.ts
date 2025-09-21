import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatListModule } from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MtxProgressModule } from '@ng-matero/extensions/progress';
import { MtxAlertModule } from '@ng-matero/extensions/alert';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-bookmarks',
  templateUrl: './bookmarks.html',
  styleUrl: './bookmarks.scss',
  imports: [
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatListModule,
    MatListModule,
    MatTableModule,
    MatTabsModule,
    MtxProgressModule,
    MtxAlertModule,
  ],
})
export class Bookmarks{


}
