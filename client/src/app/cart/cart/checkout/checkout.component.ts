import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { CartService } from 'src/app/_services/cart.service';
import { CheckoutService } from 'src/app/_services/checkout.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent  implements OnInit {
  user!: User;
  data: any;
  constructor(
    private accountService: AccountService, 
    private cartService: CartService,
    private checkoutService: CheckoutService,
    private toastr: ToastrService,
    ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user
      }
    })
  }

  ngOnInit() {
    this.checkout()
  }

  checkout(){
    this.checkoutService.getOrder(this.user.id).subscribe({
      next: response => {
        this.data = response[0]
        console.log(response)
      },
      error: error => this.toastr.error(error.error)
    })
  }

  buy(){
    
  }
}
