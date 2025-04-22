import { Routes } from "@angular/router";
import { DetailsComponent } from "./components/details/details.component";
import HomeComponent from "./pages/home/home.component";

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    title: 'Home page',
  },
  {
    path: 'details/:id',
    component: DetailsComponent,
    title: 'Home details',
  },
  // {
  //   path: "",
  //   children: [
  //     {
  //       path: "Home",
  //       component: HomeComponent,
  //       children: [
  //         {
  //           path: 'details/:id',
  //           component: DetailsComponent,
  //           title: 'Home details',
  //         },
  //       ],
  //     },
  //   ],
  // },
];

export default routes;
