import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

@Component({
    selector: "my-app",
    template: require("./app.component.html")
})
export class AppComponent {

    constructor(private router: Router) {
    }

}