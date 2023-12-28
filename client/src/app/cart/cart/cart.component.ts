import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Cart } from 'src/app/_models/cart';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { CartService } from 'src/app/_services/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent   implements OnInit{
  user!: User;
  cart: Cart | undefined;

  constructor(
    private accountService: AccountService, 
    private cartService: CartService,
    private toastr: ToastrService,
    ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user
      }
    })
  }
   
  ngOnInit() {
    this.getCart(this.user.id)
  }

  getCart(id: number) {
    this.cartService.getCart(id).subscribe({
      next: response => {
        console.log(response)
        this.cart = response;
      }
    })
  } 


  removeItem(cartItemId: number) {
    this.cartService.removeItemFromCart(this.user.id, cartItemId).subscribe({
      next: response => {
        this.getCart(this.user.id);
        this.toastr.success("Removed from cart")
      },
      error: error => this.toastr.error(error.error)
    })
  }
}
