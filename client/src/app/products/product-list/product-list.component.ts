import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Pagination } from 'src/app/_models/pagination';
import { Product } from 'src/app/_models/product';
import { ProductParams } from 'src/app/_models/productParams';
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
  products: any;
  user!: User;
  pageNumber = 1;
  pageSize = 2;
  pagination?: Pagination;
  productParams: any = {
    minPrice: 0,
    maxPrice: 1000000000,
    pageNumber: 1,
    pageSize: 5,
    searchTerm: '',
  }

  constructor(
    private productService: ProductsService, 
    private cartService: CartService,
    public accountService: AccountService,
    private toastr: ToastrService,
    ) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user => {
          if (user) this.user = user
        }
      })
     }

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getProducts(this.productParams).subscribe((data: any)=>{
      this.products = data.result;
      this.pagination = data.pagination;
    })
  }

  resetFilters() {
    this.productParams = this.productService.resetProductParams();
    this.loadProducts();
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadProducts();
    // if (this.userParams && this.userParams?.pageNumber !== event.page) {
    //   this.memberService.setUserParams(this.userParams);
    //   this.userParams.pageNumber = event.page;
    //   this.loadMembers();
    // }
  }

  addItemToCart(productId: any) {
    this.cartService.addItemToCart(productId, this.user.id).subscribe({
      next: response => this.toastr.success("Added to a cart"),
      error: error => this.toastr.error(error.error)
    })
  }

  addProductLike(product: Product) {
    this.productService.addLike(product.id).subscribe({
      next: () => this.toastr.success('You have liked ' + product)
    })
  }

}