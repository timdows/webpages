import { Component, OnInit } from '@angular/core';
import { Configuration } from "app/app.configuration";
import { Http } from "@angular/http";
import { DisplayImage } from "app/_models/display-image";

@Component({
	selector: 'app-root',
	templateUrl: './app.component.pug',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

	private allImages = [];
	topImage: DisplayImage;
	bottomImage: DisplayImage;
	countdownMinutes: number;
	countdownSeconds: number;

	private switch = true;

	constructor(
		private configuration: Configuration,
		private http: Http) { }

	ngOnInit(): void {
		this.getAvailableImages();

		// Get the countdown every second
		setInterval(() => {
			this.getCountdown();
		}, 1000);
	}

	private getAvailableImages(): void {
		this.http.get("images.json")
			.subscribe((data) => {
				this.allImages = data.json();

				this.topImage = new DisplayImage(this.allImages[this.getRandomNumber()]);
				this.bottomImage = new DisplayImage(this.allImages[this.getRandomNumber()]);

				this.changeImages();
			});

		setTimeout(() => {
			this.getAvailableImages();
		}, 60 * 60 * 1000);
	}

	private changeImages(): void {
		if (this.switch) {
			this.topImage = new DisplayImage(this.allImages[this.getRandomNumber()]);
		}
		else {
			this.bottomImage = new DisplayImage(this.allImages[this.getRandomNumber()]);
		}

		this.switch = !this.switch;

		setTimeout(() => {
			this.changeImages();
		}, 7 * 1000);
	}

	private getRandomNumber(): number {
		return this.randomIntFromInterval(0, this.allImages.length - 1)
	}

	private randomIntFromInterval(min, max): number {
		let number = Math.floor(Math.random() * (max - min + 1) + min);
		return number;
	}

	// Gets the countdown value from nodejs (server.js) with the seconds the screen will be on
	private getCountdown(): void {
		this.http.get("countdown.json")
			.subscribe((data) => {
				let countdown = data.json().countdown;
				this.countdownMinutes = Math.floor(countdown / 60);
				this.countdownSeconds = countdown % 60;
			});
	}
}