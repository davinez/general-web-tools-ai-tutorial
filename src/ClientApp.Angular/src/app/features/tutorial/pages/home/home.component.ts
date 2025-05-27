import {Component, inject} from '@angular/core';
import {CommonModule} from '@angular/common';

import {IHousingLocation, HousingLocationComponent} from '../../components/housing-location/housing-location.component';
import {HousingService} from '../../services/housing.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, HousingLocationComponent],
  template: `
    <section>
      <form>
      <!-- template variable the #filter
      More. https://v18.angular.dev/guide/templates -->
        <input type="text" placeholder="Filter by city" #filter>
        <button class="primary" type="button" (click)="filterResults(filter.value)">Search</button>
      </form>
    </section>
    <section class="results">
     <app-housing-location *ngFor="let housingLocation of filteredLocationList" [housingLocation]="housingLocation"></app-housing-location>
    </section>
  `,
  styleUrls: ['./home.component.css'],
})
export default class HomeComponent {
  housingService: HousingService = inject(HousingService);
  
  housingLocationList: IHousingLocation[] = [];

  filteredLocationList: IHousingLocation[] = [];

  constructor() {
    this.housingService.getAllHousingLocations().then((housingLocationList: IHousingLocation[]) => {
      this.housingLocationList = housingLocationList;
      this.filteredLocationList = housingLocationList;
    });
  }

  filterResults(text: string) {
    if (!text) {
      this.filteredLocationList = this.housingLocationList;
      return;
    }
    this.filteredLocationList = this.housingLocationList.filter((housingLocation) =>
      housingLocation?.city.toLowerCase().includes(text.toLowerCase()),
    );
  }

}