import {NgModule, Component} from "@angular/core";
import {RouterModule} from "@angular/router";

import { AboutComponent } from "./about.component";

const ROUTES = [
  { path: "", component: AboutComponent }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)]
})
class AboutModule {}