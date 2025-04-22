import { Injectable } from '@angular/core';
import { IHousingLocation } from '../components/housing-location/housing-location.component';

@Injectable({
  providedIn: 'root'
})
export class HousingService {

  // not in use
  readonly baseUrl = 'https://angular.dev/assets/images/tutorials/common';

  url: string = 'http://localhost:7000/api/tutorial/locations';

  constructor() { }

  // For this example, the code uses fetch. 
  // For more advanced use cases consider 
  // using HttpClient provided by Angular.
  // more https://v18.angular.dev/guide/http
  async getAllHousingLocations(): Promise<IHousingLocation[]> {
    const data = await fetch(this.url);
    return (await data.json()) ?? [];
  }

  async getHousingLocationById(id: number): Promise<IHousingLocation | undefined> {
    const data = await fetch(`${this.url}/${id}`);
    return (await data.json()) ?? {};
  }

  submitApplication(firstName: string, lastName: string, email: string) {
    console.log(
      `Homes application received: firstName: ${firstName}, lastName: ${lastName}, email: ${email}.`,
    );
  }


}
