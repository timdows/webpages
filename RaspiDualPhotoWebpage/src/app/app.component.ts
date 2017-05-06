import { Component, OnInit } from '@angular/core';
import { Configuration } from "app/app.configuration";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.pug',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

	private topImages = ["1.jpg", "2.jpg"];
	private bottomImages = ["2.jpg", "1.jpg"];

	topImage: string;
	bottomImage: string;

	constructor(private configuration: Configuration){}

	ngOnInit(): void {
		this.topImage = `url(${this.configuration.ImageLocation}/1.jpg)`;
		this.bottomImage = `url(${this.configuration.ImageLocation}/2.jpg)`;
	}


}
