import { Routes, RouterModule } from "@angular/router";
import { ProjectComponent } from "./project/project.component";

const appRoutes: Routes = [
    { path: "", component: ProjectComponent }
];

export const routing = RouterModule.forRoot(appRoutes);