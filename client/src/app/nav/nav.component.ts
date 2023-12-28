import { AccountService } from '../_services/account.service';
import { Observable, of, take } from 'rxjs';
import { User } from '../_models/user';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CartService } from '../_services/cart.service';
import { Cart } from '../_models/cart';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  cart: Cart | undefined;
  user!: User;

  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private cartService: CartService
  ) { }

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user
        this.getCart(this.user.id);
      }
    })
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => this.router.navigateByUrl('/products'),
      error: error => this.toastr.error(error.error)
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  getCart(id: number) {
    this.cartService.getCart(id).subscribe({
      next: response => {
        console.log(response)
        this.cart = response;
      }
    })
  } 
}
