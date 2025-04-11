import { Component } from "@angular/core";
// Material
import { MatSlideToggleModule } from '@angular/material/slide-toggle';


@Component({
  selector: "app-public-home-page",
  templateUrl: "./public-home.component.html",
  //styleUrls: ["./home.component.scss"],
  imports: [
    // Material
    MatSlideToggleModule,
  ],
  standalone: true,
})
export default class HomeComponent {

  constructor(
  ) {}

}