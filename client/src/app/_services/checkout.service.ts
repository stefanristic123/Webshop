import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/enviroment.development';
import { Cart } from '../_models/cart';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  baseUrl = environment.apiUrl;
  private cartStatus = new BehaviorSubject<Cart | null>(null);
  currentCart$ = this.cartStatus.asObservable();

  constructor(private http: HttpClient) { }

  getOrder(userId: number) {
    return this.http.get<any>(this.baseUrl + 'order/' + userId);
  }

  createOrder(userId: number) {
    return this.http.post(this.baseUrl + 'order/' + userId, {});
  }
}
