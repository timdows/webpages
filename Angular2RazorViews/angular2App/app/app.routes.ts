import { Routes, RouterModule } from "@angular/router";
import { ProjectComponent } from "./project/project.component";
import { AboutComponent } from "./about/about.component";

const appRoutes: Routes = [
	{ path: "", component: ProjectComponent },
	{
		path: "about",
		//loadChildren: "./about/about.bundle"
		loadChildren: "./about/about.bundle#AboutComponent"
		//component: AboutComponent
	}
];

export const routing = RouterModule.forRoot(appRoutes);