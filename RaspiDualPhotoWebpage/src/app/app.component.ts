import { Component, OnInit } from '@angular/core';
import { Configuration } from "app/app.configuration";
import { Http } from "@angular/http";

@Component({
	selector: 'app-root',
	templateUrl: './app.component.pug',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

	private allImages = [];
	topImages = new Array<DisplayImage>();
	bottomImages = new Array<DisplayImage>();

	private switch = true;

	constructor(
		private configuration: Configuration,
		private http: Http) { }

	ngOnInit(): void {
		this.http.get("images.json")
			.subscribe((data) => {
				this.allImages = data.json();

				this.topImages.push(new DisplayImage(this.allImages[1]));
				this.topImages.push(new DisplayImage(this.allImages[1]));
				this.topImages.push(new DisplayImage(this.allImages[2]));

				this.bottomImages.push(new DisplayImage(this.allImages[2]));
				this.bottomImages.push(new DisplayImage(this.allImages[2]));
				this.bottomImages.push(new DisplayImage(this.allImages[3]));

				this.changeImages();
			});
	}

	private changeImages() {
		if (this.switch) {
			let topNumber = this.randomIntFromInterval(0, this.allImages.length - 1);
			let newTopImage = new DisplayImage(this.allImages[topNumber]);
			this.topImages.push(newTopImage);
			this.topImages.shift();
		}
		else {
			let bottomNumber = this.randomIntFromInterval(0, this.allImages.length - 1);
			let newBottomImage = new DisplayImage(this.allImages[bottomNumber]);
			this.bottomImages.push(newBottomImage);
			this.bottomImages.shift();
		}

		this.switch = !this.switch;

		setTimeout(() => {
			this.changeImages();
		}, 7 * 1000);
	}

	private randomIntFromInterval(min, max) {
		var number = Math.floor(Math.random() * (max - min + 1) + min);
		return number;
	}
}

export class DisplayImage {
	directory: string;
	url: string;
	
	constructor(path: string) {
		this.url = `url('/${path}')`;

		let split = path.split('/');
		if (split.length === 3) {
			this.directory = split[1];
		}
	}
}