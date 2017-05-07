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

	constructor(
		private configuration: Configuration,
		private http: Http) { }

	ngOnInit(): void {
		this.http.get("images.json")
			.subscribe((data) => {
				this.allImages = data.json();

				let newTopImage = new DisplayImage(this.setImage(0));
				this.topImages.push(newTopImage);
				this.topImages.push(newTopImage);
				this.topImages.push(newTopImage);

				let newBottomImage = new DisplayImage(this.setImage(1));
				this.bottomImages.push(newBottomImage);
				this.bottomImages.push(newBottomImage);
				this.bottomImages.push(newBottomImage);

				this.changeImages();
			});
	}

	private changeImages() {
		var topNumber = this.randomIntFromInterval(0, this.allImages.length - 1);
		var bottomNumber = this.randomIntFromInterval(0, this.allImages.length - 1);

		let newTopImage = new DisplayImage(this.setImage(topNumber));
		this.topImages.push(newTopImage);
		this.topImages.shift();

		let newBottomImage = new DisplayImage(this.setImage(bottomNumber));
		this.bottomImages.push(newBottomImage);
		this.bottomImages.shift();

		setTimeout(() => {
			this.changeImages();
		}, 7 * 1000);
	}

	private setImage(imageNumber: number) {
		return `url('/${this.allImages[imageNumber]}')`;
	}

	private randomIntFromInterval(min, max) {
		var number = Math.floor(Math.random() * (max - min + 1) + min);
		console.log(number);
		return number;
	}
}

export class DisplayImage {
	constructor(public url: string){

	}
}