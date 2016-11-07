import { Component, OnInit } from "@angular/core";

@Component({
	selector: "aboutcomponent",
	templateUrl: "view/about/index.html"
})
export class AboutComponent implements OnInit {

	message: string;

	constructor() {
		console.log("loaded AboutComponent");
	}

	ngOnInit() {
		this.message = "From the angular2 AboutComponent class";
	}
}
