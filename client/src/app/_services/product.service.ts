import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/enviroment.development';
import { Product } from '../_models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getProducts() {
    return this.http.get<Product[]>(this.baseUrl + 'products');
  }

  getProduct(name: string) {
    return this.http.get<Product>(this.baseUrl + 'products/' + name);
  }

  setMainPhoto(photoId: number, id: number) {
    return this.http.put(this.baseUrl + 'products/set-main-photo/' + photoId + '/' + id, {});
  }

  deletePhoto(photoId: number, id: number) {
    return this.http.delete(this.baseUrl + 'products/delete-photo/' + photoId + '/' + id);
  }
}