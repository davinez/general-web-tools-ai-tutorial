import { ChangeDetectionStrategy, Component } from "@angular/core";
import { RouterModule, RouterOutlet } from "@angular/router";
import { HeaderComponent } from "./header.component";
import { FooterComponent } from "./footer.component";

@Component({
  selector: "app-public-root",
  templateUrl: "./public-layout.component.html",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [HeaderComponent, FooterComponent, RouterOutlet, RouterModule],
  standalone: true,
  styleUrls: ['./public-layout.component.scss'],
})
export class PublicLayoutComponent {
}
