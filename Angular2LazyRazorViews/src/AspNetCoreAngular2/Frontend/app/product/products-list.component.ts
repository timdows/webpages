import {Component} from "@angular/core";
import {ProductModel} from "./product.model";
import {ProductsService} from "./products.service";

@Component({
    templateUrl: "view/product/list.html"
})
export class ProductsListComponent {

    constructor(private productsService: ProductsService) {

    }

    products: ProductModel[];

    ngOnInit() {
        this.productsService
            .getProducts()
            .subscribe(data => {
                this.products = data;
            });
    }
}