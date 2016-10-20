import { Component, OnInit } from "@angular/core";

@Component({
	selector: "projectcomponent",
	templateUrl: "view/projects/index.html"
})
export class ProjectComponent implements OnInit {

	message: string;

	constructor() {
	}

	ngOnInit() {
		this.message = "From the angular2 ProjectComponent class";
	}
}
