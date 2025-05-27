import {Component, Input} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from '@angular/router';


export interface IHousingLocation {
  id: number;
  name: string;
  city: string;
  state: string;
  photo: string;
  availableUnits: number;
  wifi: boolean;
  laundry: boolean;
}


@Component({
  selector: 'app-housing-location',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './housing-location.component.html',
  styleUrl: './housing-location.component.css'
})
export class HousingLocationComponent {
  /*
You have to add the ! because 
the input is expecting the value to be passed. 
In this case, there is no default value. 
In our example application case we know that the value
will be passed in - this is by design. 
The exclamation point is called the non-null assertion operator and 
it tells the TypeScript compiler that the value of this property 
won't be null or undefined. 
  */

// Render dynamic text with text interpolation
// https://v18.angular.dev/guide/templates/binding#render-dynamic-text-with-text-interpolation
  @Input() housingLocation!: IHousingLocation;
}
