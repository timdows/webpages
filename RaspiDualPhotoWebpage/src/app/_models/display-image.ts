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