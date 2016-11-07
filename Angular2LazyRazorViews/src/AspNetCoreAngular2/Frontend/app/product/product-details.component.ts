import {Component} from "@angular/core";
import {ProductModel} from "./product.model";
import {ProductsService} from "./products.service";
import {ActivatedRoute} from "@angular/router";

@Component({
    templateUrl: "view/product/detail.html"
})
export class ProductDetailsComponent {

    constructor(
        private route: ActivatedRoute,
        private productsService: ProductsService) {

    }

    product: ProductModel;

    ngOnInit() {
        let id = parseInt(this.route.snapshot.params["id"], 10);
        this.productsService
            .getProduct(id)
            .subscribe(data => this.product = data);
    }

}