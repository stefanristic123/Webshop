import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Product } from 'src/app/_models/product';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { CartService } from 'src/app/_services/cart.service';
import { ProductsService } from 'src/app/_services/product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent  implements OnInit {
  products: Product[] = [];
  user!: User;

  constructor(
    private productService: ProductsService, 
    private cartService: CartService,
    public accountService: AccountService,
    private toastr: ToastrService,
    ) { }

  ngOnInit(): void {
    this.loadProducts();
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user
      }
    })
  }

  loadProducts() {
    this.productService.getProducts().subscribe({
      next: products => this.products = products
    })
  }

  addItemToCart(productId: number) {
    this.cartService.addItemToCart(productId, this.user.id).subscribe({
      next: response => this.toastr.success("Added to a cart"),
      error: error => this.toastr.error(error.error)
    })
  }
}