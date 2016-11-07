import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { AppComponent } from "./app.component";
import { Configuration } from "./app.constants";
import { routing } from "./app.routes";
import { HttpModule, JsonpModule } from "@angular/http";
import { MaterialModule } from "@angular/material";
import { Angular2FlexModule } from "angular2-flex";
import { AUTH_PROVIDERS } from "angular2-jwt";

import { ProjectComponent } from "./project/project.component";
//import { AboutComponent } from "./about/about.component";

@NgModule({
    imports: [
        BrowserModule,
        CommonModule,
        FormsModule,
		routing,
        HttpModule,
        JsonpModule,
        MaterialModule.forRoot(),
        Angular2FlexModule.forRoot()
    ],
    declarations: [
        AppComponent,
		ProjectComponent,
		//AboutComponent
    ],
    providers: [
        Configuration,
        AUTH_PROVIDERS
    ],
    bootstrap: [AppComponent],
})
export class AppModule {
}