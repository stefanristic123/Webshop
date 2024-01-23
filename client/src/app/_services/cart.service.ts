import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/enviroment.development';
import { Product } from '../_models/product';
import { Cart } from '../_models/cart';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl = environment.apiUrl;
  private cartStatus = new BehaviorSubject<Cart | null>(null);
  currentCart$ = this.cartStatus.asObservable();

  constructor(private http: HttpClient) { }

  getCart(id: number) {
    return this.http.get<Cart>(this.baseUrl + 'cart/' + id);
  }

  addItemToCart(userId: number, productId: number) {
    return this.http.post(this.baseUrl + 'cart/add-item/' +  productId + '/' + userId, {});
  }

  removeItemFromCart(userId: number, cartItemId: number) {
    return this.http.delete(this.baseUrl + 'cart/remove-item/' + userId + '/' + cartItemId, {});
  }
}